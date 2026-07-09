using System;
using System.Collections.Generic;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// 构造SQL IN子句值的Build基类。
    /// </summary>
    public abstract class InBuilderBase<T> : List<T>, IInBuilder
    {
        /// <summary>
        /// 当前位置。
        /// </summary>
        protected int currentPosition = 0;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="sectionCount"></param>
        public InBuilderBase(int sectionCount=256) { this.SectionCount = sectionCount; }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="sectionCount"></param>
        public InBuilderBase( IEnumerable<T> collection, int sectionCount=256)
            : base(collection)
        {
            this.SectionCount = sectionCount;
        }

        /// <summary>
        /// In值每节包含的最大值个数。
        /// </summary>
        public int SectionCount { get; set; }

        /// <summary>
        /// 移到首位。
        /// </summary>
        public void MoveFirst()
        {
            this.currentPosition = 0;
        }

        /// <summary>
        /// 格式化值。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected abstract string FormatValue(T value);

        /// <summary>
        /// 获取下一个In值。
        /// </summary>
        /// <returns></returns>
        public virtual string GetNextInValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Empty);
            int maxCount = this.SectionCount;
            int count = 0;
            if (maxCount <= 0)
            {
                maxCount = this.Count;
            }
            while (this.currentPosition < this.Count && count < maxCount)
            {
                sb.Append(count == 0 ? "(" : ",");
                sb.Append(this.FormatValue(this[currentPosition]));
                count++;
                currentPosition++;
            }
            if (count > 0)
            {
                sb.Append(")");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取是否还有下一个In值。
        /// </summary>
        public virtual bool HasNext
        {
            get { return this.currentPosition < this.Count; }
        }

        /// <summary>
        /// 执行循环操作。
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public virtual void Do(Action<string> action)
        {
            while (this.HasNext)
            {
                action(this.GetNextInValues());
            }
            this.MoveFirst();
        }
    }
}
