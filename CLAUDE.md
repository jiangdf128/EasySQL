# CLAUDE.md

此文件为 Claude Code（claude.ai/code）提供在此仓库中工作的指导。

## 构建和测试

```bash
# 还原依赖并构建
dotnet build

# 运行测试
dotnet test

# 清理后重新构建
dotnet clean && dotnet build
```

## 项目架构

EasySQL 是一个轻量级跨数据库 SQL 查询构建库，核心理念：**用 C# 流式 API 生成方言感知的 SQL 字符串**，不执行 SQL，只输出 SQL 语句和参数。

### 核心层次

```
用户代码
  └─ SchemaBase（子类）        ← 用户定义的每张表的 Schema
       ├─ 列名常量 + QuoteField 方法
       └─ Select / Join 方法
  └─ QueryBuilder              ← 主查询构建器（继承 SchemaBase）
       ├─ From / Where / GroupBy / OrderBy / Having
       ├─ Union / Exists / NotExists
       ├─ BuildSql() / BuildCountSql()
       └─ Parameters 字典（防 SQL 注入）
  └─ InsertBuilder / UpdateBuilder / DeleteBuilder
       └─ 各自有 Parameters 字典

SQL 方言层（为上述构建器服务）
  └─ ISQLDialect → SQLDialectBase → 7 种具体方言
       └─ IDbFunction → DbFunctionBase → 7 种函数实现
       └─ SQLDialectFactory（根据 IDbConnection 类型自动匹配方言）
```

### 关键类职责

| 类 | 职责 |
|----|------|
| `SchemaBase` | 表结构基类，提供 Select/Join/QuoteField，构造函数接受别名和方言 |
| `QueryBuilder` | 完整 SELECT 构建器，含分页、Union、子查询、Exists |
| `SQLDialectBase.BuildSql()` | 核心 SQL 生成逻辑（含 6 种数据库的分页处理） |
| `SQLDialectFactory` | 根据连接类型名（如 `sqlconnection`）自动选择方言，默认 SQL Server |
| `DbContext` | 静态全局数据库上下文，管理连接和事务 |
| `InBuilderBase<T>` | 通用 IN 子句值构造器，支持分节（应对 Oracle 限制） |

### 方言与函数

7 种方言：SqlServer / MySQL / PostgreSQL / Oracle / SQLite / Jet（Access）/ DB2

每种方言关联一个 `IDbFunction` 实现，封装了该数据库的：
- 字符串函数：`Concat`、`SubString`、`CharIndex`、`Replace`、`Quote`
- 日期函数：`DateAdd`、`DateDiff`、`Year`、`Month`、`Sysdate`
- 数学函数：`Round`、`Floor`、`BitAnd`、`BitOr`

### 线程安全

- QueryBuilder 使用 `_lock` 专用锁对象，不用 `lock(this)`
- `BuildCountSql()` 不修改实例状态
- `SQLDialectFactory.DefaultDialect` 使用双重检查锁
- 各 Builder 的 `Parameters` 字典写入都有锁保护

### 参数化查询（防 SQL 注入）

```csharp
var qb = new QueryBuilder()
    .From(user)
    .Where("Id = @UserId AND Status = @Status")
    .AddParameter("UserId", 123)
    .AddParameter("Status", 1);
// SQL 和 Parameters 分离，调用者用 Dapper/ADO.NET 安全执行
```

### 项目结构

```
src/EasySQL/          — 核心库
  Abstractions/       — 6 个接口（ISQLDialect、IDbFunction 等）
  Base/               — 4 个抽象基类（SchemaBase、SQLDialectBase 等）
  Dialects/           — 8 个方言文件（含工厂类）
  Functions/          — 8 个数据库函数实现
tests/EasySQL.Test/   — xunit 测试项目
  Entities/           — 实体类（Dapper.Contrib 映射）
  Schemas/            — Schema 类（继承 SchemaBase）
```

### 注意事项

- 项目禁用 `ImplicitUsings`、启用 `Nullable`
- 目标框架 net10.0，注释全部使用中文
- `ISQLDialect` 属性可为 null（构造函数内自动回退到默认方言）
- 连接类型匹配基于 `IDbConnection.GetType().Name.ToLower()`，例如 Npgsql 的 `npgsqlconnection`
