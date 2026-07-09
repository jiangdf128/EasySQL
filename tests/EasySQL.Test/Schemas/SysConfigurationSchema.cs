using EasySQL;

namespace EasySQL.Test
{
	public class SysConfigurationSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string SYSCONFIGURATION="SysConfiguration";
        
        ///<summary> 
    	/// 列名常量，选项名Key
        ///</summary>
		public const string ITEMKEY="ItemKey";
		
        ///<summary> 
    	/// 列名常量，选项值
        ///</summary>
		public const string ITEMVALUE="ItemValue";
		
        ///<summary> 
    	/// 列名常量，选项长文本值
        ///</summary>
		public const string LONGVALUE="LongValue";
		
        ///<summary> 
    	/// 列名常量，是否取长文本值
        ///</summary>
		public const string ISLONGVALUE="IsLongValue";
		
        ///<summary> 
    	/// 列名常量，选项分类编号
        ///</summary>
		public const string CATEGORYID="CategoryId";
		
        ///<summary> 
    	/// 列名常量，行号
        ///</summary>
		public const string LINE="Line";
		
        ///<summary> 
    	/// 列名常量，数据类型
        ///</summary>
		public const string DATATYPE="DataType";
		
        ///<summary> 
    	/// 列名常量，数据合法性验证类型或公式
        ///</summary>
		public const string VALIDTYPE="ValidType";
		
        ///<summary> 
    	/// 列名常量，选项对应的数据字典编号（如果是下拉选择）
        ///</summary>
		public const string DICTIONARYID="DictionaryId";
		
        ///<summary> 
    	/// 列名常量，选项功能说明
        ///</summary>
		public const string DESCRIPTION="Description";
		
        ///<summary> 
    	/// 列名常量，是否必须录入值
        ///</summary>
		public const string ISMUSTINPUT="IsMustInput";
		
        ///<summary> 
    	/// 列名常量，是否可以编辑
        ///</summary>
		public const string ISEDITABLE="IsEditable";
		
        ///<summary> 
    	/// 列名常量，对后台管理用户在UI上是否可见
        ///</summary>
		public const string ISVISIBLE="IsVisible";
		
        ///<summary> 
    	/// 列名常量，创建时间
        ///</summary>
		public const string CREATETIME="CreateTime";
		
        ///<summary> 
    	/// 列名常量，上次编辑时间
        ///</summary>
		public const string LASTEDITTIME="LastEditTime";
		
        ///<summary> 
    	/// 列名常量，版本号
        ///</summary>
		public const string VERSION="Version";
		
        #endregion
    
