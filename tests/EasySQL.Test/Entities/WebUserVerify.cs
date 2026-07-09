using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WebUserVerify")]
	public class WebUserVerify
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
		public long Id {get;set;}
		
        ///<summary> 
    	/// 用户账号编号
        ///</summary>
		public int AccountUserId {get;set;}
		
        ///<summary> 
    	/// 操作用户编号
        ///</summary>
		public int OperateUserId {get;set;}
		
        ///<summary> 
    	/// 被核查人员姓名
        ///</summary>
		public string VerifiedName {get;set;}
		
        ///<summary> 
    	/// 被核查人员身份证号
        ///</summary>
		public string VerifiedIdNumber {get;set;}
		
        ///<summary> 
    	/// 核查的产品，bit位值，如：1：核查身份证，2：核查犯罪记录
        ///</summary>
		public long? Products {get;set;}
		
        ///<summary> 
    	/// 登录日志编号
        ///</summary>
		public long? LoginLogId {get;set;}
		
        ///<summary> 
    	/// 核查客户端的IP地址
        ///</summary>
		public string ClientIp {get;set;}
		
        ///<summary> 
    	/// 核查客户端所在的纬度
        ///</summary>
		public decimal? Lat {get;set;}
		
        ///<summary> 
    	/// 核查客户端所在的经度
        ///</summary>
		public decimal? Lng {get;set;}
		
        ///<summary> 
    	/// 核查状态，1：核查中，2：成功，3：失败
        ///</summary>
		public short Status {get;set;}
		
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
