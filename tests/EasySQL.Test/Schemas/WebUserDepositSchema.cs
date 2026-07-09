using EasySQL;

namespace EasySQL.Test
{
	public class WebUserDepositSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string WEBUSERDEPOSIT="WebUserDeposit";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，用户账户编号
        ///</summary>
		public const string ACCOUNTUSERID="AccountUserId";
		
        ///<summary> 
    	/// 列名常量，操作用户编号（若为子账户操作则为子用户编号）
        ///</summary>
		public const string OPERATEUSERID="OperateUserId";
		
        ///<summary> 
    	/// 列名常量，充值发生时间
        ///</summary>
		public const string OCCURTIME="OccurTime";
		
        ///<summary> 
    	/// 列名常量，所使用的充值产品编号（如10元面额，30元面额）
        ///</summary>
		public const string DEPOSITPRODUCTID="DepositProductId";
		
        ///<summary> 
    	/// 列名常量，充值对应的公司收款账号编号
        ///</summary>
		public const string COMPANYACCOUNTID="CompanyAccountId";
		
        ///<summary> 
    	/// 列名常量，充值的付款金额（实际金额）
        ///</summary>
		public const string MONEYAMOUNT="MoneyAmount";
		
        ///<summary> 
    	/// 列名常量，充入用户账户的金额数
        ///</summary>
		public const string BALANCEAMOUNT="BalanceAmount";
		
        ///<summary> 
    	/// 列名常量，支付方式，1：微信，2：支付宝，3：银联
        ///</summary>
		public const string PAYMETHODID="PayMethodId";
		
        ///<summary> 
    	/// 列名常量，第三方支付平台的支付流水号
        ///</summary>
		public const string PAYPLATFORMBILL="PayPlatformBill";
		
        ///<summary> 
    	/// 列名常量，支付人姓名
        ///</summary>
		public const string PAYERNAME="PayerName";
		
        ///<summary> 
    	/// 列名常量，支付人账号
        ///</summary>
		public const string PAYERACCOUNT="PayerAccount";
		
        ///<summary> 
    	/// 列名常量，支付人的OpenId（如：微信公众号的用户OpenID）
        ///</summary>
		public const string PAYEROPENID="PayerOpenId";
		
        ///<summary> 
    	/// 列名常量，充值成功对应的账户流水账编号
        ///</summary>
		public const string JOURNALID="JournalId";
		
        ///<summary> 
    	/// 列名常量，摘要
        ///</summary>
		public const string SUMMARY="Summary";
		
        ///<summary> 
    	/// 列名常量，登录日志编号
        ///</summary>
		public const string LOGINLOGID="LoginLogId";
		
        ///<summary> 
    	/// 列名常量，客户端IP地址
        ///</summary>
		public const string CLIENTIP="ClientIp";
		
        ///<summary> 
    	/// 列名常量，充值客户端所在的纬度
        ///</summary>
		public const string LAT="Lat";
		
        ///<summary> 
    	/// 列名常量，充值客户端所在的经度
        ///</summary>
		public const string LNG="Lng";
		
