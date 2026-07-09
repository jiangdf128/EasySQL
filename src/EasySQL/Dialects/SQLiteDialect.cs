namespace EasySQL
{
   public class SQLiteDialect:SQLDialectBase
    {
        static readonly IDbFunction dbFunc = new SQLiteFunctions();

        public override string DialectName { get { return "SQLite"; } }

        public override IDbFunction Func { get { return dbFunc; } }

        public override bool IsBracketJoin => false;
    }
}
