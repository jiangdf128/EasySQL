using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WebUserDeposit")]
	public class WebUserDeposit
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
		public long Id {get;set;}
		
        ///<summary> 
    	/// 用户账户编号
        ///</summary>
		public int AccountUserId {get;set;}
		
        ///<summary> 
    	/// 操作用户编号（若为子账户操作则为子用户编号）
        ///</summary>
		public int OperateUserId {get;set;}
		
        ///<summary> 
    	/// 充值发生时间
        ///</summary>
		public DateTime OccurTime {get;set;}
		
        ///<summary> 
    	/// 所使用的充值产品编号（如10元面额，30元面额）
        ///</summary>
		public int? DepositProductId {get;set;}
		
        ///<summary> 
    	/// 充值对应的公司收款账号编号
        ///</summary>
		public int? CompanyAccountId {get;set;}
		
        ///<summary> 
    	/// 充值的付款金额（实际金额）
        ///</summary>
		public decimal MoneyAmount {get;set;}
		
        ///<summary> 
    	/// 充入用户账户的金额数
        ///</summary>
		public decimal BalanceAmount {get;set;}
		
        ///<summary> 
    	/// 支付方式，1：微信，2：支付宝，3：银联
        ///</summary>
		public short PayMethodId {get;set;}
		
        ///<summary> 
    	/// 第三方支付平台的支付流水号
        ///</summary>
		public string PayPlatformBill {get;set;}
		
        ///<summary> 
    	/// 支付人姓名
        ///</summary>
		public string PayerName {get;set;}
		
        ///<summary> 
    	/// 支付人账号
        ///</summary>
		public string PayerAccount {get;set;}
		
        ///<summary> 
    	/// 支付人的OpenId（如：微信公众号的用户OpenID）
        ///</summary>
		public string PayerOpenId {get;set;}
		
        ///<summary> 
    	/// 充值成功对应的账户流水账编号
        ///</summary>
		public long? JournalId {get;set;}
		
        ///<summary> 
    	/// 摘要
        ///</summary>
		public string Summary {get;set;}
		
        ///<summary> 
    	/// 登录日志编号
        ///</summary>
		public long? LoginLogId {get;set;}
		
        ///<summary> 
    	/// 客户端IP地址
        ///</summary>
		public string ClientIp {get;set;}
		
        ///<summary> 
    	/// 充值客户端所在的纬度
        ///</summary>
		public decimal? Lat {get;set;}
		
        ///<summary> 
    	/// 充值客户端所在的经度
        ///</summary>
		public decimal? Lng {get;set;}
		
        ///<summary> 
    	/// 充值状态，1：充值中，2：充值成功，3：充值失败
        ///</summary>
		public short Status {get;set;}
		
        ///<summary> 
    	/// 扩展位选项
        ///</summary>
		public int ExtOption {get;set;}
		
        ///<summary> 
    	/// 创建时间
        ///</summary>
		public DateTime CreateTime {get;set;}
		
        ///<summary> 
    	/// 版本号
        ///</summary>
		public int Version {get;set;}
		
    }
}
