using System.Collections.Generic;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// 批量插入数据的SQL语句构造器。
    /// </summary>
    public class InsertBuilder
    {
        private const string INSERT_SQL = "INSERT INTO {0} ({1}) {2}";
        private SchemaBase Table { get; set; }
        private IList<string> Fields { get; set; }

        /// <summary>
        /// 创建一个批量插入数据的SQL语句构造器实例。
        /// </summary>
        /// <param name="table">插入数据的目标表。</param>
        public InsertBuilder(SchemaBase table)
        {
            Table = table;
            Fields = new List<string>();
        }

        /// <summary>
        /// 设置插入字段。
        /// </summary>
        /// <param name="fields">字段名1,字段名2......</param>
        public InsertBuilder Insert(params string[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] != null && fields[i].Trim().Length > 0)
                {
                    Fields.Add(this.Table.SQLDialect.QuoteField(fields[i]));
                }
            }
            return this;
        }


        /// <summary>
        /// 构造批量插入子查询数据的SQL语句。
        /// </summary>
        /// <param name="fromQuery">数据来源的子查询构造器。</param>
        /// <returns>返回可用的SQL语句。</returns>
        public string BuildSql(QueryBuilder fromQuery)
        {
            return this.BuildSql(fromQuery.BuildSql());
        }

        /// <summary>
        /// 构造批量插入子查询数据的SQL语句。
        /// </summary>
        /// <param name="fromQuery">数据来源的子查询SQL语句。</param>
        /// <returns></returns>
        public string BuildSql(string fromQuery)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Fields.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(Fields[i]);
            }
            return string.Format(INSERT_SQL, this.Table.SQLDialect.QuoteTable(Table.IsPartialTableName ? Table.PartialTableName : Table.TableName), sb.ToString(), fromQuery);
        }
    }
}
