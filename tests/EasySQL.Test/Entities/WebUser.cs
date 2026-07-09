using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WebUsers")]
	public class WebUser
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
		public int Id {get;set;}
		
        ///<summary> 
    	/// 真实姓名
        ///</summary>
		public string RealName {get;set;}
		
        ///<summary> 
    	/// 昵称
        ///</summary>
		public string NickName {get;set;}
		
        ///<summary> 
    	/// 会员类型，1：个人会员，2：公司会员
        ///</summary>
		public short? MemberType {get;set;}
		
        ///<summary> 
    	/// 会员等级
        ///</summary>
		public short? DevelopGrade {get;set;}
		
        ///<summary> 
    	/// 广告等级
        ///</summary>
		public short? AdvertiseGrade {get;set;}
		
        ///<summary> 
    	/// 主会员编号（父级）
        ///</summary>
		public int? ParentId {get;set;}
		
        ///<summary> 
    	/// 推广会员编号
        ///</summary>
		public int? DeveloperId {get;set;}
		
        ///<summary> 
    	/// 身份证号
        ///</summary>
		public string IdNumber {get;set;}
		
        ///<summary> 
    	/// 性别
        ///</summary>
		public short? Gender {get;set;}
		
        ///<summary> 
    	/// 生日
        ///</summary>
		public DateTime? Birthday {get;set;}
		
        ///<summary> 
    	/// 头像附件编号
        ///</summary>
		public long? PhotoId {get;set;}
		
        ///<summary> 
    	/// 省份
        ///</summary>
		public int? ProvinceId {get;set;}
		
        ///<summary> 
    	/// 城市
        ///</summary>
		public int? CityId {get;set;}
		
        ///<summary> 
    	/// 区县
        ///</summary>
		public int? DistrictId {get;set;}
		
        ///<summary> 
    	/// 乡镇
        ///</summary>
		public int? TownId {get;set;}
		
        ///<summary> 
    	/// 村庄
        ///</summary>
		public int? VillageId {get;set;}
		
        ///<summary> 
    	/// 籍贯省份
        ///</summary>
		public int? HomeProvinceId {get;set;}
		
        ///<summary> 
    	/// 籍贯城市
        ///</summary>
		public int? HomeCityId {get;set;}
		
        ///<summary> 
    	/// 籍贯区县
        ///</summary>
		public int? HomeDistrictId {get;set;}
		
        ///<summary> 
    	/// 籍贯乡镇
        ///</summary>
		public int? HomeTownId {get;set;}
		
        ///<summary> 
    	/// 籍贯村委
        ///</summary>
		public int? HomeVillage1Id {get;set;}
		
        ///<summary> 
    	/// 籍贯村组
        ///</summary>
		public int? HomeVillage2Id {get;set;}
		
        ///<summary> 
    	/// 是否实名认证
        ///</summary>
		public bool? IsCertified {get;set;}
		
        ///<summary> 
    	/// 维度
        ///</summary>
		public decimal? Lat {get;set;}
		
        ///<summary> 
    	/// 经度
        ///</summary>
		public decimal? Lng {get;set;}
		
        ///<summary> 
    	/// 余额
        ///</summary>
		public decimal Balance {get;set;}
		
        ///<summary> 
    	/// 锁定余额
        ///</summary>
		public decimal LockedBalance {get;set;}
		
        ///<summary> 
    	/// 余额校验值
        ///</summary>
		public string BalanceCheckText {get;set;}
		
        ///<summary> 
    	/// 随机安全码
        ///</summary>
		public string RandomNumber {get;set;}
		
        ///<summary> 
    	/// 用户状态，1：正常，2：停用
        ///</summary>
		public short Status {get;set;}
		
        ///<summary> 
    	/// 扩展位选项值
        ///</summary>
		public long ExtOption {get;set;}
		
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
