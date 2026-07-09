using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("SmsGateway")]
	public class SmsGateway
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
        [ExplicitKey]
		public short Id {get;set;}
		
        ///<summary> 
    	/// 网关名称
        ///</summary>
		public string Name {get;set;}
		
        ///<summary> 
    	/// API调用用户ID
        ///</summary>
		public string AccessUid {get;set;}
		
        ///<summary> 
    	/// API调用密钥
        ///</summary>
		public string AccessSecret {get;set;}
		
        ///<summary> 
    	/// 网关的网址
        ///</summary>
		public string GatewayUrl {get;set;}
		
        ///<summary> 
    	/// 实现短信接口所在的程序包
        ///</summary>
		public string PackageFile {get;set;}
		
        ///<summary> 
    	/// 实现短信接口的类名
        ///</summary>
		public string ClassName {get;set;}
		
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
