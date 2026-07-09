using EasySQL;

namespace EasySQL.Test
{
	public class ChinaAreaSchema:SchemaBase
	{   
        #region 数据表名及列名的常量定义
        
        public const string CHINAAREAS="ChinaAreas";
        
        ///<summary> 
    	/// 列名常量，编号
        ///</summary>
		public const string ID="Id";
		
        ///<summary> 
    	/// 列名常量，父级编号
        ///</summary>
		public const string PARENTID="ParentId";
		
        ///<summary> 
    	/// 列名常量，地区名
        ///</summary>
		public const string NAME="Name";
		
        ///<summary> 
    	/// 列名常量，简称1
        ///</summary>
		public const string SHORTNAME1="ShortName1";
		
        ///<summary> 
    	/// 列名常量，简称2
        ///</summary>
		public const string SHORTNAME2="ShortName2";
		
        ///<summary> 
    	/// 列名常量，层次
        ///</summary>
		public const string TREELEVEL="TreeLevel";
		
        ///<summary> 
    	/// 列名常量，排序行号
        ///</summary>
		public const string LINE="Line";
		
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
			get { return CHINAAREAS; }
		}

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public ChinaAreaSchema(string alias, ISQLDialect dialect) :base(alias, dialect) { }
		
		 /// <summary>
         /// 构造函数。
         /// </summary>
         /// <param name="alias">表（或视图）的别名。</param>
         public ChinaAreaSchema(string alias):this(alias,null) {}
		
         /// <summary>
         /// 构造函数。
         /// </summary>
 		 public ChinaAreaSchema():this(string.Empty,null) {}
        
		#endregion
        
        #region 列名称属性
        ///<summary> 
    	/// 编号
        ///</summary>
		public string Id { get { return ID; } }
		
        ///<summary> 
    	/// 父级编号
        ///</summary>
		public string ParentId { get { return PARENTID; } }
		
        ///<summary> 
    	/// 地区名
        ///</summary>
		public string Name { get { return NAME; } }
		
        ///<summary> 
    	/// 简称1
        ///</summary>
		public string ShortName1 { get { return SHORTNAME1; } }
		
        ///<summary> 
    	/// 简称2
        ///</summary>
		public string ShortName2 { get { return SHORTNAME2; } }
		
        ///<summary> 
    	/// 层次
        ///</summary>
		public string TreeLevel { get { return TREELEVEL; } }
		
        ///<summary> 
    	/// 排序行号
        ///</summary>
		public string Line { get { return LINE; } }
		
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
    	/// 父级编号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetParentId(bool needPrefix=true)
        {
            return this.QuoteField(PARENTID,needPrefix);
        }
		
        ///<summary> 
    	/// 地区名
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetName(bool needPrefix=true)
        {
            return this.QuoteField(NAME,needPrefix);
        }
		
        ///<summary> 
    	/// 简称1
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetShortName1(bool needPrefix=true)
        {
            return this.QuoteField(SHORTNAME1,needPrefix);
        }
		
        ///<summary> 
    	/// 简称2
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetShortName2(bool needPrefix=true)
        {
            return this.QuoteField(SHORTNAME2,needPrefix);
        }
		
        ///<summary> 
    	/// 层次
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetTreeLevel(bool needPrefix=true)
        {
            return this.QuoteField(TREELEVEL,needPrefix);
        }
		
        ///<summary> 
    	/// 排序行号
        /// <param name="needPrefix">是否需要表别名限定符。</param>
        ///</summary>
		public string GetLine(bool needPrefix=true)
        {
            return this.QuoteField(LINE,needPrefix);
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
