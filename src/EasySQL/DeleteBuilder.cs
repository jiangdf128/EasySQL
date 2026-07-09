using System.Text;

namespace EasySQL
{
    /// <summary>
    /// 批量删除数据的SQL语句构造器。
    /// </summary>
    public class DeleteBuilder
    {
        private const string DELETE_SQL = "DELETE FROM {0} WHERE {1}";
        private StringBuilder _wherebuilder = null;
        private SchemaBase Table { get; set; }

        /// <summary>
        /// 创建一个批量删除数据的SQL语句构造器。
        /// </summary>
        /// <param name="table">删除数据的目标表。</param>
        public DeleteBuilder(SchemaBase table)
        {
            Table = table;
        }

        /// <summary>
        /// 创建一个批量删除数据的SQL语句构造器。
        /// </summary>
        /// <param name="table">删除数据的目标表。</param>
        /// <param name="format">格式化条件。</param>
        /// <param name="args">条件参数。</param>
        public DeleteBuilder(SchemaBase table,string format, params object[] args):this(table)
        {
            this.WhereFormat(format, args);
        }

        /// <summary>
        /// 批量修改的条件子句。
        /// </summary>
        /// <param name="items">查询条件1，查询条件2......</param>
        public DeleteBuilder Where(params string[] items)
        {
            QueryBuilder.BuildClause(ref _wherebuilder, " AND ", items);
            return this;
        }

        /// <summary>
        /// 添加查询条件子句。
        /// </summary>
        /// <param name="format">条件子句占位符字符串。</param>
        /// <param name="args"></param>
        public DeleteBuilder WhereFormat(string format, params object[] args)
        {
            return  this.Where(string.Format(format, args));
        }

        /// <summary>
        /// 构造批量删除表数据的SQL语句。
        /// </summary>
        /// <returns>返回可用的SQL语句。</returns>
        public string BuildSql()
        {
            //不允许不指定查询条件进行批量删除。
            string where = _wherebuilder == null ? "1=2" : _wherebuilder.ToString();

            return string.Format(DELETE_SQL, this.Table.SQLDialect.QuoteTable(Table.IsPartialTableName ? Table.PartialTableName : Table.TableName), where);
        }
    }
}
