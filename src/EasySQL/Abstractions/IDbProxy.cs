using System.Data;

namespace EasySQL
{
    /// <summary>
    /// 数据库连接代理接口.
    /// </summary>
    public interface IDbProxy
    {
        /// <summary>
        /// 数据库ID。
        /// </summary>
        string DatabaseId { get; }

        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        string ConnectString { get;  }

        /// <summary>
        /// 配置连接信息。
        /// </summary>
        /// <param name="dbid">数据库ID。</param>
        /// <param name="conncetString">数据库连接字符串。</param>
        IDbProxy Config(string dbid, string conncetString);

        /// <summary>
        /// 打开一个数据库连接。
        /// </summary>
        /// <returns></returns>
        IDbConnection Open();
    }
}
