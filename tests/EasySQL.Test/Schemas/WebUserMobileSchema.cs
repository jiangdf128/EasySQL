using EasySQL;

namespace EasySQL.Test
{
	public class WebUserMobileSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string WEBUSERMOBILES="WebUserMobiles";
        
        ///<summary> 
    	/// 列名常量，用户编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，手机所属国家。默认为中国，以配合手机号的唯一性。预留，以扩展作为国外手机短信登录使用。
        ///</summary>
		public const string COUNTRYID="CountryId";
		
        ///<summary> 
    	/// 列名常量，手机号
        ///</summary>
		public const string MOBILE="Mobile";
		
        #endregion
    
    	#region 实现SchemaBase抽象类接口。
		/// <summary>
        /// 获取表（或视图）架构的名称。
        /// </summary>
        /// <remarks>该名称与数据库里面的名称对应。</remarks>
		public override string TableName
		{
			get { return WEBUSERMOBILES; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public WebUserMobileSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public WebUserMobileSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public WebUserMobileSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 用户编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 手机所属国家。默认为中国，以配合手机号的唯一性。预留，以扩展作为国外手机短信登录使用。
        ///</summary>
		public string CountryId { get { return COUNTRYID; } }
		
        ///<summary> 
    	/// 手机号
        ///</summary>
		public string Mobile { get { return MOBILE; } }
		
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
    	/// 手机所属国家。默认为中国，以配合手机号的唯一性。预留，以扩展作为国外手机短信登录使用。
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCountryId(bool needPrefix=true)
        {
            return this.QuoteField(COUNTRYID,needPrefix);
        }
		
        ///<summary> 
    	/// 手机号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetMobile(bool needPrefix=true)
        {
            return this.QuoteField(MOBILE,needPrefix);
        }
		
        #endregion
    }
}
