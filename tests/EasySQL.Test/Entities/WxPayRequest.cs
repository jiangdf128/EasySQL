using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WxPayRequest")]
	public class WxPayRequest
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
		public long Id {get;set;}
		
        ///<summary> 
    	/// 业务对象类型
        ///</summary>
		public short ObjType {get;set;}
		
        ///<summary> 
    	/// 业务对象编号
        ///</summary>
		public long ObjId {get;set;}
		
        ///<summary> 
    	/// 商户号
        ///</summary>
		public string MerchantId {get;set;}
		
        ///<summary> 
    	/// 交易类型
        ///</summary>
		public short TradeType {get;set;}
		
        ///<summary> 
    	/// 付款金额
        ///</summary>
		public decimal PayAmount {get;set;}
		
        ///<summary> 
    	/// 支付商品
        ///</summary>
		public string PayBody {get;set;}
		
        ///<summary> 
    	/// 微信OPENID
        ///</summary>
		public string OpenId {get;set;}
		
        ///<summary> 
    	/// 微信预支付ID
        ///</summary>
		public string PrepayId {get;set;}
		
        ///<summary> 
    	/// 回调时间
        ///</summary>
		public DateTime? CallbackTime {get;set;}
		
        ///<summary> 
    	/// 微信端的交易号
        ///</summary>
		public string TranId {get;set;}
		
        ///<summary> 
    	/// 支付状态，0：未发起支付，1：已发起支付，2：支付成功，3：支付失败，4：支付错误。
        ///</summary>
		public short PayStatus {get;set;}
		
        ///<summary> 
    	/// 微信OPENID
        ///</summary>
		public DateTime RequestTime {get;set;}
		
        ///<summary> 
    	/// 对账状态，0：未对账，1：发起对账，2：对账成功，3：对账失败，4：对账错误。
        ///</summary>
		public short CheckStatus {get;set;}
		
        ///<summary> 
    	/// 对账时间
        ///</summary>
		public DateTime? CheckTime {get;set;}
		
        ///<summary> 
    	/// 客户端IP地址
        ///</summary>
		public string RequestIp {get;set;}
		
        ///<summary> 
    	/// 扩展位选项值
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
