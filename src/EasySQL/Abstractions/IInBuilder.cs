using System;

namespace EasySQL
{
    /// <summary>
    /// SQL IN 子句值构造器接口，用于将大量值分批构造为多组 IN 子句，
    /// 主要解决 Oracle 等数据库对 IN 列表元素数量的限制。
    /// </summary>
    public interface IInBuilder
    {
        /// <summary>
        /// In值每节包含的最大值个数。
        /// </summary>
        int SectionCount { get; set; }

        /// <summary>
        /// 移到首位。
        /// </summary>
        void MoveFirst();

        /// <summary>
        /// 获取下一个In值。
        /// </summary>
        /// <returns></returns>
        string GetNextInValues();

        /// <summary>
        /// 获取是否还有下一个In值。
        /// </summary>
        bool HasNext { get; }

        /// <summary>
        /// 执行循环操作。
        /// </summary>
        /// <param name="action"></param>
        void Do(Action<string> action);
    }
}
