using System;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// SQL 方言抽象基类，提供 <see cref="ISQLDialect"/> 的默认实现。
    /// 包含 SQL 构建的核心逻辑（SELECT/FROM/JOIN/WHERE/GROUP BY/ORDER BY）
    /// 以及 7 种数据库的分页语法支持。子类需覆写 <see cref="DialectName"/>、
    /// <see cref="Func"/> 和 <see cref="QuoteKeyWord"/>。
    /// </summary>
    public abstract class SQLDialectBase : ISQLDialect
    {
        /// <summary>
        /// 获取数据库方言名称，如 "SQLServer"、"MySQL"、"PostgreSQL" 等。
        /// </summary>
        public abstract string DialectName { get; }

        /// <summary>
        /// 获取该方言对应的数据库函数集。
        /// </summary>
        public abstract IDbFunction Func { get; }

        /// <summary>
        /// 翻译连接条件为完整的 JOIN 子句（含 ON 条件）。
        /// </summary>
        /// <param name="join">连接条件对象。</param>
        /// <returns>完整的 JOIN 子句字符串。</returns>
        public virtual string GetJoinClause(JoinCondition join)
        {
            string joinClause, tableName, alias;
            joinClause = string.Empty;
            tableName = join.TargetSchema.TableName;
            switch (join.JoinClass)
            {
                case JoinType.Inner:
                    joinClause = "INNER JOIN "; break;
                case JoinType.Left:
                    joinClause = "LEFT OUTER JOIN "; break;
                case JoinType.Right:
                    joinClause = "RIGHT OUTER JOIN "; break;
                case JoinType.Outer:
                    joinClause = "FULL OUTER JOIN "; break;
            }
            if (!(join.TargetSchema is QueryBuilder))
            {
                //如果不是子查询，则需要进行Quote操作。
                tableName = this.QuoteTable(join.TargetSchema.IsPartialTableName ? join.TargetSchema.PartialTableName : join.TargetSchema.TableName);
            }
            alias = (join.TargetSchema.Alias != null && join.TargetSchema.Alias.Trim().Length > 0) ?$" {join.TargetSchema.Alias.Trim()}" : string.Empty;
            return (this.DialectName == "Jet") ? $"{joinClause}{tableName}{alias} on ({join.OnClause})" : $"{joinClause}{tableName}{alias} on {join.OnClause}";
        }

        /// <summary>
        /// 判断指定词是否为该数据库的保留关键字。默认返回 false。
        /// </summary>
        /// <param name="word">要检查的词。</param>
        /// <returns>是保留关键字则为 true。</returns>
        public virtual bool IsKeyWord(string word) { return false; }

        /// <summary>
        /// 修饰字段名，如果是保留关键字则进行转义处理。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <returns>安全的字段引用字符串。</returns>
        public virtual string QuoteField(string fieldName)
        {
            if (this.IsKeyWord(fieldName))
            {
                return this.QuoteKeyWord(fieldName);
            }
            return fieldName;
        }

        /// <summary>
        /// 修饰表名，如果是保留关键字则进行转义处理。
        /// </summary>
        /// <param name="tableName">表名称。</param>
        /// <returns>安全的表名引用字符串。</returns>
        public virtual string QuoteTable(string tableName)
        {
            if (this.IsKeyWord(tableName))
            {
                return this.QuoteKeyWord(tableName);
            }
            return tableName;
        }

        /// <summary>
        /// 为保留关键字添加转义引号。子类覆写以使用各数据库特定的引号字符。
        /// </summary>
        /// <param name="word">保留关键字。</param>
        /// <returns>转义后的关键字字符串。</returns>
        protected virtual string QuoteKeyWord(string word) { return word; }

        /// <summary>
        /// 获取方言是否需要括弧扩起连接子句。
        /// </summary>
        public virtual bool IsBracketJoin { get { return false; } }


        #region 分页支持

        // various internally used strings
        private const string fmtTop = "TOP {0} ";

        private const string fmtRowLimit = " LIMIT {0}";
        private const string fmtRowOffset = " OFFSET {0}";

        // {0} = 原SQL查询。
        // {2} = rowLimit + rowOffest, {1} = rowOffset
        private const string fmtOracleLimit = "SELECT * FROM ({0}) WHERE r>{1} and r<={2}";

        // Uwe Kitzmann: change template to support Sybase ASA dialect of paged queries
        private const string fmtSybaseASALimit = " TOP {0} ";
        private const string fmtSybaseASAOffset = " START AT {0} ";

        // Firebird
        private const string fmtFirebirdLimit = " FIRST {0} ";
        private const string fmtFirebirdOffset = " SKIP {0} ";

        private const string fmtDb2Limit = " FETCH FIRST {0} ROWS ONLY ";

        /// <summary>
        /// This method returns the SQL string for restricting the number of rows if a limit
        /// has been specified and the empty string otherwise.
        /// </summary>
        /// <returns>A fragment for use in an sql statement.</returns>
        protected virtual string GetTopLimit(int rowLimit)
        {
            return rowLimit > 0 ? String.Format(fmtTop, rowLimit) : "";
        }

        /// <summary>
        /// This method returns the SQL string for restricting the number of rows if a limit
        /// has been specified and the empty string otherwise.
        /// </summary>
        /// <returns>A fragment for use in an sql statement.</returns>
        protected virtual string GetRowLimit(int rowLimit)
        {
            return rowLimit > 0 ? String.Format(fmtRowLimit, rowLimit) : "";
        }

        /// <summary>
        /// This method returns the SQL string for skipping past a number of rows. This is useful
        /// in combination with RowLimit for paging of data.
        /// </summary>
        /// <returns>A fragment for use in an sql statement.</returns>
        protected virtual string GetRowOffset(int rowLimit, int rowOffset)
        {
            // always include offset if a row limit is applied (required for paging)
            return rowLimit > 0 || rowOffset > 0 ? String.Format(fmtRowOffset, rowOffset) : "";
        }

        #endregion

        /// <summary>
        /// 构建完整的 SQL 查询语句。
        /// 包含 SELECT、FROM、JOIN、WHERE、GROUP BY、HAVING、ORDER BY 及分页子句的生成。
        /// </summary>
        /// <param name="qb">查询构建器实例。</param>
        /// <param name="rowLimit">返回记录数上限，0 表示不限制。</param>
        /// <param name="rowOffset">偏移行数，从 0 开始。</param>
        /// <param name="forCount">是否生成 COUNT 计数语句。</param>
        /// <returns>完整的 SQL 查询语句字符串。</returns>
        public virtual string BuildSql(QueryBuilder qb, int rowLimit, int rowOffset, bool forCount)
        {
            StringBuilder selectBuilder = new StringBuilder((!qb.IsDistinct || forCount) ? "SELECT " : "SELECT DISTINCT ");
            StringBuilder fromBuilder = new StringBuilder(string.Format("{0}FROM ", qb.Readable ? System.Environment.NewLine : " "));
            StringBuilder fromItem = new StringBuilder();
            StringBuilder sqlbuilder = new StringBuilder();
            bool handleLimit = false;

            if (forCount)
            {
                if (qb.IsDistinct && qb.FromItems[0].SelectFields.Count > 0)
                {
                    //如果是Distinct开头的SQL语句，则计数也需要使用Disintct，但必需有主键。
                    //为了配合自动分页功能，因此默认以第一个表的第一列作为主键。蒋德福 2012-12-21。
                    //这样做，程序员必需清楚，第一列必需传入主键。
                    string field = qb.FromItems[0].SelectFields[0];
                    int idx = field.LastIndexOf(" AS ");
                    if (idx >= 0)
                    {
                        field = field.Substring(0, idx);
                    }
                    selectBuilder.Append(string.Format("Count(DISTINCT {0}) AS RowsCount", field));
                }
                else
                {
                    //翻译成Count语句，否则将翻译成非Count语句。
                    selectBuilder.Append("Count(1) AS RowsCount");
                }
            }

            if (qb.IsSubCount)
            {
                selectBuilder.Append("1 AS r");
            }

            //添加分页支持功能（SQLServere，Jet，FireBird，SysbaseASA）。
            if (!qb.IsSubCount && !forCount && rowLimit > 0)
            {
                // SQL Server and MS Access
                if (qb.SQLDialect.DialectName == "SQLServer")
                {
                    handleLimit = true;
                }
                else if (qb.SQLDialect.DialectName == "Jet")
                {
                    selectBuilder.Append(this.GetTopLimit(rowLimit + rowOffset));
                    handleLimit = true;
                }
                else if (qb.SQLDialect.DialectName.Equals("Firebird"))
                {
                    selectBuilder.Append(String.Format(fmtFirebirdLimit, rowLimit));
                    selectBuilder.Append(String.Format(fmtFirebirdOffset, rowOffset));
                    handleLimit = true;
                }
                // Uwe Kitzmann: Support for Sybase ASA
                else if (qb.SQLDialect.DialectName.StartsWith("SybaseASA"))
                {
                    selectBuilder.Append(String.Format(fmtSybaseASALimit, rowLimit));
                    selectBuilder.Append(String.Format(fmtSybaseASAOffset, rowOffset));
                    handleLimit = true;
                }
            }

            for (int i = 0, idx1 = 0; i < qb.FromItems.Count; i++)
            {
                if (qb.FromItems[i].GetSelectClause().Length > 0 && !forCount && !qb.IsSubCount)
                {
                    if (idx1 > 0)
                    {
                        selectBuilder.Append(",");
                    }
                    selectBuilder.Append(qb.FromItems[i].GetSelectClause());
                    idx1++;
                }

                if (i == 0)
                {
                    fromItem.Append(qb.FromItems[i].QuoteName());
                }
                int brks = 0;
                if (qb.FromItems[i].GetJoinClause(out brks).Length > 0)
                {
                    fromItem.Append(" ");
                    fromItem.Append(qb.FromItems[i].GetJoinClause(out brks));
                    if (qb.SQLDialect.IsBracketJoin)
                    {
                        for (int j = 0; j < brks; j++)
                        {
                            fromItem.Insert(0, "(");
                        }
                    }
                }
            }

            fromBuilder.Append(fromItem.ToString());
            sqlbuilder.Append(string.Format("{0}{1}", selectBuilder.ToString(), fromBuilder.ToString()));
            string existsWhere = qb.GetExistsWhere();
            if (qb.Wherebuilder != null && qb.Wherebuilder.Length > 0)
            {
                sqlbuilder.Append(string.Format("{0}WHERE (", qb.Readable ? System.Environment.NewLine : " "));
                sqlbuilder.Append(qb.Wherebuilder.ToString());
                sqlbuilder.Append(")");
                if (existsWhere.Length > 0)
                {
                    sqlbuilder.Append(" AND ");
                    sqlbuilder.Append(existsWhere);
                }
            }
            else
            {
                if (existsWhere.Length > 0)
                {
                    sqlbuilder.Append(string.Format("{0}WHERE {1}", qb.Readable ? System.Environment.NewLine : " ", existsWhere));
                }
            }

            if (qb.Groupbybuilder != null && qb.Groupbybuilder.Length > 0)
            {
                sqlbuilder.Append(string.Format("{0}GROUP BY ", qb.Readable ? System.Environment.NewLine : " "));
                sqlbuilder.Append(qb.Groupbybuilder.ToString());
            }

            if (qb.Havingbuilder != null && qb.Havingbuilder.Length > 0)
            {
                sqlbuilder.Append(string.Format("{0}HAVING ", qb.Readable ? System.Environment.NewLine : " "));
                sqlbuilder.Append(qb.Havingbuilder.ToString());
            }

            if (qb.Orderbybuilder != null && qb.Orderbybuilder.Length > 0 && !forCount && !qb.IsSubCount)
            {
                sqlbuilder.Append(string.Format("{0}ORDER BY ", qb.Readable ? System.Environment.NewLine : " "));
                sqlbuilder.Append(qb.Orderbybuilder.ToString());
                if (rowLimit > 0 && qb.SQLDialect.DialectName == "SQLServer")
                {
                    sqlbuilder.Append(string.Format("{0}OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY ", qb.Readable ? System.Environment.NewLine : " ", rowOffset, rowLimit));
                }
            }

            //如果没有任何Order by，为了支持Sql新的分页语法，则使用常量排序。
            if ((qb.Orderbybuilder == null || qb.Orderbybuilder.Length == 0) && !forCount && !qb.IsSubCount)
            {
                if (rowLimit > 0 && qb.SQLDialect.DialectName == "SQLServer")
                {
                    sqlbuilder.Append(string.Format("{0}ORDER BY 1 ", qb.Readable ? System.Environment.NewLine : " "));
                    sqlbuilder.Append(string.Format("{0}OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY ", qb.Readable ? System.Environment.NewLine : " ", rowOffset, rowLimit));
                }
            }

            //添加分页支持功能（Oracle，及其他数据库引擎）。
            if (!forCount && rowLimit > 0 && !handleLimit)
            {
                // Oracle and OracleODP Oracle分页支持。
                if (qb.SQLDialect.DialectName.StartsWith("Oracle"))
                {
                    string sql = sqlbuilder.ToString();
                    sql = string.Format("SELECT P.*,rownum AS r from ({0}) P", sql);
                    sqlbuilder.Remove(0, sqlbuilder.Length);
                    sql = string.Format(fmtOracleLimit, sql, rowOffset, rowOffset + rowLimit);
                    sqlbuilder.Append(sql);
                }
                else if (qb.SQLDialect.DialectName.StartsWith("DB2"))
                {
                    sqlbuilder.Append(string.Format(fmtDb2Limit, rowLimit + rowOffset));
                }
                else
                {
                    //其他数据库分页支持。
                    sqlbuilder.Append(this.GetRowLimit(rowLimit));
                    sqlbuilder.Append(this.GetRowOffset(rowLimit, rowOffset));
                }
            }
            return sqlbuilder.ToString();
        }
    }
}
