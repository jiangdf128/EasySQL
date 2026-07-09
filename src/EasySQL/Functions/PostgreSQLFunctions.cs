using System;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// 为PostgreSQL实现了其数据库函数接口定义；
    /// </summary>
    public class PostgreSQLFunctions : DbFunctionBase
    {
        #region DbFunctionBase 成员


        public override string ToInteger(string field)
        {
            return string.Format("cast({0} as integer)", field);
        }

        public override string IsNull(string field1, string field2)
        {
            return string.Format("coalesce({0},{1})", field1, field2);
        }

        public override string Concat(string field1, params string[] fields)
        {
            StringBuilder sb = new StringBuilder(field1);
            for (int i = 0; i < fields.Length; i++)
            {
                sb.Append(string.Format("||{0}", fields[i]));
            }
            return sb.ToString();
        }

        public override string Length(string field)
        {
            return string.Format("length({0})", field);
        }

        public override string ToChar(string field)
        {
            return string.Format("cast({0} as varchar)", field);
        }

        public override string Replace(string field, string oitem, string nitem)
        {
            return string.Format("replace({0},{1},{2})", field, oitem, nitem);
        }

        public override string CharIndex(string field, string item, int start)
        {
            //db2的搜索位置有点不一样。
            return $"strpos(substring({field}, {start}),{item}) + {start} - 1";
        }


        public override string Sysdate
        {
            get { return "now()"; }
        }

        public override string Truncate(string datefield)
        {
            return string.Format("date({0})", datefield);
        }


        public override string DateAdd(DatePart datepart, string numberExpression, string datefield)
        {
            string function = "{0}+{1} {2}";
            string part = string.Empty;
            switch (datepart)
            {
                case DatePart.Year:
                    part = "years"; break;
                case DatePart.Month:
                    part = "months"; break;
                case DatePart.Day:
                    part = "days"; break;
                case DatePart.Hour:
                    part = "hours"; break;
                case DatePart.Minute:
                    part = "minutes"; break;
                case DatePart.Second:
                    part = "seconds"; break;
            }
            return string.Format(function, datefield, numberExpression, part);
        }

        public override string DateDiff(DatePart datepart, string startdate, string enddate)
        {
            string function = string.Empty;
            switch (datepart)
            {
                case DatePart.Year:
                    function =$"(extract(year from {enddate})-extract(year from {startdate}))"; break;
                case DatePart.Month:
                    function = $"((extract(year from {enddate})-extract(year from {startdate}))*12+(extract(month from {enddate})-extract(month from {startdate})))"; break;
                case DatePart.Day:
                    function = $"(date({enddate})-date({startdate}))"; break;
                case DatePart.Hour:
                    function = $"(date({enddate})-date({startdate}))*24+(extract(hour from {enddate})-extract(hour from {startdate}))"; break;
                case DatePart.Minute:
                    function = $"((date({enddate})-date({startdate}))*24*60+(extract(hour from {enddate})-extract(hour from {startdate}))*60+ (extract(minute from {enddate})-extract(minute from {startdate})))"; break;
                case DatePart.Second:
                    function = $"((date({enddate})-date({startdate}))*24*60*60+(extract(hour from {enddate})-extract(hour from {startdate}))*60*60+(extract(minute from {enddate})-extract(minute from {startdate}))*60+(floor(extract(second from {enddate}))-floor(extract(second from {startdate}))))"; break;
            }
            return function;

        }

        public override string Year(string field)
        {
            return $"extract(year from {field})";
        }

        public override string Month(string field)
        {
            return $"extract(month from {field})";
        }

        public override string Day(string field)
        {
            return $"extract(day from {field})";
        }

        public override string Hour(string field)
        {
            return $"extract(hour from {field})";
        }

        public override string Minute(string field)
        {
            return $"extract(minute from {field})";
        }

        public override string WeekDay(string field)
        {
            return $"abs(date({field})-'2018-04-29'+1)%7";
        }

        public override string ServerTimeSql
        {
            get { return "select now()"; }
        }

        #endregion
    }
}