    	#region 实现SchemaBase抽象类接口。
		/// <summary>
        /// 获取表（或视图）架构的名称。
        /// </summary>
        /// <remarks>该名称与数据库里面的名称对应。</remarks>
		public override string TableName
		{
			get { return SYSCONFIGURATION; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public SysConfigurationSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public SysConfigurationSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public SysConfigurationSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 选项名Key
        ///</summary>
		public string ItemKey { get { return ITEMKEY; } }
		
        ///<summary> 
    	/// 选项值
        ///</summary>
		public string ItemValue { get { return ITEMVALUE; } }
		
        ///<summary> 
    	/// 选项长文本值
        ///</summary>
		public string LongValue { get { return LONGVALUE; } }
		
        ///<summary> 
    	/// 是否取长文本值
        ///</summary>
		public string IsLongValue { get { return ISLONGVALUE; } }
		
        ///<summary> 
    	/// 选项分类编号
        ///</summary>
		public string CategoryId { get { return CATEGORYID; } }
		
        ///<summary> 
    	/// 行号
        ///</summary>
		public string Line { get { return LINE; } }
		
        ///<summary> 
    	/// 数据类型
        ///</summary>
		public string DataType { get { return DATATYPE; } }
		
        ///<summary> 
    	/// 数据合法性验证类型或公式
        ///</summary>
		public string ValidType { get { return VALIDTYPE; } }
		
        ///<summary> 
    	/// 选项对应的数据字典编号（如果是下拉选择）
        ///</summary>
		public string DictionaryId { get { return DICTIONARYID; } }
		
        ///<summary> 
    	/// 选项功能说明
        ///</summary>
		public string Description { get { return DESCRIPTION; } }
		
        ///<summary> 
    	/// 是否必须录入值
        ///</summary>
		public string IsMustInput { get { return ISMUSTINPUT; } }
		
        ///<summary> 
    	/// 是否可以编辑
        ///</summary>
		public string IsEditable { get { return ISEDITABLE; } }
		
        ///<summary> 
    	/// 对后台管理用户在UI上是否可见
        ///</summary>
		public string IsVisible { get { return ISVISIBLE; } }
		
        ///<summary> 
    	/// 创建时间
        ///</summary>
		public string CreateTime { get { return CREATETIME; } }
		
        ///<summary> 
    	/// 上次编辑时间
        ///</summary>
		public string LastEditTime { get { return LASTEDITTIME; } }
		
        ///<summary> 
    	/// 版本号
        ///</summary>
		public string Version { get { return VERSION; } }
		
        #endregion
        
        #region 格式化列名称访问方法
        ///<summary> 
    	/// 选项名Key
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetItemKey(bool needPrefix=true)
        {
            return this.QuoteField(ITEMKEY,needPrefix);
        }
		
        ///<summary> 
    	/// 选项值
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetItemValue(bool needPrefix=true)
        {
            return this.QuoteField(ITEMVALUE,needPrefix);
        }
		
        ///<summary> 
    	/// 选项长文本值
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLongValue(bool needPrefix=true)
        {
            return this.QuoteField(LONGVALUE,needPrefix);
        }
		
        ///<summary> 
    	/// 是否取长文本值
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetIsLongValue(bool needPrefix=true)
        {
            return this.QuoteField(ISLONGVALUE,needPrefix);
        }
		
        ///<summary> 
    	/// 选项分类编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCategoryId(bool needPrefix=true)
        {
            return this.QuoteField(CATEGORYID,needPrefix);
        }
		
        ///<summary> 
    	/// 行号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLine(bool needPrefix=true)
        {
            return this.QuoteField(LINE,needPrefix);
        }
		
        ///<summary> 
    	/// 数据类型
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetDataType(bool needPrefix=true)
        {
            return this.QuoteField(DATATYPE,needPrefix);
        }
		
        ///<summary> 
    	/// 数据合法性验证类型或公式
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetValidType(bool needPrefix=true)
        {
            return this.QuoteField(VALIDTYPE,needPrefix);
        }
		
        ///<summary> 
    	/// 选项对应的数据字典编号（如果是下拉选择）
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetDictionaryId(bool needPrefix=true)
        {
            return this.QuoteField(DICTIONARYID,needPrefix);
        }
		
        ///<summary> 
    	/// 选项功能说明
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetDescription(bool needPrefix=true)
        {
            return this.QuoteField(DESCRIPTION,needPrefix);
        }
		
        ///<summary> 
    	/// 是否必须录入值
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetIsMustInput(bool needPrefix=true)
        {
            return this.QuoteField(ISMUSTINPUT,needPrefix);
        }
		
        ///<summary> 
    	/// 是否可以编辑
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetIsEditable(bool needPrefix=true)
        {
            return this.QuoteField(ISEDITABLE,needPrefix);
        }
		
        ///<summary> 
    	/// 对后台管理用户在UI上是否可见
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetIsVisible(bool needPrefix=true)
        {
            return this.QuoteField(ISVISIBLE,needPrefix);
        }
		
        ///<summary> 
    	/// 创建时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCreateTime(bool needPrefix=true)
        {
            return this.QuoteField(CREATETIME,needPrefix);
        }
		
        ///<summary> 
    	/// 上次编辑时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLastEditTime(bool needPrefix=true)
        {
            return this.QuoteField(LASTEDITTIME,needPrefix);
        }
		
        ///<summary> 
    	/// 版本号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetVersion(bool needPrefix=true)
        {
            return this.QuoteField(VERSION,needPrefix);
        }
		
        #endregion
    }
}
