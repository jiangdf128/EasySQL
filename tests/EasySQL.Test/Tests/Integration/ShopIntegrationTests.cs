using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using Xunit;

namespace EasySQL.Test.Tests.Integration
{
    [Collection("Database")]
    public class ShopIntegrationTests : TestBase
    {
        private static void CleanAll(IDbConnection conn)
        {
            conn.Execute("DELETE FROM OrderPayments");
            conn.Execute("DELETE FROM OrderItems");
            conn.Execute("DELETE FROM Orders");
            conn.Execute("DELETE FROM Products");
            conn.Execute("DELETE FROM Users");
        }

        // ================================================================
        // 简单 CRUD — Dapper.Contrib
        // ================================================================

        [Fact]
        public void Contrib_UserCrud_ShouldWork()
        {
            EasySQLContext.Default.Do(conn =>
            {
                CleanAll(conn);
                var user = new User { UserName = "张三", Balance = 1000 };
                conn.Insert(user);
                Assert.True(user.Id > 0);

                var fetched = conn.Get<User>(user.Id);
                Assert.Equal("张三", fetched.UserName);

                fetched.Balance = 500;
                conn.Update(fetched);
                Assert.Equal(500, conn.Get<User>(user.Id).Balance);

                conn.Delete(fetched);
                Assert.Null(conn.Get<User>(user.Id));
            });
        }

        [Fact]
        public void Contrib_ProductCrud_ShouldWork()
        {
            EasySQLContext.Default.Do(conn =>
            {
                CleanAll(conn);
                var prod = new Product { Name = "iPad", Price = 3599, Stock = 200 };
                conn.Insert(prod);
                Assert.True(prod.Id > 0);

                var fetched = conn.Get<Product>(prod.Id);
                Assert.Equal("iPad", fetched.Name);

                fetched.Price = 3299;
                conn.Update(fetched);
                Assert.Equal(3299, conn.Get<Product>(prod.Id).Price);

                conn.Delete(fetched);
                Assert.Null(conn.Get<Product>(prod.Id));
            });
        }

        // ================================================================
        // 完整下单流程 — Dapper.Contrib 做单表写入，EasySQL 做多表 JOIN 验证
        // ================================================================

        [Fact]
        public void CreateOrder_FullFlow_ShouldSucceed()
        {
            EasySQLContext.Default.Do(conn =>
            {
                CleanAll(conn);

                // 单表写入 → Dapper.Contrib
                var user = new User { UserName = "李四", Balance = 5000 };
                conn.Insert(user);
                var prod = new Product { Name = "iPhone", Price = 6999, Stock = 100 };
                conn.Insert(prod);
                var order = new Order { UserId = user.Id, OrderNo = "ORD_FULL", TotalAmount = 6999 };
                conn.Insert(order);
                conn.Insert(new OrderItem { OrderId = order.Id, ProductId = prod.Id, Quantity = 1, UnitPrice = 6999 });
                conn.Insert(new OrderPayment { OrderId = order.Id, PayMethod = 1, Amount = 6999, Status = 2 });

                // 复杂多表查询 → EasySQL
                var sa = new UserTableDef("SA");
                var sb = new OrderTableDef("SB");
                var sc= new OrderPaymentTableDef("SC");


                sa.Select(true, sa.UserName);
                sb.Select(true, sb.OrderNo, sb.TotalAmount);
                sc.Select(true, sc.Amount, sc.Status);
                sa.Join(sb, $"{sa.GetId()} = {sb.GetUserId()}");
                sb.Join(sc, $"{sb.GetId()} = {sc.GetOrderId()}");

                var qb = new QueryBuilder().From(sa, sb, sc)
                    .Where($"{sb.GetOrderNo()} = 'ORD_FULL'");

                var rows = conn.Query(qb.BuildSql(), qb.Parameters.ToDynamicParameters()).ToList();
                Assert.Single(rows);

                // 复杂批量删除 → EasySQL（QueryBuilder 构造子查询 + DeleteBuilder 级联清理）
                var sp = new OrderPaymentTableDef();
                var si = new OrderItemTableDef();
                var so = new OrderTableDef();
                so.Select(true, so.Id);
                var subQb = new QueryBuilder().From(so)
                    .Where($"{so.GetOrderNo()} = 'ORD_FULL'");
                string subSql = subQb.BuildSql();

                conn.Execute(new DeleteBuilder(sp)
                    .Where($"{sp.GetOrderId()} IN ({subSql})")
                    .BuildSql());
                conn.Execute(new DeleteBuilder(si)
                    .Where($"{si.GetOrderId()} IN ({subSql})")
                    .BuildSql());
                conn.Execute(new DeleteBuilder(so)
                    .Where($"{so.GetOrderNo()} = 'ORD_FULL'")
                    .BuildSql());
                conn.Delete(prod);
                conn.Delete(user);
            });
        }

