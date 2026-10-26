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
                tableName = this.QuoteTable(join.TargetTableDef.IsPartialTableName ? join.TargetTableDef.PartialTableName! : join.TargetTableDef.TableName);
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
        /// 分页语法是否强制要求 ORDER BY。
        /// SQL Server / Oracle / DB2 的 OFFSET/FETCH 语法必须配合 ORDER BY，
        /// 由基类统一在缺少 ORDER BY 时补一个常量排序（ORDER BY 1）。
        /// </summary>
        protected virtual bool RequireOrderByForPaging { get { return false; } }

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
        /// <remarks>
        /// 这是面向方言实现方的低阶入口，保留"分页即带总数列"的旧行为；
        /// 非分页场景请调用方显式使用 5 参重载并传入 <c>includeTotalCount: false</c>。
        /// </remarks>
        public virtual string BuildSql(QueryBuilder qb, int rowLimit, int rowOffset, bool forCount)
        {
            return this.BuildSql(qb, rowLimit, rowOffset, forCount, includeTotalCount: true);
        }

        /// <summary>
        /// 构建完整的 SQL 查询语句，可控制分页时是否注入 <c>COUNT(*) OVER() AS TotalRows</c> 总数列。
        /// </summary>
        /// <param name="qb">查询构建器。</param>
        /// <param name="rowLimit">返回最大可能的记录数，0 表示不限制。</param>
        /// <param name="rowOffset">偏移位置（从0行开始算起）。</param>
        /// <param name="forCount">是否为计数构造。</param>
        /// <param name="includeTotalCount">分页时是否注入总记录数列。false 时只做分页不返回总数，
        /// 调用方需配合 <see cref="QueryBuilder.BuildCountSql"/> 单独查询总数。</param>
        public virtual string BuildSql(QueryBuilder qb, int rowLimit, int rowOffset, bool forCount, bool includeTotalCount)
        {
            // UNION 查询必须整体构建（排序与分页都作用于合并后的结果集）
            if (qb.UnionList != null && qb.UnionList.Count > 0)
            {
                return this.BuildUnionSql(qb, rowLimit, rowOffset, forCount, includeTotalCount);
            }
            return this.BuildSelectSql(qb, rowLimit, rowOffset, forCount, includeTotalCount);
        }

        /// <summary>
        /// 构建单表（无 UNION）的 SELECT 语句。
        /// </summary>
        protected virtual string BuildSelectSql(QueryBuilder qb, int rowLimit, int rowOffset, bool forCount, bool includeTotalCount)
        {
            StringBuilder selectBuilder = new StringBuilder((!qb.IsDistinct || forCount) ? "SELECT " : "SELECT DISTINCT ");
            StringBuilder fromBuilder = new StringBuilder(string.Format("{0}FROM ", qb.PrettyPrint ? System.Environment.NewLine : " "));
            StringBuilder fromItem = new StringBuilder();
            StringBuilder sqlbuilder = new StringBuilder();

            if (forCount)
            {
                selectBuilder.Append("Count(1) AS RowsCount");
            }

            bool hasSelectFields = false;
            for (int i = 0, idx1 = 0; i < qb.FromItems.Count; i++)
            {
                if (qb.FromItems[i].GetSelectClause().Length > 0 && !forCount)
                {
                    if (idx1 > 0)
                    {
                        selectBuilder.Append(",");
                    }
                    selectBuilder.Append(qb.FromItems[i].GetSelectClause());
                    idx1++;
                    hasSelectFields = true;
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

            // 没有指定查询列时补一个默认列，避免生成 "SELECT FROM ..." 这样的非法语句
            if (!hasSelectFields && !forCount)
            {
                if (qb.IsCountInner && !qb.IsDistinct)
                {
                    // 计数内层子查询：只关心行数，用常量列即可
                    selectBuilder.Append("1 AS r");
                }
                else
                {
                    selectBuilder.Append(GetDefaultSelectColumns(qb));
                }
            }

            // 分页时自动注入 COUNT(*) OVER() 列，一次查询同时返回数据与总记录数
            if (rowLimit > 0 && includeTotalCount && !forCount && !qb.IsCountInner && selectBuilder.Length > 0)
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

            bool hasOrderBy = false;
            if (qb.Orderbybuilder != null && qb.Orderbybuilder.Length > 0 && !forCount && !qb.IsCountInner && !qb.SuppressOrderBy)
            {
                sqlbuilder.Append(string.Format("{0}ORDER BY ", qb.PrettyPrint ? System.Environment.NewLine : " "));
                sqlbuilder.Append(qb.Orderbybuilder.ToString());
                hasOrderBy = true;
            }

            // 部分数据库的分页语法（SQL Server / Oracle / DB2 的 OFFSET/FETCH）强制要求 ORDER BY，
            // 调用方没有指定时补一个常量排序（按第一列升序）。
            if (!forCount && !qb.IsCountInner && rowLimit > 0 && this.RequireOrderByForPaging && !hasOrderBy)
            {
                sqlbuilder.Append(string.Format("{0}ORDER BY 1 ", qb.PrettyPrint ? System.Environment.NewLine : " "));
            }

            // 委托给子类的分页处理（各方言覆写此方法以提供各自的分页语法）
            return ApplyPaging(sqlbuilder, rowLimit, rowOffset, forCount, qb.IsCountInner, qb.PrettyPrint);
        }

        /// <summary>
        /// 构建 UNION 查询的 SQL。UNION 的排序与分页都作用于合并后的结果集，
        /// 因此各分支不再输出 ORDER BY，整体语句在需要排序或分页时会包裹为派生表。
        /// </summary>
        protected virtual string BuildUnionSql(QueryBuilder qb, int rowLimit, int rowOffset, bool forCount, bool includeTotalCount)
        {
            string sep = qb.PrettyPrint ? System.Environment.NewLine : " ";
            StringBuilder body = new StringBuilder();

            // 1) 逐个构建分支（分支内不输出 ORDER BY）
            qb.SuppressOrderBy = true;
            try
            {
                body.Append(this.BuildSelectSql(qb, 0, 0, false, false));
                for (int i = 0; i < qb.UnionList!.Count; i++)
                {
                    var item = qb.UnionList[i].Key;
                    // 分支内不输出 ORDER BY（支持嵌套的 UNION）
                    string branchSql = item.BuildSqlWithoutOrderBy();
                    body.Append(sep);
                    body.Append(qb.UnionList[i].Value ? "UNION ALL" : "UNION");
                    body.Append(sep);
                    body.Append(branchSql);
                    // 合并被 UNION 的查询的参数到主查询，确保执行时所有参数可用
                    foreach (var kv in item.Parameters)
                        qb.Parameters[kv.Key] = kv.Value;
                }
            }
            finally
            {
                qb.SuppressOrderBy = false;
            }

            // 2) 计数：对合并后的结果集计数
            if (forCount)
            {
                string countAlias = string.IsNullOrWhiteSpace(qb.Alias) ? "A" : qb.Alias;
                return $"SELECT Count(1) AS RowsCount{sep}FROM ({body}) {countAlias}";
            }

            bool needPaging = rowLimit > 0;
            bool hasOrderBy = !qb.SuppressOrderBy && !qb.IsCountInner
                              && qb.Orderbybuilder != null && qb.Orderbybuilder.Length > 0;

            // 3) 既不排序也不分页：直接输出 UNION 语句
            if (!hasOrderBy && !needPaging)
            {
                return body.ToString();
            }

            // 4) 需要排序或分页：包裹为派生表，在外层排序 / 分页
            string alias = string.IsNullOrWhiteSpace(qb.Alias) ? "A" : qb.Alias;
            StringBuilder outer = new StringBuilder();
            outer.Append("SELECT ");
            if (needPaging && includeTotalCount)
            {
                outer.Append($"*,COUNT(*) OVER() AS {QueryBuilder.PagingTotalAlias} ");
            }
            else
            {
                outer.Append("* ");
            }
            outer.Append($"{sep}FROM ({body}) {alias}");

            // ORDER BY：把调用方的排序表达式映射到 UNION 结果集的输出列名
            string orderBy = hasOrderBy ? qb.BuildUnionOrderByClause(alias) : string.Empty;
            if (orderBy.Length == 0 && needPaging && this.RequireOrderByForPaging)
            {
                // 分页强制要求排序，但调用方没有指定：按结果集的第一个输出列排序
                string? firstCol = qb.GetFirstUnionOutputColumn();
                if (!string.IsNullOrEmpty(firstCol))
                {
                    orderBy = $"{alias}.{qb.SQLDialect!.QuoteField(firstCol)}";
                }
                else
                {
                    // 首个分支选的是 * ，输出列名无从得知，退化为按序号排序
                    orderBy = "1";
                }
            }
            if (orderBy.Length > 0)
            {
                outer.Append($"{sep}ORDER BY {orderBy}");
            }

            // 分页
            if (needPaging)
            {
                return this.ApplyPaging(outer, rowLimit, rowOffset, false, false, qb.PrettyPrint);
            }
            return outer.ToString();
        }

        /// <summary>
        /// 获取没有指定查询列时的默认查询列（单表带别名时用 <c>别名.*</c>，否则用 <c>*</c>）。
        /// </summary>
        private static string GetDefaultSelectColumns(QueryBuilder qb)
        {
            if (qb.FromItems.Count == 1)
            {
                string alias = qb.FromItems[0].Alias;
                if (!string.IsNullOrWhiteSpace(alias))
                {
                    return $"{alias.Trim()}.*";
                }
            }
            return "*";
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

        /// <summary>
        /// 构造 SELECT ... INTO / CREATE TABLE ... AS SELECT 语句。
        /// 默认使用 CREATE TABLE AS SELECT 语法（MySQL、PostgreSQL、SQLite、Oracle）。
        /// SQL Server 覆写为 SELECT ... INTO 语法。
        /// </summary>
        /// <param name="qb">查询构建器。</param>
        /// <param name="targetTable">目标表名。</param>
        /// <param name="isTemp">是否为临时表。</param>
        /// <returns>完整 SQL 语句。</returns>
        public virtual string BuildIntoSql(QueryBuilder qb, string targetTable, bool isTemp)
        {
            string selectSql = qb.BuildSql();
            string tempKeyword = isTemp ? "TEMPORARY " : "";
            return $"CREATE {tempKeyword}TABLE {QuoteTable(targetTable)} AS {selectSql}";
        }
    }
}
