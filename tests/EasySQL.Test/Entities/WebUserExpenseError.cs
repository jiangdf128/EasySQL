using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WebUserExpenseError")]
	public class WebUserExpenseError
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
		public int Id {get;set;}
		
        ///<summary> 
    	/// 消费编号
        ///</summary>
		public long ExpenseId {get;set;}
		
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
