using System;
using System.Collections.Generic;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// SQL 查询构建器，是 EasySQL 的核心入口。
    /// 通过流式 API 构建<strong>类型安全的 SQL 语句</strong>——
    /// 用 TableDef 的字段 getter 方法代替硬编码字符串，字段名变更时编译期即可发现错误。
    /// </summary>
    /// <remarks>
    /// <h3>标准写法</h3>
    /// <code>
    /// // ① 实例化 TableDef（多表按 sa/sb/sc 命名，别名全大写）
    /// var sa = new UserTableDef("SA");
    /// var sb = new OrderTableDef("SB");
    ///
    /// // ② 在各 TableDef 上定义 SELECT 字段
    /// sa.Select(true, sa.Name, sa.Email);
    /// sb.Select(true, sb.Amount);
    ///
    /// // ③ TableDef 之间定义 JOIN 关系
    /// sa.Join(sb, $"{sa.GetId()} = {sb.GetUserId()}");
    ///
    /// // ④ QueryBuilder 一次性 FROM，然后 WHERE / ORDER BY
    /// var qb = new QueryBuilder()
    ///     .From(sa, sb)
    ///     .Where($"{sa.GetStatus()} = {sa.AsParam("Status")}")
    ///     .AddParameter("Status", 1)
    ///     .OrderBy($"{sa.GetName()} ASC");
    ///
    /// string sql = qb.BuildSql();
    ///
    /// // ⑤ 执行（配合 Dapper）
    /// var users = conn.Query&lt;User&gt;(sql, qb.Parameters.ToDynamicParameters());
    /// </code>
    /// </remarks>
    public class QueryBuilder:TableDefBase
    {
        private readonly object _lock = new object();
        private List<KeyValuePair<QueryBuilder,bool>>? _unionList = null;
        private List<QueryBuilder>? _existsList=null;
        private List<QueryBuilder>? _notExistsList=null;

        private StringBuilder? _wherebuilder=null;
        private StringBuilder? _groupbybuilder=null;
        private StringBuilder? _orderbybuilder=null;
        private StringBuilder? _havingbuilder=null;

        //在做嵌套SQL的计数查询的时候，去掉Order排序。
        private bool _isCountInner=false;

        /// <summary>
        /// 参数化查询的参数集合，用于收集 WHERE 条件中的参数名与值。
        /// 调用 BuildSql() 后可通过此属性获取所有已注册的参数。
        /// </summary>
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();

        internal List<KeyValuePair<QueryBuilder, bool>>? UnionList => _unionList;
        internal List<QueryBuilder>? ExistsList => _existsList;
        internal List<QueryBuilder>? NotExistsList => _notExistsList;

        internal StringBuilder? Wherebuilder => _wherebuilder;
        internal StringBuilder? Groupbybuilder => _groupbybuilder;
        internal StringBuilder? Orderbybuilder => _orderbybuilder;
        internal StringBuilder? Havingbuilder => _havingbuilder;

        /// <summary>
        /// 是否为计数查询的内层子查询。内层子查询不需要 ORDER BY（否则派生表里会多一次无意义的排序）。
        /// </summary>
        internal bool IsCountInner => _isCountInner;

        /// <summary>
        /// 是否抑制 ORDER BY 输出。构建 UNION 的各个分支时置为 true——
        /// UNION 的排序必须写在合并后的语句上，不能写在单个分支里。
        /// </summary>
        internal bool SuppressOrderBy { get; set; }

        /// <summary>
        /// 分页查询时自动注入的计数列别名。调用 <see cref="BuildSql(int, int, bool)"/> 分页时，
        /// SQL 中会追加 <c>COUNT(*) OVER() AS TotalRows</c>，调用方可从返回结果第一行读取该列获取总记录数。
        /// </summary>
        public const string PagingTotalAlias = "TotalRows";

        /// <summary>
        /// 是否需要对构造返回的Sql语句节使用换行，以提高可读性。
        /// </summary>
        public bool PrettyPrint { get; set; }

        /// <summary>
        /// 是否为Distinct语句。
        /// </summary>
        public bool IsDistinct { get; set; }

        /// <summary>
        /// 读取该查询的目标表（或视图、子查询）的列表。
        /// </summary>
        public List<TableDefBase> FromItems { get; protected set; }

        /// <summary>
        /// 创建一个<see cref="QueryBuilder"/>实例。
        /// </summary>
        /// <param name="alias">作为子查询的别名参数。</param>
        /// <param name="dialect">SQL方言。</param>
        public QueryBuilder(string alias, ISQLDialect? dialect)
            : base(alias, dialect)
        {
            this.FromItems = new List<TableDefBase>();
            this.PrettyPrint = true;
        }

        /// <summary>
        /// 创建一个<see cref="QueryBuilder"/>实例。
        /// </summary>
        /// <param name="alias">作为子查询的别名参数。</param>
        public QueryBuilder(string alias)
            : this(alias, null)
        {
        }

        /// <summary>
        /// 创建一个<see cref="QueryBuilder"/>实例。
        /// </summary>
        public QueryBuilder()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// 指定查询数据源（FROM 子句）。支持多表、视图和子查询。
        /// <strong>多个 TableDef 之间如有 JOIN 关系，应先在各 TableDef 上调用 Join/LeftJoin，再一次性 From。</strong>
        /// </summary>
        /// <example>
        /// <code>
        /// // 单表
        /// qb.From(sa);
        ///
        /// // 多表（JOIN 关系在 TableDef 上定义）
        /// sa.Join(sb, $"{sa.GetId()} = {sb.GetUserId()}");
        /// qb.From(sa, sb);
        ///
        /// // 子查询
        /// var subQb = new QueryBuilder("sub")
        ///     .From(sa)
        ///     .Where($"{sa.GetStatus()} = {sa.AsParam("Status")}")
        ///     .AddParameter("Status", 1);
        /// qb.From(subQb);
        /// </code>
        /// </example>
        /// <param name="items">数据源 TableDef 列表。</param>
        public QueryBuilder From( params TableDefBase[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Check(items[i]);
                if (items[i] != null)
                {
                    this.FromItems.Add(items[i]);
                }
            }
            return this;
        }

        /// <summary>
        /// 添加 WHERE 条件，多个条件以 AND 连接。
        /// <strong>必须使用 TableDef 字段 getter + 插值写法</strong>，回避硬编码字段名。
        /// </summary>
        /// <example>
        /// <code>
        /// // 静态条件
        /// qb.Where($"{sa.GetStatus()} = 1");
        ///
        /// // 参数化条件（防注入）— 配合 AsParam + AddParameter
        /// qb.Where($"{su.GetStatus()} = {su.AsParam("Status")}",
        ///          $"{su.GetName()} LIKE {su.AsParam("Name")}")
        ///    .AddParameter("Status", 1)
        ///    .AddParameter("Name", "%张%");
        ///
        /// // BETWEEN、IN 等
        /// qb.Where($"{su.GetId()} BETWEEN {su.AsParam("MinId")} AND {su.AsParam("MaxId")}")
        ///    .AddParameter("MinId", 1)
        ///    .AddParameter("MaxId", 1000);
        /// </code>
        /// </example>
        /// <param name="items">WHERE 条件表达式，多个以 AND 连接。</param>
        public QueryBuilder Where(params string[] items)
        {
            BuildClause(ref _wherebuilder, " AND ", items);
            return this;
        }

        /// <summary>
        /// 添加查询条件子句。
        /// </summary>
        /// <param name="format">条件子句占位符字符串。</param>
        /// <param name="args"></param>
        public QueryBuilder WhereFormat(string format, params object[] args)
        {
            return this.Where(string.Format(format, args));
        }

        /// <summary>
        /// 注册一个参数化查询参数，用于防止 SQL 注入。
        /// 在 WHERE 条件中用 <see cref="TableDefBase.AsParam"/> 生成占位符，然后用本方法注册参数值。
        /// </summary>
        /// <example>
        /// <code>
        /// qb.Where($"{su.GetId()} = {su.AsParam("UserId")} AND " +
        ///          $"{su.GetStatus()} = {su.AsParam("Status")}")
        ///    .AddParameter("UserId", 123)
        ///    .AddParameter("Status", 1);
        ///
        /// // 配合 Dapper 执行参数化查询
        /// var dp = qb.Parameters.ToDynamicParameters();
        /// var users = conn.Query&lt;User&gt;(sql, dp);
        /// </code>
        /// </example>
        /// <param name="name">参数名称（不含 @ / : 前缀）。</param>
        /// <param name="value">参数值。</param>
        public QueryBuilder AddParameter(string name, object value)
        {
            lock (_lock) { Parameters[name] = value; }
            return this;
        }

        /// <summary>
        /// 注册多个参数化查询参数。
        /// </summary>
        /// <param name="parameters">以参数名（不含 @ 前缀）为键的参数字典。</param>
        public QueryBuilder AddParameters(IDictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                lock (_lock)
                {
                    foreach (var kv in parameters)
                        Parameters[kv.Key] = kv.Value;
                }
            }
            return this;
        }

        /// <summary>
        /// 清除所有已注册的参数。
        /// </summary>
        public QueryBuilder ClearParameters()
        {
            Parameters.Clear();
            return this;
        }
        /// <summary>
        /// 添加 GROUP BY 分组。
        /// </summary>
        /// <example>
        /// <code>
        /// su.Select(su.GetStatus());
        /// su.SelectExpression("Count(1) AS Cnt");
        /// qb.From(su)
        ///    .GroupBy($"{su.GetStatus()}")
        ///    .Having($"Count(1) > {su.AsParam("MinCnt")}")
        ///    .AddParameter("MinCnt", 5);
        /// </code>
        /// </example>
        /// <param name="items">分组列表达式。</param>
        public QueryBuilder GroupBy(params string[] items)
        {
            BuildClause(ref _groupbybuilder, ",", items);
            return this;
        }

        /// <summary>
        /// 清除GroupBy子句。
        /// </summary>
        public QueryBuilder ClearGroupBy()
        {
            _groupbybuilder = new StringBuilder();
            return this;
        }

        /// <summary>
        /// 清除Orderby子句。
        /// </summary>
        public QueryBuilder ClearOrderBy()
        {
            _orderbybuilder = new StringBuilder();
            return this;
        }

        /// <summary>
        /// 清除掉TableDef中的Selected Fields.
        /// </summary>
		public QueryBuilder ClearTableDefItemSelectFields()
		{
		    for (int i = 0; i < this.FromItems.Count; i++)
		    {
		        this.FromItems[i].ClearSelect();
		    }
            return this;
		}

        /// <summary>
        /// 添加 HAVING 分组过滤条件，多个条件以 AND 连接。
        /// </summary>
        /// <example>
        /// <code>
        /// qb.GroupBy($"{su.GetStatus()}")
        ///    .Having($"Count(1) > {su.AsParam("MinCount")}")
        ///    .AddParameter("MinCount", 5);
        /// </code>
        /// </example>
        /// <param name="items">HAVING 条件表达式。</param>
        public QueryBuilder Having(params string[] items)
        {
            BuildClause(ref _havingbuilder, " AND ", items);
            return this;
        }
        /// <summary>
        /// 添加 ORDER BY 排序。
        /// </summary>
        /// <example>
        /// <code>
        /// qb.OrderBy($"{su.GetName()} ASC",
        ///            $"{su.GetCreateTime()} DESC");
        /// </code>
        /// </example>
        /// <param name="items">排序表达式，如 "Name ASC"、"CreateTime DESC"。</param>
        public QueryBuilder OrderBy(params string[] items)
        {
            BuildClause(ref _orderbybuilder, ",", items);
            return this;
        }

        /// <summary>
        /// 添加 UNION / UNION ALL 合并查询。
        /// <strong>子查询只生成 SQL，参数统一在主 Builder 上注册。</strong>
        /// </summary>
        /// <example>
        /// <code>
        /// // 子查询只定义结构，不注册参数
        /// var qb1 = new QueryBuilder()
        ///     .From(su)
        ///     .Where($"{su.GetStatus()} = {su.AsParam("Active")}");
        /// var qb2 = new QueryBuilder()
        ///     .From(su)
        ///     .Where($"{su.GetStatus()} = {su.AsParam("Inactive")}");
        ///
        /// // 参数统一在主 Builder 上注册
        /// qb1.Union(qb2, isUnionAll: true);
        /// qb1.AddParameter("Active", 1)
        ///    .AddParameter("Inactive", 0);
        ///
        /// string sql = qb1.BuildSql();
        /// var users = conn.Query&lt;User&gt;(sql, qb1.Parameters.ToDynamicParameters());
        /// </code>
        /// </example>
        /// <param name="queryBuilder">要合并的查询（不能是自身）。</param>
        /// <param name="isUnionAll">true 用 UNION ALL，false 用 UNION。</param>
        public QueryBuilder Union(QueryBuilder queryBuilder, bool isUnionAll)
        {
            if (queryBuilder!=null && queryBuilder != this)
            {
                if (this._unionList == null)
                {
                    this._unionList = new List<KeyValuePair<QueryBuilder, bool>>();
                }
                this._unionList.Add(new KeyValuePair<QueryBuilder, bool>(queryBuilder, isUnionAll));
            }
            return this;
        }

        /// <summary>
        /// 添加 EXISTS 子查询条件。
        /// </summary>
        /// <example>
        /// <code>
        /// // 查询有订单的用户
        /// var subQb = new QueryBuilder()
        ///     .From(sb)
        ///     .Where($"{sb.GetUserId()} = {sa.GetId()}");
        /// var qb = new QueryBuilder()
        ///     .From(sa)
        ///     .Exists(subQb);
        /// </code>
        /// </example>
        /// <param name="queryBuilder">EXISTS 内部的子查询。</param>
        public QueryBuilder Exists(QueryBuilder queryBuilder)
        {
            if (queryBuilder != this)
            {
                if (_existsList == null)
                {
                    _existsList = new List<QueryBuilder>(1);
                }
                this._existsList.Add(queryBuilder);
            }
            return this;
        }

        /// <summary>
        /// 添加Not Exists查询条件。
        /// </summary>
        /// <param name="queryBuilder"></param>
        public QueryBuilder NotExists(QueryBuilder queryBuilder)
        {
            if (queryBuilder != this)
            {
                if (_notExistsList == null)
                {
                    _notExistsList = new List<QueryBuilder>();
                }
                this._notExistsList.Add(queryBuilder);
            }
            return this;
        }

        internal static void BuildClause(ref StringBuilder? builder, string seprator, params string[] items)
        {
            if (builder == null)
            {
                builder = new StringBuilder();
            }
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null && items[i] != string.Empty)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(seprator);
                    }
                    builder.Append(items[i].Trim());
                }
            }
        }

        /// <summary>
        /// 构造SQL语句。
        /// </summary>
        /// <returns>根据自身包括的查询对象，返回可执行的Sql查询语句。</returns>
        public string BuildSql()
        {
            lock (_lock)
            {
                return this.BuildSql(0, 0, false, false);
            }
        }

        /// <summary>
        /// 构造完整的 SQL 查询语句，含分页。
        /// </summary>
        /// <example>
        /// <code>
        /// // 基本查询
        /// string sql = qb.BuildSql();
        ///
        /// // 分页查询（SQL Server/Oracle → OFFSET/FETCH，MySQL/PostgreSQL → LIMIT/OFFSET）
        /// string pagedSql = qb.BuildSql(rowLimit: 20, rowOffset: 0);
        ///
        /// // 分页查询但不返回总记录数（不注入 COUNT(*) OVER()，性能更好）
        /// string pagedSql2 = qb.BuildSql(rowLimit: 20, rowOffset: 0, includeTotalCount: false);
        ///
        /// // 配合 Dapper 执行
        /// var users = conn.Query&lt;User&gt;(sql, qb.Parameters.ToDynamicParameters());
        /// </code>
        /// </example>
        /// <param name="rowLimit">返回记录数上限，0 表示不限制。</param>
        /// <param name="rowOffset">偏移行数，从 0 开始。</param>
        /// <param name="includeTotalCount">是否注入 <c>COUNT(*) OVER() AS TotalRows</c> 总数列。
        /// 默认为 true：一次查询同时返回分页数据和总记录数，调用方从结果第一行的 TotalRows 列读取总数。
        /// 设为 false：只做分页不返回总数，避免窗口函数带来的性能开销，
        /// 调用方需配合 <see cref="BuildCountSql"/> 另外执行一次计数查询。</param>
        public string BuildSql(int rowLimit, int rowOffset=0, bool includeTotalCount=true)
        {
            lock (_lock)
            {
                return this.BuildSql(rowLimit, rowOffset, false, includeTotalCount);
            }
        }

        /// <summary>
        /// 构造Sql 的Count(0)语句。
        /// </summary>
        /// <remarks>
        /// 有 GROUP BY 或 DISTINCT 时，用子查询包装实现计数（分别统计分组数与去重后的行数）；
        /// 其余情况直接生成 <c>SELECT Count(1) AS RowsCount ...</c>，性能最好。
        /// </remarks>
        /// <returns>根据自身包括的查询对象，返回可执行的Sql Count查询语句。</returns>
        public string BuildCountSql()
        {
            bool needWrap = (this._groupbybuilder != null && this._groupbybuilder.Length > 0) || this.IsDistinct;
            if (!needWrap)
            {
                lock (_lock)
                {
                    return this.BuildSql(0, 0, true, false);
                }
            }

            // 有 GROUP BY 或 DISTINCT 时，用子查询包装实现计数。锁内操作保证线程安全。
            lock (_lock)
            {
                string safeAlias = string.IsNullOrWhiteSpace(this.Alias) ? "A" : this.Alias;
                bool saved = this._isCountInner;
                // 内层子查询不需要 ORDER BY
                this._isCountInner = true;
                try
                {
                    // 先构建内层 SQL（SELECT ... FROM ... GROUP BY ...）
                    string innerSql = this.BuildSql(0, 0, false, false);
                    // 外层包装：SELECT Count(1) FROM (innerSql) AS alias
                    return $"SELECT Count(1) AS RowsCount{PrettyPrintLine()}FROM ({innerSql}) {safeAlias}";
                }
                finally
                {
                    this._isCountInner = saved;
                }
            }
        }

        private string PrettyPrintLine()
        {
            return this.PrettyPrint ? System.Environment.NewLine : " ";
        }

        /// <summary>
        /// 构建与 Dapper.SqlBuilder 配合使用的 SQL 模板。
        /// 生成的 SQL 中 WHERE 和 ORDER BY 位置由 <c>/**where**/</c> 和 <c>/**orderby**/</c>
        /// 占位符替代，供 Dapper.SqlBuilder 在运行时动态填充，而静态的 SELECT/FROM/JOIN 仍由 EasySQL 管理。
        /// </summary>
        /// <example>
        /// <code>
        /// su.Select(su.GetName(), su.GetEmail());
        /// var qb = new QueryBuilder().From(su);
        /// string template = qb.BuildTemplateSql();
        /// // → "SELECT u.Name,u.Email FROM Users u /**where**/ /**orderby**/"
        ///
        /// var builder = new Dapper.SqlBuilder();
        /// var t = builder.AddTemplate(template);
        /// builder.Where($"{su.GetStatus()} = @Status", new { Status = 1 });
        /// builder.OrderBy($"{su.GetName()} ASC");
        /// var users = conn.Query&lt;User&gt;(t.RawSql, t.Parameters);
        /// </code>
        /// </example>
        public string BuildTemplateSql()
        {
            lock (_lock)
            {
                var savedWhere = _wherebuilder;
                var savedOrderBy = _orderbybuilder;
                _wherebuilder = null;
                _orderbybuilder = null;

                try
                {
                    // 模板不是分页查询，不需要总数列
                    string sql = BuildSql(0, 0, false, false);
                    string sep = this.PrettyPrint ? System.Environment.NewLine : " ";
                    // UNION 查询需要包裹为派生表，否则占位符会落在最后一个分支后面
                    if (this._unionList != null && this._unionList.Count > 0)
                    {
                        string alias = string.IsNullOrWhiteSpace(this.Alias) ? "A" : this.Alias;
                        sql = $"SELECT * FROM ({sql}) {alias}";
                    }
                    return $"{sql}{sep}/**where**/{sep}/**orderby**/";
                }
                finally
                {
                    _wherebuilder = savedWhere;
                    _orderbybuilder = savedOrderBy;
                }
            }
        }

        /// <summary>
        /// 构建SQL查询。
        /// </summary>
        /// <param name="rowLimit">返回最大可能的记录数。</param>
        /// <param name="rowOffset">偏移位置（从0行开始算起）。</param>
        /// <param name="forCount">是否翻译成Count语句。</param>
        /// <param name="includeTotalCount">分页时是否注入 COUNT(*) OVER() 总数列。</param>
        /// <returns></returns>
        private string BuildSql(int rowLimit, int rowOffset, bool forCount, bool includeTotalCount)
        {
            return this.SQLDialect!.BuildSql(this, rowLimit, rowOffset, forCount, includeTotalCount);
        }

        /// <summary>
        /// 以抑制 ORDER BY 的方式构建该查询——用于构建 UNION 的各个分支
        /// （UNION 的排序必须写在合并后的语句上，不能写在单个分支里）。
        /// </summary>
        internal string BuildSqlWithoutOrderBy()
        {
            lock (_lock)
            {
                this.SuppressOrderBy = true;
                try
                {
                    // 分支不是分页查询，不需要总数列
                    return this.BuildSql(0, 0, false, false);
                }
                finally
                {
                    this.SuppressOrderBy = false;
                }
            }
        }

        /// <summary>
        /// 生成 UNION 查询外层（派生表）的 ORDER BY 子句。
        /// UNION 合并后的结果集，其列名来自首个分支的 SELECT 列表，
        /// 因此调用方写的排序表达式（如 <c>u.Name</c>）需要映射为结果集的输出列名。
        /// </summary>
        /// <param name="tableAlias">派生表的别名。</param>
        /// <returns>ORDER BY 子句内容（不含 "ORDER BY" 关键字），没有排序时返回空字符串。</returns>
        internal string BuildUnionOrderByClause(string tableAlias)
        {
            if (this._orderbybuilder == null || this._orderbybuilder.Length == 0)
            {
                return string.Empty;
            }

            var info = this.GetUnionOutputInfo();
            var items = SplitTopLevel(this._orderbybuilder.ToString(), ',');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < items.Count; i++)
            {
                string item = items[i].Trim();
                if (item.Length == 0) continue;

                // 分离排序方向
                string direction = string.Empty;
                if (item.EndsWith(" ASC", StringComparison.OrdinalIgnoreCase))
                {
                    direction = " ASC";
                    item = item.Substring(0, item.Length - 4).Trim();
                }
                else if (item.EndsWith(" DESC", StringComparison.OrdinalIgnoreCase))
                {
                    direction = " DESC";
                    item = item.Substring(0, item.Length - 5).Trim();
                }

                string column = this.MapUnionOrderByColumn(item, info);
                if (sb.Length > 0) sb.Append(",");
                sb.Append($"{tableAlias}.{this.SQLDialect!.QuoteField(column)}{direction}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取 UNION 结果集的第一个输出列名，无法确定时返回 null。
        /// </summary>
        internal string? GetFirstUnionOutputColumn()
        {
            var info = this.GetUnionOutputInfo();
            return info.Columns.Count > 0 ? info.Columns[0].Output : null;
        }

        /// <summary>
        /// 取得 UNION 首个分支的输出列信息（源表达式 → 输出列名）。
        /// </summary>
        private UnionOutputInfo GetUnionOutputInfo()
        {
            var result = new UnionOutputInfo();
            for (int i = 0; i < this.FromItems.Count; i++)
            {
                var fields = this.FromItems[i].SelectFields;
                if (fields == null) continue;
                for (int j = 0; j < fields.Count; j++)
                {
                    if (string.IsNullOrWhiteSpace(fields[j])) continue;
                    var column = SplitSelectField(fields[j]);
                    // 输出列名未知的（如 u.*）：结果集的列名由表结构决定，构建期无从得知
                    if (column.Output == "*" || column.Output.EndsWith(".*", StringComparison.Ordinal))
                    {
                        result.HasUnknownColumns = true;
                        continue;
                    }
                    result.Columns.Add(column);
                }
            }
            // 没有指定查询列时，生成的语句是 "别名.*"，输出列名同样无从得知
            if (result.Columns.Count == 0)
            {
                result.HasUnknownColumns = true;
            }
            return result;
        }

        /// <summary>
        /// 把排序表达式映射到 UNION 结果集的输出列名。
        /// </summary>
        private string MapUnionOrderByColumn(string expression, UnionOutputInfo info)
        {
            // 1) 与 SELECT 列的源表达式匹配
            for (int i = 0; i < info.Columns.Count; i++)
            {
                if (string.Equals(info.Columns[i].Source, expression, StringComparison.OrdinalIgnoreCase))
                {
                    return info.Columns[i].Output;
                }
            }
            // 2) 与输出列名直接匹配
            for (int i = 0; i < info.Columns.Count; i++)
            {
                if (string.Equals(info.Columns[i].Output, expression, StringComparison.OrdinalIgnoreCase))
                {
                    return info.Columns[i].Output;
                }
            }
            // 3) 去掉表前缀后再与输出列名 / 源表达式的列名匹配
            int dot = expression.LastIndexOf('.');
            if (dot >= 0)
            {
                string bare = expression.Substring(dot + 1).Trim();
                for (int i = 0; i < info.Columns.Count; i++)
                {
                    if (string.Equals(info.Columns[i].Output, bare, StringComparison.OrdinalIgnoreCase))
                    {
                        return info.Columns[i].Output;
                    }
                    // 源列名相同但输出列名不同（如 "Name AS UserName"）→ 用输出列名排序
                    if (string.Equals(GetColumnName(info.Columns[i].Source), bare, StringComparison.OrdinalIgnoreCase))
                    {
                        return info.Columns[i].Output;
                    }
                }
                // 4) 首个分支选的是 * （输出列名构建期未知）：按去掉表前缀的列名排序
                if (info.HasUnknownColumns)
                {
                    return bare;
                }
            }
            throw new NotSupportedException(
                $"UNION 查询的排序字段 [{expression}] 不在 SELECT 列中，无法映射到结果集的输出列，请改用结果集的输出列名排序。");
        }

        /// <summary>
        /// 取表达式的列名部分（去掉表前缀），如 "u.Name" → "Name"。
        /// </summary>
        private static string GetColumnName(string expression)
        {
            int dot = expression.LastIndexOf('.');
            return dot >= 0 ? expression.Substring(dot + 1).Trim() : expression.Trim();
        }

        /// <summary>
        /// 拆分一个 SELECT 列表达式，得到它的源表达式与输出列名。
        /// </summary>
        private static UnionOutputColumn SplitSelectField(string field)
        {
            string text = field.Trim();
            int asIndex = text.LastIndexOf(" AS ", StringComparison.OrdinalIgnoreCase);
            if (asIndex > 0)
            {
                return new UnionOutputColumn(text.Substring(0, asIndex).Trim(), text.Substring(asIndex + 4).Trim());
            }
            int dot = text.LastIndexOf('.');
            string output = dot >= 0 ? text.Substring(dot + 1).Trim() : text;
            return new UnionOutputColumn(text, output);
        }

        /// <summary>
        /// 按顶层分隔符拆分字符串，括号内的分隔符不参与拆分。
        /// </summary>
        private static List<string> SplitTopLevel(string text, char separator)
        {
            var result = new List<string>();
            int depth = 0;
            int start = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == '(') depth++;
                else if (c == ')') depth--;
                else if (c == separator && depth == 0)
                {
                    result.Add(text.Substring(start, i - start));
                    start = i + 1;
                }
            }
            result.Add(text.Substring(start));
            return result;
        }

        /// <summary>
        /// 将 EXISTS/NOT EXISTS 子查询的参数合并到主查询的 Parameters 中。
        /// 由 BuildSql 内部自动调用，无需手动调用。
        /// </summary>
        internal void MergeSubQueryParameters()
        {
            if (this._existsList != null)
            {
                foreach (var sq in this._existsList)
                    foreach (var kv in sq.Parameters)
                        Parameters[kv.Key] = kv.Value;
            }
            if (this._notExistsList != null)
            {
                foreach (var sq in this._notExistsList)
                    foreach (var kv in sq.Parameters)
                        Parameters[kv.Key] = kv.Value;
            }
        }

        /// <summary>
        /// 获取注册的 EXISTS / NOT EXISTS 子查询条件字符串。
        /// </summary>
        /// <returns>拼接好的 EXISTS/NOT EXISTS 子句（AND 连接）。</returns>
        public string GetExistsWhere()
        {
            StringBuilder existsBuilder = new StringBuilder();
            if (this._existsList != null)
            {
                for (int i = 0; i < this._existsList.Count; i++)
                {
                    if (existsBuilder.Length > 0)
                        existsBuilder.Append(" AND ");
                    existsBuilder.Append(string.Format("EXISTS ({0})", this._existsList[i].BuildSql()));
                }
            }
            if (this._notExistsList != null)
            {
                for (int i = 0; i < this._notExistsList.Count; i++)
                {
                    if (existsBuilder.Length > 0)
                        existsBuilder.Append(" AND ");
                    existsBuilder.Append(string.Format("NOT EXISTS ({0})", this._notExistsList[i].BuildSql()));
                }
            }
            return existsBuilder.ToString();
        }


        /// <summary>
        /// 查询的表名称，亦即其构造Sql作为子查询的SQL语句，形如：(Select UserName,Password From Users)。
        /// </summary>
        public override string TableName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(this.BuildSql());
                sb.Append(")");
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取Where查询条件字符串（一般供程序员测试用）。
        /// </summary>
        public string WhereClause
        {
            get { return (this._wherebuilder != null && this._wherebuilder.Length > 0) ? this._wherebuilder.ToString() : string.Empty; }
        }

        /// <summary>
        /// UNION 结果集的输出列信息：源表达式（SELECT 列表中的写法）与输出列名。
        /// </summary>
        private sealed class UnionOutputColumn
        {
            /// <summary>SELECT 列的源表达式，如 "u.Name"。</summary>
            public string Source { get; }

            /// <summary>结果集中的输出列名，如 "Name"。</summary>
            public string Output { get; }

            public UnionOutputColumn(string source, string output)
            {
                this.Source = source;
                this.Output = output;
            }
        }

        /// <summary>
        /// UNION 首个分支的输出列信息。
        /// </summary>
        private sealed class UnionOutputInfo
        {
            /// <summary>已知的输出列清单。</summary>
            public List<UnionOutputColumn> Columns { get; } = new List<UnionOutputColumn>();

            /// <summary>首个分支选了 * ，输出列名在构建期无从得知。</summary>
            public bool HasUnknownColumns { get; set; }
        }
    }
}
