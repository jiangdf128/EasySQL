using System.Collections.Generic;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// INSERT 语句构造器。支持 INSERT INTO ... SELECT 子查询模式。
    /// </summary>
    /// <remarks>
    /// <code>
    /// var su = new UserSchema();
    /// var insert = new InsertBuilder(su)
    ///     .Insert(su.GetName(), su.GetEmail(), su.GetStatus())
    ///     .BuildSql("SELECT '张三', 'zhangsan@test.com', 1");
    /// string sql = insert.BuildSql();
    /// </code>
    /// </remarks>
    public class InsertBuilder
    {
        private const string INSERT_SQL = "INSERT INTO {0} ({1}) {2}";
        private SchemaBase Table { get; set; }
        private IList<string> Fields { get; set; }

        /// <summary>
        /// 创建 INSERT 语句构造器。
        /// </summary>
        /// <param name="table">插入目标表 Schema。</param>
        public InsertBuilder(SchemaBase table)
        {
            Table = table;
            Fields = new List<string>();
        }

        /// <summary>
        /// 设置要插入的列名。推荐用 Schema 的字段 getter 获取列名。
        /// </summary>
        /// <example>
        /// <code>
        /// insert.Insert(su.GetName(), su.GetEmail());
        /// </code>
        /// </example>
        /// <param name="fields">列名，推荐用 <c>schema.GetXxx()</c> 获取。</param>
        public InsertBuilder Insert(params string[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] != null && fields[i].Trim().Length > 0)
                {
                    Fields.Add(this.Table.SQLDialect!.QuoteField(fields[i]));
                }
            }
            return this;
        }


        /// <summary>
        /// 构造 INSERT ... SELECT 语句，数据来自子查询。
        /// </summary>
        /// <example>
        /// <code>
        /// string sql = insert.BuildSql(fromQuery);
        /// </code>
        /// </example>
        /// <param name="fromQuery">子查询构建器。</param>
        /// <returns>INSERT SQL 字符串。</returns>
        public string BuildSql(QueryBuilder fromQuery)
        {
            return this.BuildSql(fromQuery.BuildSql());
        }

        /// <summary>
        /// 构造 INSERT ... SELECT 语句，数据来自给定的 SELECT SQL。
        /// </summary>
        /// <param name="fromQuery">SELECT 语句字符串。</param>
        /// <returns>INSERT SQL 字符串。</returns>
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
            return string.Format(INSERT_SQL, this.Table.SQLDialect!.QuoteTable(Table.IsPartialTableName ? Table.PartialTableName : Table.TableName), sb.ToString(), fromQuery);
        }
    }
}
