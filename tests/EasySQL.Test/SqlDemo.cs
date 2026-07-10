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

            var su = new DemoUserSchema("u");
            su.Select(su.GetName());
            su.Select(su.GetEmail());
            var qb = new QueryBuilder().From(su);
            qb.PrettyPrint = false;
            Print("简单查询", qb.BuildSql());

            su.ClearSelect();
            su.Select();
            var qb2 = new QueryBuilder().From(su);
            qb2.PrettyPrint = false;
            Print("全字段 *", qb2.BuildSql());

            su.ClearSelect();
            su.Select(su.GetStatus());
            var qb3 = new QueryBuilder().From(su);
            qb3.IsDistinct = true;
            qb3.PrettyPrint = false;
            Print("DISTINCT", qb3.BuildSql());
        }

        static void DemoJoins()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== JOIN 连接 ==========");

            var su = new DemoUserSchema("u");
            var so = new DemoOrderSchema("o");
            su.Select(su.GetName());
            so.Select(so.GetAmount());
            su.Join(so, "u.Id = o.UserId");

            var qb = new QueryBuilder().From(su);
            qb.PrettyPrint = false;
            Print("INNER JOIN", qb.BuildSql());

            su.ClearJoins();
            su.LeftJoin(so, "u.Id = o.UserId");
            var qb2 = new QueryBuilder().From(su);
            qb2.PrettyPrint = false;
            Print("LEFT JOIN", qb2.BuildSql());
        }

        static void DemoWhereGroupOrder()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== WHERE / GROUP BY / ORDER BY ==========");

            var su = new DemoUserSchema("u");
            su.Select(su.GetName());
            su.Select(su.GetEmail());

            var qb = new QueryBuilder().From(su)
                .Where("u.Status = 1", "u.Email IS NOT NULL")
                .OrderBy("u.Name ASC");
            qb.PrettyPrint = false;
            Print("条件 + 排序", qb.BuildSql());

            su.ClearSelect();
            su.Select(su.GetStatus());
            su.SelectExpression("Count(1) AS Cnt");
            var qb2 = new QueryBuilder().From(su)
                .GroupBy("u.Status")
                .Having("Count(1) > 5");
            qb2.PrettyPrint = false;
            Print("分组 + Having", qb2.BuildSql());
        }

        static void DemoCount()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== COUNT 计数 ==========");

            var su = new DemoUserSchema("u");
            su.Select(su.GetName());

            var qb = new QueryBuilder().From(su)
                .Where("u.Status = 1");
            qb.PrettyPrint = false;
            Print("无分组 COUNT", qb.BuildCountSql());

            su.ClearSelect();
            su.Select(su.GetStatus());
            su.SelectExpression("Count(1) AS Cnt");
            var qb2 = new QueryBuilder().From(su)
                .GroupBy("u.Status");
            qb2.PrettyPrint = false;
            Print("有分组 COUNT（子查询包装）", qb2.BuildCountSql());
        }

        static void DemoPaging()
        {
            Console.WriteLine("========== 分页 ==========");

            var su = new DemoUserSchema("u");
            su.Select(su.GetName());
            su.Select(su.GetEmail());

            // SQL Server
            SQLDialectFactory.UseSqlServerDialect();
            var qb1 = new QueryBuilder().From(su).OrderBy("u.Name ASC");
            qb1.PrettyPrint = false;
            Print("SQL Server (OFFSET/FETCH)", qb1.BuildSql(rowLimit: 10, rowOffset: 5));

            // MySQL
            SQLDialectFactory.UseDialect("mysqlconnection");
            var qb2 = new QueryBuilder().From(su);
            qb2.PrettyPrint = false;
            Print("MySQL (LIMIT/OFFSET)", qb2.BuildSql(rowLimit: 10, rowOffset: 5));

            // PostgreSQL
            SQLDialectFactory.UseDialect("npgsqlconnection");
            var qb3 = new QueryBuilder().From(su);
            qb3.PrettyPrint = false;
            Print("PostgreSQL (LIMIT/OFFSET)", qb3.BuildSql(rowLimit: 20, rowOffset: 0));

            // Oracle
            SQLDialectFactory.UseDialect("oracleconnection");
            var qb4 = new QueryBuilder().From(su);
            qb4.PrettyPrint = false;
            Print("Oracle (OFFSET/FETCH)", qb4.BuildSql(rowLimit: 10, rowOffset: 5));

            // SQLite
            SQLDialectFactory.UseDialect("sqliteconnection");
            var qb5 = new QueryBuilder().From(su);
            qb5.PrettyPrint = false;
            Print("SQLite (LIMIT/OFFSET)", qb5.BuildSql(rowLimit: 15, rowOffset: 0));

            // Jet (MS Access)
            SQLDialectFactory.UseDialect("oledbconnection");
            var qb6 = new QueryBuilder().From(su);
            qb6.PrettyPrint = false;
            Print("Jet / MS Access (TOP)", qb6.BuildSql(rowLimit: 10, rowOffset: 0));

            // DB2
            SQLDialectFactory.UseDialect("db2connection");
            var qb7 = new QueryBuilder().From(su);
            qb7.PrettyPrint = false;
            Print("DB2 (FETCH FIRST)", qb7.BuildSql(rowLimit: 10, rowOffset: 0));
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

            var su = new DemoUserSchema("u");
            su.Select(su.GetName());

            var qb1 = new QueryBuilder().From(su).Where("u.Status = 1");
            qb1.PrettyPrint = false;

            var qb2 = new QueryBuilder().From(su).Where("u.Status = 2");
            qb2.PrettyPrint = false;

            qb1.Union(qb2, isUnionAll: true);
            Print("UNION ALL", qb1.BuildSql());
        }

        static void DemoParameterized()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== 参数化查询 ==========");

            var su = new DemoUserSchema("u");
            su.Select(su.GetName());

            var qb = new QueryBuilder().From(su)
                .Where("u.Id = @UserId", "u.Status = @Status")
                .AddParameter("UserId", 123)
                .AddParameter("Status", 1);
            qb.PrettyPrint = false;

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

            var su = new DemoUserSchema("u");
            var so = new DemoOrderSchema("o");
            su.Select(su.GetName());

            var subQb = new QueryBuilder().From(so)
                .Where("o.UserId = u.Id");

            var qb = new QueryBuilder().From(su)
                .Exists(subQb);
            qb.PrettyPrint = false;

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
