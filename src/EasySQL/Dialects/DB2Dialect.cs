using System.Text;
namespace EasySQL
{
    /// <summary>
    /// IBM DB2 数据库方言实现（DB2 11.1+，支持 OFFSET/FETCH 标准语法）。
    /// </summary>
    public class DB2Dialect:SQLDialectBase
    {
        static readonly IDbFunction dbFunc = new DB2Functions();

        /// <inheritdoc/>
        public override DialectType DialectType => DialectType.DB2;

        /// <inheritdoc/>
        public override string DialectName { get { return "DB2"; } }

        /// <inheritdoc/>
        public override IDbFunction Func { get { return dbFunc; } }

        /// <inheritdoc/>
        public override bool IsBracketJoin => false;

        /// <inheritdoc/>
        protected override string ApplyPaging(StringBuilder sql, int rowLimit, int rowOffset, bool forCount, bool isSubCount, bool readable)
        {
            if (forCount || isSubCount || rowLimit <= 0)
                return sql.ToString();

            // DB2 11.1+ 支持与 SQL Server / Oracle 相同的 OFFSET/FETCH 标准语法
            sql.Append(string.Format("{0}OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY",
                readable ? System.Environment.NewLine : " ", rowOffset, rowLimit));
            return sql.ToString();
        }
    }
}
