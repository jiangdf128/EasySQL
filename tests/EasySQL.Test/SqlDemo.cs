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
            DemoEdgeCases();

            Console.WriteLine("=== 演示结束 ===");
        }

        static void Print(string title, string sql)
        {
            Console.WriteLine($"── {title} ──");
            Console.WriteLine(sql);
            Console.WriteLine();
        }

        static void PrintException(string title, Action action)
        {
            Console.WriteLine($"── {title} ──");
            try
            {
                action();
                Console.WriteLine("(未抛出异常)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"异常: {ex.GetType().Name} - {ex.Message}");
            }
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
            Print("有分组 COUNT（子查询包装，内层无 ORDER BY）", qb2.BuildCountSql());

            // DISTINCT 多列：计数口径必须与结果集一致 → 派生表包装
            var s2 = new DemoUserTableDef("u");
            s2.Select(true, s2.Name, s2.Email);
            var qb3 = new QueryBuilder().From(s2);
            qb3.IsDistinct = true;
            Print("DISTINCT 多列 COUNT（派生表包装）", qb3.BuildCountSql());

            // DISTINCT + 首列是表达式：旧实现会生成 Count(DISTINCT Count(1)) 这样的非法 SQL
            var s3 = new DemoUserTableDef("u");
            s3.SelectExpression("Count(1) AS Cnt");
            var qb4 = new QueryBuilder().From(s3);
            qb4.IsDistinct = true;
            Print("DISTINCT 表达式 COUNT", qb4.BuildCountSql());
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

            Console.WriteLine("────────── 分页的两种模式 ──────────");
            SQLDialectFactory.UseSqlServerDialect();

            // 模式一：一次查询（默认）—— 注入 COUNT(*) OVER()
            var qbA = new QueryBuilder().From(s).OrderBy($"{s.GetName()} ASC");
            Print("模式一：一次查询（含总数列）", qbA.BuildSql(rowLimit: 10, rowOffset: 5));

            // 模式二：两次查询 —— 分页不带窗口函数 + 单独计数
            var qbB = new QueryBuilder().From(s).OrderBy($"{s.GetName()} ASC");
            Print("模式二：分页（不含总数列）", qbB.BuildSql(rowLimit: 10, rowOffset: 5, includeTotalCount: false));
            Print("模式二：单独计数", qbB.BuildCountSql());
            Console.WriteLine("  两条语句共用同一组参数: " + string.Join(", ", qbB.Parameters.Keys));
            Console.WriteLine();

            // 模式二的参数化写法：WHERE 完全一致，只是执行两条语句
            var qbP = new QueryBuilder().From(s)
                .Where($"{s.GetStatus()} = {s.AsParam("Status")}")
                .AddParameter("Status", 1)
                .OrderBy($"{s.GetName()} ASC");
            Print("模式二：参数化分页", qbP.BuildSql(rowLimit: 10, rowOffset: 20, includeTotalCount: false));
            Print("模式二：参数化计数", qbP.BuildCountSql());
            Console.WriteLine("  参数: " + string.Join(", ", qbP.Parameters.Select(kv => kv.Key + "=" + kv.Value)));
            Console.WriteLine();

            Console.WriteLine("────────── 分页与查询结构的组合 ──────────");

            // 分页 + GROUP BY：总记录数是分组数，不是明细行数
            var g1 = new DemoUserTableDef("u");
            g1.Select(true, g1.Status);
            g1.SelectExpression("Count(1) AS Cnt");
            var qbG = new QueryBuilder().From(g1)
                .GroupBy(g1.GetStatus())
                .OrderBy($"{g1.GetStatus()} ASC");
            Print("分页 + GROUP BY（总数 = 分组数）", qbG.BuildSql(rowLimit: 10));
            Print("分页 + GROUP BY 的计数语句", qbG.BuildCountSql());

            // 分页 + DISTINCT：总记录数是去重后的行数
            var d1 = new DemoUserTableDef("u");
            d1.Select(true, d1.Name, d1.Email);
            var qbD = new QueryBuilder().From(d1);
            qbD.IsDistinct = true;
            Print("分页 + DISTINCT（总数 = 去重行数）", qbD.BuildSql(rowLimit: 10));
            Print("分页 + DISTINCT 的计数语句", qbD.BuildCountSql());

            // 分页 + JOIN
            var j1 = new DemoUserTableDef("u");
            var jo = new DemoOrderTableDef("o");
            j1.Select(true, j1.Name);
            jo.Select(true, jo.Amount);
            j1.Join(jo, $"{j1.GetId()} = {jo.GetUserId()}");
            var qbJ = new QueryBuilder().From(j1, jo)
                .OrderBy($"{j1.GetName()} ASC");
            Print("分页 + JOIN", qbJ.BuildSql(rowLimit: 10, includeTotalCount: false));

            // 分页 + EXISTS 子查询
            var e1 = new DemoUserTableDef("u");
            e1.Select(true, e1.Name);
            var eo = new DemoOrderTableDef("o");
            var eSub = new QueryBuilder().From(eo).Where($"{eo.GetUserId()} = {e1.GetId()}");
            var qbE = new QueryBuilder().From(e1)
                .Exists(eSub)
                .OrderBy($"{e1.GetName()} ASC");
            Print("分页 + EXISTS", qbE.BuildSql(rowLimit: 10, includeTotalCount: false));

            // 分页 + FROM 子查询
            var q1 = new DemoUserTableDef("u");
            q1.Select(true, q1.Name);
            var subQb = new QueryBuilder("sub")
                .From(q1)
                .Where($"{q1.GetStatus()} = 1");
            var qbSub = new QueryBuilder().From(subQb);
            Print("分页 + FROM 子查询", qbSub.BuildSql(rowLimit: 10, includeTotalCount: false));

            // 分页 + HAVING
            var h1 = new DemoUserTableDef("u");
            h1.Select(true, h1.Status);
            h1.SelectExpression("Count(1) AS Cnt");
            var qbH = new QueryBuilder().From(h1)
                .GroupBy(h1.GetStatus())
                .Having("Count(1) > 5")
                .OrderBy($"{h1.GetStatus()} ASC");
            Print("分页 + HAVING", qbH.BuildSql(rowLimit: 10, includeTotalCount: false));

            // 分页 + 多列排序
            var qbMs = new QueryBuilder().From(s)
                .OrderBy($"{s.GetName()} ASC", $"{s.GetEmail()} DESC");
            Print("分页 + 多列排序", qbMs.BuildSql(rowLimit: 10, includeTotalCount: false));

            // 分页 + 多个 WHERE 条件（参数化）
            var qbW = new QueryBuilder().From(s)
                .Where($"{s.GetStatus()} = {s.AsParam("Status")}",
                       $"{s.GetName()} LIKE {s.AsParam("Name")}")
                .AddParameter("Status", 1)
                .AddParameter("Name", "%张%")
                .OrderBy($"{s.GetName()} ASC");
            Print("分页 + 多个 WHERE 条件", qbW.BuildSql(rowLimit: 10, includeTotalCount: false));

            // 分页 + snake_case 自动别名（总数列不能与自动别名冲突）
            var sn = new DemoSnakeUserTableDef("u");
            sn.Select(true, sn.UserName, sn.CreateTime);
            EasySQLContext.AutoAlias = true;
            var qbSn = new QueryBuilder().From(sn)
                .OrderBy($"{sn.GetCreateTime()} DESC");
            Print("分页 + snake_case 自动别名", qbSn.BuildSql(rowLimit: 10));
            EasySQLContext.AutoAlias = false;

            Console.WriteLine("────────── 分页的边界情况 ──────────");

            // 没有 ORDER BY：SQL Server / Oracle / DB2 的 OFFSET/FETCH 必须配 ORDER BY，缺省补 ORDER BY 1
            var qbC = new QueryBuilder().From(s);
            Print("无 ORDER BY 分页（补 ORDER BY 1）", qbC.BuildSql(rowLimit: 10, includeTotalCount: false));

            // WHERE 里含 "ORDER BY " 字面量：不能再用字符串匹配判断有无 ORDER BY
            var qbD2 = new QueryBuilder().From(s)
                .Where($"{s.GetName()} = 'ORDER BY x'");
            Print("WHERE 含 'ORDER BY ' 字面量（仍补 ORDER BY 1）", qbD2.BuildSql(rowLimit: 10, includeTotalCount: false));

            // 没有指定查询列：补默认列，避免生成 "SELECT FROM ..." 这样的非法语句
            var s2 = new DemoUserTableDef("u");
            var qbE2 = new QueryBuilder().From(s2);
            Print("无查询列（补 u.*）", qbE2.BuildSql(rowLimit: 10, includeTotalCount: false));

            // rowLimit = 0：不分页，也不注入总数列
            var qbZ = new QueryBuilder().From(s);
            Print("rowLimit=0（不分页、无总数列）", qbZ.BuildSql(rowLimit: 0));

            // 页码越界：窗口函数模式返回 0 行，取不到总数；两次查询模式不受影响
            Console.WriteLine("── 页码越界（offset 超出总行数）──");
            Console.WriteLine("  模式一：结果为空 → 读不到 TotalRows");
            Console.WriteLine("  模式二：分页仍返回 0 行，计数语句照常返回总数");
            Console.WriteLine();

            // 单行输出（PrettyPrint = false）
            var qbF = new QueryBuilder().From(s).OrderBy($"{s.GetName()} ASC");
            qbF.PrettyPrint = false;
            Print("PrettyPrint=false", qbF.BuildSql(rowLimit: 10, rowOffset: 5, includeTotalCount: false));
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

            Console.WriteLine("---------- UNION 的分页与计数 ----------");

            // 分页：作用于合并后的结果集（排序表达式会映射为结果集的输出列名）
            var s1 = new DemoUserTableDef("u");
            s1.Select(true, s1.Name, s1.Email);
            var s2 = new DemoUserTableDef("u2");
            s2.Select(true, s2.Name, s2.Email);

            var p1 = new QueryBuilder().From(s1).OrderBy($"{s1.GetName()} ASC");
            var p2 = new QueryBuilder().From(s2);
            p1.Union(p2, isUnionAll: true);
            Print("UNION 分页", p1.BuildSql(rowLimit: 10, rowOffset: 5, includeTotalCount: false));

            // 计数：覆盖全部分支
            var c1 = new QueryBuilder().From(s1);
            var c2 = new QueryBuilder().From(s2);
            c1.Union(c2, isUnionAll: true);
            Print("UNION 计数", c1.BuildCountSql());
        }

        static void DemoEdgeCases()
        {
            SQLDialectFactory.UseSqlServerDialect();
            Console.WriteLine("========== 边界情况与错误提示 ==========");

            // UNION 的排序字段不在 SELECT 列中 → 无法映射到结果集输出列
            var s1 = new DemoUserTableDef("u");
            s1.Select(true, s1.Name);
            var s2 = new DemoUserTableDef("u2");
            s2.Select(true, s2.Name);
            var qb1 = new QueryBuilder().From(s1).OrderBy($"{s1.GetId()} ASC");
            var qb2 = new QueryBuilder().From(s2);
            qb1.Union(qb2, isUnionAll: true);
            PrintException("UNION 排序字段不在 SELECT 列中", () => qb1.BuildSql(rowLimit: 10, includeTotalCount: false));

            // 只给 offset 不给 limit：分页子句不会生成（各库 LIMIT 语义不同，暂不支持）
            var s5 = new DemoUserTableDef("u");
            s5.Select(true, s5.Name);
            var qb7 = new QueryBuilder().From(s5);
            Print("仅 offset（rowLimit=0）：不分页", qb7.BuildSql(rowLimit: 0, rowOffset: 100));
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
