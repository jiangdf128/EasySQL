using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WebUserWx")]
	public class WebUserWx
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
		public int Id {get;set;}
		
        ///<summary> 
    	/// 用户ID
        ///</summary>
		public int UserId {get;set;}
		
        ///<summary> 
    	/// 微信OpenID
        ///</summary>
		public string OpenId {get;set;}
		
        ///<summary> 
    	/// 创建时间
        ///</summary>
		public DateTime CreateTime {get;set;}
		
    }
}
