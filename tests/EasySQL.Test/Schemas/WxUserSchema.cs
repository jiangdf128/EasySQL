using EasySQL;

namespace EasySQL.Test
{
	public class WxUserSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string WXUSERS="WxUsers";
        
        ///<summary> 
    	/// 列名常量，微信OpenID
        ///</summary>
		public const string OPENID="OpenId";
		
        ///<summary> 
    	/// 列名常量，昵称
        ///</summary>
		public const string NICKNAME="NickName";
		
        ///<summary> 
    	/// 列名常量，性别
        ///</summary>
		public const string GENDER="Gender";
		
        ///<summary> 
    	/// 列名常量，国家
        ///</summary>
		public const string COUNTRY="Country";
		
        ///<summary> 
    	/// 列名常量，省份
        ///</summary>
		public const string PROVINCE="Province";
		
        ///<summary> 
    	/// 列名常量，城市
        ///</summary>
		public const string CITY="City";
		
        ///<summary> 
    	/// 列名常量，头像照URL地址
        ///</summary>
		public const string HEADIMGURL="HeadImgUrl";
		
        ///<summary> 
    	/// 列名常量，特权
        ///</summary>
		public const string PRIVILEGE="Privilege";
		
        ///<summary> 
    	/// 列名常量，UnionID
        ///</summary>
		public const string UNIONID="UnionId";
		
        ///<summary> 
    	/// 列名常量，关注状态，0：未关注，1：关注，2：取消关注
        ///</summary>
		public const string BINDSTATUS="BindStatus";
		
        ///<summary> 
    	/// 列名常量，关注时间
        ///</summary>
		public const string BINDTIME="BindTime";
		
        ///<summary> 
    	/// 列名常量，取消关注时间
        ///</summary>
		public const string UNBINDTIME="UnbindTime";
		
        ///<summary> 
    	/// 列名常量，扩展位选项
        ///</summary>
		public const string EXTOPTION="ExtOption";
		
        ///<summary> 
    	/// 列名常量，创建时间
        ///</summary>
		public const string CREATETIME="CreateTime";
		
        #endregion
    
    	#region 实现SchemaBase抽象类接口。
		/// <summary>
        /// 获取表（或视图）架构的名称。
        /// </summary>
        /// <remarks>该名称与数据库里面的名称对应。</remarks>
		public override string TableName
		{
			get { return WXUSERS; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public WxUserSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public WxUserSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public WxUserSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 微信OpenID
        ///</summary>
		public string OpenId { get { return OPENID; } }
		
        ///<summary> 
    	/// 昵称
        ///</summary>
		public string NickName { get { return NICKNAME; } }
		
        ///<summary> 
    	/// 性别
        ///</summary>
		public string Gender { get { return GENDER; } }
		
        ///<summary> 
    	/// 国家
        ///</summary>
		public string Country { get { return COUNTRY; } }
		
        ///<summary> 
    	/// 省份
        ///</summary>
		public string Province { get { return PROVINCE; } }
		
        ///<summary> 
    	/// 城市
        ///</summary>
		public string City { get { return CITY; } }
		
        ///<summary> 
    	/// 头像照URL地址
        ///</summary>
		public string HeadImgUrl { get { return HEADIMGURL; } }
		
        ///<summary> 
    	/// 特权
        ///</summary>
		public string Privilege { get { return PRIVILEGE; } }
		
        ///<summary> 
    	/// UnionID
        ///</summary>
		public string UnionId { get { return UNIONID; } }
		
        ///<summary> 
    	/// 关注状态，0：未关注，1：关注，2：取消关注
        ///</summary>
		public string BindStatus { get { return BINDSTATUS; } }
		
        ///<summary> 
    	/// 关注时间
        ///</summary>
		public string BindTime { get { return BINDTIME; } }
		
        ///<summary> 
    	/// 取消关注时间
        ///</summary>
		public string UnbindTime { get { return UNBINDTIME; } }
		
        ///<summary> 
    	/// 扩展位选项
        ///</summary>
		public string ExtOption { get { return EXTOPTION; } }
		
        ///<summary> 
    	/// 创建时间
        ///</summary>
		public string CreateTime { get { return CREATETIME; } }
		
        #endregion
        
        #region 格式化列名称访问方法
        ///<summary> 
    	/// 微信OpenID
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetOpenId(bool needPrefix=true)
        {
            return this.QuoteField(OPENID,needPrefix);
        }
		
        ///<summary> 
    	/// 昵称
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetNickName(bool needPrefix=true)
        {
            return this.QuoteField(NICKNAME,needPrefix);
        }
		
        ///<summary> 
    	/// 性别
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetGender(bool needPrefix=true)
        {
            return this.QuoteField(GENDER,needPrefix);
        }
		
        ///<summary> 
    	/// 国家
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCountry(bool needPrefix=true)
        {
            return this.QuoteField(COUNTRY,needPrefix);
        }
		
        ///<summary> 
    	/// 省份
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetProvince(bool needPrefix=true)
        {
            return this.QuoteField(PROVINCE,needPrefix);
        }
		
        ///<summary> 
    	/// 城市
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCity(bool needPrefix=true)
        {
            return this.QuoteField(CITY,needPrefix);
        }
		
        ///<summary> 
    	/// 头像照URL地址
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetHeadImgUrl(bool needPrefix=true)
        {
            return this.QuoteField(HEADIMGURL,needPrefix);
        }
		
        ///<summary> 
    	/// 特权
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetPrivilege(bool needPrefix=true)
        {
            return this.QuoteField(PRIVILEGE,needPrefix);
        }
		
        ///<summary> 
    	/// UnionID
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetUnionId(bool needPrefix=true)
        {
            return this.QuoteField(UNIONID,needPrefix);
        }
		
        ///<summary> 
    	/// 关注状态，0：未关注，1：关注，2：取消关注
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetBindStatus(bool needPrefix=true)
        {
            return this.QuoteField(BINDSTATUS,needPrefix);
        }
		
        ///<summary> 
    	/// 关注时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetBindTime(bool needPrefix=true)
        {
            return this.QuoteField(BINDTIME,needPrefix);
        }
		
        ///<summary> 
    	/// 取消关注时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetUnbindTime(bool needPrefix=true)
        {
            return this.QuoteField(UNBINDTIME,needPrefix);
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
		
        #endregion
    }
}
