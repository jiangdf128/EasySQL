using EasySQL;

namespace EasySQL.Test
{
	public class WebUserExpenseSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string WEBUSEREXPENSE="WebUserExpense";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，用户编号
        ///</summary>
		public const string USERID="UserId";
		
        ///<summary> 
    	/// 列名常量，核查编号
        ///</summary>
		public const string VERIFYID="VerifyId";
		
        ///<summary> 
    	/// 列名常量，发生时间
        ///</summary>
		public const string OCCURTIME="OccurTime";
		
        ///<summary> 
    	/// 列名常量，核查的产品编号
        ///</summary>
		public const string PRODUCTID="ProductId";
		
        ///<summary> 
    	/// 列名常量，价格
        ///</summary>
		public const string PRICE="Price";
		
        ///<summary> 
    	/// 列名常量，消费发生金额
        ///</summary>
		public const string BALANCEAMOUNT="BalanceAmount";
		
        ///<summary> 
    	/// 列名常量，账户锁定的金额
        ///</summary>
		public const string LOCKEDBALANCE="LockedBalance";
		
        ///<summary> 
    	/// 列名常量，第三方数据服务流水号
        ///</summary>
		public const string SERVICEBILL="ServiceBill";
		
        ///<summary> 
    	/// 列名常量，消费扣款对应的流水帐号
        ///</summary>
		public const string JOURNALID="JournalId";
		
        ///<summary> 
    	/// 列名常量，数据类型
        ///</summary>
		public const string DATATYPE="DataType";
		
        ///<summary> 
    	/// 列名常量，数据编号
        ///</summary>
		public const string DATAID="DataId";
		
        ///<summary> 
    	/// 列名常量，摘要
        ///</summary>
		public const string SUMMARY="Summary";
		
        ///<summary> 
    	/// 列名常量，状态，1：核查中，2：成功，3：失败
        ///</summary>
		public const string STATUS="Status";
		
        ///<summary> 
    	/// 列名常量，扩展位选项
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
			get { return WEBUSEREXPENSE; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public WebUserExpenseSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public WebUserExpenseSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public WebUserExpenseSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 用户编号
        ///</summary>
		public string UserId { get { return USERID; } }
		
        ///<summary> 
    	/// 核查编号
        ///</summary>
		public string VerifyId { get { return VERIFYID; } }
		
        ///<summary> 
    	/// 发生时间
        ///</summary>
		public string OccurTime { get { return OCCURTIME; } }
		
        ///<summary> 
    	/// 核查的产品编号
        ///</summary>
		public string ProductId { get { return PRODUCTID; } }
		
        ///<summary> 
    	/// 价格
        ///</summary>
		public string Price { get { return PRICE; } }
		
        ///<summary> 
    	/// 消费发生金额
        ///</summary>
		public string BalanceAmount { get { return BALANCEAMOUNT; } }
		
        ///<summary> 
    	/// 账户锁定的金额
        ///</summary>
		public string LockedBalance { get { return LOCKEDBALANCE; } }
		
        ///<summary> 
    	/// 第三方数据服务流水号
        ///</summary>
		public string ServiceBill { get { return SERVICEBILL; } }
		
        ///<summary> 
    	/// 消费扣款对应的流水帐号
        ///</summary>
		public string JournalId { get { return JOURNALID; } }
		
        ///<summary> 
    	/// 数据类型
        ///</summary>
		public string DataType { get { return DATATYPE; } }
		
        ///<summary> 
    	/// 数据编号
        ///</summary>
		public string DataId { get { return DATAID; } }
		
        ///<summary> 
    	/// 摘要
        ///</summary>
		public string Summary { get { return SUMMARY; } }
		
        ///<summary> 
    	/// 状态，1：核查中，2：成功，3：失败
        ///</summary>
		public string Status { get { return STATUS; } }
		
        ///<summary> 
    	/// 扩展位选项
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
    	/// 用户编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetUserId(bool needPrefix=true)
        {
            return this.QuoteField(USERID,needPrefix);
        }
		
        ///<summary> 
    	/// 核查编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetVerifyId(bool needPrefix=true)
        {
            return this.QuoteField(VERIFYID,needPrefix);
        }
		
        ///<summary> 
    	/// 发生时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetOccurTime(bool needPrefix=true)
        {
            return this.QuoteField(OCCURTIME,needPrefix);
        }
		
        ///<summary> 
    	/// 核查的产品编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetProductId(bool needPrefix=true)
        {
            return this.QuoteField(PRODUCTID,needPrefix);
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
    	/// 消费发生金额
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetBalanceAmount(bool needPrefix=true)
        {
            return this.QuoteField(BALANCEAMOUNT,needPrefix);
        }
		
        ///<summary> 
    	/// 账户锁定的金额
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLockedBalance(bool needPrefix=true)
        {
            return this.QuoteField(LOCKEDBALANCE,needPrefix);
        }
		
        ///<summary> 
    	/// 第三方数据服务流水号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetServiceBill(bool needPrefix=true)
        {
            return this.QuoteField(SERVICEBILL,needPrefix);
        }
		
        ///<summary> 
    	/// 消费扣款对应的流水帐号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetJournalId(bool needPrefix=true)
        {
            return this.QuoteField(JOURNALID,needPrefix);
        }
		
        ///<summary> 
    	/// 数据类型
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetDataType(bool needPrefix=true)
        {
            return this.QuoteField(DATATYPE,needPrefix);
        }
		
        ///<summary> 
    	/// 数据编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetDataId(bool needPrefix=true)
        {
            return this.QuoteField(DATAID,needPrefix);
        }
		
        ///<summary> 
    	/// 摘要
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetSummary(bool needPrefix=true)
        {
            return this.QuoteField(SUMMARY,needPrefix);
        }
		
        ///<summary> 
    	/// 状态，1：核查中，2：成功，3：失败
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetStatus(bool needPrefix=true)
        {
            return this.QuoteField(STATUS,needPrefix);
        }
		
        ///<summary> 
    	/// 扩展位选项
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
