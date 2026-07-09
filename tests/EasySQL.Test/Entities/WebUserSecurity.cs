using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WebUserSecurity")]
	public class WebUserSecurity
	{        
        ///<summary> 
    	/// 用户编号
        ///</summary>
        [ExplicitKey]
		public int Id {get;set;}
		
        ///<summary> 
    	/// 用户名
        ///</summary>
		public string LoginName {get;set;}
		
        ///<summary> 
    	/// 登录密码
        ///</summary>
		public string LoginPassword {get;set;}
		
        ///<summary> 
    	/// Email地址
        ///</summary>
		public string Email {get;set;}
		
        ///<summary> 
    	/// Email地址是否经验证确认
        ///</summary>
		public bool? IsEmailConfirmed {get;set;}
		
        ///<summary> 
    	/// 是否子用户
        ///</summary>
		public bool IsChildUser {get;set;}
		
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
