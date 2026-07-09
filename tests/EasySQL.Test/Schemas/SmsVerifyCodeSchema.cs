using EasySQL;

namespace EasySQL.Test
{
	public class SmsVerifyCodeSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string SMSVERIFYCODES="SmsVerifyCodes";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，手机隶属国家
        ///</summary>
		public const string COUNTRYID="CountryId";
		
        ///<summary> 
    	/// 列名常量，手机号
        ///</summary>
		public const string MOBILE="Mobile";
		
        ///<summary> 
    	/// 列名常量，验证码
        ///</summary>
		public const string VERIFYCODE="VerifyCode";
		
        ///<summary> 
    	/// 列名常量，验证码类型，1：登录验证码，2：产品查询验证码
        ///</summary>
		public const string CODETYPE="CodeType";
		
        ///<summary> 
    	/// 列名常量，发送验证码的短信网关编号
        ///</summary>
		public const string GATEWAYID="GatewayId";
		
        ///<summary> 
    	/// 列名常量，发送状态，1：发送中，2：发送成功，3：发送失败
        ///</summary>
		public const string SENDSTATUS="SendStatus";
		
        ///<summary> 
    	/// 列名常量，短信发送时间
        ///</summary>
		public const string SENDTIME="SendTime";
		
        ///<summary> 
    	/// 列名常量，验证码的到期时间
        ///</summary>
		public const string EXPIRETIME="ExpireTime";
		
        ///<summary> 
    	/// 列名常量，申请发送验证码的客户端IP地址
        ///</summary>
		public const string CLIENTIP="ClientIp";
		
        ///<summary> 
    	/// 列名常量，验证码是否已使用
        ///</summary>
		public const string ISUSED="IsUsed";
		
        ///<summary> 
    	/// 列名常量，验证码被使用的时间
        ///</summary>
		public const string USEDTIME="UsedTime";
		
        ///<summary> 
    	/// 列名常量，使用验证码的客户端IP地址
        ///</summary>
		public const string USEDIP="UsedIp";
		
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
			get { return SMSVERIFYCODES; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public SmsVerifyCodeSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public SmsVerifyCodeSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public SmsVerifyCodeSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 手机隶属国家
        ///</summary>
		public string CountryId { get { return COUNTRYID; } }
		
        ///<summary> 
    	/// 手机号
        ///</summary>
		public string Mobile { get { return MOBILE; } }
		
        ///<summary> 
    	/// 验证码
        ///</summary>
		public string VerifyCode { get { return VERIFYCODE; } }
		
        ///<summary> 
    	/// 验证码类型，1：登录验证码，2：产品查询验证码
        ///</summary>
		public string CodeType { get { return CODETYPE; } }
		
        ///<summary> 
    	/// 发送验证码的短信网关编号
        ///</summary>
		public string GatewayId { get { return GATEWAYID; } }
		
        ///<summary> 
    	/// 发送状态，1：发送中，2：发送成功，3：发送失败
        ///</summary>
		public string SendStatus { get { return SENDSTATUS; } }
		
        ///<summary> 
    	/// 短信发送时间
        ///</summary>
		public string SendTime { get { return SENDTIME; } }
		
        ///<summary> 
    	/// 验证码的到期时间
        ///</summary>
		public string ExpireTime { get { return EXPIRETIME; } }
		
        ///<summary> 
    	/// 申请发送验证码的客户端IP地址
        ///</summary>
		public string ClientIp { get { return CLIENTIP; } }
		
        ///<summary> 
    	/// 验证码是否已使用
        ///</summary>
		public string IsUsed { get { return ISUSED; } }
		
        ///<summary> 
    	/// 验证码被使用的时间
        ///</summary>
		public string UsedTime { get { return USEDTIME; } }
		
        ///<summary> 
    	/// 使用验证码的客户端IP地址
        ///</summary>
		public string UsedIp { get { return USEDIP; } }
		
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
    	/// 手机隶属国家
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
		
        ///<summary> 
    	/// 验证码
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetVerifyCode(bool needPrefix=true)
        {
            return this.QuoteField(VERIFYCODE,needPrefix);
        }
		
        ///<summary> 
    	/// 验证码类型，1：登录验证码，2：产品查询验证码
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCodeType(bool needPrefix=true)
        {
            return this.QuoteField(CODETYPE,needPrefix);
        }
		
        ///<summary> 
    	/// 发送验证码的短信网关编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetGatewayId(bool needPrefix=true)
        {
            return this.QuoteField(GATEWAYID,needPrefix);
        }
		
        ///<summary> 
    	/// 发送状态，1：发送中，2：发送成功，3：发送失败
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetSendStatus(bool needPrefix=true)
        {
            return this.QuoteField(SENDSTATUS,needPrefix);
        }
		
        ///<summary> 
    	/// 短信发送时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetSendTime(bool needPrefix=true)
        {
            return this.QuoteField(SENDTIME,needPrefix);
        }
		
        ///<summary> 
    	/// 验证码的到期时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetExpireTime(bool needPrefix=true)
        {
            return this.QuoteField(EXPIRETIME,needPrefix);
        }
		
        ///<summary> 
    	/// 申请发送验证码的客户端IP地址
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetClientIp(bool needPrefix=true)
        {
            return this.QuoteField(CLIENTIP,needPrefix);
        }
		
        ///<summary> 
    	/// 验证码是否已使用
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetIsUsed(bool needPrefix=true)
        {
            return this.QuoteField(ISUSED,needPrefix);
        }
		
        ///<summary> 
    	/// 验证码被使用的时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetUsedTime(bool needPrefix=true)
        {
            return this.QuoteField(USEDTIME,needPrefix);
        }
		
        ///<summary> 
    	/// 使用验证码的客户端IP地址
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetUsedIp(bool needPrefix=true)
        {
            return this.QuoteField(USEDIP,needPrefix);
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
