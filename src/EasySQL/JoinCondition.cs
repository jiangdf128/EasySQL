namespace EasySQL
{
    /// <summary>
    /// 辅助构造查询连接条件的功能类，其能返回特定连接条件的查询子句。
    /// </summary>
    public sealed class JoinCondition
    {
        /// <summary>
        /// 连接条件的源表（或视图、子查询）
        /// </summary>
        public ITableSchema SourceSchema { get; private set; }

        /// <summary>
        /// 连接条件的目标表（或视图、子查询）
        /// </summary>
        public ITableSchema TargetSchema { get; private set; }

        /// <summary>
        /// 查询连接类型
        /// </summary>
        public JoinType JoinClass { get; private set; }

        /// <summary>
        /// 连接条件
        /// </summary>
        public string OnClause { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="clause"></param>
        /// <param name="joinType"></param>
        public JoinCondition(ITableSchema source, ITableSchema target, string clause, JoinType joinType)
        {
            this.SourceSchema    = source;
            this.TargetSchema = target;
            this.OnClause = clause;
            this.JoinClass = joinType;
        }

        /// <summary>
        /// 返回用于构造查询的连接条件子句。
        /// </summary>
        /// <returns></returns>
        public string GetJoinClause()
        {
            var dialect = this.SourceSchema.SQLDialect! ?? this.TargetSchema.SQLDialect!;
           return dialect.GetJoinClause(this);
        }
    }
}
