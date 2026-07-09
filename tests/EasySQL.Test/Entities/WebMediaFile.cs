using System;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("WebMediaFiles")]
	public class WebMediaFile
	{        
        ///<summary> 
    	/// 编号
        ///</summary>
		public long Id {get;set;}
		
        ///<summary> 
    	/// 对象类型
        ///</summary>
		public short ObjType {get;set;}
		
        ///<summary> 
    	/// 对象编号
        ///</summary>
		public long ObjId {get;set;}
		
        ///<summary> 
    	/// 附件分类编号
        ///</summary>
		public int? CategoryId {get;set;}
		
        ///<summary> 
    	/// 行号
        ///</summary>
		public int Line {get;set;}
		
        ///<summary> 
    	/// 文件名称
        ///</summary>
		public string FileName {get;set;}
		
        ///<summary> 
    	/// 缩略图文件名称（如果有，像图片，视频等）
        ///</summary>
		public string ThumbFileName {get;set;}
		
        ///<summary> 
    	/// 文件后缀名
        ///</summary>
		public string FileSuffix {get;set;}
		
        ///<summary> 
    	/// 文件标题
        ///</summary>
		public string FileTitle {get;set;}
		
        ///<summary> 
    	/// 扩展位选项
        ///</summary>
		public int ExtOption {get;set;}
		
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
