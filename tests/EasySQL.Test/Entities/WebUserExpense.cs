using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WebUserExpense")]
	public class WebUserExpense
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
    	/// 核查编号
        ///</summary>
		public long VerifyId {get;set;}
		
        ///<summary> 
    	/// 发生时间
        ///</summary>
		public DateTime OccurTime {get;set;}
		
        ///<summary> 
    	/// 核查的产品编号
        ///</summary>
		public long ProductId {get;set;}
		
        ///<summary> 
    	/// 价格
        ///</summary>
		public decimal Price {get;set;}
		
        ///<summary> 
    	/// 消费发生金额
        ///</summary>
		public decimal BalanceAmount {get;set;}
		
        ///<summary> 
    	/// 账户锁定的金额
        ///</summary>
		public decimal LockedBalance {get;set;}
		
        ///<summary> 
    	/// 第三方数据服务流水号
        ///</summary>
		public string ServiceBill {get;set;}
		
        ///<summary> 
    	/// 消费扣款对应的流水帐号
        ///</summary>
		public long? JournalId {get;set;}
		
        ///<summary> 
    	/// 数据类型
        ///</summary>
		public short? DataType {get;set;}
		
        ///<summary> 
    	/// 数据编号
        ///</summary>
		public long? DataId {get;set;}
		
        ///<summary> 
    	/// 摘要
        ///</summary>
		public string Summary {get;set;}
		
        ///<summary> 
    	/// 状态，1：核查中，2：成功，3：失败
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
		
        ///<summary> 
    	/// 版本号
        ///</summary>
		public int Version {get;set;}
		
    }
}
