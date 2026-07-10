namespace EasySQL
{
    /// <summary>
    /// SQLite 数据库方言实现。
    /// </summary>
    public class SQLiteDialect:SQLDialectBase
    {
        static readonly IDbFunction dbFunc = new SQLiteFunctions();

        /// <inheritdoc/>
        public override DialectType DialectType => DialectType.SQLite;

        /// <inheritdoc/>
        public override string DialectName { get { return "SQLite"; } }

        /// <inheritdoc/>
        public override IDbFunction Func { get { return dbFunc; } }

        /// <inheritdoc/>
        public override bool IsBracketJoin => false;
    }
}
