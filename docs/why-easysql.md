# EasySQL：为什么我们要再造一个 SQL Builder？

> 一个轻量级、跨数据库、类型安全的 .NET SQL 查询构建器。

## 背景

.NET 生态里从来不缺 SQL 工具。从 Entity Framework 到 Dapper，从 SqlKata 到 PetaPoco，选择已经很多了。那为什么还要写 EasySQL？

答案是：**我们需要一个"只生成 SQL 字符串"的类型安全查询构建器**。

## 已有的方案有什么问题？

### Entity Framework / EF Core

太重了。它要管理整个对象生命周期、变更追踪、导航属性、Migration……当你只是想写一个带 JOIN 的查询，它生成了几百行 SQL 你根本看不到。EasySQL 的思路完全相反——**你看到的就是你得到的**。

### Dapper

Dapper 很棒，但它只解决"执行 SQL + 结果映射"。SQL 本身你还是要手写字符串。当查询条件动态变化、需要分页、跨数据库时，字符串拼接很快变成一场灾难。

### SqlKata

最接近 EasySQL 的竞品。它的思路是用字符串做字段名，运行时构建：

```csharp
// SqlKata — 运行时字符串
var query = new Query("Users")
    .Select("Id", "Name", "Email")
    .Where("Status", 1);
```

这段代码有个隐患：**如果字段 `Status` 被改成 `State`，SqlKata 不会告诉你。只有测试（或生产环境报错）才能发现。**

## EasySQL 的解法：Schema 驱动

EasySQL 要求你先定义一个 TableDef 类：

```csharp
public class UserTableDef : TableDefBase
{
    public const string TABLE = "Users";
    public const string ID = "Id";
    public const string NAME = "Name";
    public const string STATUS = "Status";

    public override string TableName => TABLE;

    // 实例属性 → SELECT 多字段
    public string Id => ID;
    public string Name => NAME;
    public string Status => STATUS;

    // GetXxx 方法 → WHERE / JOIN / ORDER BY
    public string GetId(bool needPrefix = true) => QuoteField(ID, needPrefix);
    public string GetStatus(bool needPrefix = true) => QuoteField(STATUS, needPrefix);
}
```

然后写查询体验完全不一样：

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

如果你把 `STATUS` 改成 `STATE`，**编译器立刻告诉你哪些地方需要修改**。这就是 Schema 驱动的价值。

## 核心能力

### 1. 6 种数据库方言，自动适配

| 方言 | 分页 | 参数前缀 | 关键字转义 |
|------|------|---------|-----------|
| SQL Server | `OFFSET/FETCH` | `@` | `[word]` |
| MySQL | `LIMIT/OFFSET` | `@` | `` `word` `` |
| PostgreSQL | `LIMIT/OFFSET` | `@` | `"word"` |
| Oracle | `OFFSET/FETCH` | `:` | — |
| SQLite | `LIMIT/OFFSET` | `@` | — |
| DB2 | `OFFSET/FETCH` | `@` | — |

连接数据库时自动检测并切换方言，确保生成正确的 SQL。

### 2. 分页一条 SQL

```sql
-- BuildSql(rowLimit: 10, rowOffset: 5) 自动注入
SELECT SA.Name, COUNT(*) OVER() AS TotalRows
FROM Users SA
ORDER BY SA.Name ASC
OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY
```

一次查询同时返回分页数据和总记录数，不需要 `SELECT COUNT(*)` 再查一次。

### 3. 临时表零成本接入

```csharp
var tmp = new TempTableDef("#tmp", "Id", "Name") { Alias = "t" };
tmp.Select(tmp["Name"]);
sa.Join(tmp, $"{sa.GetId()} = {tmp["Id"]}");

// 需要创建临时表？
string sql = new InsertBuilder(tmp).BuildIntoSql(fromQuery: qb);
// SQL Server → SELECT ... INTO #tmp FROM ...
// PostgreSQL → CREATE TEMP TABLE #tmp AS SELECT ...
```

不用写类文件，一行代码定义一个临时表，立即参与 JOIN 和子查询。

### 4. 安全防护

```csharp
// UPDATE / DELETE 没有 WHERE → 抛异常
new UpdateBuilder(table).Set(table.GetName(), "x").BuildSql();
// ❌ InvalidOperationException: UPDATE 必须指定 WHERE 条件

// 参数化防注入
qb.Where($"{sa.GetId()} = {sa.AsParam("Id")}").AddParameter("Id", 123);
// → WHERE SA.Id = @Id
```

## 和 SqlKata 的对比

| 维度 | EasySQL | SqlKata |
|------|---------|---------|
| 字段引用 | 编译期常量 | 运行时字符串 |
| 字段改名 | 编译器报错 | 测试才能发现 |
| 定义成本 | 一个 TableDef 类 | 无需定义 |
| 分页 | `BuildSql(limit, offset)` 一条 SQL | 手动写两次查询 |
| 临时表 | TempTableDef 内置 | 不支持 |
| JOIN | Schema 实例间定义 | 字符串拼接 |
| 适合场景 | 中大型项目，Schema 相对稳定 | 快速原型，一次性脚本 |

**这不是谁好谁坏的问题——是场景不同。** 如果你的项目有 50 张表、10 个开发者，字段改名是常事，EasySQL 的 TableDef 成本很快被类型安全收回来。如果只是写个一次性报表脚本，SqlKata 确实更快。

## 快速开始

```bash
dotnet add package EasySQL
```

```csharp
// 1. 定义 TableDef（或从数据库生成）
public class UserTableDef : TableDefBase { ... }

// 2. 构建查询
var sa = new UserTableDef("SA");
sa.Select(true, sa.Name, sa.Email);
var qb = new QueryBuilder().From(sa).Where(...);

// 3. 拿 SQL 去执行
string sql = qb.BuildSql();
var users = conn.Query<User>(sql);
```

## 开源 & 路线图

- GitHub: [github.com/jiangdf128/EasySQL](https://github.com/jiangdf128/EasySQL)
- NuGet: [EasySQL](https://www.nuget.org/packages/EasySQL)
- 许可证: MIT

正在计划的功能：
- [ ] CTE（Common Table Expression）
- [ ] 窗口函数
- [ ] 从数据库自动生成 TableDef 的 CLI 工具
- [ ] Source Generator 版本的编译期 SQL 校验

欢迎 Star、Issue、PR。
