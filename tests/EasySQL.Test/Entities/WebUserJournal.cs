using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WebUserJournal")]
	public class WebUserJournal
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
    	/// 发生时间
        ///</summary>
		public DateTime OccurTime {get;set;}
		
        ///<summary> 
    	/// 科目编号
        ///</summary>
		public short SubjectId {get;set;}
		
        ///<summary> 
    	/// 金额
        ///</summary>
		public decimal Amount {get;set;}
		
        ///<summary> 
    	/// 发生业务的对象类型
        ///</summary>
		public short? BusinessType {get;set;}
		
        ///<summary> 
    	/// 发生业务的对象编号
        ///</summary>
		public long? BusinessId {get;set;}
		
        ///<summary> 
    	/// 摘要
        ///</summary>
		public string Summary {get;set;}
		
        ///<summary> 
    	/// 扩展位选项
        ///</summary>
		public int ExtOption {get;set;}
		
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
