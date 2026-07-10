# EasySQL 下划线（snake_case）字段命名支持方案分析报告

> 日期：2026-07-11
> 版本：v2（修正方向：常量写 snake_case，SELECT 用 AS 别名转 PascalCase）

---

## 一、场景定义

**数据库列是 snake_case**（`user_name`、`create_time`、`order_no`），**C# 代码想要 PascalCase**（`UserName`、`CreateTime`、`OrderNo`）。

期望效果：

```sql
-- SELECT：结果集列名是 PascalCase（通过 AS 别名）
SELECT u.user_name AS UserName, u.create_time AS CreateTime FROM users u

-- WHERE/JOIN：直接引用原始列名，不需要别名
WHERE u.user_name = @Name AND u.create_time >= @StartTime
```

---

## 二、现状分析

### 2.1 字段名流转路径

```
TableDef 常量字符串 → 属性 getter → GetXxx() / Select() → SQL
```

### 2.2 关键方法调用链

```
Select(field) ─────────────────────────────────────────────┐
Select(field, needPrefix) ─────────────────────────────────┤
Select(field, fieldAlias) ─────────────────────────────────┤→ GetSqlFormatField(field, alias, prefix)
Select(field, fieldAlias, needPrefix) ─────────────────────┘   → prefix + QuoteField(field) + AS alias
                                                                → 添加到 SelectFields 列表

QuoteField(field, needPrefix) ──→ GetSqlFormatField(field, "", needPrefix)  ← 问题！
QuoteField(field) ──────────────→ QuoteField(field, true)
```

### 2.3 核心问题：QuoteField 复用了 GetSqlFormatField

```csharp
// TableDefBase.cs:696-698
public string QuoteField(string field, bool needPrefix)
{
    return this.GetSqlFormatField(field, string.Empty, needPrefix);  // ← 第二个参数是 ""
}
```

```csharp
// TableDefBase.cs:122-131
private string GetSqlFormatField(string field, string fieldAlias, bool needPrefix)
{
    string prefix = ...;
    // ⚠️ 如果在这里加自动别名，会污染 WHERE/JOIN！
    string extractFieldAlias = ... ? " AS ..." : "";
    return $"{prefix}{QuoteField(field)}{extractFieldAlias}";
}
```

**`QuoteField`（用于 WHERE/JOIN/ORDER BY）和 `Select`（用于 SELECT 子句）共享同一个方法。** 如果在 `GetSqlFormatField` 中加自动别名逻辑，会导致 WHERE 子句也出现 `AS` 别名，产生非法 SQL。

### 2.4 调用点汇总

| 调用方 | 上下文 | 是否需要别名 | 当前实现 |
|--------|--------|-------------|----------|
| `Select()` 系列 | SELECT 列 | **是**（自动生成 PascalCase） | 走 `GetSqlFormatField` |
| `QuoteField()` → `GetXxx()` | WHERE / JOIN / ORDER BY / GROUP BY / HAVING | **否** | 走 `GetSqlFormatField`（复用） |
| `InsertBuilder.Insert()` | INSERT 列名 | **否** | 直接调 `SQLDialect.QuoteField` |
| `UpdateBuilder.Set()` | UPDATE SET 列名 | **否** | 直接调 `SQLDialect.QuoteField` |
| `SelectExpressionAlias()` | SELECT 表达式 | 用户显式指定 | 直接调 `SQLDialect.QuoteField` |

---

## 三、方案设计：SELECT 时自动 AS 别名

### 3.1 核心思路

1. **TableDef 常量值 = 数据库真实列名**（snake_case）
2. **`QuoteField` 与 `GetSqlFormatField` 解耦**：`QuoteField` 不再复用 `GetSqlFormatField`
3. **`GetSqlFormatField`（仅用于 SELECT）自动生成 PascalCase 别名**
4. **WHERE/JOIN 原样输出 snake_case**，干净无污染

### 3.2 转换方向

```
snake_case (DB)  →  PascalCase (C# 别名)
──────────────────────────────────────────
user_name        →  UserName
create_time      →  CreateTime
order_no         →  OrderNo
id               →  Id
total_amount     →  TotalAmount
is_deleted       →  IsDeleted
```

### 3.3 改动后的调用链

```
Select(field) ──────────────────────────────┐
Select(field, needPrefix) ──────────────────┤→ GetSqlFormatField(field, alias, prefix)
Select(field, fieldAlias) ──────────────────┤   → 如果没有显式 alias → 自动 snake→PascalCase
Select(field, fieldAlias, needPrefix) ──────┘   → SELECT user_name AS UserName ✅

QuoteField(field) ──→ 直接拼接：prefix + SQLDialect.QuoteField(field)
                       → WHERE u.user_name = @val ✅（无 AS 别名污染）
```

