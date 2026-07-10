using System;
using System.Collections.Generic;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// 构造 SQL IN 子句值的泛型基类，继承自 <see cref="List{T}"/>。
    /// 支持按指定节大小分批输出 IN 值，以应对 Oracle 等数据库对 IN 元素数量的限制。
    /// </summary>
    /// <typeparam name="T">IN 子句中值的类型（int、long、string 等）。</typeparam>
    public abstract class InBuilderBase<T> : List<T>, IInBuilder
    {
        /// <summary>
        /// 当前读取位置，用于迭代获取下一组 IN 值。
        /// </summary>
        protected int currentPosition = 0;

        /// <summary>
        /// 使用指定节大小初始化 <see cref="InBuilderBase{T}"/> 实例。
        /// </summary>
        /// <param name="sectionCount">每节 IN 值最大个数，默认 256。</param>
        public InBuilderBase(int sectionCount=256) { this.SectionCount = sectionCount; }

        /// <summary>
        /// 使用已有集合和指定节大小初始化 <see cref="InBuilderBase{T}"/> 实例。
        /// </summary>
        /// <param name="collection">要分批的值集合。</param>
        /// <param name="sectionCount">每节 IN 值最大个数，默认 256。</param>
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
        /// 将迭代位置重置到集合起始处。
        /// </summary>
        public void MoveFirst()
        {
            this.currentPosition = 0;
        }

        /// <summary>
        /// 将值格式化为 SQL 字面量。子类需覆写以实现具体类型的格式化（如字符串加引号）。
        /// </summary>
        /// <param name="value">要格式化的值。</param>
        /// <returns>格式化后的 SQL 字面量字符串。</returns>
        protected abstract string FormatValue(T value);

        /// <summary>
        /// 获取下一组 IN 值，格式为 "(v1,v2,...)"。若已无剩余则返回空字符串。
        /// </summary>
        /// <returns>下一组 IN 值的 SQL 片段。</returns>
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
        /// 遍历所有 IN 值分组，对每组执行指定操作。遍历完成后自动重置位置。
        /// </summary>
        /// <param name="action">对每组 IN 值执行的操作。</param>
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
