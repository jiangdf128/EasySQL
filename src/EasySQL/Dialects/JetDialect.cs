namespace EasySQL
{
    /// <summary>
    /// MS Access（Jet） 数据库方言实现。
    /// </summary>
    public class JetDialect:SQLDialectBase
    {
        static readonly IDbFunction dbFunc = new JetFunctions();

        /// <inheritdoc/>
        public override string DialectName { get { return "Jet"; } }

        /// <inheritdoc/>
        public override IDbFunction Func { get { return dbFunc; } }

        /// <inheritdoc/>
        public override bool IsBracketJoin => true;

        protected override string QuoteKeyWord(string word)
        {
            return $"[{word}]";
        }
    }
}
