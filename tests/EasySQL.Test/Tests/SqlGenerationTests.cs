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
            s.Select(s.GetName());
            s.Select(s.GetEmail());

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
            s.Select(s.GetStatus());

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
            s.Select(s.GetName());

            var qb = new QueryBuilder().From(s)
                .Where("u.Status = 1", "u.Email IS NOT NULL");

            var sql = qb.BuildSql();
            LogSql(sql);
            Assert.Contains("WHERE (u.Status = 1 AND u.Email IS NOT NULL)", sql);
        }

        [Fact]
        public void GroupByAndHaving_ShouldGenerateCorrectClauses()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(s.GetStatus());
            s.SelectExpression("Count(1) AS Cnt");

            var qb = new QueryBuilder().From(s)
                .GroupBy("u.Status")
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
            s.Select(s.GetName(), s.GetEmail());

            var qb = new QueryBuilder().From(s)
                .OrderBy("u.Name ASC", "u.Email DESC");

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
            s.Select(s.GetName());
            sb.Select(sb.GetAmount());
            s.Join(sb, "u.Id = o.UserId");

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
            s.Select(s.GetName());
            sb.Select(sb.GetAmount());
            s.LeftJoin(sb, "u.Id=o.UserId");

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
            s.Select(s.GetName());

            var qb = new QueryBuilder().From(s)
                .Where("u.Status = 1");

            var sql = qb.BuildCountSql();
            LogSql(sql);
            Assert.Contains("Count(1) AS RowsCount", sql);
            Assert.DoesNotContain("u.Name", sql); // SELECT 字段被替换为计数
        }

        [Fact]
        public void BuildCountSql_WithGroupBy_ShouldGenerateWrappedCount()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(s.GetStatus());
            s.SelectExpression("Count(1) AS Cnt");

            var qb = new QueryBuilder().From(s)
                .GroupBy("u.Status");

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
            s.Select(s.GetName(), s.GetEmail());

            var qb = new QueryBuilder().From(s)
                .OrderBy("u.Name ASC");

            var sql = qb.BuildSql(rowLimit: 10, rowOffset: 5);
            LogSql(sql);
            Assert.Contains("OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY", sql);
        }

        [Fact]
        public void Paging_SqlServer_NoOrderBy_ShouldAddDummyOrderBy()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(s.GetName());

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
            s.Select(s.GetName());

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
            s.Select(s.GetName());

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
            s.Select(s.GetName());

            var qb = new QueryBuilder().From(s);

            var sql = qb.BuildSql(rowLimit: 10, rowOffset: 5);
            LogSql(sql);
            Assert.Contains("OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY", sql);
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
                .Insert("Id", "Name", "Email")
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
                .Where("Id = @Id")
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
                .Where("Id = @Id")
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
            s.Select(s.GetName());

            var qb1 = new QueryBuilder().From(s)
                .Where("u.Status = 1");

            var qb2 = new QueryBuilder().From(s)
                .Where("u.Status = 2");

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
            s.Select(s.GetName());

            var qb = new QueryBuilder().From(s)
                .Where("u.Id = @UserId", "u.Status = @Status")
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
                .Where("Id = @Id")
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
            s.Select(s.GetName());

            var subQb = new QueryBuilder().From(sb)
                .Where("o.UserId = u.Id");

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
            s.Select(s.GetName());

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
            s.Select(s.GetName());

            var qb = new QueryBuilder().From(s)
                .Where("u.Status = 1")
                .OrderBy("u.Name ASC");

            var sql = qb.BuildTemplateSql();
            LogSql(sql);
            Assert.Contains("SELECT u.Name", sql);
            Assert.Contains("FROM Users u", sql);
            Assert.Contains("/**where**/", sql);
            Assert.Contains("/**orderby**/", sql);
            Assert.DoesNotContain("Status", sql);
            Assert.DoesNotContain("u.Name ASC", sql);
        }

        // ================================================================
        // BuildIntoSql 测试（SELECT INTO / CREATE TABLE AS SELECT）
        // ================================================================
        [Fact]
        public void BuildIntoSql_SqlServer_ShouldUseSelectInto()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var s = new TestUserTableDef("u");
            s.Select(s.GetName(), s.GetEmail());

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
            s.Select(s.GetName());

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
            s.Select(s.GetName());

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
            s.Select(s.GetName());

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
            s.Select(s.GetName());

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

            sa.Select(sa.GetName());
            // tmp["Name"] 返回带前缀的 "t.Name"，用 Select(field) 即可（同 GetXxx 用法）
            tmp.Select(tmp["Name"]);
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
            s.Select(s.GetName(), s.GetEmail());

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
    }
}
