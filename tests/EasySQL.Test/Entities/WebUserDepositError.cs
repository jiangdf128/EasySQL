using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WebUserDepositError")]
	public class WebUserDepositError
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
		public int Id {get;set;}
		
        ///<summary> 
    	/// 充值编号
        ///</summary>
		public long DepositId {get;set;}
		
        ///<summary> 
    	/// 错误代码
        ///</summary>
		public string ErrorCode {get;set;}
		
        ///<summary> 
    	/// 错误消息
        ///</summary>
		public string ErrorMessage {get;set;}
		
        ///<summary> 
    	/// 创建时间
        ///</summary>
		public DateTime CreateTime {get;set;}
		
    }
}
