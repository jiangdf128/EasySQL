namespace EasySQL
{
    /// <summary>
    /// SQL 方言接口，定义各数据库在字段引号、表名引号、关键字转义、
    /// 连接语法、分页语法等方面的差异。通过 <see cref="SQLDialectFactory"/>
    /// 可根据数据库连接自动匹配合适的方言实现。
    /// </summary>
    public interface ISQLDialect
    {

        /// <summary>
        /// 获取数据库方言类型枚举。
        /// </summary>
        DialectType DialectType { get; }

        /// <summary>
        /// 获取参数化查询的参数前缀符。SQL Server/MySQL/PostgreSQL 为 @，Oracle 为 :。
        /// </summary>
        string ParameterPrefix { get; }

        /// <summary>
        /// 获取数据库方言名称。
        /// </summary>
        string DialectName { get; }

        /// <summary>
        /// 判断是否数据库的保留词。
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        bool IsKeyWord(string word);

        /// <summary>
        /// 返回数据库方言的引用字段。
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        string QuoteField(string fieldName);

        /// <summary>
        /// 返回数据库方言的引用表名。
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string QuoteTable(string tableName);

        /// <summary>
        /// 获取特定数据库方言的部分系统函数。
        /// </summary>
        IDbFunction Func { get; }

        /// <summary>
        /// 翻译连接条件子句。
        /// </summary>
        /// <param name="join"></param>
        /// <returns></returns>
        string GetJoinClause(JoinCondition join);

        /// <summary>
        /// 获取方言是否需要括弧扩起连接子句。
        /// </summary>
        bool IsBracketJoin { get; }

        /// <summary>
        /// 构造QueryBuilder对象的SQL语句。
        /// </summary>
        /// <param name="qb"></param>
        /// <param name="rowLimit"></param>
        /// <param name="rowOffset"></param>
        /// <param name="forCount">是否为计数构造。</param>
        /// <returns></returns>
        string BuildSql(QueryBuilder qb, int rowLimit, int rowOffset, bool forCount);

        /// <summary>
        /// 构造 SELECT ... INTO / CREATE TABLE ... AS SELECT 语句，
        /// 从查询结果创建新表（包括临时表）。各方言自动选择正确的语法。
        /// </summary>
        /// <param name="qb">查询构建器。</param>
        /// <param name="targetTable">目标表名。</param>
        /// <param name="isTemp">是否为临时表。</param>
        /// <returns>完整 SQL 语句。</returns>
        string BuildIntoSql(QueryBuilder qb, string targetTable, bool isTemp);
    }
}
