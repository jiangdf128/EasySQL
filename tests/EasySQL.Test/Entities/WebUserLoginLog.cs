using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WebUserLoginLog")]
	public class WebUserLoginLog
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
		public long Id {get;set;}
		
        ///<summary> 
    	/// 用户编号
        ///</summary>
		public int UserId {get;set;}
		
        ///<summary> 
    	/// 会话Guid号
        ///</summary>
		public string SessionGuid {get;set;}
		
        ///<summary> 
    	/// Cookies Guid号
        ///</summary>
		public string CookiesGuid {get;set;}
		
        ///<summary> 
    	/// 登录入口类型，1：web，2：App，3：小程序
        ///</summary>
		public short? TerminalType {get;set;}
		
        ///<summary> 
    	/// 终端设备名称
        ///</summary>
		public string TerminalName {get;set;}
		
        ///<summary> 
    	/// 终端设备系统类型及版本信息
        ///</summary>
		public string TerminalOs {get;set;}
		
        ///<summary> 
    	/// 登录的IP地址
        ///</summary>
		public string LoginIp {get;set;}
		
        ///<summary> 
    	/// 登录所在地纬度
        ///</summary>
		public decimal? Lat {get;set;}
		
        ///<summary> 
    	/// 登录所在地经度
        ///</summary>
		public decimal? Lng {get;set;}
		
        ///<summary> 
    	/// 登录时间
        ///</summary>
		public DateTime LoginTime {get;set;}
		
        ///<summary> 
    	/// 安全退出时间
        ///</summary>
		public DateTime? LogoutTime {get;set;}
		
        ///<summary> 
    	/// 最后一次心跳活动时间
        ///</summary>
		public DateTime? LastActiveTime {get;set;}
		
        ///<summary> 
    	/// 版本号
        ///</summary>
		public int Version {get;set;}
		
    }
}
