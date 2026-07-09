using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WebUserKey")]
	public class WebUserKey
	{        
        ///<summary> 
    	/// 用户ID
        ///</summary>
        [ExplicitKey]
		public int UserId {get;set;}
		
        ///<summary> 
    	/// 用户私钥
        ///</summary>
		public string PrivateKey {get;set;}
		
        ///<summary> 
    	/// 用户公钥
        ///</summary>
		public string PublicKey {get;set;}
		
        ///<summary> 
    	/// 创建时间
        ///</summary>
		public DateTime CreateTime {get;set;}
		
    }
}
