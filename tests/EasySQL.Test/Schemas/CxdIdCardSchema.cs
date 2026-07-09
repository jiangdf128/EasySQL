using EasySQL;

namespace EasySQL.Test
{
	public class CxdIdCardSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string CXDIDCARDS="CxdIdCards";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，身份证号
        ///</summary>
		public const string IDNUMBER="IdNumber";
		
        ///<summary> 
    	/// 列名常量，姓名
        ///</summary>
		public const string NAME="Name";
		
        ///<summary> 
    	/// 列名常量，性别
        ///</summary>
		public const string GENDER="Gender";
		
        ///<summary> 
    	/// 列名常量，生日
        ///</summary>
		public const string BIRTHDAY="Birthday";
		
        ///<summary> 
    	/// 列名常量，身份证头像对应附件编号
        ///</summary>
		public const string PHOTOID="PhotoId";
		
        ///<summary> 
    	/// 列名常量，身份证详细地址
        ///</summary>
		public const string ADDRESS="Address";
		
        ///<summary> 
    	/// 列名常量，籍贯省份
        ///</summary>
		public const string PROVINCEID="ProvinceId";
		
        ///<summary> 
    	/// 列名常量，籍贯城市
        ///</summary>
		public const string CITYID="CityId";
		
        ///<summary> 
    	/// 列名常量，籍贯区县
        ///</summary>
		public const string DISTRICTID="DistrictId";
		
        ///<summary> 
    	/// 列名常量，籍贯乡镇
        ///</summary>
		public const string TOWNID="TownId";
		
        ///<summary> 
    	/// 列名常量，籍贯村庄
        ///</summary>
		public const string VILLAGEID="VillageId";
		
        ///<summary> 
    	/// 列名常量，身份证有效期起始日期
        ///</summary>
		public const string VALIDATEDDATE1="ValidatedDate1";
		
        ///<summary> 
    	/// 列名常量，身份证有效期截至日期
        ///</summary>
		public const string VALIDATEDDATE2="ValidatedDate2";
		
        ///<summary> 
    	/// 列名常量，发证机关
        ///</summary>
		public const string GRANTOFFICE="GrantOffice";
		
        ///<summary> 
    	/// 列名常量，上次和公安部数据同步时间
        ///</summary>
		public const string LASTSYNCHRONIZEDTIME="LastSynchronizedTime";
		
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
			get { return CXDIDCARDS; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public CxdIdCardSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public CxdIdCardSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public CxdIdCardSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 身份证号
        ///</summary>
		public string IdNumber { get { return IDNUMBER; } }
		
        ///<summary> 
    	/// 姓名
        ///</summary>
		public string Name { get { return NAME; } }
		
        ///<summary> 
    	/// 性别
        ///</summary>
		public string Gender { get { return GENDER; } }
		
        ///<summary> 
    	/// 生日
        ///</summary>
		public string Birthday { get { return BIRTHDAY; } }
		
        ///<summary> 
    	/// 身份证头像对应附件编号
        ///</summary>
		public string PhotoId { get { return PHOTOID; } }
		
        ///<summary> 
    	/// 身份证详细地址
        ///</summary>
		public string Address { get { return ADDRESS; } }
		
        ///<summary> 
    	/// 籍贯省份
        ///</summary>
		public string ProvinceId { get { return PROVINCEID; } }
		
        ///<summary> 
    	/// 籍贯城市
        ///</summary>
		public string CityId { get { return CITYID; } }
		
        ///<summary> 
    	/// 籍贯区县
        ///</summary>
		public string DistrictId { get { return DISTRICTID; } }
		
        ///<summary> 
    	/// 籍贯乡镇
        ///</summary>
		public string TownId { get { return TOWNID; } }
		
        ///<summary> 
    	/// 籍贯村庄
        ///</summary>
		public string VillageId { get { return VILLAGEID; } }
		
        ///<summary> 
    	/// 身份证有效期起始日期
        ///</summary>
		public string ValidatedDate1 { get { return VALIDATEDDATE1; } }
		
        ///<summary> 
    	/// 身份证有效期截至日期
        ///</summary>
		public string ValidatedDate2 { get { return VALIDATEDDATE2; } }
		
        ///<summary> 
    	/// 发证机关
        ///</summary>
		public string GrantOffice { get { return GRANTOFFICE; } }
		
        ///<summary> 
    	/// 上次和公安部数据同步时间
        ///</summary>
		public string LastSynchronizedTime { get { return LASTSYNCHRONIZEDTIME; } }
		
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
    	/// 身份证号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetIdNumber(bool needPrefix=true)
        {
            return this.QuoteField(IDNUMBER,needPrefix);
        }
		
        ///<summary> 
    	/// 姓名
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetName(bool needPrefix=true)
        {
            return this.QuoteField(NAME,needPrefix);
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
    	/// 身份证头像对应附件编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetPhotoId(bool needPrefix=true)
        {
            return this.QuoteField(PHOTOID,needPrefix);
        }
		
        ///<summary> 
    	/// 身份证详细地址
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetAddress(bool needPrefix=true)
        {
            return this.QuoteField(ADDRESS,needPrefix);
        }
		
        ///<summary> 
    	/// 籍贯省份
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetProvinceId(bool needPrefix=true)
        {
            return this.QuoteField(PROVINCEID,needPrefix);
        }
		
        ///<summary> 
    	/// 籍贯城市
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCityId(bool needPrefix=true)
        {
            return this.QuoteField(CITYID,needPrefix);
        }
		
        ///<summary> 
    	/// 籍贯区县
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetDistrictId(bool needPrefix=true)
        {
            return this.QuoteField(DISTRICTID,needPrefix);
        }
		
        ///<summary> 
    	/// 籍贯乡镇
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetTownId(bool needPrefix=true)
        {
            return this.QuoteField(TOWNID,needPrefix);
        }
		
        ///<summary> 
    	/// 籍贯村庄
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetVillageId(bool needPrefix=true)
        {
            return this.QuoteField(VILLAGEID,needPrefix);
        }
		
        ///<summary> 
    	/// 身份证有效期起始日期
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetValidatedDate1(bool needPrefix=true)
        {
            return this.QuoteField(VALIDATEDDATE1,needPrefix);
        }
		
        ///<summary> 
    	/// 身份证有效期截至日期
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetValidatedDate2(bool needPrefix=true)
        {
            return this.QuoteField(VALIDATEDDATE2,needPrefix);
        }
		
        ///<summary> 
    	/// 发证机关
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetGrantOffice(bool needPrefix=true)
        {
            return this.QuoteField(GRANTOFFICE,needPrefix);
        }
		
        ///<summary> 
    	/// 上次和公安部数据同步时间
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLastSynchronizedTime(bool needPrefix=true)
        {
            return this.QuoteField(LASTSYNCHRONIZEDTIME,needPrefix);
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
