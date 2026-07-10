using System.Text;

namespace EasySQL
{
    /// <summary>
    /// 为Oracle实现了其数据库函数接口定义；
    /// </summary>
    public class OracleFunctions : DbFunctionBase
    {

        #region DbFunctionBase 成员

        /// <inheritdoc/>
        public override string BitAnd(string field1, string field2)
        {
            return string.Format("BITAND({0},{1})",field1,field2);
        }

        /// <inheritdoc/>
        public override string BitOr(string field1, string field2)
        {
            return string.Format("(({0}+{1})-BITAND({0},{1}))", field1, field2);
        }

        /// <inheritdoc/>
        public override string IsNull(string field1, string field2)
        {
            return string.Format("nvl({0},{1})", field1, field2);
        }

        /// <inheritdoc/>
        public override string Concat(string field1, params string[] fields)
        {
            StringBuilder sb = new StringBuilder(field1);
            for (int i = 0; i < fields.Length; i++)
            {
                sb.Append(string.Format("||{0}", fields[i]));
            }
            return sb.ToString();
        }

        /// <inheritdoc/>
        public override string Length(string field)
        {
            return string.Format("length({0})", field);
        }

        /// <inheritdoc/>
        public override string SubString(string field, int start, int length)
        {
            return string.Format("substr({0},{1},{2})", field, start, length);
        }

        /// <inheritdoc/>
        public override string ToChar(string field)
        {
            return string.Format("to_char({0})", field);
        }

        /// <inheritdoc/>
        public override string Replace(string field, string oitem, string nitem)
        {
            return string.Format("replace({0},{1},{2})", field, oitem, nitem);
        }

        /// <inheritdoc/>
        public override string CharIndex(string field, string item, int start)
        {
            //db2的搜索位置有点不一样。
            return string.Format("instr({0},{1},{2})", field , item, start);
        }

        /// <inheritdoc/>
        public override string Sysdate
        {
            get { return "sysdate"; }
        }

        /// <inheritdoc/>
        public override string Truncate(string datefield)
        {
            return string.Format("trunc({0})", datefield);
        }

        /// <inheritdoc/>
        public override string DateAdd(DatePart datepart, string numberExpression, string datefield)
        {
            string function = string.Empty;
            switch (datepart)
            {
                case DatePart.Year:
                    function = "add_months({0},{1}*12)"; break;
                case DatePart.Month:
                    function = "add_months({0},{1})"; break;
                case DatePart.Day:
                    function = "({0}+{1})"; break;
                case DatePart.Hour:
                    function = "({0}+{1}/24)"; break;
                case DatePart.Minute:
                    function = "({0}+{1}/(24*60))"; break;
                case DatePart.Second:
                    function = "({0}+{1}/(24*60*60))"; break;
            }
            return string.Format(function, datefield, numberExpression);
        }

        /// <inheritdoc/>
        public override string DateDiff(DatePart datepart, string startdate, string enddate)
        {
            string function = string.Empty;
            switch (datepart)
            {
                case DatePart.Year:
                    function="(extract(year from {1})-extract(year from {0}))"; break;
                case DatePart.Month:
                    function = "(extract(year from {1})-extract(year from {0}))*12+extract(month from {1})-extract(month from {0})"; break;
                case DatePart.Day:
                    function = "(trunc({1})-trunc({0}))"; break;
                case DatePart.Hour:
                    function = "((trunc({1})-trunc({0}))*24+(to_number(to_char({1},'hh24'))-to_number(to_char({0},'hh24'))))"; break;
                case DatePart.Minute:
                    function = "((trunc({1})-trunc({0}))*24*60+(to_number(to_char({1},'hh24'))-to_number(to_char({0},'hh24')))*60+to_number(to_char({1},'mi'))-to_number(to_char({0},'mi')))"; break;
                case DatePart.Second:
                    function = "((trunc({1})-trunc({0}))*24*60*60+(to_number(to_char({1},'hh24'))-to_number(to_char({0},'hh24')))*60*60+(to_number(to_char({1},'mi'))-to_number(to_char({0},'mi')))*60+to_number(to_char({1},'ss'))-to_number(to_char({0},'ss')))"; break;
            }
            return string.Format(function, startdate, enddate);
        }

        /// <inheritdoc/>
        public override string Year(string field)
        {
            return string.Format("extract(year from {0})", field);
        }

        /// <inheritdoc/>
        public override string Month(string field)
        {
            return string.Format("extract(month from {0})", field);
        }

        /// <inheritdoc/>
        public override string Day(string field)
        {
            return string.Format("extract(day from {0})", field);
        }

        /// <inheritdoc/>
        public override string Hour(string field)
        {
            return string.Format("to_number(to_char({0},'hh24'))", field);
        }

        /// <inheritdoc/>
        public override string Minute(string field)
        {
            return string.Format("to_number(to_char({0},'mi'))", field);
        }

        /// <inheritdoc/>
        public override string WeekDay(string field)
        {
            return string.Format("to_number(to_char({0},'D'))", field);
        }

        /// <inheritdoc/>
        public override string ServerTimeSql
        {
            get { return "select sysdate from dual"; }
        }

        #endregion
    }
}
