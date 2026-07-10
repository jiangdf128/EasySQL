using System.Data;

namespace EasySQL
{
    /// <summary>
    /// EasySQL 数据库上下文接口，用于依赖注入场景。
    /// 通过 DI 容器管理生命周期，支持与 <see cref="EasySQLContext.Default"/> 共享同一份配置状态。
    /// </summary>
    public interface IEasySQLContext
    {
        /// <summary>
        /// 获取是否已完成配置初始化。
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// 事务错误全局日志回调。当事务回滚时触发，可用于记录错误详情。
        /// </summary>
        Action<Exception, IDbConnection>? LogTransactionError { get; set; }

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
        /// <param name="action">数据处理函数。</param>
        /// <param name="databaseId">数据库 ID。</param>
        /// <param name="databaseName">可切换的数据库名称。</param>
        void Do(Action<IDbConnection> action, string? databaseId = null, string? databaseName = null);

        /// <summary>
        /// 使用数据库连接执行异步操作，自动释放连接。
        /// </summary>
        /// <param name="action">数据处理函数。</param>
        /// <param name="databaseId">数据库 ID。</param>
        /// <param name="databaseName">可切换的数据库名称。</param>
        Task DoAsync(Func<IDbConnection, Task> action, string? databaseId = null, string? databaseName = null);

        /// <summary>
        /// 使用默认数据库进行同步事务操作，碰到错误会自动回滚。
        /// </summary>
        /// <param name="action">事务操作函数，接收连接和事务对象，返回是否提交。</param>
        /// <param name="databaseId">数据库 ID。</param>
        /// <param name="databaseName">可切换的数据库名称。</param>
        /// <param name="il">事务隔离级别。</param>
        void DoTransaction(Func<IDbConnection, IDbTransaction, bool> action, string? databaseId = null, string? databaseName = null, IsolationLevel il = IsolationLevel.Unspecified);

        /// <summary>
        /// 使用默认数据库进行异步事务操作，碰到错误会自动回滚。
        /// </summary>
        /// <param name="action">事务操作函数，接收连接和事务对象，返回是否提交。</param>
        /// <param name="databaseId">数据库 ID。</param>
        /// <param name="databaseName">可切换的数据库名称。</param>
        /// <param name="il">事务隔离级别。</param>
        Task DoTransactionAsync(Func<IDbConnection, IDbTransaction, Task<bool>> action, string? databaseId = null, string? databaseName = null, IsolationLevel il = IsolationLevel.Unspecified);
    }
}
