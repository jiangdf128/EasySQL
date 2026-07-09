using EasySQL;

namespace EasySQL.Test
{
	public class DepositProductSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string DEPOSITPRODUCTS="DepositProducts";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，充值产品名
        ///</summary>
		public const string NAME="Name";
		
        ///<summary> 
    	/// 列名常量，产品介绍
        ///</summary>
		public const string DESCRIPTION="Description";
		
        ///<summary> 
    	/// 列名常量，排序行号
        ///</summary>
		public const string LINE="Line";
		
        ///<summary> 
    	/// 列名常量，价格
        ///</summary>
		public const string PRICE="Price";
		
        ///<summary> 
    	/// 列名常量，实际充值额
        ///</summary>
		public const string DEPOSITAMOUNT="DepositAmount";
		
        ///<summary> 
    	/// 列名常量，生效时间，从何时开始将出现在充值页面
        ///</summary>
		public const string EFFECTTIME="EffectTime";
		
        ///<summary> 
    	/// 列名常量，库存数，若为null，则不启用库存限制
        ///</summary>
		public const string STOCKAMOUNT="StockAmount";
		
        ///<summary> 
    	/// 列名常量，锁定的库存数
        ///</summary>
		public const string LOCKSTOCKAMOUNT="LockStockAmount";
		
        ///<summary> 
    	/// 列名常量，折扣到期时间
        ///</summary>
		public const string EXPIRETIME="ExpireTime";
		
        ///<summary> 
    	/// 列名常量，状态，0：下架，1：上架
        ///</summary>
		public const string STATUS="Status";
		
        ///<summary> 
    	/// 列名常量，位扩展选项
        ///</summary>
		public const string EXTOPTION="ExtOption";
		
        ///<summary> 
    	/// 列名常量，创建时间
        ///</summary>
		public const string CREATETIME="CreateTime";
		
        ///<summary> 
    	/// 列名常量，版本号
        ///</summary>
		public const string VERSION="Version";
		
        #endregion
    
    	#region 实现SchemaBase抽象类接口。
		/// <summary>
        /// 获取表（或视图）架构的名称。
        /// </summary>
        /// <remarks>该名称与数据库里面的名称对应。</remarks>
		public override string TableName
		{
			get { return DEPOSITPRODUCTS; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public DepositProductSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public DepositProductSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public DepositProductSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 充值产品名
        ///</summary>
		public string Name { get { return NAME; } }
		
        ///<summary> 
    	/// 产品介绍
        ///</summary>
		public string Description { get { return DESCRIPTION; } }
		
        ///<summary> 
    	/// 排序行号
        ///</summary>
		public string Line { get { return LINE; } }
		
        ///<summary> 
    	/// 价格
        ///</summary>
		public string Price { get { return PRICE; } }
		
        ///<summary> 
    	/// 实际充值额
        ///</summary>
		public string DepositAmount { get { return DEPOSITAMOUNT; } }
		
        ///<summary> 
    	/// 生效时间，从何时开始将出现在充值页面
        ///</summary>
		public string EffectTime { get { return EFFECTTIME; } }
		
        ///<summary> 
    	/// 库存数，若为null，则不启用库存限制
        ///</summary>
		public string StockAmount { get { return STOCKAMOUNT; } }
		
        ///<summary> 
    	/// 锁定的库存数
        ///</summary>
		public string LockStockAmount { get { return LOCKSTOCKAMOUNT; } }
		
        ///<summary> 
    	/// 折扣到期时间
        ///</summary>
		public string ExpireTime { get { return EXPIRETIME; } }
		
        ///<summary> 
    	/// 状态，0：下架，1：上架
        ///</summary>
		public string Status { get { return STATUS; } }
		
        ///<summary> 
    	/// 位扩展选项
        ///</summary>
		public string ExtOption { get { return EXTOPTION; } }
		
        ///<summary> 
    	/// 创建时间
        ///</summary>
		public string CreateTime { get { return CREATETIME; } }
		
        ///<summary> 
    	/// 版本号
        ///</summary>
		public string Version { get { return VERSION; } }
		
        #endregion
        
        #region 格式化列名称访问方法
        ///<summary> 
    	/// 编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetId(bool needPrefix=true)
        {
            return this.QuoteField(ID,needPrefix);
        }
		
        ///<summary> 
    	/// 充值产品名
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetName(bool needPrefix=true)
        {
            return this.QuoteField(NAME,needPrefix);
        }
		
        ///<summary> 
    	/// 产品介绍
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetDescription(bool needPrefix=true)
        {
            return this.QuoteField(DESCRIPTION,needPrefix);
        }
		
        ///<summary> 
    	/// 排序行号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLine(bool needPrefix=true)
        {
            return this.QuoteField(LINE,needPrefix);
        }
		
        ///<summary> 
    	/// 价格
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetPrice(bool needPrefix=true)
        {
            return this.QuoteField(PRICE,needPrefix);
        }
		
        ///<summary> 
    	/// 实际充值额
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetDepositAmount(bool needPrefix=true)
        {
            return this.QuoteField(DEPOSITAMOUNT,needPrefix);
        }
		
        ///<summary> 
    	/// 生效时间，从何时开始将出现在充值页面
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetEffectTime(bool needPrefix=true)
        {
            return this.QuoteField(EFFECTTIME,needPrefix);
        }
		
        ///<summary> 
    	/// 库存数，若为null，则不启用库存限制
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetStockAmount(bool needPrefix=true)
        {
            return this.QuoteField(STOCKAMOUNT,needPrefix);
        }
		
        ///<summary> 
    	/// 锁定的库存数
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLockStockAmount(bool needPrefix=true)
        {
            return this.QuoteField(LOCKSTOCKAMOUNT,needPrefix);
        }
		
        ///<summary> 
    	/// 折扣到期时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetExpireTime(bool needPrefix=true)
        {
            return this.QuoteField(EXPIRETIME,needPrefix);
        }
		
        ///<summary> 
    	/// 状态，0：下架，1：上架
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetStatus(bool needPrefix=true)
        {
            return this.QuoteField(STATUS,needPrefix);
        }
		
        ///<summary> 
    	/// 位扩展选项
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetExtOption(bool needPrefix=true)
        {
            return this.QuoteField(EXTOPTION,needPrefix);
        }
		
        ///<summary> 
    	/// 创建时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCreateTime(bool needPrefix=true)
        {
            return this.QuoteField(CREATETIME,needPrefix);
        }
		
        ///<summary> 
    	/// 版本号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetVersion(bool needPrefix=true)
        {
            return this.QuoteField(VERSION,needPrefix);
        }
		
        #endregion
    }
}
