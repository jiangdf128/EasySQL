using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("CxdIdCards")]
	public class CxdIdCard
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
		public int Id {get;set;}
		
        ///<summary> 
    	/// 身份证号
        ///</summary>
		public string IdNumber {get;set;}
		
        ///<summary> 
    	/// 姓名
        ///</summary>
		public string Name {get;set;}
		
        ///<summary> 
    	/// 性别
        ///</summary>
		public short? Gender {get;set;}
		
        ///<summary> 
    	/// 生日
        ///</summary>
		public DateTime? Birthday {get;set;}
		
        ///<summary> 
    	/// 身份证头像对应附件编号
        ///</summary>
		public long? PhotoId {get;set;}
		
        ///<summary> 
    	/// 身份证详细地址
        ///</summary>
		public string Address {get;set;}
		
        ///<summary> 
    	/// 籍贯省份
        ///</summary>
		public int? ProvinceId {get;set;}
		
        ///<summary> 
    	/// 籍贯城市
        ///</summary>
		public int? CityId {get;set;}
		
        ///<summary> 
    	/// 籍贯区县
        ///</summary>
		public int? DistrictId {get;set;}
		
        ///<summary> 
    	/// 籍贯乡镇
        ///</summary>
		public int? TownId {get;set;}
		
        ///<summary> 
    	/// 籍贯村庄
        ///</summary>
		public int? VillageId {get;set;}
		
        ///<summary> 
    	/// 身份证有效期起始日期
        ///</summary>
		public DateTime? ValidatedDate1 {get;set;}
		
        ///<summary> 
    	/// 身份证有效期截至日期
        ///</summary>
		public DateTime? ValidatedDate2 {get;set;}
		
        ///<summary> 
    	/// 发证机关
        ///</summary>
		public string GrantOffice {get;set;}
		
        ///<summary> 
    	/// 上次和公安部数据同步时间
        ///</summary>
		public DateTime LastSynchronizedTime {get;set;}
		
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
