using System.Data;

namespace EasySQL
{
    /// <summary>
    /// EasySQL 数据库上下文，持有数据库连接配置和状态。
    /// 可独立实例化（用于多库隔离或单元测试），也可通过 <see cref="Default"/> 静态单例使用。
    /// </summary>
    /// <remarks>
    /// <code>
    /// // DI 注册（使用 Default 单例）
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
    ///             var s = new UserTableDef("u");
    ///             s.Select(s.GetName());
    ///             var qb = new QueryBuilder().From(s);
    ///             return await conn.QueryAsync&lt;User&gt;(qb.BuildSql());
    ///         });
    ///     }
    /// }
    ///
    /// // 独立实例（多库隔离 / 单元测试）
    /// var context = new EasySQLContext();
    /// context.Configure(new EasySQLOptions
    /// {
    ///     Proxies = { new SqlServerProxy().Config("test", testConnString) }
    /// });
    /// </code>
    /// </remarks>
    public class EasySQLContext : IEasySQLContext
    {
        /// <summary>
        /// 全局默认上下文单例。非 DI 场景直接使用 <c>EasySQLContext.Default.Open(...)</c> 即可操作数据库。
        /// DI 场景中，<see cref="EasySQLServiceExtensions.AddEasySQL"/> 也会将此实例注册到容器。
        /// </summary>
        public static readonly EasySQLContext Default = new();

        private readonly object _lock = new();
        private readonly Dictionary<string, IDbProxy> _dbs = new();
        private IDbProxy? _defaultDb;

        /// <inheritdoc/>
        public bool IsInitialized => _defaultDb != null;

        /// <inheritdoc/>
        public Action<Exception, IDbConnection>? LogTransactionError { get; set; }

        /// <summary>
        /// 使用配置选项初始化上下文。
        /// </summary>
        /// <param name="options">配置选项。</param>
        public void Configure(EasySQLOptions options)
        {
            lock (_lock)
            {
                if (options.ClearExisting)
                {
                    _defaultDb = null;
                    _dbs.Clear();
                }

                foreach (var db in options.Proxies)
                {
                    if (_dbs.ContainsKey(db.DatabaseId))
                    {
                        throw new InvalidOperationException(
                            $"数据库 ID \"{db.DatabaseId}\" 重复注册，每个 ID 必须唯一。");
                    }

                    _dbs.Add(db.DatabaseId, db);
                    _defaultDb ??= db;
                }
            }

            // 用户显式指定方言时优先覆盖；否则首次 Open() 时自动对齐
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
            IDbConnection con;
            if (string.IsNullOrWhiteSpace(databaseId))
            {
                if (_defaultDb == null)
                {
                    throw new InvalidOperationException("数据库上下文尚未初始化，请先调用 Configure 或 ConfigContext。");
                }
                con = _defaultDb.Open();
            }
            else
            {
                if (!_dbs.TryGetValue(databaseId, out var proxy))
                {
                    throw new InvalidOperationException($"未找到数据库 ID：{databaseId}");
                }
                con = proxy.Open();
            }

            if (!string.IsNullOrWhiteSpace(databaseName) &&
                !string.Equals(con.Database, databaseName, StringComparison.OrdinalIgnoreCase))
            {
                con.ChangeDatabase(databaseName);
            }

            // 根据实际连接类型自动对齐全局方言，确保后续 TableDef/QueryBuilder 使用正确的语法
            SQLDialectFactory.UseDialect(con);

            return con;
        }

        /// <inheritdoc/>
        public void Do(Action<IDbConnection> action, string? databaseId = null, string? databaseName = null)
        {
            using var db = Open(databaseId, databaseName);
            action(db);
        }

        /// <inheritdoc/>
        public async Task DoAsync(Func<IDbConnection, Task> action, string? databaseId = null, string? databaseName = null)
        {
            using var db = Open(databaseId, databaseName);
            await action(db);
        }

        /// <inheritdoc/>
        public void DoTransaction(Func<IDbConnection, IDbTransaction, bool> action, string? databaseId = null, string? databaseName = null, IsolationLevel il = IsolationLevel.Unspecified)
        {
            using var db = Open(databaseId, databaseName);
            ExecuteTransaction(db, tran => action(db, tran), il);
        }

        /// <inheritdoc/>
        public async Task DoTransactionAsync(Func<IDbConnection, IDbTransaction, Task<bool>> action, string? databaseId = null, string? databaseName = null, IsolationLevel il = IsolationLevel.Unspecified)
        {
            using var db = Open(databaseId, databaseName);
            await ExecuteTransactionAsync(db, async tran => await action(db, tran), il);
        }

        /// <summary>
        /// 同步事务执行，使用当前上下文的 <see cref="LogTransactionError"/>。
        /// </summary>
        private void ExecuteTransaction(IDbConnection dbConnection, Func<IDbTransaction, bool> action, IsolationLevel il)
        {
            var tran = dbConnection.BeginTransaction(il);
            try
            {
                if (action(tran))
                    tran.Commit();
                else
                    tran.Rollback();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                LogTransactionError?.Invoke(ex, dbConnection);
                throw;
            }
        }

        /// <summary>
        /// 异步事务执行，使用当前上下文的 <see cref="LogTransactionError"/>。
        /// </summary>
        private async Task ExecuteTransactionAsync(IDbConnection dbConnection, Func<IDbTransaction, Task<bool>> action, IsolationLevel il)
        {
            var tran = dbConnection.BeginTransaction(il);
            try
            {
                if (await action(tran))
                    tran.Commit();
                else
                    tran.Rollback();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                LogTransactionError?.Invoke(ex, dbConnection);
                throw;
            }
        }

        private static string GetDialectName(DialectType type) => type switch
        {
            DialectType.SqlServer => "sqlconnection",
            DialectType.MySQL => "mysqlconnection",
            DialectType.PostgreSQL => "npgsqlconnection",
            DialectType.Oracle => "oracleconnection",
            DialectType.SQLite => "sqliteconnection",
            DialectType.DB2 => "db2connection",
            _ => "sqlconnection"
        };
    }
}
