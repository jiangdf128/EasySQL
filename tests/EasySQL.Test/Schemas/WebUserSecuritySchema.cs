using EasySQL;

namespace EasySQL.Test
{
	public class WebUserSecuritySchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string WEBUSERSECURITY="WebUserSecurity";
        
        ///<summary> 
    	/// 列名常量，用户编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，用户名
        ///</summary>
		public const string LOGINNAME="LoginName";
		
        ///<summary> 
    	/// 列名常量，登录密码
        ///</summary>
		public const string LOGINPASSWORD="LoginPassword";
		
        ///<summary> 
    	/// 列名常量，Email地址
        ///</summary>
		public const string EMAIL="Email";
		
        ///<summary> 
    	/// 列名常量，Email地址是否经验证确认
        ///</summary>
		public const string ISEMAILCONFIRMED="IsEmailConfirmed";
		
        ///<summary> 
    	/// 列名常量，是否子用户
        ///</summary>
		public const string ISCHILDUSER="IsChildUser";
		
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
			get { return WEBUSERSECURITY; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public WebUserSecuritySchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public WebUserSecuritySchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public WebUserSecuritySchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 用户编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 用户名
        ///</summary>
		public string LoginName { get { return LOGINNAME; } }
		
        ///<summary> 
    	/// 登录密码
        ///</summary>
		public string LoginPassword { get { return LOGINPASSWORD; } }
		
        ///<summary> 
    	/// Email地址
        ///</summary>
		public string Email { get { return EMAIL; } }
		
        ///<summary> 
    	/// Email地址是否经验证确认
        ///</summary>
		public string IsEmailConfirmed { get { return ISEMAILCONFIRMED; } }
		
        ///<summary> 
    	/// 是否子用户
        ///</summary>
		public string IsChildUser { get { return ISCHILDUSER; } }
		
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
    	/// 用户编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetId(bool needPrefix=true)
        {
            return this.QuoteField(ID,needPrefix);
        }
		
        ///<summary> 
    	/// 用户名
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLoginName(bool needPrefix=true)
        {
            return this.QuoteField(LOGINNAME,needPrefix);
        }
		
        ///<summary> 
    	/// 登录密码
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLoginPassword(bool needPrefix=true)
        {
            return this.QuoteField(LOGINPASSWORD,needPrefix);
        }
		
        ///<summary> 
    	/// Email地址
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetEmail(bool needPrefix=true)
        {
            return this.QuoteField(EMAIL,needPrefix);
        }
		
        ///<summary> 
    	/// Email地址是否经验证确认
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetIsEmailConfirmed(bool needPrefix=true)
        {
            return this.QuoteField(ISEMAILCONFIRMED,needPrefix);
        }
		
        ///<summary> 
    	/// 是否子用户
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetIsChildUser(bool needPrefix=true)
        {
            return this.QuoteField(ISCHILDUSER,needPrefix);
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
