using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
namespace EasySQL
{
    /// <summary>
    /// SQL 方言工厂类，根据数据库连接类型自动匹配对应的方言。
    /// 默认使用 SQL Server 方言。
    /// </summary>
    public static class SQLDialectFactory
    {
        private static readonly object _lock = new object();
        private static ISQLDialect? defaultDialect;

        private const string sqlconnection = "sqlconnection";
        private const string sqlceconnection = "sqlceconnection";
        private const string npgsqlconnection = "npgsqlconnection";
        private const string sqliteconnection = "sqliteconnection";
        private const string mysqlconnection = "mysqlconnection";
        private const string oracleconnection = "oracleconnection";
        private const string oledbconnection = "oledbconnection";
        private const string db2connection = "db2connection";

        private static readonly Dictionary<string, ISQLDialect> dialectDictionary
            = new Dictionary<string, ISQLDialect>
            {
                [sqlconnection] = new SqlServerDialect(),
                [sqlceconnection] = new SqlServerDialect(),
                [npgsqlconnection] = new PostgreSQLDialect(),
                [sqliteconnection] = new SQLiteDialect(),
                [mysqlconnection] = new MySQLDialect(),
                [oracleconnection] = new OracleDialect(),
                [oledbconnection] = new JetDialect(),
                [db2connection] = new DB2Dialect()
            };

        /// <summary>
        /// 获取或设置默认的数据库方言。
        /// </summary>
        public static ISQLDialect DefaultDialect
        {
            get
            {
                if (defaultDialect == null)
                {
                    lock (_lock)
                    {
                        if (defaultDialect == null)
                        {
                            defaultDialect = dialectDictionary[sqlconnection];
                        }
                    }
                }
                return defaultDialect;
            }
            set
            {
                if (value != null)
                {
                    lock (_lock)
                    {
                        defaultDialect = value;
                    }
                }
            }
        }

        /// <summary>
        /// 根据名称切换默认方言。
        /// </summary>
        /// <param name="name">方言名称，对应连接类型的小写类名（如 "sqlconnection"）。</param>
        public static void UseDialect(string name)
        {
            var dialect = GetDialect(name);
            if (dialect == null)
            {
                throw new Exception($"The SQLDialect name of {name} is not exists!");
            }
            lock (_lock)
            {
                DefaultDialect = dialect!;
            }
        }

        /// <summary>
        /// 根据数据库连接对象自动检测并切换方言。
        /// </summary>
        /// <param name="connection">数据库连接对象。</param>
        public static void UseDialect(IDbConnection connection)
        {
            UseDialect(connection.GetType().Name.ToLower());
        }

        /// <summary>
        /// 根据名称获取对应的方言实例，未找到则返回 null。
        /// </summary>
        /// <param name="name">方言名称。</param>
        /// <returns>方言实例，未找到时返回 null。</returns>
        public static ISQLDialect? GetDialect(string name)
        {
            if (dialectDictionary.ContainsKey(name))
            {
                return dialectDictionary[name];
            }
            return null;
        }

        /// <summary>
        /// 根据数据库连接对象获取对应的方言实例。
        /// </summary>
        /// <param name="connection">数据库连接对象。</param>
        /// <returns>对应的方言实例。</returns>
        public static ISQLDialect? GetDialect(IDbConnection connection)
        {
            return GetDialect(connection.GetType().Name.ToLower());
        }

        /// <summary>
        /// 切换到 SQL Server 方言。
        /// </summary>
        public static void UseSqlServerDialect()
        {
            DefaultDialect = GetDialect(sqlconnection)!;
        }

        /// <summary>
        /// 切换到 PostgreSQL 方言。
        /// </summary>
        public static void UsePostgreSQLDialect()
        {
            DefaultDialect = GetDialect(npgsqlconnection)!;
        }
    }
}
