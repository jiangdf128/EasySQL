using EasySQL;

namespace EasySQL.Test
{
	public class WebUserDepositErrorSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string WEBUSERDEPOSITERROR="WebUserDepositError";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，充值编号
        ///</summary>
		public const string DEPOSITID="DepositId";
		
        ///<summary> 
    	/// 列名常量，错误代码
        ///</summary>
		public const string ERRORCODE="ErrorCode";
		
        ///<summary> 
    	/// 列名常量，错误消息
        ///</summary>
		public const string ERRORMESSAGE="ErrorMessage";
		
        ///<summary> 
    	/// 列名常量，创建时间
        ///</summary>
		public const string CREATETIME="CreateTime";
		
        #endregion
    
    	#region 实现SchemaBase抽象类接口。
		/// <summary>
        /// 获取表（或视图）架构的名称。
        /// </summary>
        /// <remarks>该名称与数据库里面的名称对应。</remarks>
		public override string TableName
		{
			get { return WEBUSERDEPOSITERROR; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public WebUserDepositErrorSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public WebUserDepositErrorSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public WebUserDepositErrorSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 充值编号
        ///</summary>
		public string DepositId { get { return DEPOSITID; } }
		
        ///<summary> 
    	/// 错误代码
        ///</summary>
		public string ErrorCode { get { return ERRORCODE; } }
		
        ///<summary> 
    	/// 错误消息
        ///</summary>
		public string ErrorMessage { get { return ERRORMESSAGE; } }
		
        ///<summary> 
    	/// 创建时间
        ///</summary>
		public string CreateTime { get { return CREATETIME; } }
		
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
    	/// 充值编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetDepositId(bool needPrefix=true)
        {
            return this.QuoteField(DEPOSITID,needPrefix);
        }
		
        ///<summary> 
    	/// 错误代码
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetErrorCode(bool needPrefix=true)
        {
            return this.QuoteField(ERRORCODE,needPrefix);
        }
		
        ///<summary> 
    	/// 错误消息
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetErrorMessage(bool needPrefix=true)
        {
            return this.QuoteField(ERRORMESSAGE,needPrefix);
        }
		
        ///<summary> 
    	/// 创建时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCreateTime(bool needPrefix=true)
        {
            return this.QuoteField(CREATETIME,needPrefix);
        }
		
        #endregion
    }
}
