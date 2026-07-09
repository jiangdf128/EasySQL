namespace EasySQL
{
    /// <summary>
    /// 日期（或时间）部分描述，配合<see cref="IDbFunction"/>的日期型函数接口定义。
    /// </summary>
    public enum DatePart
    {
        /// <summary>
        /// 年。
        /// </summary>
        Year,
        /// <summary>
        /// 月。
        /// </summary>
        Month,
        /// <summary>
        /// 日。
        /// </summary>
        Day,
        /// <summary>
        /// 小时。
        /// </summary>
        Hour,
        /// <summary>
        /// 分。
        /// </summary>
        Minute,
        /// <summary>
        /// 秒。
        /// </summary>
        Second
    }
}
