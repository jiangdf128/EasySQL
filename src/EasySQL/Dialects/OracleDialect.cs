namespace EasySQL
{
    /// <summary>
    /// Oracle 数据库方言实现。
    /// </summary>
    public class OracleDialect:SQLDialectBase
    {
        static readonly IDbFunction dbFunc = new OracleFunctions();

        /// <inheritdoc/>
        public override string DialectName { get { return "Oracle"; } }

        /// <inheritdoc/>
        public override IDbFunction Func { get { return dbFunc; } }

        /// <inheritdoc/>
        public override bool IsBracketJoin => false;
    }
}
