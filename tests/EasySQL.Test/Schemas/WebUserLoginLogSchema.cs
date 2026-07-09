using EasySQL;

namespace EasySQL.Test
{
	public class WebUserLoginLogSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string WEBUSERLOGINLOG="WebUserLoginLog";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，用户编号
        ///</summary>
		public const string USERID="UserId";
		
        ///<summary> 
    	/// 列名常量，会话Guid号
        ///</summary>
		public const string SESSIONGUID="SessionGuid";
		
        ///<summary> 
    	/// 列名常量，Cookies Guid号
        ///</summary>
		public const string COOKIESGUID="CookiesGuid";
		
        ///<summary> 
    	/// 列名常量，登录入口类型，1：web，2：App，3：小程序
        ///</summary>
		public const string TERMINALTYPE="TerminalType";
		
        ///<summary> 
    	/// 列名常量，终端设备名称
        ///</summary>
		public const string TERMINALNAME="TerminalName";
		
        ///<summary> 
    	/// 列名常量，终端设备系统类型及版本信息
        ///</summary>
		public const string TERMINALOS="TerminalOs";
		
        ///<summary> 
    	/// 列名常量，登录的IP地址
        ///</summary>
		public const string LOGINIP="LoginIp";
		
        ///<summary> 
    	/// 列名常量，登录所在地纬度
        ///</summary>
		public const string LAT="Lat";
		
        ///<summary> 
    	/// 列名常量，登录所在地经度
        ///</summary>
		public const string LNG="Lng";
		
        ///<summary> 
    	/// 列名常量，登录时间
        ///</summary>
		public const string LOGINTIME="LoginTime";
		
        ///<summary> 
    	/// 列名常量，安全退出时间
        ///</summary>
		public const string LOGOUTTIME="LogoutTime";
		
        ///<summary> 
    	/// 列名常量，最后一次心跳活动时间
        ///</summary>
		public const string LASTACTIVETIME="LastActiveTime";
		
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
			get { return WEBUSERLOGINLOG; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public WebUserLoginLogSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public WebUserLoginLogSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public WebUserLoginLogSchema():this(string.Empty,null) {}
        
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
    	/// 会话Guid号
        ///</summary>
		public string SessionGuid { get { return SESSIONGUID; } }
		
        ///<summary> 
    	/// Cookies Guid号
        ///</summary>
		public string CookiesGuid { get { return COOKIESGUID; } }
		
        ///<summary> 
    	/// 登录入口类型，1：web，2：App，3：小程序
        ///</summary>
		public string TerminalType { get { return TERMINALTYPE; } }
		
        ///<summary> 
    	/// 终端设备名称
        ///</summary>
		public string TerminalName { get { return TERMINALNAME; } }
		
        ///<summary> 
    	/// 终端设备系统类型及版本信息
        ///</summary>
		public string TerminalOs { get { return TERMINALOS; } }
		
        ///<summary> 
    	/// 登录的IP地址
        ///</summary>
		public string LoginIp { get { return LOGINIP; } }
		
        ///<summary> 
    	/// 登录所在地纬度
        ///</summary>
		public string Lat { get { return LAT; } }
		
        ///<summary> 
    	/// 登录所在地经度
        ///</summary>
		public string Lng { get { return LNG; } }
		
        ///<summary> 
    	/// 登录时间
        ///</summary>
		public string LoginTime { get { return LOGINTIME; } }
		
        ///<summary> 
    	/// 安全退出时间
        ///</summary>
		public string LogoutTime { get { return LOGOUTTIME; } }
		
        ///<summary> 
    	/// 最后一次心跳活动时间
        ///</summary>
		public string LastActiveTime { get { return LASTACTIVETIME; } }
		
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
    	/// 会话Guid号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetSessionGuid(bool needPrefix=true)
        {
            return this.QuoteField(SESSIONGUID,needPrefix);
        }
		
        ///<summary> 
    	/// Cookies Guid号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCookiesGuid(bool needPrefix=true)
        {
            return this.QuoteField(COOKIESGUID,needPrefix);
        }
		
        ///<summary> 
    	/// 登录入口类型，1：web，2：App，3：小程序
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetTerminalType(bool needPrefix=true)
        {
            return this.QuoteField(TERMINALTYPE,needPrefix);
        }
		
        ///<summary> 
    	/// 终端设备名称
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetTerminalName(bool needPrefix=true)
        {
            return this.QuoteField(TERMINALNAME,needPrefix);
        }
		
        ///<summary> 
    	/// 终端设备系统类型及版本信息
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetTerminalOs(bool needPrefix=true)
        {
            return this.QuoteField(TERMINALOS,needPrefix);
        }
		
        ///<summary> 
    	/// 登录的IP地址
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLoginIp(bool needPrefix=true)
        {
            return this.QuoteField(LOGINIP,needPrefix);
        }
		
        ///<summary> 
    	/// 登录所在地纬度
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLat(bool needPrefix=true)
        {
            return this.QuoteField(LAT,needPrefix);
        }
		
        ///<summary> 
    	/// 登录所在地经度
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLng(bool needPrefix=true)
        {
            return this.QuoteField(LNG,needPrefix);
        }
		
        ///<summary> 
    	/// 登录时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLoginTime(bool needPrefix=true)
        {
            return this.QuoteField(LOGINTIME,needPrefix);
        }
		
        ///<summary> 
    	/// 安全退出时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLogoutTime(bool needPrefix=true)
        {
            return this.QuoteField(LOGOUTTIME,needPrefix);
        }
		
        ///<summary> 
    	/// 最后一次心跳活动时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLastActiveTime(bool needPrefix=true)
        {
            return this.QuoteField(LASTACTIVETIME,needPrefix);
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
