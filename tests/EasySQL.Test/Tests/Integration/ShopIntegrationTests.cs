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
                var u = new UserSchema("u");
                var o = new OrderSchema("o");
                var p = new OrderPaymentSchema("p");


                u.Select(true, u.UserName);
                o.Select(true, o.OrderNo, o.TotalAmount);
                p.Select(true, p.Amount, p.Status);
                u.Join(o, $"{u.GetId(true)} = {o.GetUserId(true)}");
                o.Join(p, $"{o.GetId(true)} = {p.GetOrderId(true)}");

                var qb = new QueryBuilder().From(u, o, p)
                    .Where($"{o.GetOrderNo()} = 'ORD_FULL'");

                var rows = conn.Query(qb.BuildSql(), qb.Parameters.ToDynamicParameters()).ToList();
                Assert.Single(rows);

                // 复杂批量删除 → EasySQL（QueryBuilder 构造子查询 + DeleteBuilder 级联清理）
                var sp = new OrderPaymentSchema();
                var si = new OrderItemSchema();
                var so = new OrderSchema();
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

                var u = new UserSchema("u");
                var o = new OrderSchema("o");
                var i = new OrderItemSchema("i");
                u.Select(true, u.UserName);
                o.Select(true, o.OrderNo);
                i.Select(true, i.Quantity, i.UnitPrice);
                u.Join(o, $"{u.GetId(true)} = {o.GetUserId(true)}");
                o.Join(i, $"{o.GetId(true)} = {i.GetOrderId(true)}");

                var qb = new QueryBuilder().From(u, o, i)
                    .Where($"{o.GetOrderNo()} = 'ORD_JOIN'");

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

                var i = new OrderItemSchema("i");
                i.Select(true, i.ProductId);
                i.SelectExpression("SUM(Quantity) AS TotalQty");
                var qb = new QueryBuilder().From(i)
                    .GroupBy($"{i.GetProductId(true)}")
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
        public void Paging_OrderList_ShouldWork()
        {
            EasySQLContext.Default.Do(conn =>
            {
                CleanAll(conn);
                var user = new User { UserName = "分页" };
                conn.Insert(user);
                for (int i = 1; i <= 15; i++)
                    conn.Insert(new Order { UserId = user.Id, OrderNo = $"PAGE_{i:D3}", TotalAmount = i * 100 });

                var o = new OrderSchema("o");
                o.Select(true, o.OrderNo, o.TotalAmount);
                var qb = new QueryBuilder().From(o)
                    .Where($"{o.GetOrderNo()} LIKE 'PAGE_%'")
                    .OrderBy($"{o.GetOrderNo()} ASC");

                string sql = qb.BuildSql(rowLimit: 5, rowOffset: 5);
                Assert.Contains(QueryBuilder.PagingTotalAlias, sql);

                var rows = conn.Query(sql, qb.Parameters.ToDynamicParameters()).ToList();
                Assert.Equal(5, rows.Count);
                Assert.Equal(15, Convert.ToInt32(((IDictionary<string, object>)rows[0])[QueryBuilder.PagingTotalAlias]));

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
                var u = new UserSchema("u");
                u.Select(true, u.UserName);
                var qb = new QueryBuilder().From(u)
                    .Where($"{u.GetUserName()} = {u.AsParam("Name")}")
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
                new UpdateBuilder(new UserSchema()).Set(new UserSchema().GetUserName(), "x").BuildSql());
        }

        [Fact]
        public void Delete_NoWhere_ShouldThrow()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new DeleteBuilder(new UserSchema()).BuildSql());
        }
    }
}
