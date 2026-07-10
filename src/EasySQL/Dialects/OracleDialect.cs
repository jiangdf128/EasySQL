using System.Text;
namespace EasySQL
{
    /// <summary>
    /// Oracle 数据库方言实现。
    /// </summary>
    public class OracleDialect:SQLDialectBase
    {
        static readonly IDbFunction dbFunc = new OracleFunctions();

        /// <inheritdoc/>
        public override DialectType DialectType => DialectType.Oracle;

        /// <inheritdoc/>
        public override string DialectName { get { return "Oracle"; } }

        /// <inheritdoc/>
        public override IDbFunction Func { get { return dbFunc; } }

        /// <inheritdoc/>
        public override bool IsBracketJoin => false;

        /// <summary>
        /// Oracle 使用 : 作为参数前缀（而非 @）。
        /// </summary>
        public override string ParameterPrefix => ":";

        /// <inheritdoc/>
        protected override string ApplyPaging(StringBuilder sql, int rowLimit, int rowOffset, bool forCount, bool isSubCount, bool readable)
        {
            if (forCount || isSubCount || rowLimit <= 0)
                return sql.ToString();

            // Oracle 12c+ 使用与 SQL Server 相同的 OFFSET/FETCH 语法
            sql.Append(string.Format("{0}OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY",
                readable ? System.Environment.NewLine : " ", rowOffset, rowLimit));
            return sql.ToString();
        }
    }
}
