using System;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// 为DB2实现了其数据库函数接口定义；
    /// </summary>
    public class DB2Functions : DbFunctionBase
    {
        #region DbFunctionBase 成员


        /// <inheritdoc/>
        public override string ToInteger(string field)
        {
            return string.Format("Integer({0})",field);
        }

        /// <inheritdoc/>
        public override string BitAnd(string field1, string field2)
        {
            //需要增加DB2的UDF函数，才能适用。详细可以参考：
            //http://www.devx.com/ibm/Article/28089.
            //使用DB命令窗口中，请使用以下命令行创建该函数：
            //CREATE FUNCTION BITAND (N1 Integer, N2 Integer) RETURNS Integer LANGUAGE SQL SPECIFIC BITANDOracle CONTAINS SQL NO EXTERNAL ACTION DETERMINISTIC BEGIN ATOMIC DECLARE M1, M2, S Integer;DECLARE RetVal Integer DEFAULT 0;SET (M1, M2, S) = (N1, N2, 0);WHILE M1 > 0 AND M2 > 0 AND S < 32 DO  SET RetVal = RetVal + MOD(M1,2)*MOD(M2,2)*power(2,S); SET (M1, M2, S) = (M1/2, M2/2, S+1);END WHILE;RETURN RetVal;END
            return string.Format("BITAND({0},{1})", field1, field2);
        }

        /// <inheritdoc/>
        public override string BitOr(string field1, string field2)
        {
            return string.Format("(({0}+{1})-BITAND({0},{1}))", field1, field2);
        }

        /// <inheritdoc/>
        public override string IsNull(string field1, string field2)
        {
            return string.Format("coalesce({0},{1})", field1, field2);
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
        public override string ToChar(string field)
        {
            return string.Format("cast({0} as varchar)", field);
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
            return string.Format("locate({0},{1},{2})", item, field, start);
        }


        /// <inheritdoc/>
        public override string Sysdate
        {
            get { return "current timestamp"; }
        }

        /// <inheritdoc/>
        public override string Truncate(string datefield)
        {
            return string.Format("date({0})", datefield);
        }


        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override string DateDiff(DatePart datepart, string startdate, string enddate)
        {
            string function = string.Empty;
            switch (datepart)
            {
                case DatePart.Year:
                    function = "(year({1})-year({0}))"; break;
                case DatePart.Month:
                    function = "((year({1})-year({0}))*12+month({1})-month({0}))"; break;
                case DatePart.Day:
                    function = "(days({1}) - days({0}))"; break;
                case DatePart.Hour:
                    function = "((days({1})-days({0}))*24+hour({1})-hour({0}))"; break;
                case DatePart.Minute:
                    function = "((days({1})-days({0}))*24*60+(hour({1})-hour({0}))*60+minute({1})-minute({0}))";break;
                case DatePart.Second:
                    function = "((days({1})-days({0}))*24*60*60+(hour({1})-hour({0}))*60*60+(minute({1})-minute({0}))*60+second({1})-second({0}))"; break;
            }
            return string.Format(function, startdate, enddate);

        }

        /// <inheritdoc/>
        public override string WeekDay(string field)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override string ServerTimeSql
        {
            get { return "select current timestamp from sysibm.sysdummy1"; }
        }

        #endregion
    }
}
