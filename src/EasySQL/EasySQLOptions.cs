namespace EasySQL
{
    /// <summary>
    /// EasySQL DI 配置选项。
    /// </summary>
    public class EasySQLOptions
    {
        /// <summary>
        /// 已注册的数据库代理列表。
        /// </summary>
        public List<IDbProxy> Proxies { get; } = new();

        /// <summary>
        /// 默认数据库方言类型。
        /// </summary>
        public DialectType DefaultDialect { get; set; } = DialectType.SqlServer;

        /// <summary>
        /// 是否清除已有的配置再重新注册。
        /// </summary>
        public bool ClearExisting { get; set; } = false;

        /// <summary>
        /// 注册一个数据库连接代理。
        /// </summary>
        /// <param name="proxy">数据库代理实例。</param>
        public void AddDatabase(IDbProxy proxy)
        {
            Proxies.Add(proxy);
        }
    }
}
