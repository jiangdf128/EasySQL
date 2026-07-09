using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("DepositProducts")]
	public class DepositProduct
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
		public int Id {get;set;}
		
        ///<summary> 
    	/// 充值产品名
        ///</summary>
		public string Name {get;set;}
		
        ///<summary> 
    	/// 产品介绍
        ///</summary>
		public string Description {get;set;}
		
        ///<summary> 
    	/// 排序行号
        ///</summary>
		public int Line {get;set;}
		
        ///<summary> 
    	/// 价格
        ///</summary>
		public decimal Price {get;set;}
		
        ///<summary> 
    	/// 实际充值额
        ///</summary>
		public decimal DepositAmount {get;set;}
		
        ///<summary> 
    	/// 生效时间，从何时开始将出现在充值页面
        ///</summary>
		public DateTime? EffectTime {get;set;}
		
        ///<summary> 
    	/// 库存数，若为null，则不启用库存限制
        ///</summary>
		public int? StockAmount {get;set;}
		
        ///<summary> 
    	/// 锁定的库存数
        ///</summary>
		public int? LockStockAmount {get;set;}
		
        ///<summary> 
    	/// 折扣到期时间
        ///</summary>
		public DateTime? ExpireTime {get;set;}
		
        ///<summary> 
    	/// 状态，0：下架，1：上架
        ///</summary>
		public short Status {get;set;}
		
        ///<summary> 
    	/// 位扩展选项
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
