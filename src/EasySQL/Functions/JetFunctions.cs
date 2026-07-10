using System;

namespace EasySQL
{
    /// <summary>
    /// 为Access实现了其数据库函数接口定义；
    /// </summary>
    public class JetFunctions:DbFunctionBase
    {
        #region DbFunctionBase 成员

        /// <inheritdoc/>
        public override string Case(string expression, string field1, string field2)
        {
            return string.Format("iif({0},{1},{2})",expression,field1,field2);
        }

        /// <inheritdoc/>
        public override string IsNull(string field1, string field2)
        {
            return string.Format("iif(isnull({0}),{1},{0})", field1, field2);
        }

        /// <inheritdoc/>
        public override string Lower(string field)
        {
            return string.Format("lcase({0})", field);
        }

        /// <inheritdoc/>
        public override string Upper(string field)
        {
            return string.Format("ucase({0})", field);
        }

        /// <inheritdoc/>
        public override string SubString(string field, int start, int length)
        {
            return string.Format("mid({0},{1},{2})", field, start, length);
        }

        /// <inheritdoc/>
        public override string ToChar(string field)
        {
            return string.Format("cstr({0})", field);
        }

        /// <inheritdoc/>
        public override string Replace(string field, string oitem, string nitem)
        {
            return string.Format("replace({0},{1},{2})", field, oitem, nitem);
        }

        /// <inheritdoc/>
        public override string CharIndex(string field, string item, int start)
        {
            return string.Format("instr({0},{1},{2})", start,field,item);
        }

        /// <inheritdoc/>
        public override string Floor(string field)
        {
            return string.Format("fix({0})", field);
        }

        /// <inheritdoc/>
        public override string Sysdate
        {
            get { return "now()"; }
        }

        /// <inheritdoc/>
        public override string Truncate(string datefield)
        {
            return string.Format("cdate(formatdatetime({0},2))",datefield);
        }

        /// <inheritdoc/>
        public override string DateAdd(DatePart datepart, string numberExpression, string datefield)
        {
            string function = "dateadd({0},{1},{2})";
            string part = string.Empty;
            switch (datepart)
            {
                case DatePart.Year:
                    part = "'yyyy'"; break;
                case DatePart.Month:
                    part = "'m'"; break;
                case DatePart.Day:
                    part = "'d'"; break;
                case DatePart.Hour:
                    part = "'h'"; break;
                case DatePart.Minute:
                    part = "'n'"; break;
                case DatePart.Second:
                    part = "'s'"; break;
            }
            return string.Format(function, part, numberExpression, datefield);

        }

        /// <inheritdoc/>
        public override string DateDiff(DatePart datepart, string startdate, string enddate)
        {
            string function = "datediff({0},{1},{2})";
            string part = string.Empty;
            switch (datepart)
            {
                case DatePart.Year:
                    part = "'yyyy'"; break;
                case DatePart.Month:
                    part = "'m'"; break;
                case DatePart.Day:
                    part = "'d'"; break;
                case DatePart.Hour:
                    part = "'h'"; break;
                case DatePart.Minute:
                    part = "'n'"; break;
                case DatePart.Second:
                    part = "'s'"; break;
            }
            return string.Format(function, part, startdate, enddate);
        }

        /// <inheritdoc/>
        public override string WeekDay(string field)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override string ServerTimeSql
        {
            get { return "select now()"; }
        }

        #endregion

    }
}
