using EasySQL;

namespace EasySQL.Test
{
	public class WebUserJournalSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string WEBUSERJOURNAL="WebUserJournal";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，用户编号
        ///</summary>
		public const string USERID="UserId";
		
        ///<summary> 
    	/// 列名常量，发生时间
        ///</summary>
		public const string OCCURTIME="OccurTime";
		
        ///<summary> 
    	/// 列名常量，科目编号
        ///</summary>
		public const string SUBJECTID="SubjectId";
		
        ///<summary> 
    	/// 列名常量，金额
        ///</summary>
		public const string AMOUNT="Amount";
		
        ///<summary> 
    	/// 列名常量，发生业务的对象类型
        ///</summary>
		public const string BUSINESSTYPE="BusinessType";
		
        ///<summary> 
    	/// 列名常量，发生业务的对象编号
        ///</summary>
		public const string BUSINESSID="BusinessId";
		
        ///<summary> 
    	/// 列名常量，摘要
        ///</summary>
		public const string SUMMARY="Summary";
		
        ///<summary> 
    	/// 列名常量，扩展位选项
        ///</summary>
		public const string EXTOPTION="ExtOption";
		
        ///<summary> 
    	/// 列名常量，创建时间
        ///</summary>
		public const string CREATETIME="CreateTime";
		
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
			get { return WEBUSERJOURNAL; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public WebUserJournalSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public WebUserJournalSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public WebUserJournalSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 用户编号
        ///</summary>
		public string UserId { get { return USERID; } }
		
        ///<summary> 
    	/// 发生时间
        ///</summary>
		public string OccurTime { get { return OCCURTIME; } }
		
        ///<summary> 
    	/// 科目编号
        ///</summary>
		public string SubjectId { get { return SUBJECTID; } }
		
        ///<summary> 
    	/// 金额
        ///</summary>
		public string Amount { get { return AMOUNT; } }
		
        ///<summary> 
    	/// 发生业务的对象类型
        ///</summary>
		public string BusinessType { get { return BUSINESSTYPE; } }
		
        ///<summary> 
    	/// 发生业务的对象编号
        ///</summary>
		public string BusinessId { get { return BUSINESSID; } }
		
        ///<summary> 
    	/// 摘要
        ///</summary>
		public string Summary { get { return SUMMARY; } }
		
        ///<summary> 
    	/// 扩展位选项
        ///</summary>
		public string ExtOption { get { return EXTOPTION; } }
		
        ///<summary> 
    	/// 创建时间
        ///</summary>
		public string CreateTime { get { return CREATETIME; } }
		
        ///<summary> 
    	/// 版本号
        ///</summary>
		public string Version { get { return VERSION; } }
		
        #endregion
        
        #region 格式化列名称访问方法
        ///<summary> 
    	/// 编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetId(bool needPrefix=true)
        {
            return this.QuoteField(ID,needPrefix);
        }
		
        ///<summary> 
    	/// 用户编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetUserId(bool needPrefix=true)
        {
            return this.QuoteField(USERID,needPrefix);
        }
		
        ///<summary> 
    	/// 发生时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetOccurTime(bool needPrefix=true)
        {
            return this.QuoteField(OCCURTIME,needPrefix);
        }
		
        ///<summary> 
    	/// 科目编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetSubjectId(bool needPrefix=true)
        {
            return this.QuoteField(SUBJECTID,needPrefix);
        }
		
        ///<summary> 
    	/// 金额
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetAmount(bool needPrefix=true)
        {
            return this.QuoteField(AMOUNT,needPrefix);
        }
		
        ///<summary> 
    	/// 发生业务的对象类型
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetBusinessType(bool needPrefix=true)
        {
            return this.QuoteField(BUSINESSTYPE,needPrefix);
        }
		
        ///<summary> 
    	/// 发生业务的对象编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetBusinessId(bool needPrefix=true)
        {
            return this.QuoteField(BUSINESSID,needPrefix);
        }
		
        ///<summary> 
    	/// 摘要
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetSummary(bool needPrefix=true)
        {
            return this.QuoteField(SUMMARY,needPrefix);
        }
		
        ///<summary> 
    	/// 扩展位选项
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetExtOption(bool needPrefix=true)
        {
            return this.QuoteField(EXTOPTION,needPrefix);
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
