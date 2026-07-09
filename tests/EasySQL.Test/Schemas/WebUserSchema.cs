using EasySQL;

namespace EasySQL.Test
{
	public class WebUserSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string WEBUSERS="WebUsers";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，真实姓名
        ///</summary>
		public const string REALNAME="RealName";
		
        ///<summary> 
    	/// 列名常量，昵称
        ///</summary>
		public const string NICKNAME="NickName";
		
        ///<summary> 
    	/// 列名常量，会员类型，1：个人会员，2：公司会员
        ///</summary>
		public const string MEMBERTYPE="MemberType";
		
        ///<summary> 
    	/// 列名常量，会员等级
        ///</summary>
		public const string DEVELOPGRADE="DevelopGrade";
		
        ///<summary> 
    	/// 列名常量，广告等级
        ///</summary>
		public const string ADVERTISEGRADE="AdvertiseGrade";
		
        ///<summary> 
    	/// 列名常量，主会员编号（父级）
        ///</summary>
		public const string PARENTID="ParentId";
		
        ///<summary> 
    	/// 列名常量，推广会员编号
        ///</summary>
		public const string DEVELOPERID="DeveloperId";
		
        ///<summary> 
    	/// 列名常量，身份证号
        ///</summary>
		public const string IDNUMBER="IdNumber";
		
        ///<summary> 
    	/// 列名常量，性别
        ///</summary>
		public const string GENDER="Gender";
		
        ///<summary> 
    	/// 列名常量，生日
        ///</summary>
		public const string BIRTHDAY="Birthday";
		
        ///<summary> 
    	/// 列名常量，头像附件编号
        ///</summary>
		public const string PHOTOID="PhotoId";
		
        ///<summary> 
    	/// 列名常量，省份
        ///</summary>
		public const string PROVINCEID="ProvinceId";
		
        ///<summary> 
    	/// 列名常量，城市
        ///</summary>
		public const string CITYID="CityId";
		
        ///<summary> 
    	/// 列名常量，区县
        ///</summary>
		public const string DISTRICTID="DistrictId";
		
        ///<summary> 
    	/// 列名常量，乡镇
        ///</summary>
		public const string TOWNID="TownId";
		
        ///<summary> 
    	/// 列名常量，村庄
        ///</summary>
		public const string VILLAGEID="VillageId";
		
        ///<summary> 
    	/// 列名常量，籍贯省份
        ///</summary>
		public const string HOMEPROVINCEID="HomeProvinceId";
		
        ///<summary> 
    	/// 列名常量，籍贯城市
        ///</summary>
		public const string HOMECITYID="HomeCityId";
		
        ///<summary> 
    	/// 列名常量，籍贯区县
        ///</summary>
		public const string HOMEDISTRICTID="HomeDistrictId";
		
        ///<summary> 
    	/// 列名常量，籍贯乡镇
        ///</summary>
		public const string HOMETOWNID="HomeTownId";
		
        ///<summary> 
    	/// 列名常量，籍贯村委
        ///</summary>
		public const string HOMEVILLAGE1ID="HomeVillage1Id";
		
        ///<summary> 
    	/// 列名常量，籍贯村组
        ///</summary>
		public const string HOMEVILLAGE2ID="HomeVillage2Id";
		
        ///<summary> 
    	/// 列名常量，是否实名认证
        ///</summary>
		public const string ISCERTIFIED="IsCertified";
		
        ///<summary> 
    	/// 列名常量，维度
        ///</summary>
		public const string LAT="Lat";
		
        ///<summary> 
    	/// 列名常量，经度
        ///</summary>
		public const string LNG="Lng";
		
        ///<summary> 
    	/// 列名常量，余额
        ///</summary>
		public const string BALANCE="Balance";
		
        ///<summary> 
    	/// 列名常量，锁定余额
        ///</summary>
		public const string LOCKEDBALANCE="LockedBalance";
		
        ///<summary> 
    	/// 列名常量，余额校验值
        ///</summary>
		public const string BALANCECHECKTEXT="BalanceCheckText";
		
        ///<summary> 
    	/// 列名常量，随机安全码
        ///</summary>
		public const string RANDOMNUMBER="RandomNumber";
		
        ///<summary> 
    	/// 列名常量，用户状态，1：正常，2：停用
        ///</summary>
		public const string STATUS="Status";
		
