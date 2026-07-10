namespace EasySQL
{
    /// <summary>
    /// 数据行版本控制接口，实体类实现此接口可为数据行提供乐观并发控制。
    /// </summary>
    public interface IVersion
    {
        /// <summary>
        /// 数据行版本号，每次更新时递增，用于检测并发冲突。
        /// </summary>
        int Version { get; set; }
    }
}
