namespace EasySQL
{
    /// <summary>
    /// 数据库方言类型枚举，用于替代字符串硬比较来标识数据库类型。
    /// </summary>
    public enum DialectType
    {
        /// <summary>SQL Server</summary>
        SqlServer,
        /// <summary>MySQL</summary>
        MySQL,
        /// <summary>PostgreSQL</summary>
        PostgreSQL,
        /// <summary>Oracle</summary>
        Oracle,
        /// <summary>SQLite</summary>
        SQLite,
        /// <summary>IBM DB2</summary>
        DB2
    }
}
