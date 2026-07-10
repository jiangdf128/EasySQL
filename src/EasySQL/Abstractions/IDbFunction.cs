namespace EasySQL
{
    /// <summary>
    /// 数据库函数接口，定义跨数据库通用的 SQL 函数抽象。
    /// 包括字符串处理、日期运算、数学计算、位运算等 30 余个函数，
    /// 每种数据库方言提供各自的实现以适配语法差异。
    /// </summary>
    public interface IDbFunction
    {
        /// <summary>
        /// 把某个字段转换成整型的函数。
        /// </summary>
        /// <param name="field">字段。</param>
        /// <returns></returns>
        string ToInteger(string field);

        /// <summary>
        /// 数据库位运算。
        /// </summary>
        /// <param name="field1">字段1.</param>
        /// <param name="field2">字段2.</param>
        /// <returns>返回与的位运算表达式。</returns>
        string BitAnd(string field1, string field2);

        /// <summary>
        /// 数据库位运算。
        /// </summary>
        /// <param name="field1">字段1.</param>
        /// <param name="field2">字段2.</param>
        /// <returns>返回亦或的位运算表达式。</returns>
        string BitOr(string field1, string field2);

        /// <summary>
        /// 格式化字符串值，以构造SQL。
        /// </summary>
        /// <param name="content">原字符串值。</param>
        /// <returns>返回格式化后的字符串值。</returns>
        string Quote(string content);
        /// <summary>
        /// 格式化字符串值，以构造SQL。
        /// </summary>
        /// <param name="content">原字符串值。</param>
        /// <param name="needStringQuote">是否需要字符串引号(单引号)括起来。</param>
        /// <returns>返回格式化后的字符串值。</returns>
        string Quote(string content, bool needStringQuote);

        /// <summary>
        /// 定义一个简单的Case表达式接口函数，用了支持条件取值。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="field1">取值字段1。</param>
        /// <param name="field2">取值字段2.</param>
        /// <returns>返回格式化后的条件取值的函数字符串。</returns>
        string Case(string expression, string field1, string field2);

        /// <summary>
        /// 如果为空，取值函数。
        /// </summary>
        /// <param name="field1">可能为空的字段一表达式。</param>
        /// <param name="field2">第一个为空的字段的，第二个表达式。</param>
        /// <returns>返回格式化后的为空取值的函数字符串。</returns>
        string IsNull(string field1, string field2);

        /// <summary>
        /// 连接两个字符串。
        /// </summary>
        /// <param name="field1">字符串字段1。</param>
        /// <param name="fields">连接的字符串字段2，3，4...</param>
        /// <returns>返回格式化后的字符串连接函数字符串。</returns>
        string Concat(string field1, params string[] fields);

        /// <summary>
        /// 截断字符串字段左边的空字符的数据库函数。
        /// </summary>
        /// <param name="field">字符串字段名称。</param>
        /// <returns>返回格式化后的左截断字符串字段的函数字符串。</returns>
        string LTrim(string field);


        /// <summary>
        /// 截断字符串字段右边的空字符的数据库函数。
        /// </summary>
        /// <param name="field">字符串字段名称。</param>
        /// <returns>返回格式化后的右截断字符串字段的函数字符串。</returns>
        string RTrim(string field);

        /// <summary>
        /// 截断字符串字段两边的空字符的数据库函数。
        /// </summary>
        /// <param name="field">字符串字段名称。</param>
        /// <returns>返回格式化后的截断两边空格的函数字符串。</returns>
        string Trim(string field);

        /// <summary>
        /// 获取字符串字段字符长度的数据库函数。
        /// </summary>
        /// <param name="field">字符串字段名称。</param>
        /// <returns>返回格式化后的获取字符串长度的函数字符串。</returns>
        string Length(string field);

        /// <summary>
        /// 把字符串字段全部转化为小写的数据库函数。
        /// </summary>
        /// <param name="field">字符串字段名称。</param>
        /// <returns>返回格式化后的取字段小写的函数字符串。</returns>
        string Lower(string field);
        /// <summary>
        /// 把字符串字段全部转化为大写的数据库函数。
        /// </summary>
        /// <param name="field">字符串字段名称。</param>
        /// <returns>返回格式化后的取字段大写的函数字符串。</returns>
        string Upper(string field);
        /// <summary>
        /// 从字符串字段中截取一定长度字符串的数据库函数。
        /// </summary>
        /// <param name="field">字符串字段名称。</param>
        /// <param name="start">截取开始位置。</param>
        /// <param name="length">截取长度。</param>
        /// <returns>返回格式化后截取一定长度字符串的函数字符串。</returns>
        string Substring(string field, int start, int length);

        /// <summary>
        /// 将字段数据转换成字符串数据类型的数据库函数。
        /// </summary>
        /// <param name="field">要转换的数据库字段。</param>
        /// <returns>返回格式化后的转换字符串的函数字符串。</returns>
        string ToChar(string field);

        /// <summary>
        /// 字符串替换的数据库函数。
        /// </summary>
        /// <param name="field">字符串字段名称。</param>
        /// <param name="oitem">源字符串。</param>
        /// <param name="nitem">替换成的新字符串。</param>
        /// <returns>返回格式化后的替换字符串的函数字符串。</returns>
        string Replace(string field, string oitem, string nitem);

        /// <summary>
        /// 获取某个字符串的起始位置的数据库函数。
        /// </summary>
        /// <param name="field">字符串字段名称。</param>
        /// <param name="item">要查找的字符串。</param>
        /// <param name="start">查找的起始位置。</param>
        /// <returns>返回格式化后的查找字符串位置的函数字符串。</returns>
        string CharIndex(string field, string item, int start);

        /// <summary>
        /// 对数字类型进行四舍五入的取位运算的数据库函数。
        /// </summary>
        /// <param name="field">数字型字段名称。</param>
        /// <param name="length">精度。</param>
        /// <returns>返回格式化后的数字字段取位数的函数字符串。</returns>
        string Round(string field, int length);
        /// <summary>
        /// 取整的数据库函数。
        /// </summary>
        /// <param name="field">数字型字段名称。</param>
        /// <returns>返回格式化后的数字型取整的函数字符串。</returns>
        string Floor(string field);

        /// <summary>
        /// 返回获取系统时间的数据库函数名称。
        /// </summary>
        string SysDate { get; }

        /// <summary>
        /// 把日期字段截断时间到0点0分0秒的数据库函数。
        /// </summary>
        /// <param name="datefield">日期字段名称。</param>
        /// <returns>返回格式化后的截断日期字段时间的函数字符串。</returns>
        string TruncateDate(string datefield);

        /// <summary>
        /// 在日期字段的基础上，加上某个日期（或时间）部分数的数据库函数。
        /// </summary>
        /// <param name="datepart">日期（或时间）部分。</param>
        /// <param name="number">加数。</param>
        /// <param name="datefield">日期字段名称。</param>
        /// <returns>返回格式化后的加上日期（或时间）部分的函数字符串。</returns>
        string DateAdd(DatePart datepart, int number, string datefield);

        /// <summary>
        /// 在日期字段的基础上，加上某个日期（或时间）部分数的数据库函数。
        /// </summary>
        /// <param name="datepart">日期（或时间）部分。</param>
        /// <param name="numberExpression">加数表达式。</param>
        /// <param name="datefield">日期字段名称。</param>
        /// <returns>返回格式化后的加上日期（或时间）部分的函数字符串。</returns>
        string DateAdd(DatePart datepart, string numberExpression, string datefield);


        /// <summary>
        /// 求两个日期字段的日期（或时间）差的数据库函数。
        /// </summary>
        /// <param name="datepart">日期（或时间）部分。</param>
        /// <param name="startdate">开始日期字段名称。</param>
        /// <param name="enddate">截止日期字段名称。</param>
        /// <returns>返回</returns>
        string DateDiff(DatePart datepart, string startdate, string enddate);

        /// <summary>
        /// 获取日期类型字段的年份的数据库函数。
        /// </summary>
        /// <param name="field">日期字段名称。</param>
        /// <returns>返回格式化后的获取日期字段年份的函数字符串。（注意：数据库端该函数返回值理应为整型）</returns>
        string Year(string field);

        /// <summary>
        /// 获取日期类型字段的月份的数据库函数。
        /// </summary>
        /// <param name="field">日期字段名称。</param>
        /// <returns>返回格式化后的获取日期字段月份的函数字符串。（注意：数据库端该函数返回值理应为整型）</returns>
        string Month(string field);
        /// <summary>
        /// 获取日期类型字段的日子（天）的数据库函数。
        /// </summary>
        /// <param name="field">日期字段名称。</param>
        /// <returns>返回格式化后的获取日期字段日子（天）的函数字符串。（注意：数据库端该函数返回值理应为整型）</returns>
        string Day(string field);

        /// <summary>
        /// 获取日期类型字段的小时数（24小时制）的数据库函数。
        /// </summary>
        /// <param name="field">日期字段名称。</param>
        /// <returns>返回格式化后的获取日期字段小时数（24小时制）的函数字符串。（注意：数据库端该函数返回值理应为整型）</returns>
        string Hour(string field);
        /// <summary>
        /// 获取日期类型字段的分钟数的数据库函数。
        /// </summary>
        /// <param name="field">日期字段名称。</param>
        /// <returns>返回格式化后的获取日期字段分钟数的函数字符串。（注意：数据库端该函数返回值理应为整型）</returns>
        string Minute(string field);


        /// <summary>
        /// 获取日期类型字段的周几的数据库函数。
        /// </summary>
        /// <param name="field">日期字段名称。</param>
        /// <returns>返回格式化后的获取日期字段周几的函数字符串。（注意：数据库端该函数返回值理应为整型）</returns>
        string WeekDay(string field);

        /// <summary>
        /// 获取查询当前数据库服务器的时间的SQL语句。
        /// </summary>
        string ServerTimeSql { get; }
    }
}
