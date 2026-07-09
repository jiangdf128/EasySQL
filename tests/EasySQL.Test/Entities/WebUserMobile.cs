using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WebUserMobiles")]
	public class WebUserMobile
	{        
        ///<summary> 
    	/// 用户编号
        ///</summary>
        [ExplicitKey]
		public int Id {get;set;}
		
        ///<summary> 
    	/// 手机所属国家。默认为中国，以配合手机号的唯一性。预留，以扩展作为国外手机短信登录使用。
        ///</summary>
		public int CountryId {get;set;}
		
        ///<summary> 
    	/// 手机号
        ///</summary>
		public string Mobile {get;set;}
		
    }
}
