using EasySQL;

namespace EasySQL.Test
{
	public class SmsGatewaySchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string SMSGATEWAY="SmsGateway";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，网关名称
        ///</summary>
		public const string NAME="Name";
		
        ///<summary> 
    	/// 列名常量，API调用用户ID
        ///</summary>
		public const string ACCESSUID="AccessUid";
		
        ///<summary> 
    	/// 列名常量，API调用密钥
        ///</summary>
		public const string ACCESSSECRET="AccessSecret";
		
        ///<summary> 
    	/// 列名常量，网关的网址
        ///</summary>
		public const string GATEWAYURL="GatewayUrl";
		
        ///<summary> 
    	/// 列名常量，实现短信接口所在的程序包
        ///</summary>
		public const string PACKAGEFILE="PackageFile";
		
        ///<summary> 
    	/// 列名常量，实现短信接口的类名
        ///</summary>
		public const string CLASSNAME="ClassName";
		
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
			get { return SMSGATEWAY; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public SmsGatewaySchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public SmsGatewaySchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public SmsGatewaySchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 网关名称
        ///</summary>
		public string Name { get { return NAME; } }
		
        ///<summary> 
    	/// API调用用户ID
        ///</summary>
		public string AccessUid { get { return ACCESSUID; } }
		
        ///<summary> 
    	/// API调用密钥
        ///</summary>
		public string AccessSecret { get { return ACCESSSECRET; } }
		
        ///<summary> 
    	/// 网关的网址
        ///</summary>
		public string GatewayUrl { get { return GATEWAYURL; } }
		
        ///<summary> 
    	/// 实现短信接口所在的程序包
        ///</summary>
		public string PackageFile { get { return PACKAGEFILE; } }
		
        ///<summary> 
    	/// 实现短信接口的类名
        ///</summary>
		public string ClassName { get { return CLASSNAME; } }
		
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
    	/// 网关名称
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetName(bool needPrefix=true)
        {
            return this.QuoteField(NAME,needPrefix);
        }
		
        ///<summary> 
    	/// API调用用户ID
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetAccessUid(bool needPrefix=true)
        {
            return this.QuoteField(ACCESSUID,needPrefix);
        }
		
        ///<summary> 
    	/// API调用密钥
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetAccessSecret(bool needPrefix=true)
        {
            return this.QuoteField(ACCESSSECRET,needPrefix);
        }
		
        ///<summary> 
    	/// 网关的网址
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetGatewayUrl(bool needPrefix=true)
        {
            return this.QuoteField(GATEWAYURL,needPrefix);
        }
		
        ///<summary> 
    	/// 实现短信接口所在的程序包
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetPackageFile(bool needPrefix=true)
        {
            return this.QuoteField(PACKAGEFILE,needPrefix);
        }
		
        ///<summary> 
    	/// 实现短信接口的类名
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetClassName(bool needPrefix=true)
        {
            return this.QuoteField(CLASSNAME,needPrefix);
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
