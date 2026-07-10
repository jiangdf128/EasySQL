using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
namespace EasySQL
{
    public static class SQLDialectFactory
    {
        private static readonly object _lock = new object();
        private static ISQLDialect  defaultDialect;

        private const string sqlconnection = "sqlconnection";
        private const string sqlceconnection = "sqlceconnection";
        private const string npgsqlconnection = "npgsqlconnection";
        private const string sqliteconnection = "sqliteconnection";
        private const string mysqlconnection = "mysqlconnection";
        private const string oracleconnection = "oracleconnection";
        private const string oledbconnection = "oledbconnection ";
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

        public static void UseDialect(string name)
        {
            var dialect = GetDialect(name);
            if (dialect == null)
            {
                throw new Exception($"The SQLDialect name of {name} is not exists!");
            }
            lock (_lock)
            {
                DefaultDialect = dialect;
            }
        }

        public static void UseDialect(IDbConnection connection)
        {
            UseDialect(connection.GetType().Name.ToLower());
        }

        public static ISQLDialect GetDialect(string name)
        {
            if (dialectDictionary.ContainsKey(name))
            {
                return dialectDictionary[name];
            }
            return null;
        }

        public static ISQLDialect GetDialect(IDbConnection connection)
        {
            return GetDialect(connection.GetType().Name.ToLower());
        }

        public static void UseSqlServerDialect()
        {
            DefaultDialect = GetDialect(sqlconnection);
        }

        public static void UsePostgreSQLDialect()
        {
            DefaultDialect = GetDialect(npgsqlconnection);
        }
    }
}
