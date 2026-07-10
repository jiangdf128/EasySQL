namespace EasySQL
{
    /// <summary>
    /// IBM DB2 数据库方言实现。
    /// </summary>
    public class DB2Dialect:SQLDialectBase
    {
        static readonly IDbFunction dbFunc = new DB2Functions();

        /// <inheritdoc/>
        public override string DialectName { get { return "DB2"; } }

        /// <inheritdoc/>
        public override IDbFunction Func { get { return dbFunc; } }

        /// <inheritdoc/>
        public override bool IsBracketJoin => false;
    }
}
