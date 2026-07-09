using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

namespace EasySQL
{
    /// <summary>
    /// 数据库上下文环境
    /// </summary>
    public static class DbContext
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        static readonly object contextLock=new object();
        /// <summary>
        /// 所有数据库列表。
        /// </summary>
        static Dictionary<string, IDbProxy> dbs = new Dictionary<string, IDbProxy>();

        /// <summary>
        /// 默认数据库。
        /// </summary>
        static IDbProxy defaultDb = null;

        /// <summary>
        /// 事务错误全局日志。
        /// </summary>
        public static Action<Exception,IDbConnection> LogTransactionError { get;set;}

        /// <summary>
        /// 获取是否进行配置初始化。
        /// </summary>
        public static bool IsInitialized => defaultDb != null;

        /// <summary>
        /// 配置数据库环境。
        /// </summary>
        /// <param name="items"></param>
        /// <param name="clear"></param>
        public static void ConfigContext(IEnumerable<IDbProxy> items, bool clear = false)
        {
            lock (contextLock) {
                if (clear) {
                    defaultDb = null;
                    dbs.Clear();
                }
                foreach (var db in items) {
                    if (!dbs.ContainsKey(db.DatabaseId)) {
                        dbs.Add(db.DatabaseId, db);
                        if (defaultDb == null) {
                            defaultDb = db;
                            using (var con = defaultDb.Open()) {
                                SQLDialectFactory.UseDialect(con);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 打开指定的数据库，如果不指定数据库ID，则打开默认数据库（同时也可以根据数据库名称进行切换数据库）。
        /// </summary>
        /// <param name="databaseId">数据库ID</param>
        /// <param name="databaseName">数据库名称。</param>
        /// <returns></returns>
        public static IDbConnection Open(string databaseId = null,string databaseName=null)
        {
            IDbConnection con;
            if (string.IsNullOrWhiteSpace(databaseId)){
                if (defaultDb == null) {
                    throw new Exception("Error! Can't open default databse!that is maybe uninitialized! ");
                }
                con= defaultDb.Open();
            }
            else {
                if (!dbs.ContainsKey(databaseId)) {
                    throw new Exception($"Error!Cant find database by id:{databaseId}");
                }
                con = dbs[databaseId].Open();
            }
            if(!string.IsNullOrWhiteSpace(databaseName) && !String.Equals(con.Database, databaseName,StringComparison.OrdinalIgnoreCase)) {
                con.ChangeDatabase(databaseName);
            }
            return con;
        }

        /// <summary>
        /// 数据库连接进行同步的事务操作，碰到错误会自动回滚。
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="action">事务操作函数，返回是否提交事务</param>
        /// <param name="il"></param>
        static void DoTransaction(this IDbConnection dbConnection, Func<IDbTransaction,bool> action, IsolationLevel il = IsolationLevel.Unspecified)
        {
            var tran = dbConnection.BeginTransaction(il);
            try {
                bool success= action(tran);
                if (success) {
                    tran.Commit();
                }
                else {
                    tran.Rollback();
                }
            }
            catch (Exception ex) {
                tran.Rollback();
                LogTransactionError?.Invoke(ex, dbConnection);
                throw ex;
            }
        }


        /// <summary>
        /// 数据库连接进行异步的事务操作，碰到错误会自动回滚。
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="action"></param>
        /// <param name="il"></param>
        /// <returns></returns>
        static async Task DoTransactionAsync(this IDbConnection dbConnection, Func<IDbTransaction, Task<bool>> action, IsolationLevel il = IsolationLevel.Unspecified)
        {
            var tran = dbConnection.BeginTransaction(il);
            try {
                bool success = await action(tran);
                if (success) {
                    tran.Commit();
                }
                else {
                    tran.Rollback();
                }
            }
            catch (Exception ex) {
                tran.Rollback();
                LogTransactionError?.Invoke(ex, dbConnection);
                throw ex;
            }
        }

        /// <summary>
        /// 使用默认数据库进行同步操作。
        /// </summary>
        /// <param name="action">数据处理函数。</param>
        /// <param name="databaseId">数据库ID</param>
        /// <param name="databaseName">数据库名称。</param>
        public static void Do(Action<IDbConnection> action, string databaseId = null, string databaseName = null)
        {
            using (var db = DbContext.Open(databaseId,databaseName)) {
                action(db);
            }
        }

        /// <summary>
        /// 使用默认数据库进行异步操作。
        /// </summary>
        /// <param name="action">数据处理函数。</param>
        /// <param name="databaseId">数据库ID</param>
        /// <param name="databaseName">数据库名称。</param>
        /// <returns></returns>
        public static async Task DoAsync(Func<IDbConnection, Task> action, string databaseId = null, string databaseName = null)
        {
            using (var db = DbContext.Open(databaseId,databaseName)) {
                await action(db);
            }
        }

        /// <summary>
        /// 使用默认数据库进行同步事务操作。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="databaseId">数据库ID</param>
        /// <param name="databaseName">数据库名称。</param>
        /// <param name="il"></param>
        public static void DoTransaction(Func<IDbConnection, IDbTransaction, bool> action, string databaseId = null, string databaseName = null, IsolationLevel il = IsolationLevel.Unspecified)
        {
            using (var db = DbContext.Open(databaseId, databaseName)) {
                db.DoTransaction(tran =>
                {
                    return action(db, tran);
                }, il);
            }
        }

        /// <summary>
        /// 使用默认数据库进行异步事务操作。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="databaseId">数据库ID</param>
        /// <param name="databaseName">数据库名称。</param>
        /// <param name="il"></param>
        /// <returns></returns>
        public static async Task DoTransactionAsync(Func<IDbConnection, IDbTransaction, Task<bool>> action, string databaseId = null, string databaseName = null, IsolationLevel il = IsolationLevel.Unspecified)
        {
            using (var db = DbContext.Open(databaseId, databaseName)) {
                await db.DoTransactionAsync(async tran =>
                {
                    return await action(db, tran);
                }, il);
            }
        }
    }
}
