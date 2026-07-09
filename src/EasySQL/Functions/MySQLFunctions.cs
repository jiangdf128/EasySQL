using System;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// 为MySQL实现了其数据库函数接口定义；
    /// </summary>
    public class MySQLFunctions : DbFunctionBase
    {

        #region DbFunctionBase 成员


        public override string IsNull(string field1, string field2)
        {
            return string.Format("ifnull({0},{1})", field1, field2);
        }

        public override string Concat(string field1, params string[] fields)
        {
            StringBuilder sb = new StringBuilder(string.Format("concat({0}",field1));
            for (int i = 0; i < fields.Length; i++)
            {
                sb.Append(string.Format(",{0}", fields[i]));
            }
            sb.Append(")");
            return sb.ToString();
        }

        public override string Length(string field)
        {
            return string.Format("char_length({0})", field);
        }

        public override string ToChar(string field)
        {
            return string.Format("cast({0} as char)", field);
        }

        public override string Replace(string field, string oitem, string nitem)
        {
            return string.Format("replace({0},{1},{2})", field, oitem, nitem);
        }

        public override string CharIndex(string field, string item, int start)
        {
            return string.Format("locate({0},{1},{2})", start, field, item);
        }


        public override string Sysdate
        {
            get { return "sysdate()"; }
        }

        public override string Truncate(string datefield)
        {
            return string.Format("date({0})", datefield);
        }

        public override string DateAdd(DatePart datepart, string numberExpression, string datefield)
        {
            string function = "date_add({0},interval {1} {2})";
            string part = string.Empty;
            switch (datepart)
            {
                case DatePart.Year:
                    part = "year"; break;
                case DatePart.Month:
                    part = "month"; break;
                case DatePart.Day:
                    part = "day"; break;
                case DatePart.Hour:
                    part = "hour"; break;
                case DatePart.Minute:
                    part = "minute"; break;
                case DatePart.Second:
                    part = "second"; break;
            }
            return string.Format(function, datefield, numberExpression, part);
        }

        public override string DateDiff(DatePart datepart, string startdate, string enddate)
        {
            string function = "timestampdiff({0},{1},{2})";
            string part = string.Empty;
            switch (datepart)
            {
                case DatePart.Year:
                    part = "year"; break;
                case DatePart.Month:
                    part = "month"; break;
                case DatePart.Day:
                    part = "day"; break;
                case DatePart.Hour:
                    part = "hour"; break;
                case DatePart.Minute:
                    part = "minute"; break;
                case DatePart.Second:
                    part = "second"; break;
            }
            return string.Format(function, part, startdate, enddate);
        }

        public override string WeekDay(string field)
        {
            throw new NotImplementedException();
        }

        public override string ServerTimeSql
        {
            get { return "select sysdate()"; }
        }

        #endregion

    }
}
