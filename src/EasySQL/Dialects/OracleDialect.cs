namespace EasySQL
{
    public class OracleDialect:SQLDialectBase
    {
        static readonly IDbFunction dbFunc = new OracleFunctions();

        public override string DialectName { get { return "Oracle"; } }

        public override IDbFunction Func { get { return dbFunc; } }

        public override bool IsBracketJoin => false;
    }
}
