namespace EasySQL
{
    /// <summary>
    /// 为SqlServer实现了其数据库函数接口定义；
    /// </summary>
    public class SqlServerFunctions : DbFunctionBase
    {

        #region DbFunctionBase 成员

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
            return string.Format("charindex({0},{1},{2})",field,item,start);
        }

        public override string Trim(string field)
        {
            return string.Format("ltrim(rtrim({0}))", field);
        }

        public override string Sysdate
        {
            get { return "getdate()"; }
        }

        public override string Truncate(string datefield)
        {
            return string.Format("cast(convert(varchar(10),{0},102) as datetime)", datefield);
        }

        public override string DateAdd(DatePart datepart, string numberExpression, string datefield)
        {
            string function = "dateadd({0},{1},{2})";
            string part = string.Empty;
            switch (datepart)
            {
                case DatePart.Year:
                    part = "yyyy"; break;
                case DatePart.Month:
                    part = "mm"; break;
                case DatePart.Day:
                    part = "dd"; break;
                case DatePart.Hour:
                    part = "hh"; break;
                case DatePart.Minute:
                    part = "mi"; break;
                case DatePart.Second:
                    part = "ss"; break;
            }
            return string.Format(function, part, numberExpression, datefield);
        }

        public override string DateDiff(DatePart datepart, string startdate, string enddate)
        {
            string function = "datediff({0},{1},{2})";
            string part = string.Empty;
            switch (datepart)
            {
                case DatePart.Year:
                    part = "yyyy"; break;
                case DatePart.Month:
                    part = "mm"; break;
                case DatePart.Day:
                    part = "dd"; break;
                case DatePart.Hour:
                    part = "hh"; break;
                case DatePart.Minute:
                    part = "mi"; break;
                case DatePart.Second:
                    part = "ss"; break;
            }
            return string.Format(function, part, startdate, enddate);
        }

        public override string Hour(string field)
        {
            return string.Format("datepart(hour,{0})", field);
        }

        public override string Minute(string field)
        {
            return string.Format("datepart(minute,{0})", field);
        }

        public override string WeekDay(string field)
        {
            return string.Format("datepart(dw,{0})", field);
        }

        public override string ServerTimeSql
        {
            get { return "select getdate()"; }
        }

        #endregion

    }
}
