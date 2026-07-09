using EasySQL;

namespace EasySQL.Test
{
	public class SmsTemplateSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string SMSTEMPLATES="SmsTemplates";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，网关ID
        ///</summary>
		public const string GATEWAYID="GatewayId";
		
        ///<summary> 
    	/// 列名常量，短消息模板名称
        ///</summary>
		public const string TEMPLATENAME="TemplateName";
		
        ///<summary> 
    	/// 列名常量，短消息模板代码（阿里云用）
        ///</summary>
		public const string TEMPLATECODE="TemplateCode";
		
        ///<summary> 
    	/// 列名常量，是否为模板消息
        ///</summary>
		public const string ISTEMPLATEMSG="IsTemplateMsg";
		
        ///<summary> 
    	/// 列名常量，短消息模板的内容（或Json格式模板）
        ///</summary>
		public const string LOCALTEMPLATETEXT="LocalTemplateText";
		
        ///<summary> 
    	/// 列名常量，短信签名
        ///</summary>
		public const string SIGNNAME="SignName";
		
		public const string CREATETIME="CreateTime";
		
		public const string VERSION="Version";
		
        #endregion
    
    	#region 实现SchemaBase抽象类接口。
		/// <summary>
        /// 获取表（或视图）架构的名称。
        /// </summary>
        /// <remarks>该名称与数据库里面的名称对应。</remarks>
		public override string TableName
		{
			get { return SMSTEMPLATES; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public SmsTemplateSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public SmsTemplateSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public SmsTemplateSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 网关ID
        ///</summary>
		public string GatewayId { get { return GATEWAYID; } }
		
        ///<summary> 
    	/// 短消息模板名称
        ///</summary>
		public string TemplateName { get { return TEMPLATENAME; } }
		
        ///<summary> 
    	/// 短消息模板代码（阿里云用）
        ///</summary>
		public string TemplateCode { get { return TEMPLATECODE; } }
		
        ///<summary> 
    	/// 是否为模板消息
        ///</summary>
		public string IsTemplateMsg { get { return ISTEMPLATEMSG; } }
		
        ///<summary> 
    	/// 短消息模板的内容（或Json格式模板）
        ///</summary>
		public string LocalTemplateText { get { return LOCALTEMPLATETEXT; } }
		
        ///<summary> 
    	/// 短信签名
        ///</summary>
		public string SignName { get { return SIGNNAME; } }
		
		public string CreateTime { get { return CREATETIME; } }
		
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
    	/// 网关ID
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetGatewayId(bool needPrefix=true)
        {
            return this.QuoteField(GATEWAYID,needPrefix);
        }
		
        ///<summary> 
    	/// 短消息模板名称
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetTemplateName(bool needPrefix=true)
        {
            return this.QuoteField(TEMPLATENAME,needPrefix);
        }
		
        ///<summary> 
    	/// 短消息模板代码（阿里云用）
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetTemplateCode(bool needPrefix=true)
        {
            return this.QuoteField(TEMPLATECODE,needPrefix);
        }
		
        ///<summary> 
    	/// 是否为模板消息
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetIsTemplateMsg(bool needPrefix=true)
        {
            return this.QuoteField(ISTEMPLATEMSG,needPrefix);
        }
		
        ///<summary> 
    	/// 短消息模板的内容（或Json格式模板）
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLocalTemplateText(bool needPrefix=true)
        {
            return this.QuoteField(LOCALTEMPLATETEXT,needPrefix);
        }
		
        ///<summary> 
    	/// 短信签名
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetSignName(bool needPrefix=true)
        {
            return this.QuoteField(SIGNNAME,needPrefix);
        }
		
		public string GetCreateTime(bool needPrefix=true)
        {
            return this.QuoteField(CREATETIME,needPrefix);
        }
		
		public string GetVersion(bool needPrefix=true)
        {
            return this.QuoteField(VERSION,needPrefix);
        }
		
        #endregion
    }
}
