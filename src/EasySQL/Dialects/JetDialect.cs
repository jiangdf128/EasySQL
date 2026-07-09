namespace EasySQL
{
    public class JetDialect:SQLDialectBase
    {
        static readonly IDbFunction dbFunc = new JetFunctions();

        public override string DialectName { get { return "Jet"; } }

        public override IDbFunction Func { get { return dbFunc; } }

        public override bool IsBracketJoin => true;

        protected override string QuoteKeyWord(string word)
        {
            return $"[{word}]";
        }
    }
}
