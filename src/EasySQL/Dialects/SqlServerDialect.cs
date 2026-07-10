using System.Collections.Generic;

namespace EasySQL
{
    /// <summary>
    /// SQL Server 数据库方言实现。
    /// </summary>
    public class SqlServerDialect :SQLDialectBase
    {
        static readonly IDbFunction dbFunc = new SqlServerFunctions();

        /// <inheritdoc/>
        public override string DialectName  { get { return "SQLServer"; } }

        /// <inheritdoc/>
        public override IDbFunction Func { get { return dbFunc; } }

        /// <inheritdoc/>
        public override bool IsBracketJoin => false;

        protected override string QuoteKeyWord(string word)
        {
            return $"[{word}]";
        }

        /// <inheritdoc/>
        public override bool IsKeyWord(string word)
        {
            return keyWords.Contains(word.ToUpper());
        }

        static readonly List<string> keyWords=new List<string>(){
                                                         "ABSOLUTE", "ACTION", "ADA", "ADD", "ADMIN", "AFTER", "AGGREGATE", "ALIAS", "ALL",
                                                         "ALLOCATE", "ALTER", "AND", "ANY", "ARE", "ARRAY", "AS", "ASC", "ASSERTION", "AT",
                                                         "AUTHORIZATION", "AVG", "BACKUP", "BEFORE", "BEGIN", "BETWEEN", "BINARY", "BIT",
                                                         "BIT_LENGTH", "BLOB", "BOOLEAN", "BOTH", "BREADTH", "BREAK", "BROWSE", "BULK", "BY",
                                                         "CALL", "CASCADE", "CASCADED", "CASE", "CAST", "CATALOG", "CHAR", "CHARACTER",
                                                         "CHARACTER_LENGTH", "CHAR_LENGTH", "CHECK", "CHECKPOINT", "CLASS", "CLOB", "CLOSE",
                                                         "CLUSTERED", "COALESCE", "COLLATE", "COLLATION", "COLUMN", "COMMIT", "COMPLETION",
                                                         "COMPUTE", "CONNECT", "CONNECTION", "CONSTRAINT", "CONSTRAINTS", "CONSTRUCTOR",
                                                         "CONTAINS", "CONTAINSTABLE", "CONTINUE", "CONVERT", "CORRESPONDING", "COUNT",
                                                         "CREATE", "CROSS", "CUBE", "CURRENT", "CURRENT_DATE", "CURRENT_PATH", "CURRENT_ROLE",
                                                         "CURRENT_TIME", "CURRENT_TIMESTAMP", "CURRENT_USER", "CURSOR", "CYCLE", "DATA",
                                                         "DATABASE", "DATE", "DAY", "DBCC", "DEALLOCATE", "DEC", "DECIMAL", "DECLARE",
                                                         "DEFAULT", "DEFERRABLE", "DEFERRED", "DELETE", "DENY", "DEPTH", "DEREF", "DESC",
                                                         "DESCRIBE", "DESCRIPTOR", "DESTROY", "DESTRUCTOR", "DETERMINISTIC", "DIAGNOSTICS",
                                                         "DICTIONARY", "DISCONNECT", "DISK", "DISTINCT", "DISTRIBUTED", "DOMAIN", "DOUBLE",
                                                         "DROP", "DUMMY", "DUMP", "DYNAMIC", "EACH", "ELSE", "END", "END-EXEC", "EQUALS",
                                                         "ERRLVL", "ESCAPE", "EVERY", "EXCEPT", "EXCEPTION", "EXEC", "EXECUTE", "EXISTS",
                                                         "EXIT", "EXTERNAL", "EXTRACT", "FALSE", "FETCH", "FILE", "FILLFACTOR", "FIRST",
                                                         "FLOAT", "FOR", "FOREIGN", "FORTRAN", "FOUND", "FREE", "FREETEXT", "FREETEXTTABLE",
                                                         "FROM", "FULL", "FUNCTION", "GENERAL", "GET", "GLOBAL", "GO", "GOTO", "GRANT",
                                                         "GROUP", "GROUPING", "HAVING", "HOLDLOCK", "HOST", "HOUR", "IDENTITY", "IDENTITYCOL",
                                                         "IDENTITY_INSERT", "IF", "IGNORE", "IMMEDIATE", "IN", "INCLUDE", "INDEX", "INDICATOR",
                                                         "INITIALIZE", "INITIALLY", "INNER", "INOUT", "INPUT", "INSENSITIVE", "INSERT", "INT",
                                                         "INTEGER", "INTERSECT", "INTERVAL", "INTO", "IS", "ISOLATION", "ITERATE", "JOIN",
                                                         "KEY", "KILL", "LANGUAGE", "LARGE", "LAST", "LATERAL", "LEADING", "LEFT", "LESS",
                                                         "LEVEL", "LIKE", "LIMIT", "LINENO", "LOAD", "LOCAL", "LOCALTIME", "LOCALTIMESTAMP",
                                                         "LOCATOR", "LOWER", "MAP", "MATCH", "MAX", "MIN", "MINUTE", "MODIFIES", "MODIFY",
                                                         "MODULE", "MONTH", "NAMES", "NATIONAL", "NATURAL", "NCHAR", "NCLOB", "NEW", "NEXT",
                                                         "NO", "NOCHECK", "NONCLUSTERED", "NONE", "NOT", "NULL", "NULLIF", "NUMERIC", "OBJECT",
                                                         "OCTET_LENGTH", "OF", "OFF", "OFFSETS", "OLD", "ON", "ONLY", "OPEN", "OPENDATASOURCE",
                                                         "OPENQUERY", "OPENROWSET", "OPENXML", "OPERATION", "OPTION", "OR", "ORDER",
                                                         "ORDINALITY", "OUT", "OUTER", "OUTPUT", "OVER", "OVERLAPS", "PAD", "PARAMETER",
                                                         "PARAMETERS", "PARTIAL", "PASCAL", "PATH", "PERCENT", "PLAN", "POSITION", "POSTFIX",
                                                         "PRECISION", "PREFIX", "PREORDER", "PREPARE", "PRESERVE", "PRIMARY", "PRINT", "PRIOR",
                                                         "PRIVILEGES", "PROC", "PROCEDURE", "PUBLIC", "RAISERROR", "READ", "READS", "READTEXT",
                                                         "REAL", "RECONFIGURE", "RECURSIVE", "REF", "REFERENCES", "REFERENCING", "RELATIVE",
                                                         "REPLICATION", "RESTORE", "RESTRICT", "RESULT", "RETURN", "RETURNS", "REVOKE",
                                                         "RIGHT", "ROLE", "ROLLBACK", "ROLLUP", "ROUTINE", "ROW", "ROWCOUNT", "ROWGUIDCOL",
                                                         "ROWS", "RULE", "SAVE", "SAVEPOINT", "SCHEMA", "SCOPE", "SCROLL", "SEARCH", "SECOND",
                                                         "SECTION", "SELECT", "SEQUENCE", "SESSION", "SESSION_USER", "SET", "SETS", "SETUSER",
                                                         "SHUTDOWN", "SIZE", "SMALLINT", "SOME", "SPACE", "SPECIFIC", "SPECIFICTYPE", "SQL",
                                                         "SQLCA", "SQLCODE", "SQLERROR", "SQLEXCEPTION", "SQLSTATE", "SQLWARNING", "START",
                                                         "STATE", "STATEMENT", "STATIC", "STATISTICS", "STRUCTURE", "SUBSTRING", "SUM",
                                                         "SYSTEM_USER", "TABLE", "TEMPORARY", "TERMINATE", "TEXTSIZE", "THAN", "THEN", "TIME",
                                                         "TIMESTAMP", "TIMEZONE_HOUR", "TIMEZONE_MINUTE", "TO", "TOP", "TRAILING", "TRAN",
                                                         "TRANSACTION", "TRANSLATE", "TRANSLATION", "TREAT", "TRIGGER", "TRIM", "TRUE",
                                                         "TRUNCATE", "TSEQUAL", "UNDER", "UNION", "UNIQUE", "UNKNOWN", "UNNEST", "UPDATE",
                                                         "UPDATETEXT", "UPPER", "USAGE", "USE", "USER", "USING", "VALUE", "VALUES", "VARCHAR",
                                                         "VARIABLE", "VARYING", "VIEW", "WAITFOR", "WHEN", "WHENEVER", "WHERE", "WHILE",
                                                         "WITH", "WITHOUT", "WORK", "WRITE", "WRITETEXT", "YEAR", "ZONE"
                                                     };
    }
}
