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
            DemoBuildIntoSql();
            DemoSnakeCase();

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

            var s = new DemoUserTableDef("u");
            s.Select(true, s.Name, s.Email);
            var qb = new QueryBuilder().From(s);
            Print("简单查询", qb.BuildSql());

            s.ClearSelect();
            s.Select();
            var qb2 = new QueryBuilder().From(s);
            Print("全字段 *", qb2.BuildSql());

            s.ClearSelect();
            s.Select(true, s.Status);
            var qb3 = new QueryBuilder().From(s);
            qb3.IsDistinct = true;
            Print("DISTINCT", qb3.BuildSql());
        }

        static void DemoJoins()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== JOIN 连接 ==========");

            var s = new DemoUserTableDef("u");
            var sb = new DemoOrderTableDef("o");
            s.Select(true, s.Name);
            sb.Select(true, sb.Amount);
            s.Join(sb, $"{s.GetId()} = {sb.GetUserId()}");

            var qb = new QueryBuilder().From(s);
            Print("INNER JOIN", qb.BuildSql());

            s.ClearJoins();
            s.LeftJoin(sb, $"{s.GetId()} = {sb.GetUserId()}");
            var qb2 = new QueryBuilder().From(s);
            Print("LEFT JOIN", qb2.BuildSql());
        }

        static void DemoWhereGroupOrder()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== WHERE / GROUP BY / ORDER BY ==========");

            var s = new DemoUserTableDef("u");
            s.Select(true, s.Name, s.Email);

            var qb = new QueryBuilder().From(s)
                .Where($"{s.GetStatus()} = 1", $"{s.GetEmail()} IS NOT NULL")
                .OrderBy($"{s.GetName()} ASC");
            Print("条件 + 排序", qb.BuildSql());

            s.ClearSelect();
            s.Select(true, s.Status);
            s.SelectExpression("Count(1) AS Cnt");
            var qb2 = new QueryBuilder().From(s)
                .GroupBy(s.GetStatus())
                .Having("Count(1) > 5");
            Print("分组 + Having", qb2.BuildSql());
        }

        static void DemoCount()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== COUNT 计数 ==========");

            var s = new DemoUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s)
                .Where($"{s.GetStatus()} = 1");
            Print("无分组 COUNT", qb.BuildCountSql());

            s.ClearSelect();
            s.Select(true, s.Status);
            s.SelectExpression("Count(1) AS Cnt");
            var qb2 = new QueryBuilder().From(s)
                .GroupBy(s.GetStatus());
            Print("有分组 COUNT（子查询包装）", qb2.BuildCountSql());
        }

        static void DemoPaging()
        {
            Console.WriteLine("========== 分页 ==========");

            var s = new DemoUserTableDef("u");
            s.Select(true, s.Name, s.Email);

            // SQL Server（含 COUNT OVER，一次查询返回数据+总数）
            SQLDialectFactory.UseSqlServerDialect();
            var qb1 = new QueryBuilder().From(s).OrderBy($"{s.GetName()} ASC");
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

            var user = new DemoUserTableDef();

            var insert = new InsertBuilder(user)
                .Insert(user.Id, user.Name, user.Email)
                .BuildSql("SELECT 1, 'test', 'test@test.com'");
            Print("INSERT", insert);

            var update = new UpdateBuilder(user)
                .Set(user.Name, user.AsParam("Name"))
                .Set(user.Status, 1)
                .Where($"{user.GetId()} = {user.AsParam("Id")}")
                .BuildSql();
            Print("UPDATE", update);

            var delete = new DeleteBuilder(user)
                .Where($"{user.GetId()} = {user.AsParam("Id")}")
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

            var s = new DemoUserTableDef("u");
            s.Select(true, s.Name);

            var qb1 = new QueryBuilder().From(s).Where($"{s.GetStatus()} = 1");

            var qb2 = new QueryBuilder().From(s).Where($"{s.GetStatus()} = 2");

            qb1.Union(qb2, isUnionAll: true);
            Print("UNION ALL", qb1.BuildSql());
        }

        static void DemoParameterized()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== 参数化查询 ==========");

            var s = new DemoUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s)
                .Where($"{s.GetId()} = {s.AsParam("UserId")}", $"{s.GetStatus()} = {s.AsParam("Status")}")
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

            var s = new DemoUserTableDef("u");
            var sb = new DemoOrderTableDef("o");
            s.Select(true, s.Name);

            var subQb = new QueryBuilder().From(sb)
                .Where($"{sb.GetUserId()} = {s.GetId()}");

            var qb = new QueryBuilder().From(s)
                .Exists(subQb);

            Print("EXISTS", qb.BuildSql());
        }

        static void DemoBuildIntoSql()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║  6 种数据库 BuildIntoSql（SELECT INTO / CTAS）    ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // 准备一个通用查询
            BuildAndPrintIntoSql("SQL Server", () => { SQLDialectFactory.UseSqlServerDialect(); return new SqlServerDialect(); });
            BuildAndPrintIntoSql("MySQL", () => { SQLDialectFactory.UseDialect("mysqlconnection"); return new MySQLDialect(); });
            BuildAndPrintIntoSql("PostgreSQL", () => { SQLDialectFactory.UseDialect("npgsqlconnection"); return new PostgreSQLDialect(); });
            BuildAndPrintIntoSql("Oracle", () => { SQLDialectFactory.UseDialect("oracleconnection"); return new OracleDialect(); });
            BuildAndPrintIntoSql("SQLite", () => { SQLDialectFactory.UseDialect("sqliteconnection"); return new SQLiteDialect(); });
            BuildAndPrintIntoSql("DB2", () => { SQLDialectFactory.UseDialect("db2connection"); return new DB2Dialect(); });

            Console.WriteLine();
        }

        static void BuildAndPrintIntoSql(string label, Func<ISQLDialect> dialectFactory)
        {
            var dialect = dialectFactory();
            var sa = new DemoUserTableDef("SA") { SQLDialect = dialect };
            var sb = new DemoOrderTableDef("SB") { SQLDialect = dialect };

            sa.Select(true, sa.Name, sa.Email);
            sb.Select(true, sb.Amount);

            sa.Join(sb, $"{sa.GetId()} = {sb.GetUserId()}");

            var qb = new QueryBuilder().From(sa, sb)
                .Where($"{sb.GetAmount()} > 100");

            var tmp = new TempTableDef("#tmp_result", "Name", "Email", "Amount") { SQLDialect = dialect };

            string sql = new InsertBuilder(tmp).BuildIntoSql(fromQuery: qb);
            Print($"BuildIntoSql [{label}]", sql);
        }

        static void DemoSnakeCase()
        {
            Console.WriteLine("========== 下划线字段自动别名（默认关闭）==========");

            var u = new DemoSnakeUserTableDef("u");

            // 默认关闭：无别名
            u.Select(true, u.UserName, u.Email, u.CreateTime);
            var qb = new QueryBuilder().From(u)
                .Where($"{u.GetStatus()} = {u.AsParam("Status")}")
                .OrderBy($"{u.GetCreateTime()} DESC");
            Print("AutoAlias=OFF（默认）", qb.BuildSql());

            // 开启自动别名
            u.ClearSelect();
            EasySQLContext.AutoAlias = true;
            u.Select(true, u.UserName, u.Email, u.CreateTime);
            var qb2 = new QueryBuilder().From(u)
                .Where($"{u.GetStatus()} = {u.AsParam("Status")}")
                .OrderBy($"{u.GetCreateTime()} DESC");
            Print("AutoAlias=ON", qb2.BuildSql());
            EasySQLContext.AutoAlias = false; // 恢复默认
        }

    }

    // 演示用的简短 TableDef 类
    class DemoUserTableDef : TableDefBase
    {
        public const string TABLE = "Users";
        public const string ID = "Id";
        public const string NAME = "Name";
        public const string EMAIL = "Email";
        public const string STATUS = "Status";

        public override string TableName => TABLE;
        public DemoUserTableDef(string? alias = null, ISQLDialect? dialect = null) : base(alias ?? string.Empty, dialect) { }

        // 实例属性 — 返回原始字段名（用于 Select 多字段选取）
        public string Id => ID;
        public string Name => NAME;
        public string Email => EMAIL;
        public string Status => STATUS;

        // GetXxx 方法 — 返回带引号/前缀修饰的字段名
        public string GetId(bool needPrefix = true) => QuoteField(ID, needPrefix);
        public string GetName(bool needPrefix = true) => QuoteField(NAME, needPrefix);
        public string GetEmail(bool needPrefix = true) => QuoteField(EMAIL, needPrefix);
        public string GetStatus(bool needPrefix = true) => QuoteField(STATUS, needPrefix);
    }

    class DemoOrderTableDef : TableDefBase
    {
        public const string TABLE = "Orders";
        public const string ID = "Id";
        public const string USER_ID = "UserId";
        public const string AMOUNT = "Amount";

        public override string TableName => TABLE;
        public DemoOrderTableDef(string? alias = null, ISQLDialect? dialect = null) : base(alias ?? string.Empty, dialect) { }

        // 实例属性 — 返回原始字段名（用于 Select 多字段选取）
        public string Id => ID;
        public string UserId => USER_ID;
        public string Amount => AMOUNT;

        // GetXxx 方法 — 返回带引号/前缀修饰的字段名
        public string GetId(bool needPrefix = true) => QuoteField(ID, needPrefix);
        public string GetUserId(bool needPrefix = true) => QuoteField(USER_ID, needPrefix);
        public string GetAmount(bool needPrefix = true) => QuoteField(AMOUNT, needPrefix);
    }

    /// <summary>
    /// 模拟 snake_case 数据库的 TableDef（列名含下划线）。
    /// </summary>
    class DemoSnakeUserTableDef : TableDefBase
    {
        public const string TABLE = "users";
        public const string ID = "id";
        public const string USER_NAME = "user_name";
        public const string EMAIL = "email";
        public const string CREATE_TIME = "create_time";
        public const string STATUS = "status";

        public override string TableName => TABLE;
        public DemoSnakeUserTableDef(string? alias = null, ISQLDialect? dialect = null)
            : base(alias ?? string.Empty, dialect) { }

        public string Id => ID;
        public string UserName => USER_NAME;
        public string Email => EMAIL;
        public string CreateTime => CREATE_TIME;
        public string Status => STATUS;

        public string GetId(bool p = true) => QuoteField(ID, p);
        public string GetUserName(bool p = true) => QuoteField(USER_NAME, p);
        public string GetEmail(bool p = true) => QuoteField(EMAIL, p);
        public string GetCreateTime(bool p = true) => QuoteField(CREATE_TIME, p);
        public string GetStatus(bool p = true) => QuoteField(STATUS, p);
    }
}
