using EasySQL;

namespace EasySQL.Test
{
	public class WebUserVerifySchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string WEBUSERVERIFY="WebUserVerify";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，用户账号编号
        ///</summary>
		public const string ACCOUNTUSERID="AccountUserId";
		
        ///<summary> 
    	/// 列名常量，操作用户编号
        ///</summary>
		public const string OPERATEUSERID="OperateUserId";
		
        ///<summary> 
    	/// 列名常量，被核查人员姓名
        ///</summary>
		public const string VERIFIEDNAME="VerifiedName";
		
        ///<summary> 
    	/// 列名常量，被核查人员身份证号
        ///</summary>
		public const string VERIFIEDIDNUMBER="VerifiedIdNumber";
		
        ///<summary> 
    	/// 列名常量，核查的产品，bit位值，如：1：核查身份证，2：核查犯罪记录
        ///</summary>
		public const string PRODUCTS="Products";
		
        ///<summary> 
    	/// 列名常量，登录日志编号
        ///</summary>
		public const string LOGINLOGID="LoginLogId";
		
        ///<summary> 
    	/// 列名常量，核查客户端的IP地址
        ///</summary>
		public const string CLIENTIP="ClientIp";
		
        ///<summary> 
    	/// 列名常量，核查客户端所在的纬度
        ///</summary>
		public const string LAT="Lat";
		
        ///<summary> 
    	/// 列名常量，核查客户端所在的经度
        ///</summary>
		public const string LNG="Lng";
		
        ///<summary> 
    	/// 列名常量，核查状态，1：核查中，2：成功，3：失败
        ///</summary>
		public const string STATUS="Status";
		
        ///<summary> 
    	/// 列名常量，扩展位选项
        ///</summary>
		public const string EXTOPTION="ExtOption";
		
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
			get { return WEBUSERVERIFY; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public WebUserVerifySchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public WebUserVerifySchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public WebUserVerifySchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 用户账号编号
        ///</summary>
		public string AccountUserId { get { return ACCOUNTUSERID; } }
		
        ///<summary> 
    	/// 操作用户编号
        ///</summary>
		public string OperateUserId { get { return OPERATEUSERID; } }
		
        ///<summary> 
    	/// 被核查人员姓名
        ///</summary>
		public string VerifiedName { get { return VERIFIEDNAME; } }
		
        ///<summary> 
    	/// 被核查人员身份证号
        ///</summary>
		public string VerifiedIdNumber { get { return VERIFIEDIDNUMBER; } }
		
        ///<summary> 
    	/// 核查的产品，bit位值，如：1：核查身份证，2：核查犯罪记录
        ///</summary>
		public string Products { get { return PRODUCTS; } }
		
        ///<summary> 
    	/// 登录日志编号
        ///</summary>
		public string LoginLogId { get { return LOGINLOGID; } }
		
        ///<summary> 
    	/// 核查客户端的IP地址
        ///</summary>
		public string ClientIp { get { return CLIENTIP; } }
		
        ///<summary> 
    	/// 核查客户端所在的纬度
        ///</summary>
		public string Lat { get { return LAT; } }
		
        ///<summary> 
    	/// 核查客户端所在的经度
        ///</summary>
		public string Lng { get { return LNG; } }
		
        ///<summary> 
    	/// 核查状态，1：核查中，2：成功，3：失败
        ///</summary>
		public string Status { get { return STATUS; } }
		
        ///<summary> 
    	/// 扩展位选项
        ///</summary>
		public string ExtOption { get { return EXTOPTION; } }
		
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
    	/// 用户账号编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetAccountUserId(bool needPrefix=true)
        {
            return this.QuoteField(ACCOUNTUSERID,needPrefix);
        }
		
        ///<summary> 
    	/// 操作用户编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetOperateUserId(bool needPrefix=true)
        {
            return this.QuoteField(OPERATEUSERID,needPrefix);
        }
		
        ///<summary> 
    	/// 被核查人员姓名
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetVerifiedName(bool needPrefix=true)
        {
            return this.QuoteField(VERIFIEDNAME,needPrefix);
        }
		
        ///<summary> 
    	/// 被核查人员身份证号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetVerifiedIdNumber(bool needPrefix=true)
        {
            return this.QuoteField(VERIFIEDIDNUMBER,needPrefix);
        }
		
        ///<summary> 
    	/// 核查的产品，bit位值，如：1：核查身份证，2：核查犯罪记录
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetProducts(bool needPrefix=true)
        {
            return this.QuoteField(PRODUCTS,needPrefix);
        }
		
        ///<summary> 
    	/// 登录日志编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLoginLogId(bool needPrefix=true)
        {
            return this.QuoteField(LOGINLOGID,needPrefix);
        }
		
        ///<summary> 
    	/// 核查客户端的IP地址
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetClientIp(bool needPrefix=true)
        {
            return this.QuoteField(CLIENTIP,needPrefix);
        }
		
        ///<summary> 
    	/// 核查客户端所在的纬度
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLat(bool needPrefix=true)
        {
            return this.QuoteField(LAT,needPrefix);
        }
		
        ///<summary> 
    	/// 核查客户端所在的经度
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLng(bool needPrefix=true)
        {
            return this.QuoteField(LNG,needPrefix);
        }
		
        ///<summary> 
    	/// 核查状态，1：核查中，2：成功，3：失败
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetStatus(bool needPrefix=true)
        {
            return this.QuoteField(STATUS,needPrefix);
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
		
        #endregion
    }
}
