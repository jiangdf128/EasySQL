namespace EasySQL
{
    /// <summary>
    /// SQL连接类型，分别包括内（左，右，外）连接4种。
    /// </summary>
    public enum JoinType
    {
        /// <summary>
        /// 内连接
        /// </summary>
        Inner,
        /// <summary>
        /// 左连接
        /// </summary>
        Left,
        /// <summary>
        /// 右连接
        /// </summary>
        Right,
        /// <summary>
        /// 全连接
        /// </summary>
        Outer
    }
}