---

## 四、修改清单

### 4.1 新增文件

| 文件 | 说明 |
|------|------|
| `src/EasySQL/Utils/FieldNameConverter.cs` | snake_case ↔ PascalCase 转换 + 缓存 |

### 4.2 修改文件

| 文件 | 改动内容 | 影响范围 |
|------|---------|----------|
| `TableDefBase.cs` | ① `QuoteField` 不再委托给 `GetSqlFormatField`，改为直接拼接；② `GetSqlFormatField` 增加自动别名逻辑 | 核心改动 |
| `EasySQLOptions.cs` | 新增 `AutoAlias` 开关配置 | 可选配置 |
| `SQLDialectBase.cs` | `QuoteField` 行为不变（纯关键字转义） | 无变化 |

### 4.3 无需修改的文件

- ✅ `ISQLDialect` 接口 — 不变
- ✅ 6 个方言类 — 不变
- ✅ `InsertBuilder` / `UpdateBuilder` — 本来就直调 `SQLDialect.QuoteField`，无别名
- ✅ `QueryBuilder` — 通过 `Select()` 自动获得别名
- ✅ 现有一切测试 — `AutoAlias` 默认关闭

---

## 五、详细实现设计

### 5.1 FieldNameConverter（新增）

```csharp
namespace EasySQL
{
    /// <summary>
    /// 字段命名约定转换工具。snake_case ↔ PascalCase，带静态缓存。
    /// </summary>
    internal static class FieldNameConverter
    {
        private static readonly ConcurrentDictionary<string, string> _snakeToPascalCache = new();

        /// <summary>
        /// snake_case → PascalCase。如 "user_name" → "UserName"。
        /// </summary>
        public static string SnakeToPascalCase(string snakeCase)
        {
            if (string.IsNullOrEmpty(snakeCase) || !snakeCase.Contains('_'))
            {
                // 无下划线说明可能已经是 PascalCase 或其他格式，原样返回首字母大写
                if (snakeCase?.Length > 0)
                    return char.ToUpperInvariant(snakeCase[0]) + snakeCase.Substring(1);
                return snakeCase ?? string.Empty;
            }

            return _snakeToPascalCache.GetOrAdd(snakeCase, static s =>
            {
                var sb = new StringBuilder(s.Length);
                bool nextUpper = true;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '_')
                    {
                        nextUpper = true;
                    }
                    else
                    {
                        sb.Append(nextUpper ? char.ToUpperInvariant(s[i]) : s[i]);
                        nextUpper = false;
                    }
                }
                return sb.ToString();
            });
        }
    }
}
```

转换效果：

| snake_case | PascalCase | 说明 |
|------------|-----------|------|
| `user_name` | `UserName` | 标准两词 |
| `create_time` | `CreateTime` | 标准两词 |
| `id` | `Id` | 单词（无下划线，首字母大写） |
| `order_no` | `OrderNo` | 带缩写 |
| `is_deleted` | `IsDeleted` | 布尔前缀 |
| `total_amount` | `TotalAmount` | 标准两词 |

### 5.2 TableDefBase 改动

#### 改动 1：`QuoteField` 不再复用 `GetSqlFormatField`

```csharp
// 改前（当前代码）：
public string QuoteField(string field, bool needPrefix)
{
    return this.GetSqlFormatField(field, string.Empty, needPrefix);  // 共享方法，可能污染别名
}

// 改后：
public string QuoteField(string field, bool needPrefix)
{
    string prefix = (needPrefix && !string.IsNullOrWhiteSpace(this.Alias))
        ? $"{this.Alias.Trim()}."
        : string.Empty;
    return $"{prefix}{this.SQLDialect!.QuoteField(field.Trim())}";
}
```

> 这里只负责 **前缀 + 关键字转义**，不做别名处理。

#### 改动 2：`GetSqlFormatField` 增加自动别名逻辑

