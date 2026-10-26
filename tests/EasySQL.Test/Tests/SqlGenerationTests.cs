using EasySQL;
using System.Drawing;
using Xunit;

namespace EasySQL.Test
{
    /// <summary>
    /// SQL 生成单元测试 — 验证各种场景下生成的 SQL 语句是否正确。
    /// 所有测试均为纯字符串断言，不依赖数据库连接。
    /// </summary>
    public class SqlGenerationTests
    {
        private static void LogSql(string sql, string? label= null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"=== {label ?? "SQL"} ===");
            Console.WriteLine(sql);
            Console.WriteLine();
            Console.ResetColor();
        }

        // ---- 测试用轻量 TableDef（避免与 TableDefs/ 下的真实 TableDef 冲突） ----
        class TestUserTableDef : TableDefBase
        {
            public const string TABLE = "Users";
            public const string ID = "Id";
            public const string NAME = "Name";
            public const string EMAIL = "Email";
            public const string STATUS = "Status";

            public override string TableName => TABLE;
            public TestUserTableDef(string? alias = null, ISQLDialect? dialect = null) : base(alias ?? string.Empty, dialect) { }

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

        class TestOrderTableDef : TableDefBase
        {
            public const string TABLE = "Orders";
            public const string ID = "Id";
            public const string USER_ID = "UserId";
            public const string AMOUNT = "Amount";
            public const string CREATE_TIME = "CreateTime";

            public override string TableName => TABLE;
            public TestOrderTableDef(string? alias = null, ISQLDialect? dialect = null) : base(alias ?? string.Empty, dialect) { }

            // 实例属性 — 返回原始字段名（用于 Select 多字段选取）
            public string Id => ID;
            public string UserId => USER_ID;
            public string Amount => AMOUNT;
            public string CreateTime => CREATE_TIME;

            // GetXxx 方法 — 返回带引号/前缀修饰的字段名
            public string GetId(bool needPrefix = true) => QuoteField(ID, needPrefix);
            public string GetUserId(bool needPrefix = true) => QuoteField(USER_ID, needPrefix);
            public string GetAmount(bool needPrefix = true) => QuoteField(AMOUNT, needPrefix);
            public string GetCreateTime(bool needPrefix = true) => QuoteField(CREATE_TIME, needPrefix);
        }

