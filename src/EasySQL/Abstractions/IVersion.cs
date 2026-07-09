namespace EasySQL
{
    /// <summary>
    /// Data row version interface.
    /// </summary>
    public interface IVersion
    {
        /// <summary>
        /// Data row version value.
        /// </summary>
        int Version { get; set; }
    }
}
