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

        // ---- 测试用轻量 Schema（避免与 Schemas/ 下的真实 Schema 冲突） ----
        class TestUserSchema : SchemaBase
        {
            public const string TABLE = "Users";
            public const string ID = "Id";
            public const string NAME = "Name";
            public const string EMAIL = "Email";
            public const string STATUS = "Status";

            public override string TableName => TABLE;
            public TestUserSchema(string? alias = null, ISQLDialect? dialect = null) : base(alias ?? string.Empty, dialect) { }

            public string GetId(bool needPrefix = true) => QuoteField(ID, needPrefix);
            public string GetName(bool needPrefix = true) => QuoteField(NAME, needPrefix);
            public string GetEmail(bool needPrefix = true) => QuoteField(EMAIL, needPrefix);
            public string GetStatus(bool needPrefix = true) => QuoteField(STATUS, needPrefix);
        }

        class TestOrderSchema : SchemaBase
        {
            public const string TABLE = "Orders";
            public const string ID = "Id";
            public const string USER_ID = "UserId";
            public const string AMOUNT = "Amount";
            public const string CREATE_TIME = "CreateTime";

            public override string TableName => TABLE;
            public TestOrderSchema(string? alias = null, ISQLDialect? dialect = null) : base(alias ?? string.Empty, dialect) { }

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
            var su = new TestUserSchema("u");
            su.Select(su.GetName());
            su.Select(su.GetEmail());

            var qb = new QueryBuilder().From(su);

            var sql = qb.BuildSql();
            LogSql(sql);
            Assert.Equal($"SELECT u.Name,u.Email{System.Environment.NewLine}FROM Users u", sql);
        }

        [Fact]
        public void SelectAllFields_ShouldGenerateStar()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var su = new TestUserSchema("u");
            su.Select();

            var qb = new QueryBuilder().From(su);

            var sql = qb.BuildSql();
            LogSql(sql);
            Assert.Equal($"SELECT u.*{System.Environment.NewLine}FROM Users u", sql);
        }

        [Fact]
        public void SelectDistinct_ShouldGenerateDistinctKeyword()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var su = new TestUserSchema("u");
            su.Select(su.GetStatus());

            var qb = new QueryBuilder().From(su);
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
            var su = new TestUserSchema("u");
            su.Select(su.GetName());

            var qb = new QueryBuilder().From(su)
                .Where("u.Status = 1", "u.Email IS NOT NULL");

            var sql = qb.BuildSql();
            LogSql(sql);
            Assert.Contains("WHERE (u.Status = 1 AND u.Email IS NOT NULL)", sql);
        }

        [Fact]
        public void GroupByAndHaving_ShouldGenerateCorrectClauses()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var su = new TestUserSchema("u");
            su.Select(su.GetStatus());
            su.SelectExpression("Count(1) AS Cnt");

            var qb = new QueryBuilder().From(su)
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
            var su = new TestUserSchema("u");
            su.Select(su.GetName(), su.GetEmail());

            var qb = new QueryBuilder().From(su)
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
            var su = new TestUserSchema("u");
            var so = new TestOrderSchema("o");
            su.Select(su.GetName());
            so.Select(so.GetAmount());
            su.Join(so, "u.Id = o.UserId");

            var qb = new QueryBuilder().From(su);

            var sql = qb.BuildSql();
            LogSql(sql);
            Assert.Contains("INNER JOIN Orders o on u.Id = o.UserId", sql);
        }

        [Fact]
        public void LeftJoin_ShouldGenerateLeftOuterJoin()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var su = new TestUserSchema("u");
            var so = new TestOrderSchema("o");
            su.Select(su.GetName());
            so.Select(so.GetAmount());
            su.LeftJoin(so, "u.Id=o.UserId");

            var qb = new QueryBuilder().From(su);

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
            var su = new TestUserSchema("u");
            su.Select(su.GetName());

            var qb = new QueryBuilder().From(su)
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
            var su = new TestUserSchema("u");
            su.Select(su.GetStatus());
            su.SelectExpression("Count(1) AS Cnt");

            var qb = new QueryBuilder().From(su)
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
            var su = new TestUserSchema("u");
            su.Select(su.GetName(), su.GetEmail());

            var qb = new QueryBuilder().From(su)
                .OrderBy("u.Name ASC");

            var sql = qb.BuildSql(rowLimit: 10, rowOffset: 5);
            LogSql(sql);
            Assert.Contains("OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY", sql);
        }

        [Fact]
        public void Paging_SqlServer_NoOrderBy_ShouldAddDummyOrderBy()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var su = new TestUserSchema("u");
            su.Select(su.GetName());

            var qb = new QueryBuilder().From(su);
            qb.OrderBy($" { su.GetEmail() } Asc");

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
            var su = new TestUserSchema("u");
            su.Select(su.GetName());

            var qb = new QueryBuilder().From(su);

            var sql = qb.BuildSql(rowLimit: 10, rowOffset: 5);
            LogSql(sql);
            Assert.Contains("LIMIT 10", sql);
            Assert.Contains("OFFSET 5", sql);
        }

        [Fact]
        public void Paging_PostgreSQL_ShouldUseLimitOffset()
        {
            SQLDialectFactory.UseDialect("npgsqlconnection");
            var su = new TestUserSchema("u");
            su.Select(su.GetName());

            var qb = new QueryBuilder().From(su);

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
            var su = new TestUserSchema("u");
            su.Select(su.GetName());

            var qb = new QueryBuilder().From(su);

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
            var user = new TestUserSchema();
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
            var user = new TestUserSchema();
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
            var su = new TestUserSchema();
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                new UpdateBuilder(su)
                    .Set(su.GetStatus(), 0)
                    .BuildSql();
            });
            Assert.Contains("WHERE", ex.Message);
        }

        [Fact]
        public void DeleteBuilder_ShouldGenerateCorrectDelete()
        {
            SQLDialectFactory.UseSqlServerDialect();
            var user = new TestUserSchema();
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
            var su = new TestUserSchema("u");
            su.Select(su.GetName());

            var qb1 = new QueryBuilder().From(su)
                .Where("u.Status = 1");

            var qb2 = new QueryBuilder().From(su)
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
            var su = new TestUserSchema("u");
            su.Select(su.GetName());

            var qb = new QueryBuilder().From(su)
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
            var user = new TestUserSchema();
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
            var su = new TestUserSchema("u");
            var so = new TestOrderSchema("o");
            su.Select(su.GetName());

            var subQb = new QueryBuilder().From(so)
                .Where("o.UserId = u.Id");

            var qb = new QueryBuilder().From(su)
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
            var su = new TestUserSchema("u");
            su.Select(su.GetName());

            var qb = new QueryBuilder().From(su);

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
            var su = new TestUserSchema("u");
            su.Select(su.GetName());

            var qb = new QueryBuilder().From(su)
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