        // ================================================================
        // 多表 JOIN — EasySQL
        // ================================================================

        [Fact]
        public void OrderDetail_ThreeTableJoin_ShouldWork()
        {
            EasySQLContext.Default.Do(conn =>
            {
                CleanAll(conn);

                var user = new User { UserName = "王五" };
                var prod = new Product { Name = "MacBook", Price = 9999, Stock = 50 };
                conn.Insert(user);
                conn.Insert(prod);
                var order = new Order { UserId = user.Id, OrderNo = "ORD_JOIN", TotalAmount = 9999 };
                conn.Insert(order);
                var item = new OrderItem { OrderId = order.Id, ProductId = prod.Id, Quantity = 1, UnitPrice = 9999 };
                conn.Insert(item);

                var sa = new UserTableDef("SA");
                var sb = new OrderTableDef("SB");
                var sc = new OrderItemTableDef("SC");
                sa.Select(true, sa.UserName);
                sb.Select(true, sb.OrderNo);
                sc.Select(true, sc.Quantity, sc.UnitPrice);
                sa.Join(sb, $"{sa.GetId()} = {sb.GetUserId()}");
                sb.Join(sc, $"{sb.GetId()} = {sc.GetOrderId()}");

                var qb = new QueryBuilder().From(sa, sb, sc)
                    .Where($"{sb.GetOrderNo()} = 'ORD_JOIN'");

                Assert.Single(conn.Query(qb.BuildSql(), qb.Parameters.ToDynamicParameters()));
                CleanAll(conn);
            });
        }

        // ================================================================
        // GROUP BY — EasySQL
        // ================================================================

        [Fact]
        public void GroupBy_ProductSales_ShouldWork()
        {
            EasySQLContext.Default.Do(conn =>
            {
                CleanAll(conn);

                var user = new User { UserName = "统计" };
                conn.Insert(user);
                var p1 = new Product { Name = "鼠标", Price = 99, Stock = 1000 };
                var p2 = new Product { Name = "键盘", Price = 299, Stock = 500 };
                conn.Insert(p1);
                conn.Insert(p2);

                var o1 = new Order { UserId = user.Id, OrderNo = "GS1", TotalAmount = 0 };
                var o2 = new Order { UserId = user.Id, OrderNo = "GS2", TotalAmount = 0 };
                conn.Insert(o1);
                conn.Insert(o2);

                conn.Insert(new OrderItem { OrderId = o1.Id, ProductId = p1.Id, Quantity = 3, UnitPrice = 99 });
                conn.Insert(new OrderItem { OrderId = o1.Id, ProductId = p2.Id, Quantity = 1, UnitPrice = 299 });
                conn.Insert(new OrderItem { OrderId = o2.Id, ProductId = p1.Id, Quantity = 1, UnitPrice = 99 });

                var s = new OrderItemTableDef("i");
                s.Select(true, s.ProductId);
                s.SelectExpression("SUM(Quantity) AS TotalQty");
                var qb = new QueryBuilder().From(s)
                    .GroupBy($"{s.GetProductId()}")
                    .Having("SUM(Quantity) >= 2");

                var rows = conn.Query(qb.BuildSql(), qb.Parameters.ToDynamicParameters()).ToList();
                Assert.Single(rows);
                CleanAll(conn);
            });
        }

        // ================================================================
        // 分页 — EasySQL
        // ================================================================

