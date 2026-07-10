using System.Data;

namespace EasySQL
{
    /// <summary>
    /// EasySQL 数据库上下文接口（非静态），用于依赖注入场景。
    /// 提供与 <see cref="DbContext"/> 相同的功能，但支持通过 DI 容器管理生命周期。
    /// </summary>
    public interface IEasySQLContext
    {
        /// <summary>
        /// 获取是否已完成配置初始化。
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// 打开数据库连接。不指定 databaseId 则使用默认数据库。
        /// </summary>
        /// <param name="databaseId">数据库 ID。</param>
        /// <param name="databaseName">可切换的数据库名称。</param>
        /// <returns>已打开的数据库连接。</returns>
        IDbConnection Open(string? databaseId = null, string? databaseName = null);

        /// <summary>
        /// 使用数据库连接执行同步操作，自动释放连接。
        /// </summary>
        void Do(Action<IDbConnection> action, string? databaseId = null, string? databaseName = null);

        /// <summary>
        /// 使用数据库连接执行异步操作，自动释放连接。
        /// </summary>
        Task DoAsync(Func<IDbConnection, Task> action, string? databaseId = null, string? databaseName = null);
    }
}
