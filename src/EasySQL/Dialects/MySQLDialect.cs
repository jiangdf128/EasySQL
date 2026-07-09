namespace EasySQL
{
    public class MySQLDialect:SQLDialectBase
    {
        static readonly IDbFunction dbFunc = new MySQLFunctions();

        public override string DialectName { get { return "MySQL"; } }

        public override IDbFunction Func { get { return dbFunc; } }

        public override bool IsBracketJoin => false;

        protected override string QuoteKeyWord(string word)
        {
            return $"`{word}`";
        }
    }
}
