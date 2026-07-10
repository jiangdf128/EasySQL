using System.Collections.Generic;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// DELETE 语句构造器。通过流式 API 构建<strong>类型安全的删除 SQL</strong>。
    /// 用 TableDef 的字段 getter 代替硬编码字段名，字段变更时编译立即可发现。
    /// </summary>
    /// <remarks>
    /// <code>
    /// var su = new UserTableDef();
    /// var delete = new DeleteBuilder(su)
    ///     .Where($"{su.GetId()} = {su.AsParam("Id")}")
    ///     .AddParameter("Id", 123);
    /// string sql = delete.BuildSql();
    /// // 执行：conn.Execute(sql, delete.Parameters.ToDynamicParameters());
    /// </code>
    /// <para><strong>安全机制：</strong>若无 WHERE 条件则抛出异常，防止全表误删除。确认要全表删除请用 <c>Where("1=1")</c>。</para>
    /// </remarks>
    public class DeleteBuilder
    {
        private readonly object _lock = new object();
        private const string DELETE_SQL = "DELETE FROM {0} WHERE {1}";
        private StringBuilder? _wherebuilder = null;
        private TableDefBase Table { get; set; }

        /// <summary>
        /// 参数化查询的参数集合。使用 <see cref="TableDefBase.AsParam"/> 生成占位符，
        /// 用 <see cref="AddParameter"/> 注册值，二者配合完成参数化。
        /// </summary>
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();

        /// <summary>
        /// 创建 DELETE 语句构造器。
        /// </summary>
        /// <param name="table">删除目标表 TableDef。</param>
        public DeleteBuilder(TableDefBase table)
        {
            Table = table;
        }

        /// <summary>
        /// 注册参数值。参数名不含 @ 前缀，与 <see cref="TableDefBase.AsParam"/> 生成的占位符对应。
        /// </summary>
        /// <example>
        /// <code>
        /// delete.Where($"{su.GetId()} = {su.AsParam("Id")}")
        ///       .AddParameter("Id", 123);
        /// </code>
        /// </example>
        /// <param name="name">参数名（不含 @ / : 前缀）。</param>
        /// <param name="value">参数值。</param>
        public DeleteBuilder AddParameter(string name, object value)
        {
            lock (_lock) { Parameters[name] = value; }
            return this;
        }

        /// <summary>
        /// 批量注册参数。
        /// </summary>
        public DeleteBuilder AddParameters(IDictionary<string, object> parameters)
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
        /// 创建 DELETE 构造器并预设条件（便捷重载）。
        /// </summary>
        /// <param name="table">删除目标表 TableDef。</param>
        /// <param name="format">WHERE 条件格式化字符串。</param>
        /// <param name="args">格式化参数。</param>
        public DeleteBuilder(TableDefBase table,string format, params object[] args):this(table)
        {
            this.WhereFormat(format, args);
        }

        /// <summary>
        /// 添加 WHERE 条件，多个条件以 AND 连接。推荐用 TableDef 字段 getter + AsParam。
        /// </summary>
        /// <example>
        /// <code>
        /// delete.Where($"{su.GetId()} = {su.AsParam("Id")}");
        /// </code>
        /// </example>
        /// <param name="items">WHERE 条件表达式。</param>
        public DeleteBuilder Where(params string[] items)
        {
            QueryBuilder.BuildClause(ref _wherebuilder, " AND ", items);
            return this;
        }

        /// <summary>
        /// 添加 WHERE 条件（string.Format 格式化版本）。
        /// </summary>
        /// <param name="format">格式化模板。</param>
        /// <param name="args">占位参数。</param>
        public DeleteBuilder WhereFormat(string format, params object[] args)
        {
            return  this.Where(string.Format(format, args));
        }

        /// <summary>
        /// 构造 DELETE SQL 语句。若无 WHERE 条件则生成 WHERE 1=2，防止全表误删除。
        /// </summary>
        /// <example>
        /// <code>
        /// string sql = delete.BuildSql();
        /// conn.Execute(sql, delete.Parameters.ToDynamicParameters());
        /// </code>
        /// </example>
        public string BuildSql()
        {
            string where = _wherebuilder?.ToString()
                ?? throw new InvalidOperationException("DELETE 必须指定 WHERE 条件，防止误操作全表数据。如确认要删除全表，请使用 Where(\"1=1\")。");

            return string.Format(DELETE_SQL, this.Table.SQLDialect!.QuoteTable(Table.IsPartialTableName ? Table.PartialTableName : Table.TableName), where);
        }
    }
}
