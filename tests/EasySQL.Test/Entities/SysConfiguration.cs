using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("SysConfiguration")]
	public class SysConfiguration
	{        
        ///<summary> 
    	/// 选项名Key
        ///</summary>
        [ExplicitKey]
		public string ItemKey {get;set;}
		
        ///<summary> 
    	/// 选项值
        ///</summary>
		public string ItemValue {get;set;}
		
        ///<summary> 
    	/// 选项长文本值
        ///</summary>
		public string LongValue {get;set;}
		
        ///<summary> 
    	/// 是否取长文本值
        ///</summary>
		public bool IsLongValue {get;set;}
		
        ///<summary> 
    	/// 选项分类编号
        ///</summary>
		public int? CategoryId {get;set;}
		
        ///<summary> 
    	/// 行号
        ///</summary>
		public int? Line {get;set;}
		
        ///<summary> 
    	/// 数据类型
        ///</summary>
		public string DataType {get;set;}
		
        ///<summary> 
    	/// 数据合法性验证类型或公式
        ///</summary>
		public string ValidType {get;set;}
		
        ///<summary> 
    	/// 选项对应的数据字典编号（如果是下拉选择）
        ///</summary>
		public int? DictionaryId {get;set;}
		
        ///<summary> 
    	/// 选项功能说明
        ///</summary>
		public string Description {get;set;}
		
        ///<summary> 
    	/// 是否必须录入值
        ///</summary>
		public bool IsMustInput {get;set;}
		
        ///<summary> 
    	/// 是否可以编辑
        ///</summary>
		public bool IsEditable {get;set;}
		
        ///<summary> 
    	/// 对后台管理用户在UI上是否可见
        ///</summary>
		public bool IsVisible {get;set;}
		
        ///<summary> 
    	/// 创建时间
        ///</summary>
		public DateTime CreateTime {get;set;}
		
        ///<summary> 
    	/// 上次编辑时间
        ///</summary>
		public DateTime LastEditTime {get;set;}
		
        ///<summary> 
    	/// 版本号
        ///</summary>
		public int Version {get;set;}
		
    }
}
