namespace EasySQL
{
   public class DB2Dialect:SQLDialectBase
    {
        static readonly IDbFunction dbFunc = new DB2Functions();

        public override string DialectName { get { return "DB2"; } }

        public override IDbFunction Func { get { return dbFunc; } }

        public override bool IsBracketJoin => false;
    }
}
