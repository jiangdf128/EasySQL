using System;
using System.Collections.Generic;

namespace EasySQL
{
    /// <summary>
    /// 临时表 / 过程表定义。继承 <see cref="TableDefBase"/>，无需预定义类文件，
    /// 构造时传入表名和列名即可参与联合查询。
    /// </summary>
    /// <remarks>
    /// <h3>典型场景</h3>
    /// - SQL Server <c>#temp</c> 表 / PostgreSQL <c>TEMP</c> 表
    /// - 存储过程中的中间结果表
    /// - 聚合 / 统计结果的后续 JOIN
    /// <h3>用法</h3>
    /// <code>
    /// // ① 创建临时表定义
    /// var tmp = new TempTableDef("#tmp_users", "Id", "UserName", "Status");
    ///
    /// // ② 直接参与联合查询
    /// sa.Join(tmp, $"{sa.GetId()} = {tmp["Id"]}");
    /// var qb = new QueryBuilder().From(sa, tmp)
    ///     .Where($"{tmp["Status"]} = 1");
    ///
    /// // ③ 通过 InsertBuilder.BuildIntoSql 生成 SELECT INTO 语句
    /// string sql = new InsertBuilder(tmp).BuildIntoSql(fromQuery: subQb);
    /// </code>
    /// </remarks>
    public class TempTableDef : TableDefBase
    {
        private readonly string _tableName;
        private readonly HashSet<string> _columns;

        /// <summary>
        /// 获取临时表的名称。
        /// </summary>
        public override string TableName => _tableName;

        /// <summary>
        /// 创建临时表定义（无别名，可通过 <see cref="TableDefBase.Alias"/> 属性设置）。
        /// </summary>
        /// <param name="tableName">表名，如 <c>"#temp"</c>（SQL Server）、<c>"tmp_orders"</c>（PostgreSQL）</param>
        /// <param name="columns">列名列表，用于参与查询时的字段引用</param>
        public TempTableDef(string tableName, params string[] columns)
            : base(string.Empty, null)
        {
            _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            _columns = new HashSet<string>(columns, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 通过索引器获取字段引用（自动应用方言引号和表别名前缀）。
        /// </summary>
        /// <param name="columnName">列名。</param>
        /// <returns>带方言引号和前缀的字段引用字符串。</returns>
        /// <example>
        /// <code>
        /// var tmp = new TempTableDef("#t", "Id", "Name");
        /// string field = tmp["Id"]; // → "Id"（无别名时）
        /// // 别名为 "t" 时 → "t.Id"
        /// </code>
        /// </example>
        public string this[string columnName] => QuoteField(columnName, true);
    }
}
