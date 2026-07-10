using System;
using System.Collections.Generic;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// 批量修改数据的SQL语句构造器。
    /// </summary>
    public class UpdateBuilder
    {
        private readonly object _lock = new object();
        private const string UPDATE_SQL = "UPDATE {0} SET {1} WHERE {2}";
        private SchemaBase Table { get; set; }
        private IList<string> SetList { get; set; }
        private StringBuilder _wherebuilder = null;

        /// <summary>
        /// 参数化查询的参数集合。在条件中使用 @参数名，如 Set("Name", "@Name")，然后调用 AddParameter("Name", value)。
        /// </summary>
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();

        /// <summary>
        /// 创建一个批量修改数据的SQL语句构造器。
        /// </summary>
        /// <param name="table">更新数据的目标表。</param>
        public UpdateBuilder(SchemaBase table)
        {
            Table = table;
            SetList = new List<string>();
        }

        /// <summary>
        /// 注册一个参数化查询参数，用于防止 SQL 注入。
        /// </summary>
        /// <param name="name">参数名称（不含 @ 前缀）。</param>
        /// <param name="value">参数值。</param>
        public UpdateBuilder AddParameter(string name, object value)
        {
            lock (_lock) { Parameters[name] = value; }
            return this;
        }

        /// <summary>
        /// 注册多个参数化查询参数。
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
        /// 更新字段设值函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="valueExpression">值表达式（可以是数据库参数变量名）。</param>
        public UpdateBuilder Set(string fieldName, string valueExpression)
        {
            SetList.Add(string.Format("{0}={1}", this.Table.SQLDialect.QuoteField(fieldName), valueExpression));
            return this;
        }

        /// <summary>
        /// 更新字段设值函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="numericValue">整形常量。</param>
        public UpdateBuilder Set(string fieldName, int numericValue)
        {
            this.Set(fieldName,numericValue.ToString());
            return this;
        }

        /// <summary>
        /// 更新字段设值函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="numericValue">长整形常量。</param>
        public UpdateBuilder Set(string fieldName, long numericValue)
        {
            this.Set(fieldName, numericValue.ToString());
            return this;
        }


        /// <summary>
        /// 批量修改的条件子句。
        /// </summary>
        /// <param name="items">查询条件1，查询条件2......</param>
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
        /// 构造批量更新表数据的SQL语句。
        /// </summary>
        /// <returns>返回可用的SQL语句。</returns>
        public string BuildSql()
        {
            //不允许不指定查询条件进行批量更新。
            string where = _wherebuilder == null ? "1=2" : _wherebuilder.ToString();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < SetList.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(SetList[i]);
            }

            return string.Format(UPDATE_SQL, this.Table.SQLDialect.QuoteTable(Table.IsPartialTableName ? Table.PartialTableName : Table.TableName), sb.ToString(), where);
        }
    }
}
