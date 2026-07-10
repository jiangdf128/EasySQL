using System;
using System.Collections.Generic;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// EasySQL 扩展方法集合，提供 <see cref="TableDefBase"/> 数组的快速查询入口
    /// 以及各类型的 IN 子句构造器快捷方法。
    /// </summary>
    public static class EasySQLExtensions
    {
        /// <summary>
        /// 从一组表或视图架构创建 <see cref="QueryBuilder"/> 查询实例。
        /// </summary>
        /// <param name="tables">要查询的表或视图架构数组。</param>
        /// <param name="alias">可选别名。</param>
        /// <param name="dialect">可选 SQL 方言，为 null 时使用默认方言。</param>
        /// <returns>已配置 FROM 子句的 <see cref="QueryBuilder"/> 实例。</returns>
        public static QueryBuilder Query(this TableDefBase[] tables, string? alias = null, ISQLDialect? dialect = null)
        {
            var qb = new QueryBuilder(alias, dialect);
            qb.From(tables);
            return qb;
        }

        /// <summary>
        /// 为长整型集合创建 IN 子句构造器。
        /// </summary>
        /// <param name="list">长整型值集合。</param>
        /// <param name="sectionCount">每节最大元素数，默认 256（适配 Oracle 限制）。</param>
        /// <returns>可用于分批构造 IN 子句值。</returns>
        public static IInBuilder InBuilder(this IEnumerable<long> list, int sectionCount = 256)
        {
            return new LongInClauseBuilder(list, sectionCount);
        }

        /// <summary>
        /// 为整型集合创建 IN 子句构造器。
        /// </summary>
        /// <param name="list">整型值集合。</param>
        /// <param name="sectionCount">每节最大元素数，默认 256。</param>
        /// <returns>可用于分批构造 IN 子句值。</returns>
        public static IInBuilder InBuilder(this IEnumerable<int> list, int sectionCount=256)
        {
            return  new IntegerInClauseBuilder(list, sectionCount);
        }

        /// <summary>
        /// 为字符串集合创建 IN 子句构造器，值会自动添加单引号。
        /// </summary>
        /// <param name="list">字符串值集合。</param>
        /// <param name="sectionCount">每节最大元素数，默认 256。</param>
        /// <returns>可用于分批构造 IN 子句值。</returns>
        public static IInBuilder InBuilder(this IEnumerable<string> list, int sectionCount = 256)
        {
            return new StringInClauseBuilder(list, sectionCount);
        }
    }
}
