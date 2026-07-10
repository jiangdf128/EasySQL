# EasySQL: Why Build Yet Another SQL Builder?

> A lightweight, cross-database, type-safe SQL query builder for .NET.

## The Backstory

The .NET ecosystem is not short on data access tools. From Entity Framework to Dapper, from SqlKata to PetaPoco, there's no lack of choice. So why EasySQL?

The answer: **we needed a type-safe query builder that generates SQL strings — and nothing more**.

## What's Wrong with the Existing Options?

### Entity Framework / EF Core

Too heavy. It manages object lifecycle, change tracking, navigation properties, migrations, and more. When all you want is a simple JOIN query, it can generate hundreds of lines of SQL that you never see. EasySQL takes the opposite approach: **what you see is what you get**.

### Dapper

Dapper is fantastic at what it does — executing SQL and mapping results. But the SQL itself? You're still hand-writing strings. When queries have dynamic conditions, pagination, and cross-database requirements, string concatenation becomes a nightmare fast.

### SqlKata

The closest competitor to EasySQL. It uses string-based field names, built at runtime:

```csharp
// SqlKata — runtime strings
var query = new Query("Users")
    .Select("Id", "Name", "Email")
    .Where("Status", 1);
```

There's a hidden risk: **if the `Status` column is renamed to `State`, SqlKata won't tell you. Only your tests — or production errors — will catch it**.

## EasySQL's Approach: Schema-Driven

EasySQL asks you to define a TableDef class first:

```csharp
public class UserTableDef : TableDefBase
{
    public const string TABLE = "Users";
    public const string ID = "Id";
    public const string NAME = "Name";
    public const string STATUS = "Status";

    public override string TableName => TABLE;

    // Properties → for SELECT with multiple fields
    public string Id => ID;
    public string Name => NAME;
    public string Status => STATUS;

    // GetXxx methods → for WHERE / JOIN / ORDER BY
    public string GetId(bool needPrefix = true) => QuoteField(ID, needPrefix);
    public string GetStatus(bool needPrefix = true) => QuoteField(STATUS, needPrefix);
}
```

Then writing queries feels fundamentally different:

```csharp
var sa = new UserTableDef("SA");
var sb = new OrderTableDef("SB");

sa.Select(true, sa.Name, sa.Email);
sb.Select(true, sb.Amount);

sa.Join(sb, $"{sa.GetId()} = {sb.GetUserId()}");

var qb = new QueryBuilder()
    .From(sa, sb)
    .Where($"{sa.GetStatus()} = {sa.AsParam("Status")}")
    .AddParameter("Status", 1)
    .OrderBy($"{sa.GetName()} ASC");

string sql = qb.BuildSql(rowLimit: 20, rowOffset: 0);
```

If you rename `STATUS` to `STATE`, **the compiler immediately tells you every place that needs updating**. That's the value of being schema-driven.

## Key Capabilities

### 1. Six Database Dialects, Automatic Detection

| Dialect | Pagination | Param Prefix | Keyword Quoting |
|---------|-----------|-------------|-----------------|
| SQL Server | `OFFSET/FETCH` | `@` | `[word]` |
| MySQL | `LIMIT/OFFSET` | `@` | `` `word` `` |
| PostgreSQL | `LIMIT/OFFSET` | `@` | `"word"` |
| Oracle | `OFFSET/FETCH` | `:` | — |
| SQLite | `LIMIT/OFFSET` | `@` | — |
| DB2 | `OFFSET/FETCH` | `@` | — |

The dialect is auto-detected when a connection is opened, ensuring correct SQL every time.

### 2. Single-Query Pagination

```sql
-- BuildSql(rowLimit: 10, rowOffset: 5) injects this automatically
SELECT SA.Name, COUNT(*) OVER() AS TotalRows
FROM Users SA
ORDER BY SA.Name ASC
OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY
```

One query returns both the page of data and the total row count. No separate `SELECT COUNT(*)` needed.

### 3. Zero-Cost Temp Tables

```csharp
var tmp = new TempTableDef("#tmp", "Id", "Name") { Alias = "t" };
tmp.Select(tmp["Name"]);
sa.Join(tmp, $"{sa.GetId()} = {tmp["Id"]}");

// Need to create the temp table?
string sql = new InsertBuilder(tmp).BuildIntoSql(fromQuery: qb);
// SQL Server → SELECT ... INTO #tmp FROM ...
// PostgreSQL → CREATE TEMP TABLE #tmp AS SELECT ...
```

No class file needed. One line to define a temp table, immediately usable in JOINs and subqueries.

### 4. Guardrails

```csharp
// UPDATE / DELETE without WHERE → throws
new UpdateBuilder(table).Set(table.GetName(), "x").BuildSql();
// ❌ InvalidOperationException: UPDATE must have a WHERE clause

// Parameterized queries prevent injection
qb.Where($"{sa.GetId()} = {sa.AsParam("Id")}").AddParameter("Id", 123);
// → WHERE SA.Id = @Id
```

## EasySQL vs. SqlKata

| Dimension | EasySQL | SqlKata |
|-----------|---------|---------|
| Field references | Compile-time constants | Runtime strings |
| Rename a column | Compiler catches it | Tests catch it (hopefully) |
| Setup cost | One TableDef class | None |
| Pagination | `BuildSql(limit, offset)` → single query | Manual two-query approach |
| Temp tables | TempTableDef built-in | Not supported |
| JOINs | Between TableDef instances | String concatenation |
| Best for | Mid-to-large projects with stable schemas | Quick prototypes, one-off scripts |

**It's not about better or worse — it's about different use cases.** If your project has 50 tables and 10 developers, and column renames happen regularly, the TableDef setup cost pays for itself quickly in type safety. If you're writing a one-off reporting script, SqlKata is indeed faster.

## Quick Start

```bash
dotnet add package EasySQL
```

```csharp
// 1. Define a TableDef (or generate from your database)
public class UserTableDef : TableDefBase { ... }

// 2. Build your query
var sa = new UserTableDef("SA");
sa.Select(true, sa.Name, sa.Email);
var qb = new QueryBuilder().From(sa).Where(...);

// 3. Get the SQL and execute it
string sql = qb.BuildSql();
var users = conn.Query<User>(sql);
```

## Open Source & Roadmap

- GitHub: [github.com/jiangdf128/EasySQL](https://github.com/jiangdf128/EasySQL)
- NuGet: [EasySQL](https://www.nuget.org/packages/EasySQL)
- License: MIT

Planned features:
- [ ] CTE (Common Table Expressions)
- [ ] Window functions
- [ ] CLI tool for auto-generating TableDef from databases
- [ ] Source Generator for compile-time SQL validation

Stars, issues, and PRs are welcome!
