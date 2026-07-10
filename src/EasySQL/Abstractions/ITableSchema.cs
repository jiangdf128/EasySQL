namespace EasySQL
{
    /// <summary>
    /// 表或视图结构接口，定义表名、别名、字段选择、连接等基本操作。
    /// <see cref="SchemaBase"/> 提供了该接口的默认实现。
    /// </summary>
    public interface ITableSchema
    {
        /// <summary>
        /// 获取表或视图的名称。
        /// </summary>
         string TableName { get; }

        /// <summary>
        /// 获取或设置表或视图的别名。
        /// </summary>
        string Alias { get; set; }

        /// <summary>
        /// 清空所有Select字段。
        /// </summary>
        void ClearSelect();

        /// <summary>
        /// 清空所有Joins。
        /// </summary>
        void ClearJoins();

        /// <summary>
        /// 使用的SQL方言。
        /// </summary>
        ISQLDialect? SQLDialect { get; set; }

        /// <summary>
        /// 获取或设置拆表的名称，如果设置为null则认为是默认表名称。
        /// </summary>
        string PartialTableName { get; set; }

        /// <summary>
        /// 是否为拆表的名称。
        /// </summary>
        bool IsPartialTableName { get; }
    }
}