        ///<summary> 
    	/// 列名常量，充值状态，1：充值中，2：充值成功，3：充值失败
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
			get { return WEBUSERDEPOSIT; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public WebUserDepositSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public WebUserDepositSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public WebUserDepositSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 用户账户编号
        ///</summary>
		public string AccountUserId { get { return ACCOUNTUSERID; } }
		
        ///<summary> 
    	/// 操作用户编号（若为子账户操作则为子用户编号）
        ///</summary>
		public string OperateUserId { get { return OPERATEUSERID; } }
		
        ///<summary> 
    	/// 充值发生时间
        ///</summary>
		public string OccurTime { get { return OCCURTIME; } }
		
        ///<summary> 
    	/// 所使用的充值产品编号（如10元面额，30元面额）
        ///</summary>
		public string DepositProductId { get { return DEPOSITPRODUCTID; } }
		
        ///<summary> 
    	/// 充值对应的公司收款账号编号
        ///</summary>
		public string CompanyAccountId { get { return COMPANYACCOUNTID; } }
		
        ///<summary> 
    	/// 充值的付款金额（实际金额）
        ///</summary>
		public string MoneyAmount { get { return MONEYAMOUNT; } }
		
        ///<summary> 
    	/// 充入用户账户的金额数
        ///</summary>
		public string BalanceAmount { get { return BALANCEAMOUNT; } }
		
        ///<summary> 
    	/// 支付方式，1：微信，2：支付宝，3：银联
        ///</summary>
		public string PayMethodId { get { return PAYMETHODID; } }
		
        ///<summary> 
    	/// 第三方支付平台的支付流水号
        ///</summary>
		public string PayPlatformBill { get { return PAYPLATFORMBILL; } }
		
        ///<summary> 
    	/// 支付人姓名
        ///</summary>
		public string PayerName { get { return PAYERNAME; } }
		
        ///<summary> 
    	/// 支付人账号
        ///</summary>
		public string PayerAccount { get { return PAYERACCOUNT; } }
		
        ///<summary> 
    	/// 支付人的OpenId（如：微信公众号的用户OpenID）
        ///</summary>
		public string PayerOpenId { get { return PAYEROPENID; } }
		
        ///<summary> 
    	/// 充值成功对应的账户流水账编号
        ///</summary>
		public string JournalId { get { return JOURNALID; } }
		
        ///<summary> 
    	/// 摘要
        ///</summary>
		public string Summary { get { return SUMMARY; } }
		
        ///<summary> 
    	/// 登录日志编号
        ///</summary>
		public string LoginLogId { get { return LOGINLOGID; } }
		
        ///<summary> 
    	/// 客户端IP地址
        ///</summary>
		public string ClientIp { get { return CLIENTIP; } }
		
        ///<summary> 
    	/// 充值客户端所在的纬度
        ///</summary>
		public string Lat { get { return LAT; } }
		
        ///<summary> 
    	/// 充值客户端所在的经度
        ///</summary>
		public string Lng { get { return LNG; } }
		
        ///<summary> 
    	/// 充值状态，1：充值中，2：充值成功，3：充值失败
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
    	/// 用户账户编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetAccountUserId(bool needPrefix=true)
        {
            return this.QuoteField(ACCOUNTUSERID,needPrefix);
        }
		
        ///<summary> 
    	/// 操作用户编号（若为子账户操作则为子用户编号）
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetOperateUserId(bool needPrefix=true)
        {
            return this.QuoteField(OPERATEUSERID,needPrefix);
        }
		
        ///<summary> 
    	/// 充值发生时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetOccurTime(bool needPrefix=true)
        {
            return this.QuoteField(OCCURTIME,needPrefix);
        }
		
        ///<summary> 
    	/// 所使用的充值产品编号（如10元面额，30元面额）
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetDepositProductId(bool needPrefix=true)
        {
            return this.QuoteField(DEPOSITPRODUCTID,needPrefix);
        }
		
        ///<summary> 
    	/// 充值对应的公司收款账号编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCompanyAccountId(bool needPrefix=true)
        {
            return this.QuoteField(COMPANYACCOUNTID,needPrefix);
        }
		
        ///<summary> 
    	/// 充值的付款金额（实际金额）
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetMoneyAmount(bool needPrefix=true)
        {
            return this.QuoteField(MONEYAMOUNT,needPrefix);
        }
		
        ///<summary> 
    	/// 充入用户账户的金额数
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetBalanceAmount(bool needPrefix=true)
        {
            return this.QuoteField(BALANCEAMOUNT,needPrefix);
        }
		
        ///<summary> 
    	/// 支付方式，1：微信，2：支付宝，3：银联
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetPayMethodId(bool needPrefix=true)
        {
            return this.QuoteField(PAYMETHODID,needPrefix);
        }
		
        ///<summary> 
    	/// 第三方支付平台的支付流水号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetPayPlatformBill(bool needPrefix=true)
        {
            return this.QuoteField(PAYPLATFORMBILL,needPrefix);
        }
		
        ///<summary> 
    	/// 支付人姓名
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetPayerName(bool needPrefix=true)
        {
            return this.QuoteField(PAYERNAME,needPrefix);
        }
		
        ///<summary> 
    	/// 支付人账号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetPayerAccount(bool needPrefix=true)
        {
            return this.QuoteField(PAYERACCOUNT,needPrefix);
        }
		
        ///<summary> 
    	/// 支付人的OpenId（如：微信公众号的用户OpenID）
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetPayerOpenId(bool needPrefix=true)
        {
            return this.QuoteField(PAYEROPENID,needPrefix);
        }
		
        ///<summary> 
    	/// 充值成功对应的账户流水账编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetJournalId(bool needPrefix=true)
        {
            return this.QuoteField(JOURNALID,needPrefix);
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
    	/// 登录日志编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLoginLogId(bool needPrefix=true)
        {
            return this.QuoteField(LOGINLOGID,needPrefix);
        }
		
        ///<summary> 
    	/// 客户端IP地址
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetClientIp(bool needPrefix=true)
        {
            return this.QuoteField(CLIENTIP,needPrefix);
        }
		
        ///<summary> 
    	/// 充值客户端所在的纬度
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLat(bool needPrefix=true)
        {
            return this.QuoteField(LAT,needPrefix);
        }
		
        ///<summary> 
    	/// 充值客户端所在的经度
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLng(bool needPrefix=true)
        {
            return this.QuoteField(LNG,needPrefix);
        }
		
        ///<summary> 
    	/// 充值状态，1：充值中，2：充值成功，3：充值失败
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
