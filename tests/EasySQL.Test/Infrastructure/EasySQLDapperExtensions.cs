using Dapper;
using System.Data;

namespace EasySQL
{
    /// <summary>
    /// EasySQL 与 Dapper 的桥接扩展，提供参数转换和快捷查询执行方法。
    /// </summary>
    public static class EasySQLDapperExtensions
    {
        // ================================================================
        // 方案 A：参数转换
        // ================================================================

        /// <summary>
        /// 将 EasySQL 的参数字典转换为 Dapper 的 <see cref="DynamicParameters"/> 对象，
        /// 用于直接传递给 Dapper 的 Query/Execute 方法。
        /// </summary>
        /// <param name="parameters">EasySQL 收集的参数字典（键为参数名，不含 @ 前缀）。</param>
        /// <returns>Dapper DynamicParameters 实例。</returns>
        public static DynamicParameters ToDynamicParameters(this Dictionary<string, object> parameters)
        {
            var dp = new DynamicParameters();
            if (parameters != null)
            {
                foreach (var kv in parameters)
                    dp.Add(kv.Key, kv.Value);
            }
            return dp;
        }

        // ================================================================
        // 方案 B：快捷执行方法 — QueryBuilder
        // ================================================================

        /// <summary>
        /// 执行查询并返回结果集。
        /// </summary>
        public static Task<IEnumerable<T>> QueryAsync<T>(
            this QueryBuilder qb,
            IDbConnection connection,
            IDbTransaction? transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return connection.QueryAsync<T>(
                qb.BuildSql(),
                qb.Parameters.ToDynamicParameters(),
                transaction,
                commandTimeout,
                commandType);
        }

        /// <summary>
        /// 执行查询并返回第一条记录，若无结果则返回默认值。
        /// </summary>
        public static Task<T?> QueryFirstOrDefaultAsync<T>(
            this QueryBuilder qb,
            IDbConnection connection,
            IDbTransaction? transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return connection.QueryFirstOrDefaultAsync<T>(
                qb.BuildSql(),
                qb.Parameters.ToDynamicParameters(),
                transaction,
                commandTimeout,
                commandType);
        }

        /// <summary>
        /// 执行计数查询并返回行数。
        /// </summary>
        public static Task<int> CountAsync(
            this QueryBuilder qb,
            IDbConnection connection,
            IDbTransaction? transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return connection.ExecuteScalarAsync<int>(
                qb.BuildCountSql(),
                qb.Parameters.ToDynamicParameters(),
                transaction,
                commandTimeout,
                commandType);
        }

        /// <summary>
        /// 分页查询（两次查询模式）——先查总记录数，再查当前页数据。
        /// 分页语句不注入 <c>COUNT(*) OVER()</c> 窗口函数，避免大表深分页时的额外开销。
        /// </summary>
        /// <example>
        /// <code>
        /// var (items, total) = await qb.QueryPagedAsync&lt;User&gt;(conn, rowLimit: 20, rowOffset: 40);
        /// </code>
        /// </example>
        /// <param name="qb">查询构建器。</param>
        /// <param name="connection">数据库连接。</param>
        /// <param name="rowLimit">每页记录数。</param>
        /// <param name="rowOffset">偏移行数（从 0 开始）。</param>
        /// <param name="transaction">数据库事务。</param>
        /// <param name="commandTimeout">命令超时时间（秒）。</param>
        /// <returns>当前页数据与总记录数。</returns>
        public static async Task<(IEnumerable<T> Items, int Total)> QueryPagedAsync<T>(
            this QueryBuilder qb,
            IDbConnection connection,
            int rowLimit,
            int rowOffset = 0,
            IDbTransaction? transaction = null,
            int? commandTimeout = null)
        {
            var parameters = qb.Parameters.ToDynamicParameters();
            int total = await connection.ExecuteScalarAsync<int>(
                qb.BuildCountSql(), parameters, transaction, commandTimeout);
            var items = await connection.QueryAsync<T>(
                qb.BuildSql(rowLimit, rowOffset, includeTotalCount: false), parameters, transaction, commandTimeout);
            return (items, total);
        }

        // ================================================================
        // 方案 B：快捷执行方法 — UpdateBuilder
        // ================================================================

        /// <summary>
        /// 执行 UPDATE 语句并返回受影响行数。
        /// </summary>
        public static Task<int> ExecuteAsync(
            this UpdateBuilder update,
            IDbConnection connection,
            IDbTransaction? transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return connection.ExecuteAsync(
                update.BuildSql(),
                update.Parameters.ToDynamicParameters(),
                transaction,
                commandTimeout,
                commandType);
        }

        // ================================================================
        // 方案 B：快捷执行方法 — DeleteBuilder
        // ================================================================

        /// <summary>
        /// 执行 DELETE 语句并返回受影响行数。
        /// </summary>
        public static Task<int> ExecuteAsync(
            this DeleteBuilder delete,
            IDbConnection connection,
            IDbTransaction? transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return connection.ExecuteAsync(
                delete.BuildSql(),
                delete.Parameters.ToDynamicParameters(),
                transaction,
                commandTimeout,
                commandType);
        }

        // ================================================================
        // 方案 B：快捷执行方法 — InsertBuilder
        // ================================================================

        /// <summary>
        /// 执行 INSERT 语句。
        /// 注意：InsertBuilder 的 BuildSql 需要传入子查询 SQL 作为数据源。
        /// </summary>
        public static Task<int> ExecuteAsync(
            this InsertBuilder insert,
            string fromQuerySql,
            IDbConnection connection,
            IDbTransaction? transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return connection.ExecuteAsync(
                insert.BuildSql(fromQuerySql),
                null,
                transaction,
                commandTimeout,
                commandType);
        }
    }
}
