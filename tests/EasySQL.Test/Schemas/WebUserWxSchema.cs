using EasySQL;

namespace EasySQL.Test
{
	public class WebUserWxSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string WEBUSERWX="WebUserWx";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，用户ID
        ///</summary>
		public const string USERID="UserId";
		
        ///<summary> 
    	/// 列名常量，微信OpenID
        ///</summary>
		public const string OPENID="OpenId";
		
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
			get { return WEBUSERWX; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public WebUserWxSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public WebUserWxSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public WebUserWxSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 用户ID
        ///</summary>
		public string UserId { get { return USERID; } }
		
        ///<summary> 
    	/// 微信OpenID
        ///</summary>
		public string OpenId { get { return OPENID; } }
		
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
    	/// 用户ID
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetUserId(bool needPrefix=true)
        {
            return this.QuoteField(USERID,needPrefix);
        }
		
        ///<summary> 
    	/// 微信OpenID
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetOpenId(bool needPrefix=true)
        {
            return this.QuoteField(OPENID,needPrefix);
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
