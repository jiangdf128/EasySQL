using System.Collections.Generic;
using System.Text;
using System.Data;
using System;

namespace EasySQL
{
    /// <summary>
    /// 字符串型的In字句值构造器。
    /// </summary>
    /// <remarks>考虑到Oracle，对In字句值的个数有限制，特定制该类。</remarks>
    public class StringInClauseBuilder: InBuilderBase<string>
    {

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="sectionCount"></param>
        public StringInClauseBuilder(int sectionCount=256) : base( sectionCount) { }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="sectionCount"></param>
        /// <param name="collection"></param>
        public StringInClauseBuilder(IEnumerable<string> collection, int sectionCount=256)
            : base(collection, sectionCount) { }

        /// <summary>
        /// 格式化值。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override string FormatValue(string value)
        {
            return string.Format("'{0}'", value);
        }
    }
}
