using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("SmsVerifyCodes")]
	public class SmsVerifyCode
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
		public long Id {get;set;}
		
        ///<summary> 
    	/// 手机隶属国家
        ///</summary>
		public int CountryId {get;set;}
		
        ///<summary> 
    	/// 手机号
        ///</summary>
		public string Mobile {get;set;}
		
        ///<summary> 
    	/// 验证码
        ///</summary>
		public string VerifyCode {get;set;}
		
        ///<summary> 
    	/// 验证码类型，1：登录验证码，2：产品查询验证码
        ///</summary>
		public short CodeType {get;set;}
		
        ///<summary> 
    	/// 发送验证码的短信网关编号
        ///</summary>
		public short GatewayId {get;set;}
		
        ///<summary> 
    	/// 发送状态，1：发送中，2：发送成功，3：发送失败
        ///</summary>
		public short SendStatus {get;set;}
		
        ///<summary> 
    	/// 短信发送时间
        ///</summary>
		public DateTime? SendTime {get;set;}
		
        ///<summary> 
    	/// 验证码的到期时间
        ///</summary>
		public DateTime ExpireTime {get;set;}
		
        ///<summary> 
    	/// 申请发送验证码的客户端IP地址
        ///</summary>
		public string ClientIp {get;set;}
		
        ///<summary> 
    	/// 验证码是否已使用
        ///</summary>
		public bool? IsUsed {get;set;}
		
        ///<summary> 
    	/// 验证码被使用的时间
        ///</summary>
		public DateTime? UsedTime {get;set;}
		
        ///<summary> 
    	/// 使用验证码的客户端IP地址
        ///</summary>
		public string UsedIp {get;set;}
		
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
