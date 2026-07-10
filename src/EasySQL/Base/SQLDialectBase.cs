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
        /// 获取数据库方言类型。
        /// </summary>
        public abstract DialectType DialectType { get; }

        /// <summary>
        /// 获取参数化查询的参数前缀符，默认 @。Oracle 覆写为 :。
        /// </summary>
        public virtual string ParameterPrefix => "@";

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
            tableName = join.TargetTableDef.TableName;
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
            if (!(join.TargetTableDef is QueryBuilder))
            {
                //如果不是子查询，则需要进行Quote操作。
                tableName = this.QuoteTable(join.TargetTableDef.IsPartialTableName ? join.TargetTableDef.PartialTableName : join.TargetTableDef.TableName);
            }
            alias = (join.TargetTableDef.Alias != null && join.TargetTableDef.Alias.Trim().Length > 0) ?$" {join.TargetTableDef.Alias.Trim()}" : string.Empty;
            return $"{joinClause}{tableName}{alias} on {join.OnClause}";
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

        /// <summary>
        /// 返回限制行数的 SQL 片段。rowLimit > 0 时返回 " LIMIT n"，否则返回空字符串。
        /// </summary>
        protected virtual string GetRowLimit(int rowLimit)
        {
            return rowLimit > 0 ? $" LIMIT {rowLimit}" : "";
        }

        /// <summary>
        /// 返回偏移行数的 SQL 片段。rowLimit > 0 或 rowOffset > 0 时返回 " OFFSET n"。
        /// </summary>
        protected virtual string GetRowOffset(int rowLimit, int rowOffset)
        {
            return rowLimit > 0 || rowOffset > 0 ? $" OFFSET {rowOffset}" : "";
        }

        #endregion

        /// <summary>
        /// 构建完整的 SQL 查询语句（不含分页）。分页逻辑由 <see cref="ApplyPaging"/> 处理。
        /// </summary>
        public virtual string BuildSql(QueryBuilder qb, int rowLimit, int rowOffset, bool forCount)
        {
            StringBuilder selectBuilder = new StringBuilder((!qb.IsDistinct || forCount) ? "SELECT " : "SELECT DISTINCT ");
            StringBuilder fromBuilder = new StringBuilder(string.Format("{0}FROM ", qb.PrettyPrint ? System.Environment.NewLine : " "));
            StringBuilder fromItem = new StringBuilder();
            StringBuilder sqlbuilder = new StringBuilder();

            if (forCount)
            {
                if (qb.IsDistinct && qb.FromItems.Count > 0 && qb.FromItems[0].SelectFields.Count > 0)
                {
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
                    selectBuilder.Append("Count(1) AS RowsCount");
                }
            }

            if (qb.IsSubCount)
            {
                selectBuilder.Append("1 AS r");
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
                    if (qb.SQLDialect!.IsBracketJoin)
                    {
                        for (int j = 0; j < brks; j++)
                        {
                            fromItem.Insert(0, "(");
                        }
                    }
                }
            }

            // 分页时自动注入 COUNT(*) OVER() 列，一次查询同时返回数据与总记录数
            if (rowLimit > 0 && !forCount && !qb.IsSubCount && selectBuilder.Length > 0)
            {
                selectBuilder.Append($",COUNT(*) OVER() AS {QueryBuilder.PagingTotalAlias}");
            }

            fromBuilder.Append(fromItem.ToString());
            sqlbuilder.Append(string.Format("{0}{1}", selectBuilder.ToString(), fromBuilder.ToString()));
            string existsWhere = qb.GetExistsWhere();
            // 合并 EXISTS/NOT EXISTS 子查询的参数到主查询
            qb.MergeSubQueryParameters();
            if (qb.Wherebuilder != null && qb.Wherebuilder.Length > 0)
            {
                sqlbuilder.Append(string.Format("{0}WHERE (", qb.PrettyPrint ? System.Environment.NewLine : " "));
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
                    sqlbuilder.Append(string.Format("{0}WHERE {1}", qb.PrettyPrint ? System.Environment.NewLine : " ", existsWhere));
                }
            }

            if (qb.Groupbybuilder != null && qb.Groupbybuilder.Length > 0)
            {
                sqlbuilder.Append(string.Format("{0}GROUP BY ", qb.PrettyPrint ? System.Environment.NewLine : " "));
                sqlbuilder.Append(qb.Groupbybuilder.ToString());
            }

            if (qb.Havingbuilder != null && qb.Havingbuilder.Length > 0)
            {
                sqlbuilder.Append(string.Format("{0}HAVING ", qb.PrettyPrint ? System.Environment.NewLine : " "));
                sqlbuilder.Append(qb.Havingbuilder.ToString());
            }

            if (qb.Orderbybuilder != null && qb.Orderbybuilder.Length > 0 && !forCount && !qb.IsSubCount)
            {
                sqlbuilder.Append(string.Format("{0}ORDER BY ", qb.PrettyPrint ? System.Environment.NewLine : " "));
                sqlbuilder.Append(qb.Orderbybuilder.ToString());
            }

            // 委托给子类的分页处理（各方言覆写此方法以提供各自的分页语法）
            return ApplyPaging(sqlbuilder, rowLimit, rowOffset, forCount, qb.IsSubCount, qb.PrettyPrint);
        }

        /// <summary>
        /// 对已构建的 SQL 应用分页限制。基类默认实现使用 LIMIT/OFFSET 语法（适用于 MySQL、PostgreSQL、SQLite）。
        /// 各数据库方言可覆写以实现各自的分页语法。
        /// </summary>
        /// <param name="sql">已构建的 SELECT 语句（不含分页子句）。</param>
        /// <param name="rowLimit">返回记录数上限，0 表示不限制。</param>
        /// <param name="rowOffset">偏移行数，从 0 开始。</param>
        /// <param name="forCount">是否为 COUNT 计数语句。</param>
        /// <param name="isSubCount">是否为嵌套子查询计数。</param>
        /// <param name="readable">是否生成可读格式（换行）。</param>
        /// <returns>应用分页后的完整 SQL 字符串。</returns>
        protected virtual string ApplyPaging(StringBuilder sql, int rowLimit, int rowOffset, bool forCount, bool isSubCount, bool readable)
        {
            if (!forCount && rowLimit > 0 && !isSubCount)
            {
                sql.Append(this.GetRowLimit(rowLimit));
                sql.Append(this.GetRowOffset(rowLimit, rowOffset));
            }
            return sql.ToString();
        }
    }
}
