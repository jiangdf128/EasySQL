using System;
using System.Collections.Generic;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// UPDATE 语句构造器。通过流式 API 构建<strong>类型安全的更新 SQL</strong>。
    /// 用 TableDef 的字段 getter 代替硬编码字段名，字段变更时编译立即可发现。
    /// </summary>
    /// <remarks>
    /// <code>
    /// var su = new UserTableDef();
    /// var update = new UpdateBuilder(su)
    ///     .Set(su.GetName(), su.AsParam("Name"))
    ///     .Set(su.GetStatus(), 1)
    ///     .Where($"{su.GetId()} = {su.AsParam("Id")}")
    ///     .AddParameter("Name", "张三")
    ///     .AddParameter("Id", 123);
    /// string sql = update.BuildSql();
    /// // 执行：conn.Execute(sql, update.Parameters.ToDynamicParameters());
    /// </code>
    /// <para><strong>安全机制：</strong>若无 WHERE 条件则抛出异常，防止全表误更新。确认要全表更新请用 <c>Where("1=1")</c>。</para>
    /// </remarks>
    public class UpdateBuilder
    {
        private readonly object _lock = new object();
        private const string UPDATE_SQL = "UPDATE {0} SET {1} WHERE {2}";
        private TableDefBase Table { get; set; }
        private IList<string> SetList { get; set; }
        private StringBuilder? _wherebuilder = null;

        /// <summary>
        /// 参数化查询的参数集合。使用 <see cref="TableDefBase.AsParam"/> 生成占位符，
        /// 用 <see cref="AddParameter"/> 注册值，二者配合完成参数化。
        /// </summary>
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();

        /// <summary>
        /// 创建 UPDATE 语句构造器。
        /// </summary>
        /// <param name="table">更新目标表 TableDef。</param>
        public UpdateBuilder(TableDefBase table)
        {
            Table = table;
            SetList = new List<string>();
        }

        /// <summary>
        /// 注册参数值。参数名不含 @ 前缀，与 <see cref="TableDefBase.AsParam"/> 生成的占位符对应。
        /// </summary>
        /// <example>
        /// <code>
        /// update.Where($"{su.GetId()} = {su.AsParam("Id")}")
        ///       .AddParameter("Id", 123);
        /// </code>
        /// </example>
        /// <param name="name">参数名（不含 @ / : 前缀）。</param>
        /// <param name="value">参数值。</param>
        public UpdateBuilder AddParameter(string name, object value)
        {
            lock (_lock) { Parameters[name] = value; }
            return this;
        }

        /// <summary>
        /// 批量注册参数。
        /// </summary>
        public UpdateBuilder AddParameters(IDictionary<string, object> parameters)
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
        /// 设置字段值。字段名推荐用 TableDef 的字段 getter，值可以是常量或参数占位符。
        /// </summary>
        /// <example>
        /// <code>
        /// // 常量值
        /// update.Set(su.GetStatus(), 1);
        /// // 参数化值
        /// update.Set(su.GetName(), su.AsParam("Name"));
        /// </code>
        /// </example>
        /// <param name="fieldName">字段名，推荐用 <c>schema.GetXxx()</c> 获取。</param>
        /// <param name="valueExpression">值表达式（常量或参数占位符）。</param>
        public UpdateBuilder Set(string fieldName, string valueExpression)
        {
            SetList.Add(string.Format("{0}={1}", this.Table.SQLDialect!.QuoteField(fieldName), valueExpression));
            return this;
        }

        /// <summary>
        /// 设置整型字段值（常量）。
        /// </summary>
        /// <param name="fieldName">字段名。</param>
        /// <param name="numericValue">整型常量。</param>
        public UpdateBuilder Set(string fieldName, int numericValue)
        {
            this.Set(fieldName,numericValue.ToString());
            return this;
        }

        /// <summary>
        /// 设置长整型字段值（常量）。
        /// </summary>
        /// <param name="fieldName">字段名。</param>
        /// <param name="numericValue">长整型常量。</param>
        public UpdateBuilder Set(string fieldName, long numericValue)
        {
            this.Set(fieldName, numericValue.ToString());
            return this;
        }


        /// <summary>
        /// 添加 WHERE 条件，多个条件以 AND 连接。推荐用 TableDef 字段 getter + AsParam。
        /// </summary>
        /// <example>
        /// <code>
        /// update.Where($"{su.GetId()} = {su.AsParam("Id")}");
        /// </code>
        /// </example>
        /// <param name="items">WHERE 条件表达式。</param>
        public UpdateBuilder Where(params string[] items)
        {
            QueryBuilder.BuildClause(ref _wherebuilder, " AND ", items);
            return this;
        }

        /// <summary>
        /// 批量修改的条件子句。
        /// </summary>
        /// <param name="format">条件子句占位符字符串。</param>
        /// <param name="args"></param>
        public UpdateBuilder WhereFormat(string format, params object[] args)
        {
            return this.Where(string.Format(format, args));
        }

        /// <summary>
        /// 构造 UPDATE SQL 语句。若无 WHERE 条件则生成 WHERE 1=2，防止全表误更新。
        /// </summary>
        /// <example>
        /// <code>
        /// string sql = update.BuildSql();
        /// conn.Execute(sql, update.Parameters.ToDynamicParameters());
        /// </code>
        /// </example>
        public string BuildSql()
        {
            string where = _wherebuilder?.ToString()
                ?? throw new InvalidOperationException("UPDATE 必须指定 WHERE 条件，防止误操作全表数据。如确认要更新全表，请使用 Where(\"1=1\")。");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < SetList.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(SetList[i]);
            }

            return string.Format(UPDATE_SQL, this.Table.SQLDialect!.QuoteTable(Table.IsPartialTableName ? Table.PartialTableName : Table.TableName), sb.ToString(), where);
        }
    }
}
