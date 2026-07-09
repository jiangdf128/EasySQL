using System.Text;

namespace EasySQL
{
    /// <summary>
    /// 为SQLite实现了其数据库函数接口定义；
    /// </summary>
    public class SQLiteFunctions : DbFunctionBase
    {

        #region DbFunctionBase 成员

        public override string IsNull(string field1, string field2)
        {
            return string.Format("ifnull({0},{1})", field1, field2);
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

        public override string SubString(string field, int start, int length)
        {
            return string.Format("substr({0},{1},{2})", field, start, length);
        }

        public override string ToChar(string field)
        {
            //SqlLite 不区分数据类型，不用转换。
            return string.Format("{0}", field);
        }

        public override string Replace(string field, string oitem, string nitem)
        {
            return string.Format("replace({0},{1},{2})", field, oitem, nitem);
        }

        public override string CharIndex(string field, string item, int start)
        {
            //SQLite的搜索位置有点不一样。
            return string.Format("charindex({0},{1},{2})", item, field, start);
        }

        public override string Sysdate
        {
            get { return "datetime('now','localtime')"; }
        }

        public override string Truncate(string datefield)
        {
            return string.Format("date({0})", datefield);
        }

        public override string DateAdd(DatePart datepart, string numberExpression, string datefield)
        {
            string function = string.Empty;
            switch (datepart)
            {
                case DatePart.Year:
                    function = "datetime({0},'+{1} years')"; break;
                case DatePart.Month:
                    function = "datetime({0},'+{1} months')"; break;
                case DatePart.Day:
                    function = "datetime({0},'+{1} days')"; break;
                case DatePart.Hour:
                    function = "datetime({0},'+{1} hours')"; break;
                case DatePart.Minute:
                    function = "datetime({0},'+{1} minutes')"; break;
                case DatePart.Second:
                    function = "datetime({0},'+{1} seconds')"; break;
            }
            return string.Format(function, datefield, numberExpression);
        }

        public override string DateDiff(DatePart datepart, string startdate, string enddate)
        {
            string function = string.Empty;
            switch (datepart)
            {
                case DatePart.Year:
                    function = "(strftime('%Y',{1})-strftime('%Y',{0}))"; break;
                case DatePart.Month:
                    function = "((strftime('%Y',{1})-strftime('%Y',{0}))*12+ strftime('%m',{1})-strftime('%m',{0}))"; break;
                case DatePart.Day:
                    function = "(julianday(date({1}))-julianday(date({0})))"; break;
                case DatePart.Hour:
                    function = "((julianday(date({1}))-julianday(date({0})))*24+strftime('%H',{1})-strftime('%H',{0}))"; break;
                case DatePart.Minute:
                    function = "((julianday(date({1}))-julianday(date({0})))*24*60+(strftime('%H',{1})-strftime('%H',{0}))*60+strftime('%M',{1})-strftime('%M',{0}))"; break;
                case DatePart.Second:
                    function = "((julianday(date({1}))-julianday(date({0})))*24*60*60+(strftime('%H',{1})-strftime('%H',{0}))*60*60+(strftime('%M',{1})-strftime('%M',{0}))*60+strftime('%S',{1})-strftime('%S',{0}))"; break;
            }
            return string.Format(function, startdate, enddate);
        }

        public override string Year(string field)
        {
            return string.Format("cast(strftime('%Y',{0}) as integer)", field);
        }

        public override string Month(string field)
        {
            return string.Format("cast(strftime('%m',{0}) as integer)", field);
        }

        public override string Day(string field)
        {
            return string.Format("cast(strftime('%d',{0}) as integer)", field);
        }

        public override string Hour(string field)
        {
            return string.Format("cast(strftime('%H',{0}) as integer)", field);
        }

        public override string Minute(string field)
        {
            return string.Format("cast(strftime('%M',{0}) as integer)", field);
        }

        public override string WeekDay(string field)
        {
            return string.Format("cast(strftime('%w',{0}) as integer)", field);
        }

        public override string ServerTimeSql
        {
            get { return "select datetime('now','localtime')"; }
        }


        #endregion
    }
}