```csharp
private string GetSqlFormatField(string field, string fieldAlias, bool needPrefix)
{
    string prefix = (needPrefix && !string.IsNullOrWhiteSpace(this.Alias))
        ? $"{this.Alias.Trim()}."
        : string.Empty;

    // 如果没有显式指定别名，尝试自动生成
    if (string.IsNullOrWhiteSpace(fieldAlias))
    {
        // 1. 先查显式注册的 FieldAliases（保持现有行为）
        if (this.FieldAliases != null && this.FieldAliases.ContainsKey(field))
        {
            fieldAlias = this.FieldAliases[field];
        }
        // 2. 如果开启了 AutoAlias 且字段名包含下划线，自动转 PascalCase
        else if (AutoAlias && field.Contains('_'))
        {
            fieldAlias = FieldNameConverter.SnakeToPascalCase(field);
        }
    }

    string extractFieldAlias = !string.IsNullOrWhiteSpace(fieldAlias)
        ? $" AS {this.SQLDialect!.QuoteField(fieldAlias.Trim())}"
        : string.Empty;

    return $"{prefix}{this.SQLDialect!.QuoteField(field.Trim())}{extractFieldAlias}";
}
```

#### 改动 3：新增 `AutoAlias` 属性

```csharp
/// <summary>
/// 是否在 SELECT 时自动为 snake_case 字段生成 PascalCase 别名。
/// 默认 false（向后兼容）。通过 EasySQLOptions.AutoAlias 全局设置。
/// </summary>
public bool AutoAlias { get; set; } = false;
```

### 5.3 EasySQLOptions 改动

```csharp
public class EasySQLOptions
{
    // ... 现有属性 ...

    /// <summary>
    /// SELECT 时自动为 snake_case 字段生成 PascalCase 别名（如 user_name AS UserName）。
    /// 默认 false。
    /// </summary>
    public bool AutoAlias { get; set; } = false;
}
```

### 5.4 EasySQLContext 改动

在创建 `TableDefBase` 实例或将配置注入方言时，传递 `AutoAlias` 选项：

```csharp
// EasySQLContext 内部，TableDefBase 实例化后：
tableDef.AutoAlias = _options.AutoAlias;
```

---

## 六、使用示例

### 6.1 开启自动别名

```csharp
// Program.cs — 一次性配置
EasySQLContext.Default.Configure(new EasySQLOptions
{
    ConnectionString = "...",
    AutoAlias = true
});
```

### 6.2 TableDef：常量写 snake_case（数据库真实列名）

```csharp
public class UserTableDef : TableDefBase
{
    // 常量值 = 数据库真实列名（snake_case）
    public const string TABLE       = "users";
    public const string ID          = "id";
    public const string USER_NAME   = "user_name";
    public const string EMAIL       = "email";
    public const string CREATE_TIME = "create_time";
    public const string STATUS      = "status";

    public override string TableName => TABLE;

    public UserTableDef(string alias, ISQLDialect? dialect = null)
        : base(alias, dialect) { }

    // 属性返回原始列名（snake_case）
    public string Id         => ID;
    public string UserName   => USER_NAME;
    public string Email      => EMAIL;
    public string CreateTime => CREATE_TIME;
    public string Status     => STATUS;

    // GetXxx 用于 WHERE/JOIN，输出 snake_case（带前缀）
    public string GetId(bool p = true)         => QuoteField(ID, p);
    public string GetUserName(bool p = true)   => QuoteField(USER_NAME, p);
    public string GetEmail(bool p = true)      => QuoteField(EMAIL, p);
    public string GetCreateTime(bool p = true) => QuoteField(CREATE_TIME, p);
    public string GetStatus(bool p = true)     => QuoteField(STATUS, p);
}
```

### 6.3 使用效果

```csharp
var u = new UserTableDef("u");
var qb = new QueryBuilder();
qb.From(u);

// SELECT：自动生成 PascalCase 别名
u.Select(true, u.Id, u.UserName, u.Email, u.CreateTime);

// WHERE：直接用 snake_case 原列名
qb.Where($"{u.GetStatus()} = {u.AsParam("Status")}")
  .AddParameter("Status", 1);

string sql = qb.BuildSql();
```

**生成的 SQL：**

```sql
SELECT u.id AS Id,
       u.user_name AS UserName,
       u.email AS Email,
       u.create_time AS CreateTime,
       COUNT(*) OVER() AS TotalRows
FROM users u
WHERE (u.status = @Status)
```

### 6.4 Dapper 映射 — 自动匹配 C# Entity

```csharp
public class UserEntity
{
    public int Id { get; set; }           // ← SELECT ... AS Id 匹配
    public string UserName { get; set; }  // ← SELECT ... AS UserName 匹配
    public string Email { get; set; }     // ← SELECT ... AS Email 匹配
    public DateTime CreateTime { get; set; }
}

// Dapper 直接映射，无需额外配置
var users = connection.Query<UserEntity>(sql, qb.Parameters);
```