        [Fact]
        public void Paging_WithGroupBy_TotalShouldBeGroupCount()
        {
            EasySQLContext.Default.Do(conn =>
            {
                CleanAll(conn);
                var user = new User { UserName = "分组分页" };
                conn.Insert(user);
                // 3 个商品，各 2 笔订单
                for (int p = 1; p <= 3; p++)
                {
                    var prod = new Product { Name = $"分组商品{p}", Price = 10, Stock = 100 };
                    conn.Insert(prod);
                    for (int i = 1; i <= 2; i++)
                    {
                        var o = new Order { UserId = user.Id, OrderNo = $"GP_{p}_{i}", TotalAmount = 10 };
                        conn.Insert(o);
                        conn.Insert(new OrderItem { OrderId = o.Id, ProductId = prod.Id, Quantity = i, UnitPrice = 10 });
                    }
                }

                var s = new OrderItemTableDef("i");
                s.Select(true, s.ProductId);
                s.SelectExpression("Count(1) AS Cnt");
                var qb = new QueryBuilder().From(s)
                    .GroupBy($"{s.GetProductId()}")
                    .OrderBy($"{s.GetProductId()} ASC");

                // 一次查询模式：总记录数是分组数（3），不是明细行数（6）
                string sql = qb.BuildSql(rowLimit: 2, rowOffset: 0);
                var rows = conn.Query(sql, qb.Parameters.ToDynamicParameters()).ToList();
                Assert.Equal(2, rows.Count);
                Assert.Equal(3, Convert.ToInt32(((IDictionary<string, object>)rows[0])[QueryBuilder.PagingTotalAlias]));

                // 两次查询模式：计数语句同样统计分组数
                int total = conn.ExecuteScalar<int>(qb.BuildCountSql(), qb.Parameters.ToDynamicParameters());
                Assert.Equal(3, total);

                CleanAll(conn);
            });
        }

        [Fact]
        public void Paging_OrderList_ShouldWork()
        {
            EasySQLContext.Default.Do(conn =>
            {
                CleanAll(conn);
                var user = new User { UserName = "分页" };
                conn.Insert(user);
                for (int i = 1; i <= 15; i++)
                    conn.Insert(new Order { UserId = user.Id, OrderNo = $"PAGE_{i:D3}", TotalAmount = i * 100 });

                var s = new OrderTableDef("o");
                s.Select(true, s.OrderNo, s.TotalAmount);
                var qb = new QueryBuilder().From(s)
                    .Where($"{s.GetOrderNo()} LIKE 'PAGE_%'")
                    .OrderBy($"{s.GetOrderNo()} ASC");

                string sql = qb.BuildSql(rowLimit: 5, rowOffset: 5);
                Assert.Contains(QueryBuilder.PagingTotalAlias, sql);

                var rows = conn.Query(sql, qb.Parameters.ToDynamicParameters()).ToList();
                Assert.Equal(5, rows.Count);
                Assert.Equal(15, Convert.ToInt32(((IDictionary<string, object>)rows[0])[QueryBuilder.PagingTotalAlias]));

                CleanAll(conn);
            });
        }

        [Fact]
        public void Paging_WithoutTotalCount_ShouldMatchWindowFunctionResult()
        {
            EasySQLContext.Default.Do(conn =>
            {
                CleanAll(conn);
                var user = new User { UserName = "分页" };
                conn.Insert(user);
                for (int i = 1; i <= 15; i++)
                    conn.Insert(new Order { UserId = user.Id, OrderNo = $"PAGE_{i:D3}", TotalAmount = i * 100 });

                var s = new OrderTableDef("o");
                s.Select(true, s.OrderNo, s.TotalAmount);
                var qb = new QueryBuilder().From(s)
                    .Where($"{s.GetOrderNo()} LIKE 'PAGE_%'")
                    .OrderBy($"{s.GetOrderNo()} ASC");

                // 模式一：窗口函数（一次查询返回数据 + 总数）
                string windowSql = qb.BuildSql(rowLimit: 5, rowOffset: 5);
                var windowRows = conn.Query(windowSql, qb.Parameters.ToDynamicParameters()).ToList();
                int windowTotal = Convert.ToInt32(((IDictionary<string, object>)windowRows[0])[QueryBuilder.PagingTotalAlias]);

                // 模式二：两次查询（分页不带总数列 + 单独计数）
                string pageSql = qb.BuildSql(rowLimit: 5, rowOffset: 5, includeTotalCount: false);
                Assert.DoesNotContain(QueryBuilder.PagingTotalAlias, pageSql);
                var pageRows = conn.Query(pageSql, qb.Parameters.ToDynamicParameters()).ToList();
                int countTotal = conn.ExecuteScalar<int>(qb.BuildCountSql(), qb.Parameters.ToDynamicParameters());

                Assert.Equal(5, pageRows.Count);
                Assert.Equal(windowRows.Count, pageRows.Count);
                Assert.Equal(windowTotal, countTotal);
                Assert.Equal(15, countTotal);
                // 两种模式返回的记录内容一致
                for (int i = 0; i < pageRows.Count; i++)
                {
                    Assert.Equal(
                        ((IDictionary<string, object>)windowRows[i])["OrderNo"],
                        ((IDictionary<string, object>)pageRows[i])["OrderNo"]);
                }

                CleanAll(conn);
            });
        }

