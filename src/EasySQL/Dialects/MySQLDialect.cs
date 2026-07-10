namespace EasySQL
{
    /// <summary>
    /// MySQL 数据库方言实现。
    /// </summary>
    public class MySQLDialect:SQLDialectBase
    {
        static readonly IDbFunction dbFunc = new MySQLFunctions();

        /// <inheritdoc/>
        public override DialectType DialectType => DialectType.MySQL;

        /// <inheritdoc/>
        public override string DialectName { get { return "MySQL"; } }

        /// <inheritdoc/>
        public override IDbFunction Func { get { return dbFunc; } }

        /// <inheritdoc/>
        public override bool IsBracketJoin => false;

        /// <inheritdoc/>
        protected override string QuoteKeyWord(string word)
        {
            return $"`{word}`";
        }
    }
}
