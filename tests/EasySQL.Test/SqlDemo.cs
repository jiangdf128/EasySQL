using EasySQL;

namespace EasySQL.Test
{
    /// <summary>
    /// SQL 生成演示程序，直接运行可在控制台查看所有生成的 SQL 语句。
    /// 运行命令：dotnet run --project tests/EasySQL.Test
    /// </summary>
    public static class SqlDemo
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║         EasySQL 生成语句演示                       ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();

            DemoBasicSelect();
            DemoJoins();
            DemoWhereGroupOrder();
            DemoCount();
            DemoPaging();
            DemoInsertUpdateDelete();
            DemoUnion();
            DemoParameterized();
            DemoInBuilder();
            DemoExists();

            Console.WriteLine("=== 演示结束 ===");
        }

        static void Print(string title, string sql)
        {
            Console.WriteLine($"── {title} ──");
            Console.WriteLine(sql);
            Console.WriteLine();
        }

        static void DemoBasicSelect()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== 基本 SELECT ==========");

            var s = new DemoUserSchema("u");
            s.Select(s.GetName());
            s.Select(s.GetEmail());
            var qb = new QueryBuilder().From(s);
            Print("简单查询", qb.BuildSql());

            s.ClearSelect();
            s.Select();
            var qb2 = new QueryBuilder().From(s);
            Print("全字段 *", qb2.BuildSql());

            s.ClearSelect();
            s.Select(s.GetStatus());
            var qb3 = new QueryBuilder().From(s);
            qb3.IsDistinct = true;
            Print("DISTINCT", qb3.BuildSql());
        }

        static void DemoJoins()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== JOIN 连接 ==========");

            var s = new DemoUserSchema("u");
            var sb = new DemoOrderSchema("o");
            s.Select(s.GetName());
            sb.Select(sb.GetAmount());
            s.Join(sb, "u.Id = o.UserId");

            var qb = new QueryBuilder().From(s);
            Print("INNER JOIN", qb.BuildSql());

            s.ClearJoins();
            s.LeftJoin(sb, "u.Id = o.UserId");
            var qb2 = new QueryBuilder().From(s);
            Print("LEFT JOIN", qb2.BuildSql());
        }

        static void DemoWhereGroupOrder()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== WHERE / GROUP BY / ORDER BY ==========");

            var s = new DemoUserSchema("u");
            s.Select(s.GetName());
            s.Select(s.GetEmail());

            var qb = new QueryBuilder().From(s)
                .Where("u.Status = 1", "u.Email IS NOT NULL")
                .OrderBy("u.Name ASC");
            Print("条件 + 排序", qb.BuildSql());

            s.ClearSelect();
            s.Select(s.GetStatus());
            s.SelectExpression("Count(1) AS Cnt");
            var qb2 = new QueryBuilder().From(s)
                .GroupBy("u.Status")
                .Having("Count(1) > 5");
            Print("分组 + Having", qb2.BuildSql());
        }

        static void DemoCount()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== COUNT 计数 ==========");

            var s = new DemoUserSchema("u");
            s.Select(s.GetName());

            var qb = new QueryBuilder().From(s)
                .Where("u.Status = 1");
            Print("无分组 COUNT", qb.BuildCountSql());

            s.ClearSelect();
            s.Select(s.GetStatus());
            s.SelectExpression("Count(1) AS Cnt");
            var qb2 = new QueryBuilder().From(s)
                .GroupBy("u.Status");
            Print("有分组 COUNT（子查询包装）", qb2.BuildCountSql());
        }

        static void DemoPaging()
        {
            Console.WriteLine("========== 分页 ==========");

            var s = new DemoUserSchema("u");
            s.Select(s.GetName());
            s.Select(s.GetEmail());

            // SQL Server（含 COUNT OVER，一次查询返回数据+总数）
            SQLDialectFactory.UseSqlServerDialect();
            var qb1 = new QueryBuilder().From(s).OrderBy("u.Name ASC");
            Print($"SQL Server (含 {QueryBuilder.PagingTotalAlias} 计数列)", qb1.BuildSql(rowLimit: 10, rowOffset: 5));

            // MySQL
            SQLDialectFactory.UseDialect("mysqlconnection");
            var qb2 = new QueryBuilder().From(s);
            Print("MySQL (LIMIT/OFFSET)", qb2.BuildSql(rowLimit: 10, rowOffset: 5));

            // PostgreSQL
            SQLDialectFactory.UseDialect("npgsqlconnection");
            var qb3 = new QueryBuilder().From(s);
            Print("PostgreSQL (LIMIT/OFFSET)", qb3.BuildSql(rowLimit: 20, rowOffset: 0));

            // Oracle
            SQLDialectFactory.UseDialect("oracleconnection");
            var qb4 = new QueryBuilder().From(s);
            Print("Oracle (OFFSET/FETCH)", qb4.BuildSql(rowLimit: 10, rowOffset: 5));

            // SQLite
            SQLDialectFactory.UseDialect("sqliteconnection");
            var qb5 = new QueryBuilder().From(s);
            Print("SQLite (LIMIT/OFFSET)", qb5.BuildSql(rowLimit: 15, rowOffset: 0));

            // DB2
            SQLDialectFactory.UseDialect("db2connection");
            var qb6 = new QueryBuilder().From(s);
            Print("DB2 (OFFSET/FETCH)", qb6.BuildSql(rowLimit: 10, rowOffset: 5));
        }

        static void DemoInsertUpdateDelete()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== INSERT / UPDATE / DELETE ==========");

            var user = new DemoUserSchema();

            var insert = new InsertBuilder(user)
                .Insert("Id", "Name", "Email")
                .BuildSql("SELECT 1, 'test', 'test@test.com'");
            Print("INSERT", insert);

            var update = new UpdateBuilder(user)
                .Set("Name", "@Name")
                .Set("Status", 1)
                .Where("Id = @Id")
                .BuildSql();
            Print("UPDATE", update);

            var delete = new DeleteBuilder(user)
                .Where("Id = @Id")
                .BuildSql();
            Print("DELETE", delete);

            try
            {
                new UpdateBuilder(user).Set("Status", 0).BuildSql();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"── UPDATE 无条件（安全拦截）──");
                Console.WriteLine($"异常: {ex.Message}");
                Console.WriteLine();
            }
        }

        static void DemoUnion()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== UNION ==========");

            var s = new DemoUserSchema("u");
            s.Select(s.GetName());

            var qb1 = new QueryBuilder().From(s).Where("u.Status = 1");

            var qb2 = new QueryBuilder().From(s).Where("u.Status = 2");

            qb1.Union(qb2, isUnionAll: true);
            Print("UNION ALL", qb1.BuildSql());
        }

        static void DemoParameterized()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== 参数化查询 ==========");

            var s = new DemoUserSchema("u");
            s.Select(s.GetName());

            var qb = new QueryBuilder().From(s)
                .Where("u.Id = @UserId", "u.Status = @Status")
                .AddParameter("UserId", 123)
                .AddParameter("Status", 1);

            Print("参数化 SELECT", qb.BuildSql());
            Console.WriteLine($"  参数: UserId={qb.Parameters["UserId"]}, Status={qb.Parameters["Status"]}");
            Console.WriteLine();
        }

        static void DemoInBuilder()
        {
            Console.WriteLine("========== IN 子句构造器 ==========");

            var intList = new[] { 1, 2, 3, 4, 5 };
            var builder = intList.InBuilder(sectionCount: 3);
            Console.Write("  int [1-5] 分 3 个/组 → ");
            while (builder.HasNext)
                Console.Write(builder.GetNextInValues() + " ");
            Console.WriteLine();

            var strList = new[] { "Alice", "Bob", "Charlie" };
            var builder2 = strList.InBuilder(sectionCount: 2);
            Console.Write("  string 分 2 个/组 → ");
            while (builder2.HasNext)
                Console.Write(builder2.GetNextInValues() + " ");
            Console.WriteLine();
            Console.WriteLine();
        }

        static void DemoExists()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== EXISTS 子查询 ==========");

            var s = new DemoUserSchema("u");
            var sb = new DemoOrderSchema("o");
            s.Select(s.GetName());

            var subQb = new QueryBuilder().From(sb)
                .Where("o.UserId = u.Id");

            var qb = new QueryBuilder().From(s)
                .Exists(subQb);

            Print("EXISTS", qb.BuildSql());
        }
    }

    // 演示用的简短 Schema 类
    class DemoUserSchema : SchemaBase
    {
        public const string TABLE = "Users";
        public override string TableName => TABLE;
        public DemoUserSchema(string? alias = null, ISQLDialect? dialect = null) : base(alias ?? string.Empty, dialect) { }
        public string GetId(bool p = true) => QuoteField("Id", p);
        public string GetName(bool p = true) => QuoteField("Name", p);
        public string GetEmail(bool p = true) => QuoteField("Email", p);
        public string GetStatus(bool p = true) => QuoteField("Status", p);
    }

    class DemoOrderSchema : SchemaBase
    {
        public const string TABLE = "Orders";
        public override string TableName => TABLE;
        public DemoOrderSchema(string? alias = null, ISQLDialect? dialect = null) : base(alias ?? string.Empty, dialect) { }
        public string GetId(bool p = true) => QuoteField("Id", p);
        public string GetUserId(bool p = true) => QuoteField("UserId", p);
        public string GetAmount(bool p = true) => QuoteField("Amount", p);
    }
}