        // ================================================================
        // 基本 SELECT 测试
        // ================================================================
        [Fact]
        public void BuildSimpleSelect_ShouldGenerateCorrectSQL()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);
            s.Select(true, s.Email);

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildSql();
            LogSql(sql);
            Assert.Equal($"SELECT u.Name,u.Email{System.Environment.NewLine}FROM Users u", sql);
        }

        [Fact]
        public void SelectAllFields_ShouldGenerateStar()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select();

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildSql();
            LogSql(sql);
            Assert.Equal($"SELECT u.*{System.Environment.NewLine}FROM Users u", sql);
        }

        [Fact]
        public void SelectDistinct_ShouldGenerateDistinctKeyword()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Status);

            var qb = new QueryBuilder().From(s);
            qb.IsDistinct = true;

            var sql = qb.BuildSql();
            LogSql(sql);
            Assert.Equal($"SELECT DISTINCT u.Status{System.Environment.NewLine}FROM Users u", sql);
        }

        // ================================================================
        // WHERE / GROUP BY / HAVING / ORDER BY 测试
        // ================================================================
        [Fact]
        public void WhereClause_ShouldGenerateCorrectCondition()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s)
                .Where($"{s.GetStatus()} = 1", $"{s.GetEmail()} IS NOT NULL");

            var sql = qb.BuildSql();
            LogSql(sql);
            Assert.Contains("WHERE (u.Status = 1 AND u.Email IS NOT NULL)", sql);
        }

        [Fact]
        public void GroupByAndHaving_ShouldGenerateCorrectClauses()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Status);
            s.SelectExpression("Count(1) AS Cnt");

            var qb = new QueryBuilder().From(s)
                .GroupBy(s.GetStatus())
                .Having("Count(1) > 5");

            var sql = qb.BuildSql();
            LogSql(sql);
            Assert.Contains("GROUP BY u.Status", sql);
            Assert.Contains("HAVING Count(1) > 5", sql);
        }

        [Fact]
        public void OrderBy_ShouldGenerateCorrectSorting()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name, s.Email);

            var qb = new QueryBuilder().From(s)
                .OrderBy($"{s.GetName()} ASC", $"{s.GetEmail()} DESC");

            var sql = qb.BuildSql();
            LogSql(sql);
            Assert.Contains("ORDER BY u.Name ASC,u.Email DESC", sql);
        }

        // ================================================================
        // JOIN 测试
        // ================================================================
        [Fact]
        public void InnerJoin_ShouldGenerateCorrectJoinClause()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            var sb = new TestOrderTableDef("o");
            s.Select(true, s.Name);
            sb.Select(true, sb.Amount);
            s.Join(sb, $"{s.GetId()} = {sb.GetUserId()}");

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildSql();
            LogSql(sql);
            Assert.Contains("INNER JOIN Orders o on u.Id = o.UserId", sql);
        }

        [Fact]
        public void LeftJoin_ShouldGenerateLeftOuterJoin()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            var sb = new TestOrderTableDef("o");
            s.Select(true, s.Name);
            sb.Select(true, sb.Amount);
            s.LeftJoin(sb, $"{s.GetId()}={sb.GetUserId()}");

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildSql();
            LogSql(sql);
            Assert.Contains("LEFT OUTER JOIN Orders o on u.Id=o.UserId", sql);
        }

        // ================================================================
        // COUNT 测试
        // ================================================================
        [Fact]
        public void BuildCountSql_NoGroupBy_ShouldGenerateSimpleCount()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s)
                .Where($"{s.GetStatus()} = 1");

            var sql = qb.BuildCountSql();
            LogSql(sql);
            Assert.Contains("Count(1) AS RowsCount", sql);
            Assert.DoesNotContain(s.GetName(false), sql); // SELECT 字段被替换为计数
        }

        [Fact]
        public void BuildCountSql_WithGroupBy_ShouldGenerateWrappedCount()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Status);
            s.SelectExpression("Count(1) AS Cnt");

            var qb = new QueryBuilder().From(s)
                .GroupBy(s.GetStatus());

            var sql = qb.BuildCountSql();
            LogSql(sql);
            Assert.Contains("Count(1) AS RowsCount", sql);
        }

        // ================================================================
        // 分页测试 — SQL Server
        // ================================================================
        [Fact]
        public void Paging_SqlServer_WithOrderBy_ShouldUseOffsetFetch()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name, s.Email);

            var qb = new QueryBuilder().From(s)
                .OrderBy($"{s.GetName()} ASC");

            var sql = qb.BuildSql(rowLimit: 10, rowOffset: 5);
            LogSql(sql);
            Assert.Contains("OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY", sql);
        }

        [Fact]
        public void Paging_SqlServer_NoOrderBy_ShouldAddDummyOrderBy()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s);
            qb.OrderBy($" { s.GetEmail() } Asc");

            var sql = qb.BuildSql(rowLimit: 10, rowOffset: 0);
            LogSql(sql);
            Assert.Contains("ORDER BY u.Email Asc", sql);
            Assert.Contains("OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY", sql);
        }

        // ================================================================
        // 分页测试 — MySQL / PostgreSQL / SQLite（默认 LIMIT/OFFSET）
        // ================================================================
        [Fact]
        public void Paging_MySQL_ShouldUseLimitOffset()
        {
            SQLDialectFactory.UseDialect("mysqlconnection");
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildSql(rowLimit: 10, rowOffset: 5);
            LogSql(sql);
            Assert.Contains("LIMIT 10", sql);
            Assert.Contains("OFFSET 5", sql);
        }

        [Fact]
        public void Paging_PostgreSQL_ShouldUseLimitOffset()
        {
            SQLDialectFactory.UseDialect("npgsqlconnection");
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildSql(rowLimit: 20, rowOffset: 10);
            LogSql(sql);
            Assert.Contains("LIMIT 20", sql);
            Assert.Contains("OFFSET 10", sql);
        }

        // ================================================================
        // 分页测试 — Oracle
        // ================================================================
        [Fact]
        public void Paging_Oracle_ShouldUseRownumWrapper()
        {
            SQLDialectFactory.UseDialect("oracleconnection");
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildSql(rowLimit: 10, rowOffset: 5);
            LogSql(sql);
            Assert.Contains("OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY", sql);
        }

        // ================================================================
        // 分页总数列开关（includeTotalCount）
        // ================================================================
        [Fact]
        public void Paging_Default_ShouldInjectTotalRowsWindowFunction()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildSql(rowLimit: 10, rowOffset: 5);
            LogSql(sql);
            Assert.Contains($"COUNT(*) OVER() AS {QueryBuilder.PagingTotalAlias}", sql);
        }

        [Fact]
        public void Paging_IncludeTotalCountFalse_ShouldNotInjectWindowFunction()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildSql(rowLimit: 10, rowOffset: 5, includeTotalCount: false);
            LogSql(sql);
            Assert.DoesNotContain("COUNT(*) OVER()", sql);
            Assert.DoesNotContain(QueryBuilder.PagingTotalAlias, sql);
            // 分页本身仍然生效
            Assert.Contains("OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY", sql);
        }

        [Theory]
        [InlineData("sqlconnection", "OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY")]
        [InlineData("oracleconnection", "OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY")]
        [InlineData("db2connection", "OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY")]
        [InlineData("mysqlconnection", "LIMIT 10")]
        [InlineData("npgsqlconnection", "LIMIT 10")]
        [InlineData("sqliteconnection", "LIMIT 10")]
        public void Paging_IncludeTotalCountFalse_AllDialects_ShouldUseDialectPagingSyntax(string dialect, string expected)
        {
            SQLDialectFactory.UseDialect(dialect);
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildSql(rowLimit: 10, rowOffset: 5, includeTotalCount: false);
            LogSql(sql, dialect);
            Assert.Contains(expected, sql);
            Assert.DoesNotContain("COUNT(*) OVER()", sql);
        }

        [Fact]
        public void Paging_SqlServer_WhereContainsOrderByText_ShouldStillAddOrderBy()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s)
                .Where($"{s.GetName()} = 'ORDER BY x'");

            var sql = qb.BuildSql(rowLimit: 10);
            LogSql(sql);
            // WHERE 里出现 "ORDER BY " 字面量时，仍然必须补上 ORDER BY，否则 SQL Server 报语法错误
            Assert.Contains("ORDER BY 1", sql);
        }

        [Theory]
        [InlineData("sqlconnection")]
        [InlineData("oracleconnection")]
        [InlineData("db2connection")]
        public void Paging_OffsetFetchDialect_NoOrderBy_ShouldAddDummyOrderBy(string dialect)
        {
            SQLDialectFactory.UseDialect(dialect);
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildSql(rowLimit: 10, includeTotalCount: false);
            LogSql(sql, dialect);
            // OFFSET/FETCH 语法强制要求 ORDER BY
            Assert.Contains("ORDER BY 1", sql);
        }

        [Fact]
        public void BuildSql_NoSelectFields_ShouldUseWildcard()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildSql(rowLimit: 10, includeTotalCount: false);
            LogSql(sql);
            Assert.Contains("u.*", sql);
        }

        // ================================================================
        // 分页与查询结构的组合
        // ================================================================
        [Fact]
        public void Paging_WithGroupBy_ShouldCountGroupsInTotal()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Status);
            s.SelectExpression("Count(1) AS Cnt");

            var qb = new QueryBuilder().From(s)
                .GroupBy(s.GetStatus())
                .OrderBy($"{s.GetStatus()} ASC");

            var sql = qb.BuildSql(rowLimit: 10);
            LogSql(sql);
            // 分组查询的总记录数是分组数（COUNT(*) OVER() 作用在分组结果上）
            Assert.Contains($"COUNT(*) OVER() AS {QueryBuilder.PagingTotalAlias}", sql);
            Assert.Contains("GROUP BY u.Status", sql);
        }

        [Fact]
        public void Paging_WithDistinct_ShouldCountDistinctRows()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name, s.Email);

            var qb = new QueryBuilder().From(s);
            qb.IsDistinct = true;

            var sql = qb.BuildSql(rowLimit: 10);
            LogSql(sql);
            // DISTINCT 与总数列共存：DISTINCT 作用于整个行，总数列是常量不影响去重
            Assert.Contains("SELECT DISTINCT", sql);
            Assert.Contains($"COUNT(*) OVER() AS {QueryBuilder.PagingTotalAlias}", sql);
        }

        [Fact]
        public void Paging_WithJoin_ShouldKeepJoinClause()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            var o = new TestOrderTableDef("o");
            s.Select(true, s.Name);
            o.Select(true, o.Amount);
            s.Join(o, $"{s.GetId()} = {o.GetUserId()}");

            var qb = new QueryBuilder().From(s, o)
                .OrderBy($"{s.GetName()} ASC");

            var sql = qb.BuildSql(rowLimit: 10, includeTotalCount: false);
            LogSql(sql);
            Assert.Contains("INNER JOIN Orders o on u.Id = o.UserId", sql);
            Assert.Contains("OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY", sql);
        }

        [Fact]
        public void Paging_WithExists_ShouldKeepExistsClause()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);
            var o = new TestOrderTableDef("o");
            var subQb = new QueryBuilder().From(o).Where($"{o.GetUserId()} = {s.GetId()}");

            var qb = new QueryBuilder().From(s)
                .Exists(subQb)
                .OrderBy($"{s.GetName()} ASC");

            var sql = qb.BuildSql(rowLimit: 10, includeTotalCount: false);
            LogSql(sql);
            Assert.Contains("EXISTS (", sql);
            Assert.Contains("OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY", sql);
        }

        [Fact]
        public void Paging_WithSubQueryFrom_ShouldKeepSubQuery()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var subQb = new QueryBuilder("sub")
                .From(s)
                .Where($"{s.GetStatus()} = 1");

            var qb = new QueryBuilder().From(subQb);

            var sql = qb.BuildSql(rowLimit: 10, includeTotalCount: false);
            LogSql(sql);
            Assert.Contains("FROM (SELECT u.Name", sql);
            Assert.Contains("OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY", sql);
        }

        [Fact]
        public void Paging_WithAutoAlias_ShouldNotConflictWithTotalColumn()
        {
            EasySQLContext.AutoAlias = true;
            try
            {
                SQLDialectFactory.UseSqlServerDialect();
                var s = new SnakeUserTableDef("u");
                s.Select(true, s.UserName, s.CreateTime);

                var qb = new QueryBuilder().From(s)
                    .OrderBy($"{s.GetCreateTime()} DESC");

                var sql = qb.BuildSql(rowLimit: 10);
                LogSql(sql);
                // 自动别名（PascalCase）与总数列共存
                Assert.Contains("u.user_name AS UserName", sql);
                Assert.Contains($"COUNT(*) OVER() AS {QueryBuilder.PagingTotalAlias}", sql);
            }
            finally { EasySQLContext.AutoAlias = false; }
        }

        [Fact]
        public void Paging_WithHaving_ShouldKeepHavingClause()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Status);
            s.SelectExpression("Count(1) AS Cnt");

            var qb = new QueryBuilder().From(s)
                .GroupBy(s.GetStatus())
                .Having("Count(1) > 5")
                .OrderBy($"{s.GetStatus()} ASC");

            var sql = qb.BuildSql(rowLimit: 10, includeTotalCount: false);
            LogSql(sql);
            Assert.Contains("GROUP BY u.Status", sql);
            Assert.Contains("HAVING Count(1) > 5", sql);
            Assert.Contains("OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY", sql);
        }

        [Fact]
        public void Paging_WithMultipleOrderBy_ShouldKeepBothColumns()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name, s.Email);

            var qb = new QueryBuilder().From(s)
                .OrderBy($"{s.GetName()} ASC", $"{s.GetEmail()} DESC");

            var sql = qb.BuildSql(rowLimit: 10, includeTotalCount: false);
            LogSql(sql);
            Assert.Contains("ORDER BY u.Name ASC,u.Email DESC", sql);
        }

        [Fact]
        public void Paging_WithMultipleWhere_ShouldCombineWithAnd()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s)
                .Where($"{s.GetStatus()} = {s.AsParam("Status")}",
                       $"{s.GetName()} LIKE {s.AsParam("Name")}")
                .AddParameter("Status", 1)
                .AddParameter("Name", "%张%")
                .OrderBy($"{s.GetName()} ASC");

            var sql = qb.BuildSql(rowLimit: 10, includeTotalCount: false);
            LogSql(sql);
            Assert.Contains("WHERE (u.Status = @Status AND u.Name LIKE @Name)", sql);
            Assert.Equal(1, qb.Parameters["Status"]);
            Assert.Equal("%张%", qb.Parameters["Name"]);
        }

        [Fact]
        public void Paging_RowLimitZero_ShouldNotAddPagingOrTotalColumn()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildSql(rowLimit: 0);
            LogSql(sql);
            // 没有限制行数时不注入总数列，也不加分页子句
            Assert.DoesNotContain("COUNT(*) OVER()", sql);
            Assert.DoesNotContain("OFFSET", sql);
            Assert.DoesNotContain("FETCH NEXT", sql);
        }

        [Fact]
        public void Paging_CountAndPage_ShouldShareSameWhereAndParameters()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s)
                .Where($"{s.GetStatus()} = {s.AsParam("Status")}")
                .AddParameter("Status", 1)
                .OrderBy($"{s.GetName()} ASC");

            string pageSql = qb.BuildSql(rowLimit: 10, rowOffset: 20, includeTotalCount: false);
            string countSql = qb.BuildCountSql();
            LogSql(pageSql, "分页");
            LogSql(countSql, "计数");

            // 两次查询的 WHERE 必须完全一致，否则总数和分页数据对不上
            Assert.Contains("WHERE (u.Status = @Status)", pageSql);
            Assert.Contains("WHERE (u.Status = @Status)", countSql);
            Assert.Equal(1, qb.Parameters["Status"]);
        }

        // ================================================================
        // COUNT 测试（分组 / 去重口径）
        // ================================================================
        [Fact]
        public void BuildCountSql_WithGroupBy_ShouldNotContainOrderBy()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Status);
            s.SelectExpression("Count(1) AS Cnt");

            var qb = new QueryBuilder().From(s)
                .GroupBy(s.GetStatus())
                .OrderBy($"{s.GetStatus()} ASC");

            var sql = qb.BuildCountSql();
            LogSql(sql);
            Assert.Contains("Count(1) AS RowsCount", sql);
            // 计数内层子查询不需要排序
            Assert.DoesNotContain("ORDER BY", sql);
        }

        [Fact]
        public void BuildCountSql_DistinctMultipleColumns_ShouldCountDistinctRows()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name, s.Email);

            var qb = new QueryBuilder().From(s);
            qb.IsDistinct = true;

            var sql = qb.BuildCountSql();
            LogSql(sql);
            // 多列去重必须用派生表计数，不能用 Count(DISTINCT 单列)
            Assert.Contains("Count(1) AS RowsCount", sql);
            Assert.Contains("SELECT DISTINCT", sql);
            Assert.DoesNotContain("Count(DISTINCT", sql);
        }

        [Fact]
        public void BuildCountSql_DistinctWithExpression_ShouldNotGenerateInvalidSql()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.SelectExpression("Count(1) AS Cnt");

            var qb = new QueryBuilder().From(s);
            qb.IsDistinct = true;

            var sql = qb.BuildCountSql();
            LogSql(sql);
            // 旧实现会生成 Count(DISTINCT Count(1)) 这样的非法 SQL
            Assert.DoesNotContain("Count(DISTINCT Count(1))", sql);
            Assert.Contains("Count(1) AS RowsCount", sql);
        }

        // ================================================================
        // UNION 分页 / 计数测试
        // ================================================================
        [Fact]
        public void Union_WithPaging_ShouldWrapAsDerivedTable()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s1 = new TestUserTableDef("u");
            s1.Select(true, s1.Name);
            var s2 = new TestUserTableDef("u2");
            s2.Select(true, s2.Name);

            var qb1 = new QueryBuilder().From(s1)
                .Where($"{s1.GetStatus()} = {s1.AsParam("S1")}")
                .AddParameter("S1", 1);
            var qb2 = new QueryBuilder().From(s2)
                .Where($"{s2.GetStatus()} = {s2.AsParam("S2")}")
                .AddParameter("S2", 2);
            qb1.Union(qb2, isUnionAll: true);

            var sql = qb1.BuildSql(rowLimit: 10, rowOffset: 5, includeTotalCount: false);
            LogSql(sql);
            // UNION 的分页必须作用于合并后的结果集
            Assert.Contains("UNION ALL", sql);
            Assert.Contains("OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY", sql);
            Assert.Contains("FROM (SELECT u.Name", sql);
            // 分支的参数必须合并到主查询，否则执行时参数缺失
            Assert.Contains("@S1", sql);
            Assert.Contains("@S2", sql);
            Assert.Equal(1, qb1.Parameters["S1"]);
            Assert.Equal(2, qb1.Parameters["S2"]);
        }

        [Fact]
        public void Union_WithPaging_ShouldMapOrderByToOutputColumn()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s1 = new TestUserTableDef("u");
            s1.Select(true, s1.Name);
            var s2 = new TestUserTableDef("u2");
            s2.Select(true, s2.Name);

            var qb1 = new QueryBuilder().From(s1).OrderBy($"{s1.GetName()} ASC");
            var qb2 = new QueryBuilder().From(s2);
            qb1.Union(qb2, isUnionAll: true);

            var sql = qb1.BuildSql(rowLimit: 10, includeTotalCount: false);
            LogSql(sql);
            // 排序表达式要映射成结果集的输出列名
            Assert.Contains("ORDER BY A.Name ASC", sql);
        }

        [Fact]
        public void Union_WithCount_ShouldCountAllBranches()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s1 = new TestUserTableDef("u");
            s1.Select(true, s1.Name);
            var s2 = new TestUserTableDef("u2");
            s2.Select(true, s2.Name);

            var qb1 = new QueryBuilder().From(s1);
            var qb2 = new QueryBuilder().From(s2);
            qb1.Union(qb2, isUnionAll: true);

            var sql = qb1.BuildCountSql();
            LogSql(sql);
            // 计数必须覆盖 UNION 的全部分支
            Assert.Contains("Count(1) AS RowsCount", sql);
            Assert.Contains("UNION ALL", sql);
        }

        [Fact]
        public void Union_WithOrderByNotInSelect_ShouldThrow()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s1 = new TestUserTableDef("u");
            s1.Select(true, s1.Name);
            var s2 = new TestUserTableDef("u2");
            s2.Select(true, s2.Name);

            var qb1 = new QueryBuilder().From(s1).OrderBy($"{s1.GetId()} ASC");
            var qb2 = new QueryBuilder().From(s2);
            qb1.Union(qb2, isUnionAll: true);

            // 排序字段不在 SELECT 列中，无法映射到结果集的输出列，应明确报错而不是生成错误 SQL
            Assert.Throws<NotSupportedException>(() => qb1.BuildSql(rowLimit: 10, includeTotalCount: false));
        }


        // ================================================================
        // INSERT / UPDATE / DELETE 测试
        // ================================================================
        [Fact]
        public void InsertBuilder_ShouldGenerateCorrectInsert()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var user = new TestUserTableDef();
            var insert = new InsertBuilder(user)
                .Insert(user.Id, user.Name, user.Email)
                .BuildSql("SELECT 1, 'test', 'test@test.com'");
            LogSql(insert);
            Assert.Contains("INSERT INTO Users", insert);
            Assert.Contains("INSERT INTO Users (Id,Name,Email)", insert);
        }

        [Fact]
        public void UpdateBuilder_ShouldGenerateCorrectUpdate()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var user = new TestUserTableDef();
            var update = new UpdateBuilder(user)
                .Set("Name", "@Name")
                .Set("Status", 1)
                .Where($"{user.GetId()} = {user.AsParam("Id")}")
                .BuildSql();
            LogSql(update);
            Assert.Contains("UPDATE Users SET", update);
            Assert.Contains("Name=@Name", update);
            Assert.Contains("Status=1", update);
            Assert.Contains("WHERE Id = @Id", update);
        }

        [Fact]
        public void UpdateBuilder_NoWhere_ShouldPreventFullUpdate()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef();
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                new UpdateBuilder(s)
                    .Set(s.GetStatus(), 0)
                    .BuildSql();
            });
            Assert.Contains("WHERE", ex.Message);
        }

        [Fact]
        public void DeleteBuilder_ShouldGenerateCorrectDelete()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var user = new TestUserTableDef();
            var delete = new DeleteBuilder(user)
                .Where($"{user.GetId()} = {user.AsParam("Id")}")
                .BuildSql();
            LogSql(delete);
            Assert.Contains("DELETE FROM Users", delete);
            Assert.Contains("WHERE Id = @Id", delete);
        }

        // ================================================================
        // UNION 测试
        // ================================================================
        [Fact]
        public void Union_ShouldGenerateUnionClause()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb1 = new QueryBuilder().From(s)
                .Where($"{s.GetStatus()} = 1");

            var qb2 = new QueryBuilder().From(s)
                .Where($"{s.GetStatus()} = 2");

            qb1.Union(qb2, isUnionAll: true);
            var sql = qb1.BuildSql();
            LogSql(sql);
            Assert.Contains("UNION ALL", sql);
        }

        // ================================================================
        // 参数化查询测试
        // ================================================================
        [Fact]
        public void ParameterizedQuery_ShouldCollectParameters()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s)
                .Where($"{s.GetId()} = {s.AsParam("UserId")}", $"{s.GetStatus()} = {s.AsParam("Status")}")
                .AddParameter("UserId", 123)
                .AddParameter("Status", 1);
            

            Assert.Equal(123, qb.Parameters["UserId"]);
            Assert.Equal(1, qb.Parameters["Status"]);
            Assert.Contains("@UserId", qb.BuildSql());
            LogSql(qb.BuildSql());
        }

        [Fact]
        public void UpdateBuilder_Parameterized_ShouldCollectParameters()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var user = new TestUserTableDef();
            var update = new UpdateBuilder(user)
                .Set("Name", "@Name")
                .Set("Status", "@Status")
                .Where($"{user.GetId()} = {user.AsParam("Id")}")
                .AddParameter("Name", "test")
                .AddParameter("Status", 1)
                .AddParameter("Id", 10);

            Assert.Equal("test", update.Parameters["Name"]);
            Assert.Equal(1, update.Parameters["Status"]);
            Assert.Equal(10, update.Parameters["Id"]);
        }

        // ================================================================
        // IN 子句构建器测试
        // ================================================================
        [Fact]
        public void InBuilder_Int_ShouldGenerateInValues()
        {
            var list = new[] { 1, 2, 3, 4, 5 };
            var builder = list.InBuilder(sectionCount: 3);

            // 第一组 3 个
            Assert.True(builder.HasNext);
            Assert.Equal("(1,2,3)", builder.GetNextInValues());

            // 第二组 2 个
            Assert.True(builder.HasNext);
            Assert.Equal("(4,5)", builder.GetNextInValues());

            // 无剩余
            Assert.False(builder.HasNext);
        }

        [Fact]
        public void InBuilder_String_ShouldQuoteValues()
        {
            var list = new[] { "Alice", "Bob", "Charlie" };
            var builder = list.InBuilder(sectionCount: 2);

            Assert.Equal("('Alice','Bob')", builder.GetNextInValues());
            Assert.Equal("('Charlie')", builder.GetNextInValues());
        }

        // ================================================================
        // 子查询 EXISTS 测试
        // ================================================================
        [Fact]
        public void Exists_ShouldGenerateExistsClause()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            var sb = new TestOrderTableDef("o");
            s.Select(true, s.Name);

            var subQb = new QueryBuilder().From(sb)
                .Where($"{sb.GetUserId()} = {s.GetId()}");

            var qb = new QueryBuilder().From(s)
                .Exists(subQb);

            var sql = qb.BuildSql();
            Assert.Contains("EXISTS", sql);
            LogSql(sql);
        }

        // ================================================================
        // 模板 SQL（Dapper.SqlBuilder 占位符）测试
        // ================================================================
        [Fact]
        public void BuildTemplateSql_ShouldContainPlaceholders()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildTemplateSql();
            LogSql(sql);
            Assert.Contains("SELECT u.Name", sql);
            Assert.Contains("FROM Users u", sql);
            Assert.Contains("/**where**/", sql);
            Assert.Contains("/**orderby**/", sql);
        }

        [Fact]
        public void BuildTemplateSql_ShouldIgnoreStaticWhereAndOrderBy()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s)
                .Where($"{s.GetStatus()} = 1")
                .OrderBy($"{s.GetName()} ASC");

            var sql = qb.BuildTemplateSql();
            LogSql(sql);
            Assert.Contains("SELECT u.Name", sql);
            Assert.Contains("FROM Users u", sql);
            Assert.Contains("/**where**/", sql);
            Assert.Contains("/**orderby**/", sql);
            Assert.DoesNotContain("Status", sql);
            Assert.DoesNotContain($"{s.GetName()} ASC", sql);
        }

        // ================================================================
        // BuildIntoSql 测试（SELECT INTO / CREATE TABLE AS SELECT）
        // ================================================================
        [Fact]
        public void BuildIntoSql_SqlServer_ShouldUseSelectInto()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name, s.Email);

            var qb = new QueryBuilder().From(s)
                .Where($"{s.GetStatus()} = 1");

            var tmp = new TempTableDef("#tmp_users", "Name", "Email");
            string sql = new SqlServerDialect().BuildIntoSql(qb, tmp.TableName, isTemp: true);
            LogSql(sql, "SQL Server SELECT INTO");
            Assert.Contains("SELECT ", sql);
            Assert.Contains("INTO #tmp_users", sql);
            Assert.Contains("FROM Users u", sql);
        }

        [Fact]
        public void BuildIntoSql_MySQL_ShouldUseCreateTableAsSelect()
        {
            SQLDialectFactory.UseDialect("mysqlconnection");
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s);
            var tmp = new TempTableDef("tmp_users", "Name");
            string sql = new MySQLDialect().BuildIntoSql(qb, tmp.TableName, isTemp: true);
            LogSql(sql, "MySQL CREATE TEMPORARY TABLE AS SELECT");
            Assert.StartsWith("CREATE TEMPORARY TABLE", sql);
            Assert.Contains("AS SELECT", sql);
        }

        [Fact]
        public void BuildIntoSql_PostgreSQL_ShouldUseCreateTableAsSelect()
        {
            SQLDialectFactory.UseDialect("npgsqlconnection");
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s);
            var tmp = new TempTableDef("tmp_users", "Name");
            string sql = new PostgreSQLDialect().BuildIntoSql(qb, tmp.TableName, isTemp: true);
            LogSql(sql, "PostgreSQL CREATE TEMP TABLE AS SELECT");
            Assert.StartsWith("CREATE TEMPORARY TABLE", sql);
            Assert.Contains("AS SELECT", sql);
        }

        [Fact]
        public void BuildIntoSql_Oracle_ShouldUseGlobalTemporary()
        {
            SQLDialectFactory.UseDialect("oracleconnection");
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s);
            var tmp = new TempTableDef("tmp_users", "Name");
            string sql = new OracleDialect().BuildIntoSql(qb, tmp.TableName, isTemp: true);
            LogSql(sql, "Oracle CREATE GLOBAL TEMPORARY TABLE AS SELECT");
            Assert.StartsWith("CREATE GLOBAL TEMPORARY TABLE", sql);
        }

        [Fact]
        public void BuildIntoSql_DB2_ShouldUseWithData()
        {
            SQLDialectFactory.UseDialect("db2connection");
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name);

            var qb = new QueryBuilder().From(s);
            var tmp = new TempTableDef("tmp_users", "Name");
            string sql = new DB2Dialect().BuildIntoSql(qb, tmp.TableName, isTemp: true);
            LogSql(sql, "DB2 CREATE GLOBAL TEMPORARY TABLE AS SELECT WITH DATA");
            Assert.StartsWith("CREATE GLOBAL TEMPORARY TABLE", sql);
            Assert.Contains("WITH DATA", sql);
        }

        // ================================================================
        // TempTableDef 测试
        // ================================================================
        [Fact]
        public void TempTableDef_Indexer_ShouldReturnPrefixedField()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var tmp = new TempTableDef("t", "Id", "Name") { Alias = "t" };
            // 带别名时返回带前缀的字段名（与 GetXxx() 行为一致）
            Assert.Equal("t.Id", tmp["Id"]);
            Assert.Equal("t.Name", tmp["Name"]);
        }

        [Fact]
        public void TempTableDef_NoAlias_ShouldReturnPlainField()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var tmp = new TempTableDef("#temp", "Id", "Name");
            Assert.Equal("Id", tmp["Id"]);
        }

        [Fact]
        public void TempTableDef_CanJoinAndQuery()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var sa = new TestUserTableDef("SA");
            var tmp = new TempTableDef("#t", "Id", "Name") { Alias = "t" };

            sa.Select(true, sa.Name);
            // tmp["Name"] 返回带前缀的 "t.Name"，用 Select(field) 即可（同 GetXxx 用法）
            tmp.Select(false, tmp["Name"]);
            sa.Join(tmp, $"{sa.GetId()} = {tmp["Id"]}");

            var qb = new QueryBuilder().From(sa, tmp);
            string sql = qb.BuildSql();
            LogSql(sql, "TempTableDef JOIN");
            Assert.Contains("INNER JOIN #t t on SA.Id = t.Id", sql);
            Assert.Contains("SA.Name", sql);
            Assert.Contains("t.Name", sql);
        }

        [Fact]
        public void TempTableDef_InsertBuilderBuildIntoSql_ShouldWork()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(true, s.Name, s.Email);

            var qb = new QueryBuilder().From(s)
                .Where($"{s.GetStatus()} = 1");

            var tmp = new TempTableDef("#tmp", "Name", "Email");
            string sql = new InsertBuilder(tmp).BuildIntoSql(fromQuery: qb);
            LogSql(sql, "InsertBuilder.BuildIntoSql");
            Assert.Contains("INTO #tmp", sql);
        }

        // ================================================================
        // DialectType 枚举测试
        // ================================================================
        [Fact]
        public void DialectType_EachDialect_ShouldReturnCorrectEnum()
        {
            Assert.Equal(DialectType.SqlServer, new SqlServerDialect().DialectType);
            Assert.Equal(DialectType.MySQL, new MySQLDialect().DialectType);
            Assert.Equal(DialectType.PostgreSQL, new PostgreSQLDialect().DialectType);
            Assert.Equal(DialectType.Oracle, new OracleDialect().DialectType);
            Assert.Equal(DialectType.SQLite, new SQLiteDialect().DialectType);
            Assert.Equal(DialectType.DB2, new DB2Dialect().DialectType);
        }

        // ================================================================
        // 下划线字段自动别名（snake_case → PascalCase）测试
        // ================================================================

        /// <summary>
        /// 模拟 snake_case 数据库的 TableDef。
        /// </summary>
        class SnakeUserTableDef : TableDefBase
        {
            public const string TABLE = "users";
            public const string ID = "id";
            public const string USER_NAME = "user_name";
            public const string EMAIL = "email";
            public const string CREATE_TIME = "create_time";

            public override string TableName => TABLE;
            public SnakeUserTableDef(string? alias = null, ISQLDialect? dialect = null)
                : base(alias ?? string.Empty, dialect) { }

            public string Id => ID;
            public string UserName => USER_NAME;
            public string Email => EMAIL;
            public string CreateTime => CREATE_TIME;

            public string GetId(bool p = true) => QuoteField(ID, p);
            public string GetUserName(bool p = true) => QuoteField(USER_NAME, p);
            public string GetEmail(bool p = true) => QuoteField(EMAIL, p);
            public string GetCreateTime(bool p = true) => QuoteField(CREATE_TIME, p);
        }

        [Fact]
        public void SnakeCaseField_AutoAliasOff_ShouldNotAlias()
        {
            // 默认关闭
            SQLDialectFactory.UseSqlServerDialect();
            var u = new SnakeUserTableDef("u");
            u.Select(true, u.UserName, u.CreateTime);

            var qb = new QueryBuilder().From(u);
            var sql = qb.BuildSql();
            LogSql(sql, "AutoAlias OFF（默认） — 无别名");

            Assert.Contains("u.user_name", sql);
            Assert.Contains("u.create_time", sql);
            Assert.DoesNotContain("AS UserName", sql);
            Assert.DoesNotContain("AS CreateTime", sql);
        }

        [Fact]
        public void SnakeCaseField_AutoAliasOn_ShouldAliasToPascalCase()
        {
            EasySQLContext.AutoAlias = true;
            try
            {
                SQLDialectFactory.UseSqlServerDialect();
                var u = new SnakeUserTableDef("u");
                u.Select(true, u.UserName, u.CreateTime);

                var qb = new QueryBuilder().From(u);
                var sql = qb.BuildSql();
                LogSql(sql, "AutoAlias ON — 下划线字段自动别名");

                Assert.Contains("u.user_name AS UserName", sql);
                Assert.Contains("u.create_time AS CreateTime", sql);
            }
            finally { EasySQLContext.AutoAlias = false; }
        }

        [Fact]
        public void AutoAlias_On_NoUnderscore_ShouldNotAddAlias()
        {
            EasySQLContext.AutoAlias = true;
            try
            {
                SQLDialectFactory.UseSqlServerDialect();
                var u = new SnakeUserTableDef("u");
                u.Select(true, u.Id, u.Email);

                var qb = new QueryBuilder().From(u);
                var sql = qb.BuildSql();
                LogSql(sql, "AutoAlias ON — 无下划线字段不生成别名");

                Assert.Contains("u.id", sql);
                Assert.Contains("u.email", sql);
                Assert.DoesNotContain("AS Id", sql);
                Assert.DoesNotContain("AS Email", sql);
            }
            finally { EasySQLContext.AutoAlias = false; }
        }

        [Fact]
        public void ExplicitAlias_ShouldOverrideAutoAlias()
        {
            EasySQLContext.AutoAlias = true;
            try
            {
                SQLDialectFactory.UseSqlServerDialect();
                var u = new SnakeUserTableDef("u");
                u.Select(u.UserName, "Name");

                var qb = new QueryBuilder().From(u);
                var sql = qb.BuildSql();
                LogSql(sql, "显式别名覆盖自动别名");

                Assert.Contains("user_name AS Name", sql);
                Assert.DoesNotContain("AS UserName", sql);
            }
            finally { EasySQLContext.AutoAlias = false; }
        }
    }
}
