using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("SmsTemplates")]
	public class SmsTemplate
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
        [ExplicitKey]
		public int Id {get;set;}
		
        ///<summary> 
    	/// 网关ID
        ///</summary>
		public short GatewayId {get;set;}
		
        ///<summary> 
    	/// 短消息模板名称
        ///</summary>
		public string TemplateName {get;set;}
		
        ///<summary> 
    	/// 短消息模板代码（阿里云用）
        ///</summary>
		public string TemplateCode {get;set;}
		
        ///<summary> 
    	/// 是否为模板消息
        ///</summary>
		public bool? IsTemplateMsg {get;set;}
		
        ///<summary> 
    	/// 短消息模板的内容（或Json格式模板）
        ///</summary>
		public string LocalTemplateText {get;set;}
		
        ///<summary> 
    	/// 短信签名
        ///</summary>
		public string SignName {get;set;}
		
		public DateTime CreateTime {get;set;}
		
		public int Version {get;set;}
		
    }
}
