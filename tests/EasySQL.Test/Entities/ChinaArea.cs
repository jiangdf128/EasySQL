using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("ChinaAreas")]
	public class ChinaArea
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
        [ExplicitKey]
		public int Id {get;set;}
		
        ///<summary> 
    	/// 父级编号
        ///</summary>
		public int? ParentId {get;set;}
		
        ///<summary> 
    	/// 地区名
        ///</summary>
		public string Name {get;set;}
		
        ///<summary> 
    	/// 简称1
        ///</summary>
		public string ShortName1 {get;set;}
		
        ///<summary> 
    	/// 简称2
        ///</summary>
		public string ShortName2 {get;set;}
		
        ///<summary> 
    	/// 层次
        ///</summary>
		public int TreeLevel {get;set;}
		
        ///<summary> 
    	/// 排序行号
        ///</summary>
		public int Line {get;set;}
		
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
