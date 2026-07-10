# CLAUDE.md

此文件为 Claude Code（claude.ai/code）提供在此仓库中工作的指导。

## 安装

```bash
dotnet add package EasySQL.SqlBuilder
```

## 构建和测试

```bash
dotnet build                              # 构建
dotnet test                               # 运行全部测试（3 框架并行）
dotnet test -f net10.0                    # 单框架测试（避免数据库冲突）
dotnet clean && dotnet build              # 清理后重新构建
```

## 项目架构

EasySQL 是一个轻量级跨数据库 SQL 查询构建库，核心理念：**用 C# 流式 API 生成方言感知的 SQL 字符串**，不执行 SQL，只输出 SQL 语句和参数。

### 核心层次

```
用户代码
  ├─ EasySQLContext.Default    ← 全局单例（管理连接和方言）
  │     └─ IEasySQLContext     ← DI 注入接口
  ├─ TableDefBase（子类）        ← 用户定义的每张表的 TableDef
  │     ├─ 列名常量 + 实例属性（如 sa.Id）
  │     ├─ GetXxx() 方法 → QuoteField（带引号/前缀）
  │     └─ Select / Join 方法
  ├─ QueryBuilder              ← 主查询构建器（继承 TableDefBase）
  │     ├─ From / Where / GroupBy / OrderBy / Having
  │     ├─ Union / Exists / NotExists
  │     ├─ BuildSql(rowLimit, rowOffset) → 自动注入 TotalRows
  │     ├─ BuildCountSql()
  │     └─ Parameters 字典（防 SQL 注入）
  └─ InsertBuilder / UpdateBuilder / DeleteBuilder
        └─ 各自有 Parameters 字典，BuildSql() 无 WHERE 抛异常

SQL 方言层
  └─ ISQLDialect → SQLDialectBase → 6 种具体方言
       ├─ SqlServer / MySQL / PostgreSQL / Oracle / SQLite / DB2
       ├─ IDbFunction → DbFunctionBase → 6 种函数实现
       └─ SQLDialectFactory（根据 IDbConnection 类型自动匹配方言）
```

### 关键类职责

| 类 | 职责 |
|----|------|
| `EasySQLContext` | 数据库上下文实例，持有连接配置。`Default` 静态单例供全局使用 |
| `IEasySQLContext` | DI 接口，支持单例注入 |
| `TableDefBase` | 表结构基类，提供 Select/Join/QuoteField，构造函数接受别名和方言 |
| `QueryBuilder` | 完整 SELECT 构建器，含分页（自动注入 COUNT OVER）、Union、Exists |
| `SQLDialectBase.BuildSql()` | 核心 SQL 生成逻辑（含 6 种数据库的分页处理） |
| `SQLDialectFactory` | 根据连接类型名自动选择方言，默认 SQL Server |
| `InsertBuilder` | INSERT 构建器，支持 INSERT INTO ... SELECT |
| `UpdateBuilder` / `DeleteBuilder` | 更新/删除构建器，无 WHERE 时抛异常防止误操作 |

### 方言与函数

6 种方言：SqlServer / MySQL / PostgreSQL / Oracle / SQLite / DB2

每种方言关联一个 `IDbFunction` 实现，封装了该数据库的：
- 字符串函数：`Concat`、`SubString`、`CharIndex`、`Replace`、`Quote`
- 日期函数：`DateAdd`、`DateDiff`、`Year`、`Month`、`Sysdate`
- 数学函数：`Round`、`Floor`、`BitAnd`、`BitOr`

### 分页（重要）

调用 `BuildSql(rowLimit, rowOffset)` 时自动注入 `COUNT(*) OVER() AS TotalRows`，一次查询同时返回分页数据和总记录数：

```csharp
string sql = qb.BuildSql(rowLimit: 10, rowOffset: 5);
// → SELECT ...,COUNT(*) OVER() AS TotalRows FROM ... OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY
// 调用方从结果第一行的 TotalRows 列读取总数
```

