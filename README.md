# EasySQL

A lightweight, cross-database SQL query builder for .NET. Build complex, dialect-aware SQL queries with a fluent API.

## Features

- **7 Database Dialects**: SQL Server, MySQL, PostgreSQL, Oracle, SQLite, MS Access (Jet), IBM DB2
- **Fluent Query Builder**: Build `SELECT`, `INSERT`, `UPDATE`, `DELETE` statements with method chaining
- **Join Support**: Inner, Left, Right, and Full Outer joins with multi-field conditions
- **Pagination**: Built-in `LIMIT`/`OFFSET` support for all supported databases
- **Union Queries**: Combine multiple query results with `UNION` / `UNION ALL`
- **Sub-queries**: Nested queries with `EXISTS` / `NOT EXISTS` support
- **SQL Functions**: Cross-database abstraction for common SQL functions (Date, String, Math)
- **Auto Dialect Detection**: Automatically selects the correct SQL dialect based on the database connection
- **Keyword Quoting**: Automatic quoting of reserved keywords for each database

## Installation

```bash
dotnet add package EasySQL
```

## Quick Start

### Define a Table Schema

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

    public string GetId(bool needPrefix = true) => QuoteField(ID, needPrefix);
    public string GetName(bool needPrefix = true) => QuoteField(NAME, needPrefix);
    public string GetEmail(bool needPrefix = true) => QuoteField(EMAIL, needPrefix);
}
```

### Build Queries

```csharp
var user = new UserSchema("u");
var order = new OrderSchema("o");

// Simple SELECT
var qb = new QueryBuilder()
    .From(user)
    .Where(user.GetId() + " = @Id");
string sql = qb.BuildSql();

// SELECT with JOIN
user.Select(user.GetName(), user.GetEmail());
order.Select(order.GetAmount());

var qb2 = new QueryBuilder()
    .From(user)
    .Join(order, user.QuoteField("Id"), order.QuoteField("UserId"))
    .Where(user.GetId() + " > @MinId")
    .OrderBy(user.GetName() + " ASC");
string sql2 = qb2.BuildSql(rowLimit: 20, rowOffset: 0);
```

### Configure Database Context

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

// Initialize
DbContext.ConfigContext(new List<IDbProxy>
{
    new SqlServerProxy().Config("main", "Server=.;Database=MyApp;...")
});

// Use
DbContext.Do(conn =>
{
    // execute queries with Dapper or ADO.NET
});
```

## Supported Databases

| Database | Dialect Class | Connection Type |
|----------|--------------|-----------------|
| SQL Server | `SqlServerDialect` | `SqlConnection` |
| MySQL | `MySQLDialect` | `MySqlConnection` |
| PostgreSQL | `PostgreSQLDialect` | `NpgsqlConnection` |
| Oracle | `OracleDialect` | `OracleConnection` |
| SQLite | `SQLiteDialect` | `SQLiteConnection` |
| MS Access | `JetDialect` | `OleDbConnection` |
| IBM DB2 | `DB2Dialect` | `DB2Connection` |

## SQL Builder Reference

### QueryBuilder

```csharp
var qb = new QueryBuilder("alias", dialect)
    .From(schema1, schema2)
    .Where("condition1", "condition2")
    .GroupBy("column1", "column2")
    .Having("condition")
    .OrderBy("column1 ASC", "column2 DESC");

// Pagination
string sql = qb.BuildSql(rowLimit: 20, rowOffset: 0);

// Count query
string countSql = qb.BuildCountSql();

// UNION
qb.Union(otherQuery, isUnionAll: true);
```

### InsertBuilder

```csharp
var insert = new InsertBuilder(table)
    .Insert("Column1", "Column2")
    .BuildSql(fromQuery);
```

### UpdateBuilder

```csharp
var update = new UpdateBuilder(table)
    .Set("Column1", "@Value1")
    .Set("Column2", 123)
    .Where("Id = @Id")
    .BuildSql();
```

### DeleteBuilder

```csharp
var delete = new DeleteBuilder(table)
    .Where("Id = @Id")
    .BuildSql();
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
