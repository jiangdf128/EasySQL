# EasySQL

[![.NET](https://img.shields.io/badge/.NET-8.0_|_9.0_|_10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![NuGet](https://img.shields.io/badge/nuget-v1.0.0-blue)](https://www.nuget.org/packages/EasySQL.SqlBuilder)

EasySQL 是一个轻量级、跨数据库的 .NET SQL 查询构建器。**只生成 SQL，不执行 SQL**——通过流式 API 生成方言感知的 SQL 字符串和参数，交给 Dapper 或 ADO.NET 执行。

## ✨ 特性

- **6 种数据库方言**：SQL Server、MySQL、PostgreSQL、Oracle、SQLite、IBM DB2
- **流式查询构建器**：方法链式调用构建 `SELECT`、`INSERT`、`UPDATE`、`DELETE`
- **完整的 JOIN 支持**：INNER / LEFT / RIGHT / FULL OUTER JOIN
- **分页 + 总数一次查询**：`BuildSql(rowLimit, rowOffset)` 自动注入 `COUNT(*) OVER() AS TotalRows`
- **分页可关窗口函数**：`includeTotalCount: false` 只做分页，配合 `BuildCountSql()` 两次查询，性能更好
- **Union 查询**：`UNION` / `UNION ALL` 合并多个查询（支持分页与计数）
- **子查询**：`EXISTS` / `NOT EXISTS` 嵌套查询
- **参数化查询**：`AsParam()` + `AddParameter()` 防 SQL 注入
- **自动方言检测**：连接数据库时自动对齐 SQL 方言
- **关键字转义**：各数据库保留字自动加引号

## 📦 安装

```bash
dotnet add package EasySQL.SqlBuilder
```

## 🚀 快速开始

### 定义 TableDef

```csharp
using EasySQL;

public class UserTableDef : TableDefBase
{
    public const string TABLE = "Users";
    public const string ID = "Id";
    public const string NAME = "Name";
    public const string EMAIL = "Email";

    public override string TableName => TABLE;

    public UserTableDef(string alias, ISQLDialect? dialect = null) : base(alias, dialect) { }
    public UserTableDef() : this(string.Empty, null) { }

    // 实例属性 — 返回原始字段名（用于 Select 多字段选取）
    public string Id => ID;
    public string Name => NAME;
    public string Email => EMAIL;

    // GetXxx 方法 — 返回带引号/前缀修饰的字段名
    public string GetId(bool needPrefix = true) => QuoteField(ID, needPrefix);
    public string GetName(bool needPrefix = true) => QuoteField(NAME, needPrefix);
    public string GetEmail(bool needPrefix = true) => QuoteField(EMAIL, needPrefix);
}
```

### 构建查询

```csharp
var sa = new UserTableDef("SA");
var sb = new OrderTableDef("SB");

// 多字段选取（标准写法）
sa.Select(true, sa.Name, sa.Email);
sb.Select(true, sb.Amount);

// JOIN + WHERE + ORDER BY
sa.Join(sb, $"{sa.GetId()} = {sb.GetUserId()}");

var qb = new QueryBuilder()
    .From(sa, sb)
    .Where($"{sa.GetId()} = {sa.AsParam("Id")}")
    .AddParameter("Id", 123)
    .OrderBy($"{sa.GetName()} ASC");

// 分页查询（一次查询，数据 + 总记录数）
string sql = qb.BuildSql(rowLimit: 20, rowOffset: 0);
// SELECT SA.Name,SA.Email,SB.Amount,COUNT(*) OVER() AS TotalRows
// FROM Users SA INNER JOIN Orders SB on SA.Id = SB.UserId
// WHERE (SA.Id = @Id)
// ORDER BY SA.Name ASC
// OFFSET 0 ROWS FETCH NEXT 20 ROWS ONLY

// 配合 Dapper 执行
var users = conn.Query<User>(sql, qb.Parameters.ToDynamicParameters());
int total = users.First().TotalRows;
```

### 分页的两种模式

`COUNT(*) OVER()` 需要数据库读完整个结果集才能返回第一行，会阻断 Top-N 提前终止优化。
大表深分页时，改用「两次查询」模式（分页语句不带窗口函数 + 单独计数）通常更快：

```csharp
// 模式一：一次查询（默认）——数据 + 总记录数一起返回
string sql = qb.BuildSql(rowLimit: 20, rowOffset: 40);
// SELECT SA.Name,...,COUNT(*) OVER() AS TotalRows FROM ... OFFSET 40 ROWS FETCH NEXT 20 ROWS ONLY
var users = conn.Query<User>(sql, qb.Parameters.ToDynamicParameters());
int total = users.First().TotalRows;

// 模式二：两次查询——分页不带窗口函数，另发一条计数语句
string pageSql = qb.BuildSql(rowLimit: 20, rowOffset: 40, includeTotalCount: false);
// SELECT SA.Name,... FROM ... OFFSET 40 ROWS FETCH NEXT 20 ROWS ONLY
string countSql = qb.BuildCountSql();
// SELECT Count(1) AS RowsCount FROM ...
var page = conn.Query<User>(pageSql, qb.Parameters.ToDynamicParameters());
int totalCount = conn.ExecuteScalar<int>(countSql, qb.Parameters.ToDynamicParameters());
```

> 两条语句共用同一个 `qb.Parameters`，WHERE 条件完全一致；建议在同一连接 / 事务内执行以保证快照一致。
> 页码超出总页数时，模式一拿不到总数（结果为空），模式二不受影响。

### 配置数据库上下文

```csharp
// 非 DI 场景
EasySQLContext.Default.Configure(new EasySQLOptions
{
    Proxies = { new SqlServerProxy().Config("main", connectionString) }
});

EasySQLContext.Default.Do(conn =>
{
    // 使用 Dapper 或 ADO.NET 执行查询
});

// DI 场景（Program.cs）
builder.Services.AddEasySQL(options =>
    options.AddDatabase(new SqlServerProxy().Config("main", connectionString)));

// Service 中注入
public class UserService(IEasySQLContext db)
{
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await db.DoAsync(async conn =>
        {
            var u = new UserTableDef("u");
            u.Select(true, u.Name, u.Email);
            var qb = new QueryBuilder().From(u);
            return await conn.QueryAsync<User>(qb.BuildSql());
        });
    }
}
```

### 定义数据库代理

```csharp
public class SqlServerProxy : DbProxyBase
{
    public override IDbConnection Open()
    {
        var conn = new SqlConnection(this.ConnectString);
        conn.Open();
        return conn;
    }
}
```

## 🗄️ 支持的数据库

| 数据库 | 方言类 | 参数前缀 | 分页语法 |
|--------|--------|---------|---------|
| SQL Server | `SqlServerDialect` | `@` | `OFFSET/FETCH` |
| MySQL | `MySQLDialect` | `@` | `LIMIT/OFFSET` |
| PostgreSQL | `PostgreSQLDialect` | `@` | `LIMIT/OFFSET` |
| Oracle | `OracleDialect` | `:` | `OFFSET/FETCH` |
| SQLite | `SQLiteDialect` | `@` | `LIMIT/OFFSET` |
| IBM DB2 | `DB2Dialect` | `@` | `OFFSET/FETCH` |

## 📖 SQL 构建器参考

### QueryBuilder

```csharp
var qb = new QueryBuilder()
    .From(schema1, schema2)          // FROM 子句
    .Where("条件1", "条件2")          // WHERE 条件（AND 连接）
    .GroupBy("列1", "列2")            // GROUP BY 分组
    .Having("条件")                   // HAVING 过滤
    .OrderBy("列1 ASC", "列2 DESC");  // ORDER BY 排序

// 分页查询（自动注入 COUNT(*) OVER() AS TotalRows）
string sql = qb.BuildSql(rowLimit: 20, rowOffset: 0);

// 分页查询（不注入总数列，配合 BuildCountSql 两次查询）
string pageSql = qb.BuildSql(rowLimit: 20, rowOffset: 0, includeTotalCount: false);

// 计数查询（GROUP BY / DISTINCT 时自动用派生表包装，保证计数口径正确）
string countSql = qb.BuildCountSql();

// Union 合并（排序与分页作用于合并后的结果集）
qb.Union(otherQuery, isUnionAll: true);

// 子查询
qb.Exists(subQuery);
qb.NotExists(subQuery);
```

### InsertBuilder

```csharp
var insert = new InsertBuilder(table)
    .Insert(table.Name, table.Email)
    .BuildSql(fromQuery);  // INSERT INTO ... SELECT ...
```

### UpdateBuilder

```csharp
var update = new UpdateBuilder(table)
    .Set(table.GetName(), table.AsParam("Name"))
    .Set(table.GetStatus(), 1)
    .Where($"{table.GetId()} = {table.AsParam("Id")}")
    .AddParameter("Name", "张三")
    .AddParameter("Id", 123);
string sql = update.BuildSql();  // 无 WHERE 时抛异常，防全表误更新
```

### DeleteBuilder

```csharp
var delete = new DeleteBuilder(table)
    .Where($"{table.GetId()} = {table.AsParam("Id")}")
    .AddParameter("Id", 123);
string sql = delete.BuildSql();  // 无 WHERE 时抛异常，防全表误删除
```

## 📄 许可证

MIT License — 详见 [LICENSE](LICENSE)。

## 🙏 致谢

灵感来源于 [Dapper](https://github.com/DapperLib/Dapper) 项目及多年企业级应用开发实践。
