using System;
using System.Collections.Generic;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// 辅助表（或视图、子查询）类的SQL查询构建器。
    /// </summary>
    public class QueryBuilder:SchemaBase
    {
        private readonly object _lock = new object();
        private List<KeyValuePair<QueryBuilder,bool>>  _unionList;
        private List<QueryBuilder> _existsList=null;
        private List<QueryBuilder> _notExistsList=null;

        private StringBuilder _wherebuilder=null;
        private StringBuilder _groupbybuilder=null;
        private StringBuilder _orderbybuilder=null;
        private StringBuilder _havingbuilder=null;

        //在做嵌套SQL的计数查询的时候，去掉Order排序。
        private bool _isSubCount=false;

        /// <summary>
        /// 参数化查询的参数集合，用于收集 WHERE 条件中的参数名与值。
        /// 调用 BuildSql() 后可通过此属性获取所有已注册的参数。
        /// </summary>
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();

        internal List<KeyValuePair<QueryBuilder, bool>> UnionList => _unionList;
        internal List<QueryBuilder> ExistsList => _existsList;
        internal List<QueryBuilder> NotExistsList => _notExistsList;

        internal StringBuilder Wherebuilder => _wherebuilder;
        internal StringBuilder Groupbybuilder => _groupbybuilder;
        internal StringBuilder Orderbybuilder => _orderbybuilder;
        internal StringBuilder Havingbuilder => _havingbuilder;
        internal bool IsSubCount => _isSubCount;

        /// <summary>
        /// 是否需要对构造返回的Sql语句节使用换行，以提高可读性。
        /// </summary>
        public bool Readable { get; set; }

        /// <summary>
        /// 是否为Distinct语句。
        /// </summary>
        public bool IsDistinct { get; set; }

        /// <summary>
        /// 读取该查询的目标表（或视图、子查询）的列表。
        /// </summary>
        public List<SchemaBase> FromItems { get; protected set; }

        /// <summary>
        /// 创建一个<see cref="QueryBuilder"/>实例。
        /// </summary>
        /// <param name="alias">作为子查询的别名参数。</param>
        /// <param name="dialect">SQL方言。</param>
        public QueryBuilder(string alias,ISQLDialect dialect)
            : base(alias, dialect)
        {
            this.FromItems = new List<SchemaBase>();
            this.Readable = true;
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
        /// 从目标表（或视图、子查询）架构类查询数据，这类似于Sql语句里面的from子句。
        /// </summary>
        /// <param name="items">查询目标1，查询目标2......</param>
        public QueryBuilder From( params SchemaBase[] items)
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
        /// 添加查询条件子句。
        /// </summary>
        /// <param name="items">查询条件1，查询条件2......</param>
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
        /// 在条件中使用 @参数名 引用，如 Where("Id = @UserId")，然后调用 AddParameter("UserId", 123)。
        /// </summary>
        /// <param name="name">参数名称（不含 @ 前缀）。</param>
        /// <param name="value">参数值。</param>
        public QueryBuilder AddParameter(string name, object value)
        {
            Parameters[name] = value;
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
                foreach (var kv in parameters)
                    Parameters[kv.Key] = kv.Value;
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
        /// 添加查询分组子句。
        /// </summary>
        /// <param name="items">分组1，分组2......</param>
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
        /// 清除掉Schema中的Selected Fields.
        /// </summary>
		public QueryBuilder ClearSchemaItemSelectFields()
		{
		    for (int i = 0; i < this.FromItems.Count; i++)
		    {
		        this.FromItems[i].ClearSelect();
		    }
            return this;
		}

        /// <summary>
        /// 添加分组条件子句。
        /// </summary>
        /// <param name="items">分组条件1，条件2......</param>
        public QueryBuilder Having(params string[] items)
        {
            BuildClause(ref _havingbuilder, " AND ", items);
            return this;
        }
        /// <summary>
        /// 添加排序子句。
        /// </summary>
        /// <param name="items">排序字段1，字段2......</param>
        public QueryBuilder OrderBy(params string[] items)
        {
            BuildClause(ref _orderbybuilder, ",", items);
            return this;
        }

        /// <summary>
        /// 合并查询结果集。
        /// </summary>
        /// <param name="queryBuilder">查询构造对象（不能Union自身）。</param>
        /// <param name="isUnionAll">是否使用Union All。</param>
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
        /// 添加Exists查询条件。
        /// </summary>
        /// <param name="queryBuilder"></param>
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

        internal static void BuildClause(ref StringBuilder builder, string seprator, params string[] items)
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
                string sql= this.BuildSql(0, 0, false);
                if (this._unionList != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(sql);
                    for (int i = 0; i < this._unionList.Count; i++)
                    {
                        sb.Append(this.Readable ? System.Environment.NewLine : " ");
                        sb.Append(this._unionList[i].Value ? "UNION ALL" : "UNION");
                        sb.Append(this.Readable ? System.Environment.NewLine : " ");
                        sb.Append(this._unionList[i].Key.BuildSql());
                    }
                    sql = sb.ToString();
                }
                return sql;
            }
        }

        /// <summary>
        /// 构造SQL语句（注意这里会忽略掉Union的查询）。
        /// </summary>
        /// <param name="rowLimit">返回最大可能的记录数。</param>
        /// <param name="rowOffset">偏移位置（从0行开始算起）。</param>
        /// <returns></returns>
        public string BuildSql(int rowLimit, int rowOffset=0)
        {
            lock (_lock)
            {
                return this.BuildSql(rowLimit, rowOffset, false);
            }
        }

        /// <summary>
        /// 构造Sql 的Count(0)语句。
        /// </summary>
        /// <returns>根据自身包括的查询对象，返回可执行的Sql Count查询语句。</returns>
        public string BuildCountSql()
        {
            if (this._groupbybuilder != null && this._groupbybuilder.Length > 0)
            {
                // 构建子查询计数：不修改 this 实例状态，保证线程安全。
                // 先锁定以获取一致的别名快照。
                string safeAlias;
                lock (_lock)
                {
                    safeAlias = string.IsNullOrWhiteSpace(this.Alias) ? "A" : this.Alias;
                    this.Alias = safeAlias;
                }
                try
                {
                    // 用原查询构建内层 SQL，然后外裹一层 Count 子查询
                    var innerQb = new QueryBuilder(safeAlias, this.SQLDialect)
                    {
                        Readable = this.Readable
                    };
                    innerQb.From(this);
                    innerQb.SelectExpression("Count(1) AS RowsCount");

                    var outerQb = new QueryBuilder();
                    outerQb.From(innerQb);
                    return outerQb.BuildSql();
                }
                finally
                {
                    // 恢复原别名
                    lock (_lock)
                    {
                        this.Alias = safeAlias;
                    }
                }
            }
            else
            {
                return BuildSql(0, 0, true);
            }
        }

        /// <summary>
        /// 构建SQL查询。
        /// </summary>
        /// <param name="rowLimit">返回最大可能的记录数。</param>
        /// <param name="rowOffset">偏移位置（从0行开始算起）。</param>
        /// <param name="forCount">是否翻译成Count语句。</param>
        /// <param name="sqlServerNewSurpport">支持新的SQL分页语法。</param>
        /// <returns></returns>
        private string BuildSql(int rowLimit, int rowOffset, bool forCount)
        {
            return this.SQLDialect.BuildSql(this, rowLimit, rowOffset, forCount);
        }

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
    }
}
