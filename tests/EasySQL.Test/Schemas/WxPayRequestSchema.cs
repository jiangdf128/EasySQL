using EasySQL;

namespace EasySQL.Test
{
	public class WxPayRequestSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string WXPAYREQUEST="WxPayRequest";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，业务对象类型
        ///</summary>
		public const string OBJTYPE="ObjType";
		
        ///<summary> 
    	/// 列名常量，业务对象编号
        ///</summary>
		public const string OBJID="ObjId";
		
        ///<summary> 
    	/// 列名常量，商户号
        ///</summary>
		public const string MERCHANTID="MerchantId";
		
        ///<summary> 
    	/// 列名常量，交易类型
        ///</summary>
		public const string TRADETYPE="TradeType";
		
        ///<summary> 
    	/// 列名常量，付款金额
        ///</summary>
		public const string PAYAMOUNT="PayAmount";
		
        ///<summary> 
    	/// 列名常量，支付商品
        ///</summary>
		public const string PAYBODY="PayBody";
		
        ///<summary> 
    	/// 列名常量，微信OPENID
        ///</summary>
		public const string OPENID="OpenId";
		
        ///<summary> 
    	/// 列名常量，微信预支付ID
        ///</summary>
		public const string PREPAYID="PrepayId";
		
        ///<summary> 
    	/// 列名常量，回调时间
        ///</summary>
		public const string CALLBACKTIME="CallbackTime";
		
        ///<summary> 
    	/// 列名常量，微信端的交易号
        ///</summary>
		public const string TRANID="TranId";
		
        ///<summary> 
    	/// 列名常量，支付状态，0：未发起支付，1：已发起支付，2：支付成功，3：支付失败，4：支付错误。
        ///</summary>
		public const string PAYSTATUS="PayStatus";
		
        ///<summary> 
    	/// 列名常量，微信OPENID
        ///</summary>
		public const string REQUESTTIME="RequestTime";
		
        ///<summary> 
    	/// 列名常量，对账状态，0：未对账，1：发起对账，2：对账成功，3：对账失败，4：对账错误。
        ///</summary>
		public const string CHECKSTATUS="CheckStatus";
		
        ///<summary> 
    	/// 列名常量，对账时间
        ///</summary>
		public const string CHECKTIME="CheckTime";
		
        ///<summary> 
    	/// 列名常量，客户端IP地址
        ///</summary>
		public const string REQUESTIP="RequestIp";
		
        ///<summary> 
    	/// 列名常量，扩展位选项值
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
			get { return WXPAYREQUEST; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public WxPayRequestSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public WxPayRequestSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public WxPayRequestSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 业务对象类型
        ///</summary>
		public string ObjType { get { return OBJTYPE; } }
		
        ///<summary> 
    	/// 业务对象编号
        ///</summary>
		public string ObjId { get { return OBJID; } }
		
        ///<summary> 
    	/// 商户号
        ///</summary>
		public string MerchantId { get { return MERCHANTID; } }
		
        ///<summary> 
    	/// 交易类型
        ///</summary>
		public string TradeType { get { return TRADETYPE; } }
		
        ///<summary> 
    	/// 付款金额
        ///</summary>
		public string PayAmount { get { return PAYAMOUNT; } }
		
        ///<summary> 
    	/// 支付商品
        ///</summary>
		public string PayBody { get { return PAYBODY; } }
		
        ///<summary> 
    	/// 微信OPENID
        ///</summary>
		public string OpenId { get { return OPENID; } }
		
        ///<summary> 
    	/// 微信预支付ID
        ///</summary>
		public string PrepayId { get { return PREPAYID; } }
		
        ///<summary> 
    	/// 回调时间
        ///</summary>
		public string CallbackTime { get { return CALLBACKTIME; } }
		
        ///<summary> 
    	/// 微信端的交易号
        ///</summary>
		public string TranId { get { return TRANID; } }
		
        ///<summary> 
    	/// 支付状态，0：未发起支付，1：已发起支付，2：支付成功，3：支付失败，4：支付错误。
        ///</summary>
		public string PayStatus { get { return PAYSTATUS; } }
		
        ///<summary> 
    	/// 微信OPENID
        ///</summary>
		public string RequestTime { get { return REQUESTTIME; } }
		
        ///<summary> 
    	/// 对账状态，0：未对账，1：发起对账，2：对账成功，3：对账失败，4：对账错误。
        ///</summary>
		public string CheckStatus { get { return CHECKSTATUS; } }
		
        ///<summary> 
    	/// 对账时间
        ///</summary>
		public string CheckTime { get { return CHECKTIME; } }
		
        ///<summary> 
    	/// 客户端IP地址
        ///</summary>
		public string RequestIp { get { return REQUESTIP; } }
		
        ///<summary> 
    	/// 扩展位选项值
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
    	/// 业务对象类型
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetObjType(bool needPrefix=true)
        {
            return this.QuoteField(OBJTYPE,needPrefix);
        }
		
        ///<summary> 
    	/// 业务对象编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetObjId(bool needPrefix=true)
        {
            return this.QuoteField(OBJID,needPrefix);
        }
		
        ///<summary> 
    	/// 商户号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetMerchantId(bool needPrefix=true)
        {
            return this.QuoteField(MERCHANTID,needPrefix);
        }
		
        ///<summary> 
    	/// 交易类型
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetTradeType(bool needPrefix=true)
        {
            return this.QuoteField(TRADETYPE,needPrefix);
        }
		
        ///<summary> 
    	/// 付款金额
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetPayAmount(bool needPrefix=true)
        {
            return this.QuoteField(PAYAMOUNT,needPrefix);
        }
		
        ///<summary> 
    	/// 支付商品
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetPayBody(bool needPrefix=true)
        {
            return this.QuoteField(PAYBODY,needPrefix);
        }
		
        ///<summary> 
    	/// 微信OPENID
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetOpenId(bool needPrefix=true)
        {
            return this.QuoteField(OPENID,needPrefix);
        }
		
        ///<summary> 
    	/// 微信预支付ID
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetPrepayId(bool needPrefix=true)
        {
            return this.QuoteField(PREPAYID,needPrefix);
        }
		
        ///<summary> 
    	/// 回调时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCallbackTime(bool needPrefix=true)
        {
            return this.QuoteField(CALLBACKTIME,needPrefix);
        }
		
        ///<summary> 
    	/// 微信端的交易号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetTranId(bool needPrefix=true)
        {
            return this.QuoteField(TRANID,needPrefix);
        }
		
        ///<summary> 
    	/// 支付状态，0：未发起支付，1：已发起支付，2：支付成功，3：支付失败，4：支付错误。
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetPayStatus(bool needPrefix=true)
        {
            return this.QuoteField(PAYSTATUS,needPrefix);
        }
		
        ///<summary> 
    	/// 微信OPENID
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetRequestTime(bool needPrefix=true)
        {
            return this.QuoteField(REQUESTTIME,needPrefix);
        }
		
        ///<summary> 
    	/// 对账状态，0：未对账，1：发起对账，2：对账成功，3：对账失败，4：对账错误。
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCheckStatus(bool needPrefix=true)
        {
            return this.QuoteField(CHECKSTATUS,needPrefix);
        }
		
        ///<summary> 
    	/// 对账时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCheckTime(bool needPrefix=true)
        {
            return this.QuoteField(CHECKTIME,needPrefix);
        }
		
        ///<summary> 
    	/// 客户端IP地址
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetRequestIp(bool needPrefix=true)
        {
            return this.QuoteField(REQUESTIP,needPrefix);
        }
		
        ///<summary> 
    	/// 扩展位选项值
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
