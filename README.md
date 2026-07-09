# EasySQL

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![NuGet](https://img.shields.io/badge/nuget-v1.0.0-blue)](https://www.nuget.org/packages/EasySQL)

EasySQL 是一个轻量级、跨数据库的 .NET SQL 查询构建器。通过流式 API 构建复杂的、方言感知的 SQL 查询语句。

## ✨ 特性

- **7 种数据库方言**：SQL Server、MySQL、PostgreSQL、Oracle、SQLite、MS Access（Jet）、IBM DB2
- **流式查询构建器**：通过方法链式调用构建 `SELECT`、`INSERT`、`UPDATE`、`DELETE` 语句
- **完整的连接支持**：内连接、左连接、右连接、全外连接，支持多字段连接条件
- **分页支持**：所有数据库内置 `LIMIT`/`OFFSET` 支持
- **Union 查询**：使用 `UNION` / `UNION ALL` 合并多个查询结果
- **子查询**：支持 `EXISTS` / `NOT EXISTS` 嵌套查询
- **跨数据库函数**：常见 SQL 函数（日期、字符串、数学）的跨数据库抽象
- **自动方言检测**：根据数据库连接自动选择正确的 SQL 方言
- **关键字引号处理**：各数据库保留字的自动转义处理

## 📦 安装

```bash
dotnet add package EasySQL
```

## 🚀 快速开始

### 定义表结构

```csharp
using EasySQL;

public class UserSchema : SchemaBase
{
    public const string USERS = "Users";
    public const string ID = "Id";
    public const string NAME = "Name";
    public const string EMAIL = "Email";

    public override string TableName => USERS;

    public UserSchema(string alias = null, ISQLDialect dialect = null)
        : base(alias, dialect) { }

    public string Id => ID;
    public string Name => NAME;
    public string Email => EMAIL;

    // 获取带引号/前缀修饰的字段名
    public string GetId(bool needPrefix = true) => QuoteField(ID, needPrefix);
    public string GetName(bool needPrefix = true) => QuoteField(NAME, needPrefix);
    public string GetEmail(bool needPrefix = true) => QuoteField(EMAIL, needPrefix);
}
```

### 构建查询

```csharp
var user = new UserSchema("u");
var order = new OrderSchema("o");

// 简单查询
var qb = new QueryBuilder()
    .From(user)
    .Where(user.GetId() + " = @Id");
string sql = qb.BuildSql();

// 带 JOIN 的查询
user.Select(user.GetName(), user.GetEmail());
order.Select(order.GetAmount());

var qb2 = new QueryBuilder()
    .From(user)
    .Join(order, user.QuoteField("Id"), order.QuoteField("UserId"))
    .Where(user.GetId() + " > @MinId")
    .OrderBy(user.GetName() + " ASC")
    .BuildSql(rowLimit: 20, rowOffset: 0);
```

### 配置数据库上下文

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

// 初始化
DbContext.ConfigContext(new List<IDbProxy>
{
    new SqlServerProxy().Config("main", "Server=.;Database=MyApp;...")
});

// 使用
DbContext.Do(conn =>
{
    // 配合 Dapper 或 ADO.NET 执行查询
});
```

## 🗄️ 支持的数据库

| 数据库 | 方言类 | 连接类型 |
|--------|--------|----------|
| SQL Server | `SqlServerDialect` | `SqlConnection` |
| MySQL | `MySQLDialect` | `MySqlConnection` |
| PostgreSQL | `PostgreSQLDialect` | `NpgsqlConnection` |
| Oracle | `OracleDialect` | `OracleConnection` |
| SQLite | `SQLiteDialect` | `SQLiteConnection` |
| MS Access | `JetDialect` | `OleDbConnection` |
| IBM DB2 | `DB2Dialect` | `DB2Connection` |

## 📖 SQL 构建器参考

### QueryBuilder（查询构建器）

```csharp
var qb = new QueryBuilder("别名", dialect)
    .From(schema1, schema2)          // FROM 子句
    .Where("条件1", "条件2")          // WHERE 条件
    .GroupBy("列1", "列2")            // GROUP BY 分组
    .Having("条件")                   // HAVING 过滤
    .OrderBy("列1 ASC", "列2 DESC");  // ORDER BY 排序

// 分页查询
string sql = qb.BuildSql(rowLimit: 20, rowOffset: 0);

// Count 计数查询
string countSql = qb.BuildCountSql();

// Union 合并查询
qb.Union(otherQuery, isUnionAll: true);

// Exists 子查询
qb.Exists(subQuery);
qb.NotExists(subQuery);
```

### InsertBuilder（插入构建器）

```csharp
var insert = new InsertBuilder(table)
    .Insert("列1", "列2")
    .BuildSql(fromQuery);
```

### UpdateBuilder（更新构建器）

```csharp
var update = new UpdateBuilder(table)
    .Set("列1", "@Value1")
    .Set("列2", 123)
    .Where("Id = @Id")
    .BuildSql();
```

### DeleteBuilder（删除构建器）

```csharp
var delete = new DeleteBuilder(table)
    .Where("Id = @Id")
    .BuildSql();
```

## 📄 许可证

本项目基于 [MIT License](LICENSE) 开源协议发布。

## 🙏 致谢

本项目的思路源于多年的企业级应用开发实践，特别感谢 [Dapper](https://github.com/DapperLib/Dapper) 项目带来的灵感。
