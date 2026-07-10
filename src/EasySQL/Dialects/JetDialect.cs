using System.Text;
namespace EasySQL
{
    /// <summary>
    /// MS Access（Jet） 数据库方言实现。
    /// </summary>
    public class JetDialect:SQLDialectBase
    {
        static readonly IDbFunction dbFunc = new JetFunctions();

        /// <inheritdoc/>
        public override DialectType DialectType => DialectType.Jet;

        /// <inheritdoc/>
        public override string DialectName { get { return "Jet"; } }

        /// <inheritdoc/>
        public override IDbFunction Func { get { return dbFunc; } }

        /// <inheritdoc/>
        public override bool IsBracketJoin => true;

        /// <inheritdoc/>
        protected override string ApplyPaging(StringBuilder sql, int rowLimit, int rowOffset, bool forCount, bool isSubCount, bool readable)
        {
            if (forCount || isSubCount || rowLimit <= 0)
                return sql.ToString();

            // Jet（MS Access）使用 TOP 语法，需插入在 SELECT 之后
            sql.Insert(7, $"TOP {rowLimit + rowOffset} ");
            return sql.ToString();
        }

        protected override string QuoteKeyWord(string word)
        {
            return $"[{word}]";
        }
    }
}
