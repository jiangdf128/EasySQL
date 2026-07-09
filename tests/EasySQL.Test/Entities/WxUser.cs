using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WxUsers")]
	public class WxUser
	{        
        ///<summary> 
    	/// 微信OpenID
        ///</summary>
        [ExplicitKey]
		public string OpenId {get;set;}
		
        ///<summary> 
    	/// 昵称
        ///</summary>
		public string NickName {get;set;}
		
        ///<summary> 
    	/// 性别
        ///</summary>
		public short? Gender {get;set;}
		
        ///<summary> 
    	/// 国家
        ///</summary>
		public string Country {get;set;}
		
        ///<summary> 
    	/// 省份
        ///</summary>
		public string Province {get;set;}
		
        ///<summary> 
    	/// 城市
        ///</summary>
		public string City {get;set;}
		
        ///<summary> 
    	/// 头像照URL地址
        ///</summary>
		public string HeadImgUrl {get;set;}
		
        ///<summary> 
    	/// 特权
        ///</summary>
		public string Privilege {get;set;}
		
        ///<summary> 
    	/// UnionID
        ///</summary>
		public string UnionId {get;set;}
		
        ///<summary> 
    	/// 关注状态，0：未关注，1：关注，2：取消关注
        ///</summary>
		public short? BindStatus {get;set;}
		
        ///<summary> 
    	/// 关注时间
        ///</summary>
		public DateTime? BindTime {get;set;}
		
        ///<summary> 
    	/// 取消关注时间
        ///</summary>
		public DateTime? UnbindTime {get;set;}
		
        ///<summary> 
    	/// 扩展位选项
        ///</summary>
		public int ExtOption {get;set;}
		
        ///<summary> 
    	/// 创建时间
        ///</summary>
		public DateTime CreateTime {get;set;}
		
    }
}
