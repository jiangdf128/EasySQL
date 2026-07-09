using System.Collections.Generic;
using System.Text;
using System.Data;
using System;
namespace EasySQL
{
    /// <summary>
    /// 整型的In字句值构造器。
    /// </summary>
    /// <remarks>考虑到Oracle，对In字句值的个数有限制，特定制该类。</remarks>
    public class IntegerInClauseBuilder : InBuilderBase<int>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="sectionCount"></param>
        public IntegerInClauseBuilder(int sectionCount=256) : base( sectionCount) { }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="sectionCount"></param>
        public IntegerInClauseBuilder(IEnumerable<int> collection, int sectionCount=256)
            : base(collection, sectionCount) { }

        /// <summary>
        /// 格式化值。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override string FormatValue(int value)
        {
            return value.ToString();
        }
    }
}
