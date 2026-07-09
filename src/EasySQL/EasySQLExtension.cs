using System;
using System.Collections.Generic;
using System.Text;

namespace EasySQL
{
    public static class EasySQLExtension
    {
        public static QueryBuilder Query(this SchemaBase[] tables, string alias = null, ISQLDialect dialect = null)
        {
            var qb = new QueryBuilder(alias, dialect);
            qb.From(tables);
            return qb;
        }

        public static IInBuilder InBuilder(this IEnumerable<long> list, int sectionCount = 256)
        {
            return new LongInClauseBuilder(list, sectionCount);
        }

        public static IInBuilder InBuilder(this IEnumerable<int> list, int sectionCount=256)
        {
            return  new IntegerInClauseBuilder(list, sectionCount);
        }

        public static IInBuilder InBuilder(this IEnumerable<string> list, int sectionCount = 256)
        {
            return new StringInClauseBuilder(list, sectionCount);
        }
    }
}
