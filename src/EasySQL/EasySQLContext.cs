using System.Data;

namespace EasySQL
{
    /// <summary>
    /// EasySQL 数据库上下文（非静态），用于依赖注入场景。
    /// 内部委托给静态 <see cref="DbContext"/>，提供实例化的 API。
    /// </summary>
    /// <remarks>
    /// <code>
    /// // DI 注册
    /// services.AddEasySQL(options =>
    /// {
    ///     options.AddDatabase(new SqlServerProxy().Config("main", connectionString));
    /// });
    ///
    /// // 在 Service 中使用
    /// public class UserService
    /// {
    ///     private readonly IEasySQLContext _db;
    ///     public UserService(IEasySQLContext db) => _db = db;
    ///
    ///     public async Task&lt;IEnumerable&lt;User&gt;&gt; GetUsersAsync()
    ///     {
    ///         return await _db.DoAsync(async conn =>
    ///         {
    ///             var su = new UserSchema("u");
    ///             su.Select(su.GetName());
    ///             var qb = new QueryBuilder().From(su);
    ///             return await conn.QueryAsync&lt;User&gt;(qb.BuildSql());
    ///         });
    ///     }
    /// }
    /// </code>
    /// </remarks>
    public class EasySQLContext : IEasySQLContext
    {
        /// <inheritdoc/>
        public bool IsInitialized => DbContext.IsInitialized;

        /// <summary>
        /// 使用配置选项初始化上下文。
        /// </summary>
        /// <param name="options">配置选项。</param>
        public void Configure(EasySQLOptions options)
        {
            DbContext.ConfigContext(options.Proxies, options.ClearExisting);
            if (options.DefaultDialect != DialectType.SqlServer)
            {
                SQLDialectFactory.UseDialect(GetDialectName(options.DefaultDialect));
            }
        }

        /// <summary>
        /// 使用配置选项初始化上下文（异步重载，内部仍然是同步的）。
        /// 仅为方便在异步初始化流程中使用。
        /// </summary>
        public Task ConfigureAsync(EasySQLOptions options)
        {
            Configure(options);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public IDbConnection Open(string? databaseId = null, string? databaseName = null)
        {
            return DbContext.Open(databaseId, databaseName);
        }

        /// <inheritdoc/>
        public void Do(Action<IDbConnection> action, string? databaseId = null, string? databaseName = null)
        {
            DbContext.Do(action, databaseId, databaseName);
        }

        /// <inheritdoc/>
        public async Task DoAsync(Func<IDbConnection, Task> action, string? databaseId = null, string? databaseName = null)
        {
            await DbContext.DoAsync(action, databaseId, databaseName);
        }

        private static string GetDialectName(DialectType type) => type switch
        {
            DialectType.SqlServer => "sqlconnection",
            DialectType.MySQL => "mysqlconnection",
            DialectType.PostgreSQL => "npgsqlconnection",
            DialectType.Oracle => "oracleconnection",
            DialectType.SQLite => "sqliteconnection",
            DialectType.Jet => "oledbconnection",
            DialectType.DB2 => "db2connection",
            _ => "sqlconnection"
        };
    }
}