        ///<summary> 
    	/// 列名常量，扩展位选项值
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
			get { return WEBUSERS; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public WebUserSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public WebUserSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public WebUserSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 真实姓名
        ///</summary>
		public string RealName { get { return REALNAME; } }
		
        ///<summary> 
    	/// 昵称
        ///</summary>
		public string NickName { get { return NICKNAME; } }
		
        ///<summary> 
    	/// 会员类型，1：个人会员，2：公司会员
        ///</summary>
		public string MemberType { get { return MEMBERTYPE; } }
		
        ///<summary> 
    	/// 会员等级
        ///</summary>
		public string DevelopGrade { get { return DEVELOPGRADE; } }
		
        ///<summary> 
    	/// 广告等级
        ///</summary>
		public string AdvertiseGrade { get { return ADVERTISEGRADE; } }
		
        ///<summary> 
    	/// 主会员编号（父级）
        ///</summary>
		public string ParentId { get { return PARENTID; } }
		
        ///<summary> 
    	/// 推广会员编号
        ///</summary>
		public string DeveloperId { get { return DEVELOPERID; } }
		
        ///<summary> 
    	/// 身份证号
        ///</summary>
		public string IdNumber { get { return IDNUMBER; } }
		
        ///<summary> 
    	/// 性别
        ///</summary>
		public string Gender { get { return GENDER; } }
		
        ///<summary> 
    	/// 生日
        ///</summary>
		public string Birthday { get { return BIRTHDAY; } }
		
        ///<summary> 
    	/// 头像附件编号
        ///</summary>
		public string PhotoId { get { return PHOTOID; } }
		
        ///<summary> 
    	/// 省份
        ///</summary>
		public string ProvinceId { get { return PROVINCEID; } }
		
        ///<summary> 
    	/// 城市
        ///</summary>
		public string CityId { get { return CITYID; } }
		
        ///<summary> 
    	/// 区县
        ///</summary>
		public string DistrictId { get { return DISTRICTID; } }
		
        ///<summary> 
    	/// 乡镇
        ///</summary>
		public string TownId { get { return TOWNID; } }
		
        ///<summary> 
    	/// 村庄
        ///</summary>
		public string VillageId { get { return VILLAGEID; } }
		
        ///<summary> 
    	/// 籍贯省份
        ///</summary>
		public string HomeProvinceId { get { return HOMEPROVINCEID; } }
		
        ///<summary> 
    	/// 籍贯城市
        ///</summary>
		public string HomeCityId { get { return HOMECITYID; } }
		
        ///<summary> 
    	/// 籍贯区县
        ///</summary>
		public string HomeDistrictId { get { return HOMEDISTRICTID; } }
		
        ///<summary> 
    	/// 籍贯乡镇
        ///</summary>
		public string HomeTownId { get { return HOMETOWNID; } }
		
        ///<summary> 
    	/// 籍贯村委
        ///</summary>
		public string HomeVillage1Id { get { return HOMEVILLAGE1ID; } }
		
        ///<summary> 
    	/// 籍贯村组
        ///</summary>
		public string HomeVillage2Id { get { return HOMEVILLAGE2ID; } }
		
        ///<summary> 
    	/// 是否实名认证
        ///</summary>
		public string IsCertified { get { return ISCERTIFIED; } }
		
        ///<summary> 
    	/// 维度
        ///</summary>
		public string Lat { get { return LAT; } }
		
        ///<summary> 
    	/// 经度
        ///</summary>
		public string Lng { get { return LNG; } }
		
        ///<summary> 
    	/// 余额
        ///</summary>
		public string Balance { get { return BALANCE; } }
		
        ///<summary> 
    	/// 锁定余额
        ///</summary>
		public string LockedBalance { get { return LOCKEDBALANCE; } }
		
        ///<summary> 
    	/// 余额校验值
        ///</summary>
		public string BalanceCheckText { get { return BALANCECHECKTEXT; } }
		
        ///<summary> 
    	/// 随机安全码
        ///</summary>
		public string RandomNumber { get { return RANDOMNUMBER; } }
		
        ///<summary> 
    	/// 用户状态，1：正常，2：停用
        ///</summary>
		public string Status { get { return STATUS; } }
		
        ///<summary> 
    	/// 扩展位选项值
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
    	/// 真实姓名
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetRealName(bool needPrefix=true)
        {
            return this.QuoteField(REALNAME,needPrefix);
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
    	/// 会员类型，1：个人会员，2：公司会员
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetMemberType(bool needPrefix=true)
        {
            return this.QuoteField(MEMBERTYPE,needPrefix);
        }
		
        ///<summary> 
    	/// 会员等级
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetDevelopGrade(bool needPrefix=true)
        {
            return this.QuoteField(DEVELOPGRADE,needPrefix);
        }
		
        ///<summary> 
    	/// 广告等级
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetAdvertiseGrade(bool needPrefix=true)
        {
            return this.QuoteField(ADVERTISEGRADE,needPrefix);
        }
		
        ///<summary> 
    	/// 主会员编号（父级）
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetParentId(bool needPrefix=true)
        {
            return this.QuoteField(PARENTID,needPrefix);
        }
		
        ///<summary> 
    	/// 推广会员编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetDeveloperId(bool needPrefix=true)
        {
            return this.QuoteField(DEVELOPERID,needPrefix);
        }
		
        ///<summary> 
    	/// 身份证号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetIdNumber(bool needPrefix=true)
        {
            return this.QuoteField(IDNUMBER,needPrefix);
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
    	/// 生日
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetBirthday(bool needPrefix=true)
        {
            return this.QuoteField(BIRTHDAY,needPrefix);
        }
		
        ///<summary> 
    	/// 头像附件编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetPhotoId(bool needPrefix=true)
        {
            return this.QuoteField(PHOTOID,needPrefix);
        }
		
        ///<summary> 
    	/// 省份
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetProvinceId(bool needPrefix=true)
        {
            return this.QuoteField(PROVINCEID,needPrefix);
        }
		
        ///<summary> 
    	/// 城市
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCityId(bool needPrefix=true)
        {
            return this.QuoteField(CITYID,needPrefix);
        }
		
        ///<summary> 
    	/// 区县
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetDistrictId(bool needPrefix=true)
        {
            return this.QuoteField(DISTRICTID,needPrefix);
        }
		
        ///<summary> 
    	/// 乡镇
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetTownId(bool needPrefix=true)
        {
            return this.QuoteField(TOWNID,needPrefix);
        }
		
        ///<summary> 
    	/// 村庄
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetVillageId(bool needPrefix=true)
        {
            return this.QuoteField(VILLAGEID,needPrefix);
        }
		
        ///<summary> 
    	/// 籍贯省份
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetHomeProvinceId(bool needPrefix=true)
        {
            return this.QuoteField(HOMEPROVINCEID,needPrefix);
        }
		
        ///<summary> 
    	/// 籍贯城市
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetHomeCityId(bool needPrefix=true)
        {
            return this.QuoteField(HOMECITYID,needPrefix);
        }
		
        ///<summary> 
    	/// 籍贯区县
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetHomeDistrictId(bool needPrefix=true)
        {
            return this.QuoteField(HOMEDISTRICTID,needPrefix);
        }
		
        ///<summary> 
    	/// 籍贯乡镇
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetHomeTownId(bool needPrefix=true)
        {
            return this.QuoteField(HOMETOWNID,needPrefix);
        }
		
        ///<summary> 
    	/// 籍贯村委
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetHomeVillage1Id(bool needPrefix=true)
        {
            return this.QuoteField(HOMEVILLAGE1ID,needPrefix);
        }
		
        ///<summary> 
    	/// 籍贯村组
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetHomeVillage2Id(bool needPrefix=true)
        {
            return this.QuoteField(HOMEVILLAGE2ID,needPrefix);
        }
		
        ///<summary> 
    	/// 是否实名认证
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetIsCertified(bool needPrefix=true)
        {
            return this.QuoteField(ISCERTIFIED,needPrefix);
        }
		
        ///<summary> 
    	/// 维度
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLat(bool needPrefix=true)
        {
            return this.QuoteField(LAT,needPrefix);
        }
		
        ///<summary> 
    	/// 经度
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLng(bool needPrefix=true)
        {
            return this.QuoteField(LNG,needPrefix);
        }
		
        ///<summary> 
    	/// 余额
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetBalance(bool needPrefix=true)
        {
            return this.QuoteField(BALANCE,needPrefix);
        }
		
        ///<summary> 
    	/// 锁定余额
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLockedBalance(bool needPrefix=true)
        {
            return this.QuoteField(LOCKEDBALANCE,needPrefix);
        }
		
        ///<summary> 
    	/// 余额校验值
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetBalanceCheckText(bool needPrefix=true)
        {
            return this.QuoteField(BALANCECHECKTEXT,needPrefix);
        }
		
        ///<summary> 
    	/// 随机安全码
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetRandomNumber(bool needPrefix=true)
        {
            return this.QuoteField(RANDOMNUMBER,needPrefix);
        }
		
        ///<summary> 
    	/// 用户状态，1：正常，2：停用
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetStatus(bool needPrefix=true)
        {
            return this.QuoteField(STATUS,needPrefix);
        }
		
        ///<summary> 
    	/// 扩展位选项值
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