### 6.5 覆盖自动别名

```csharp
// 方式 1：显式指定别名
u.Select(u.UserName, "Name");  // → user_name AS Name

// 方式 2：通过 FieldAliases 注册
u.RegisterFieldAliases("user_name", "Name");

// 方式 3：SelectExpression 完全自己写
u.SelectExpression("u.user_name AS Name");
```

### 6.6 关闭自动别名（单表）

```csharp
public class LegacyUserTableDef : TableDefBase
{
    public LegacyUserTableDef(string alias, ISQLDialect? dialect = null)
        : base(alias, dialect)
    {
        this.AutoAlias = false;  // 此表不自动别名
    }
}
```

---

## 七、与其他方案的对比

### 方案 A（本报告推荐）：SELECT 自动 AS 别名

```
常量 = snake_case（DB 真实列名）
SELECT 自动 AS PascalCase
WHERE/JOIN 原样 snake_case
```

- ✅ 常量值与数据库一致，不会产生混淆
- ✅ WHERE/JOIN 是精确的 DB 列名，零出错
- ✅ Dapper 等 ORM 天然映射 PascalCase 结果列
- ✅ 实现简单，仅修改 TableDefBase 两个方法
- ✅ 默认关闭，完全向后兼容
- ✅ 可与 `FieldAliases` 字典共存

### 方案 B（之前的方向，不推荐）：QuoteField 转换 PascalCase → snake_case

```
常量 = PascalCase
QuoteField 自动转换成 snake_case
```

- ❌ 常量值与数据库不一致，排查问题困难
- ❌ 如果把 snake_case 常量传入会二次转换（虽然结果不变）
- ❌ QuoteField 既要处理 WHERE 又要处理 SELECT，职责混乱

### 方案 C：手动写 AS 别名

```csharp
u.Select(u.UserName, "UserName");  // 每处 Select 都要手写别名
```

- ❌ 每个 Select 调用都要写两次字段名
- ❌ `Select(true, fields...)` 多字段模式没法逐字段给别名

---

## 八、CodeSmith 模板配合

当前模板 `tools/CodeSmith/TableDef.cst` 生成的常量值就是 DB 列名：

```csharp
// 当前模板输出（DB 列名是什么，值就是什么）
public const string <%=column.Name.ToUpper()%> = "<%=column.Name%>";
```

对于 snake_case 数据库，模板天然输出正确结果：

```csharp
public const string USER_NAME = "user_name";  // DB 列名就是 "user_name"
```

**模板无需修改**，只需用户开启 `AutoAlias = true`，SELECT 时自动生成 PascalCase 别名。

---

## 九、实施计划

### 阶段 1：核心实现

| 步骤 | 文件 | 改动量 |
|------|------|--------|
| 1.1 | 新建 `src/EasySQL/Utils/FieldNameConverter.cs` | ~40 行 |
| 1.2 | `TableDefBase.cs` — `QuoteField` 独立实现 | 改 5 行 |
| 1.3 | `TableDefBase.cs` — `GetSqlFormatField` 加自动别名 | 加 8 行 |
| 1.4 | `TableDefBase.cs` — 加 `AutoAlias` 属性 | 加 3 行 |
| 1.5 | `EasySQLOptions.cs` — 加 `AutoAlias` 配置 | 加 3 行 |
| 1.6 | `EasySQLContext.cs` — 传递 `AutoAlias` 到 TableDef | 加 3 行 |

### 阶段 2：测试

| 步骤 | 内容 |
|------|------|
| 2.1 | `FieldNameConverter` 单元测试（snake→Pascal 转换正确性） |
| 2.2 | `SqlGenerationTests` 新增 AutoAlias 场景验证 |
| 2.3 | 回归：确保 AutoAlias=false 时所有现有测试 100% 通过 |

### 阶段 3：文档

| 步骤 | 内容 |
|------|------|
| 3.1 | 更新 CLAUDE.md 添加 `AutoAlias` 使用说明 |

---

## 十、总结

**推荐方案：SELECT 时自动为 snake_case 字段生成 PascalCase 的 AS 别名。**

核心思想一句话：**常量写真实 DB 列名（snake_case），SELECT 自动 AS 转 PascalCase，WHERE/JOIN 原样引用**。

- 改动范围：约 60 行新代码 + 约 20 行修改
- 兼容性：默认关闭，完全向后兼容
- 开发时间：约 2-3 小时（含测试）
