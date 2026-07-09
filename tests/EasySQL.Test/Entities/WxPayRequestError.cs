using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WxPayRequestError")]
	public class WxPayRequestError
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
		public long Id {get;set;}
		
        ///<summary> 
    	/// 微信支付请求号
        ///</summary>
		public long RequestId {get;set;}
		
        ///<summary> 
    	/// 错误类，1：微信支付请求错误，2：微信支付对账错误
        ///</summary>
		public short ErrorType {get;set;}
		
        ///<summary> 
    	/// 错误代码
        ///</summary>
		public string ErrorCode {get;set;}
		
        ///<summary> 
    	/// 错误消息
        ///</summary>
		public string ErrorMessage {get;set;}
		
        ///<summary> 
    	/// 创建时间
        ///</summary>
		public DateTime CreateTime {get;set;}
		
    }
}