        [Fact]
        public void Union_Paging_ShouldExecuteAndReturnMergedRows()
        {
            EasySQLContext.Default.Do(conn =>
            {
                CleanAll(conn);
                var user = new User { UserName = "联合" };
                conn.Insert(user);
                for (int i = 1; i <= 6; i++)
                    conn.Insert(new Order { UserId = user.Id, OrderNo = $"UN_{i:D3}", TotalAmount = i * 100 });

                var s = new OrderTableDef("o");
                s.Select(true, s.OrderNo);

                // 两个分支：编号 <= UN_003 与编号 >= UN_004
                var qb1 = new QueryBuilder().From(s)
                    .Where($"{s.GetOrderNo()} <= 'UN_003'");
                var qb2 = new QueryBuilder().From(s)
                    .Where($"{s.GetOrderNo()} >= 'UN_004'");
                qb1.Union(qb2, isUnionAll: true);

                // 计数必须覆盖 UNION 的全部分支
                int total = conn.ExecuteScalar<int>(qb1.BuildCountSql(), qb1.Parameters.ToDynamicParameters());
                Assert.Equal(6, total);

                // 分页作用于合并后的结果集
                string pageSql = qb1.BuildSql(rowLimit: 4, rowOffset: 0, includeTotalCount: false);
                var rows = conn.Query(pageSql, qb1.Parameters.ToDynamicParameters()).ToList();
                Assert.Equal(4, rows.Count);

                // 带总数列的模式同样适用（总数是合并后的总行数）
                string windowSql = qb1.BuildSql(rowLimit: 4, rowOffset: 0);
                var windowRows = conn.Query(windowSql, qb1.Parameters.ToDynamicParameters()).ToList();
                Assert.Equal(4, windowRows.Count);
                Assert.Equal(6, Convert.ToInt32(((IDictionary<string, object>)windowRows[0])[QueryBuilder.PagingTotalAlias]));

                CleanAll(conn);
            });
        }

        // ================================================================
        // 参数化防注入 — EasySQL
        // ================================================================

        [Fact]
        public void Parameterized_ShouldPreventInjection()
        {
            EasySQLContext.Default.Do(conn =>
            {
                var s = new UserTableDef("u");
                s.Select(true, s.UserName);
                var qb = new QueryBuilder().From(s)
                    .Where($"{s.GetUserName()} = {s.AsParam("Name")}")
                    .AddParameter("Name", "test' OR '1'='1");

                Assert.Contains("@Name", qb.BuildSql());
                Assert.Empty(conn.Query(qb.BuildSql(), qb.Parameters.ToDynamicParameters()));
            });
        }

        // ================================================================
        // 安全机制 — EasySQL
        // ================================================================

        [Fact]
        public void Update_NoWhere_ShouldThrow()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new UpdateBuilder(new UserTableDef()).Set(new UserTableDef().GetUserName(), "x").BuildSql());
        }

        [Fact]
        public void Delete_NoWhere_ShouldThrow()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new DeleteBuilder(new UserTableDef()).BuildSql());
        }
    }
}
