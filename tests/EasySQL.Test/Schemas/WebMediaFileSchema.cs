using EasySQL;

namespace EasySQL.Test
{
	public class WebMediaFileSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string WEBMEDIAFILES="WebMediaFiles";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，对象类型
        ///</summary>
		public const string OBJTYPE="ObjType";
		
        ///<summary> 
    	/// 列名常量，对象编号
        ///</summary>
		public const string OBJID="ObjId";
		
        ///<summary> 
    	/// 列名常量，附件分类编号
        ///</summary>
		public const string CATEGORYID="CategoryId";
		
        ///<summary> 
    	/// 列名常量，行号
        ///</summary>
		public const string LINE="Line";
		
        ///<summary> 
    	/// 列名常量，文件名称
        ///</summary>
		public const string FILENAME="FileName";
		
        ///<summary> 
    	/// 列名常量，缩略图文件名称（如果有，像图片，视频等）
        ///</summary>
		public const string THUMBFILENAME="ThumbFileName";
		
        ///<summary> 
    	/// 列名常量，文件后缀名
        ///</summary>
		public const string FILESUFFIX="FileSuffix";
		
        ///<summary> 
    	/// 列名常量，文件标题
        ///</summary>
		public const string FILETITLE="FileTitle";
		
        ///<summary> 
    	/// 列名常量，扩展位选项
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
			get { return WEBMEDIAFILES; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public WebMediaFileSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public WebMediaFileSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public WebMediaFileSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 对象类型
        ///</summary>
		public string ObjType { get { return OBJTYPE; } }
		
        ///<summary> 
    	/// 对象编号
        ///</summary>
		public string ObjId { get { return OBJID; } }
		
        ///<summary> 
    	/// 附件分类编号
        ///</summary>
		public string CategoryId { get { return CATEGORYID; } }
		
        ///<summary> 
    	/// 行号
        ///</summary>
		public string Line { get { return LINE; } }
		
        ///<summary> 
    	/// 文件名称
        ///</summary>
		public string FileName { get { return FILENAME; } }
		
        ///<summary> 
    	/// 缩略图文件名称（如果有，像图片，视频等）
        ///</summary>
		public string ThumbFileName { get { return THUMBFILENAME; } }
		
        ///<summary> 
    	/// 文件后缀名
        ///</summary>
		public string FileSuffix { get { return FILESUFFIX; } }
		
        ///<summary> 
    	/// 文件标题
        ///</summary>
		public string FileTitle { get { return FILETITLE; } }
		
        ///<summary> 
    	/// 扩展位选项
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
    	/// 对象类型
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetObjType(bool needPrefix=true)
        {
            return this.QuoteField(OBJTYPE,needPrefix);
        }
		
        ///<summary> 
    	/// 对象编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetObjId(bool needPrefix=true)
        {
            return this.QuoteField(OBJID,needPrefix);
        }
		
        ///<summary> 
    	/// 附件分类编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetCategoryId(bool needPrefix=true)
        {
            return this.QuoteField(CATEGORYID,needPrefix);
        }
		
        ///<summary> 
    	/// 行号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLine(bool needPrefix=true)
        {
            return this.QuoteField(LINE,needPrefix);
        }
		
        ///<summary> 
    	/// 文件名称
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetFileName(bool needPrefix=true)
        {
            return this.QuoteField(FILENAME,needPrefix);
        }
		
        ///<summary> 
    	/// 缩略图文件名称（如果有，像图片，视频等）
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetThumbFileName(bool needPrefix=true)
        {
            return this.QuoteField(THUMBFILENAME,needPrefix);
        }
		
        ///<summary> 
    	/// 文件后缀名
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetFileSuffix(bool needPrefix=true)
        {
            return this.QuoteField(FILESUFFIX,needPrefix);
        }
		
        ///<summary> 
    	/// 文件标题
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetFileTitle(bool needPrefix=true)
        {
            return this.QuoteField(FILETITLE,needPrefix);
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
