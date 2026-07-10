using System.Data;

namespace EasySQL
{
    /// <summary>
    /// 数据库连接代理抽象基类，实现了 <see cref="IDbProxy"/> 的基本功能。
    /// 子类只需覆写 <see cref="Open"/> 方法以提供特定数据库的连接实例。
    /// </summary>
    public abstract class DbProxyBase:IDbProxy
    {
        /// <summary>
        /// 数据库ID。
        /// </summary>
        public virtual string DatabaseId { get; protected set; }

        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public virtual string ConnectString { get; protected set; }

        /// <summary>
        /// 配置连接信息。
        /// </summary>
        /// <param name="dbid">数据库ID。</param>
        /// <param name="conncetString">数据库连接字符串。</param>
        public virtual IDbProxy Config(string dbid, string conncetString)
        {
            this.DatabaseId = dbid;
            this.ConnectString = conncetString;
            return this;
        }

        /// <summary>
        /// 打开一个数据库连接。
        /// </summary>
        /// <returns></returns>
        public abstract IDbConnection Open();
    }
}