分页语法按方言自动选择：SQL Server/Oracle/DB2 → `OFFSET/FETCH`，MySQL/PostgreSQL/SQLite → `LIMIT/OFFSET`。

### 数据库上下文

```csharp
// 非 DI 场景
EasySQLContext.Default.Configure(new EasySQLOptions { ... });
EasySQLContext.Default.Do(conn => { ... });

// DI 场景
services.AddEasySQL(options => ...);
// 注入 IEasySQLContext，即为 Default 单例
```

每次 `Open()` 自动调用 `SQLDialectFactory.UseDialect(con)` 对齐方言，确保连接什么库就生成对应的 SQL 语法。

### TableDef 标准写法

```csharp
public class UserTableDef : TableDefBase
{
    public const string TABLE = "Users";
    public const string ID = "Id";
    public const string NAME = "Name";

    public override string TableName => TABLE;
    public UserTableDef(string alias, ISQLDialect? dialect = null) : base(alias, dialect) { }
    public UserTableDef() : this(string.Empty, null) { }

    // 实例属性 — 返回原始字段名
    public string Id => ID;
    public string Name => NAME;

    // GetXxx 方法 — 返回带引号/前缀
    public string GetId(bool needPrefix = true) => QuoteField(ID, needPrefix);
    public string GetName(bool needPrefix = true) => QuoteField(NAME, needPrefix);
}

// 多字段选取（标准写法）
sa.Select(true, sa.Id, sa.Name);

// WHERE 中引用
qb.Where($"{sa.GetStatus()} = {sa.AsParam("Status")}").AddParameter("Status", 1);
```

### 下划线字段自动别名（默认关闭）

TableDef 常量写数据库真实列名（snake_case），开启 `AutoAlias` 后 SELECT 自动生成 PascalCase 别名：

```sql
-- AutoAlias=true 时 SELECT 自动加 AS PascalCase
SELECT u.user_name AS UserName, u.create_time AS CreateTime FROM users u

-- WHERE/JOIN：原样引用，无别名污染
WHERE u.user_name = @Name
```

控制开关（默认 `false`）：
```csharp
// 全局设置
EasySQLContext.AutoAlias = true;    // 开启
EasySQLContext.AutoAlias = false;   // 关闭（默认）

// 或通过配置
EasySQLContext.Default.Configure(new EasySQLOptions { AutoAlias = true });
```

**使用 Dapper + `DefaultTypeMap.MatchNamesWithUnderscores` 的用户无需开启此选项。**

### 项目结构

```
src/EasySQL/              — 核心库
  Abstractions/           — 接口（ISQLDialect、IDbFunction 等）
  Base/                   — 抽象基类（TableDefBase、SQLDialectBase）
  Dialects/               — 6 种方言 + 工厂类
  Functions/              — 6 种数据库函数实现
tests/EasySQL.Test/       — 测试项目
  Entities/               — 5 张电商实体（User/Product/Order/OrderItem/OrderPayment）
  TableDefs/                — 5 个 TableDef 类
  Infrastructure/         — 测试基础（TestBase、DbProxy、Dapper 桥接扩展）
  Tests/
    Integration/          — 集成测试（真实 SQL Server 数据库）
    SqlGenerationTests.cs — SQL 生成单元测试
  SqlDemo.cs              — 演示程序（可直接运行）
  ddl/sqlserver/          — SQL Server DDL 脚本
```

### 测试说明

- **集成测试**需本地 SQL Server（Windows 集成验证），数据库 `EasySqlTest`，用 DDL 脚本建表
- **单框架测试**：`dotnet test -f net10.0`（多框架并行会共享数据库导致冲突）
- 单表 CRUD → Dapper.Contrib；复杂查询/批量操作 → EasySQL

### 注意事项

- 项目禁用 `ImplicitUsings`、启用 `Nullable`
- 多目标框架：`net8.0;net9.0;net10.0`
- 注释全部使用中文
- `ISQLDialect` 属性可为 null（构造函数内自动回退到默认方言）
- 连接类型匹配基于 `IDbConnection.GetType().Name.ToLower()`
