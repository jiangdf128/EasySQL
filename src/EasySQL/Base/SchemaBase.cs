using System;
using System.Collections.Generic;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// 实现了ITableSchema部分接口的抽象类，所有生成的表（或视图）结构的Schema类，都需要继承于这个抽象类。
    /// </summary>
    public abstract class SchemaBase:ITableSchema
    {
        internal List<JoinCondition> Joins { private set; get; }
        internal List<string> SelectFields { get; set; }
        protected Dictionary<string, string> FieldAliases { get; set; }
        protected string partialTableName = null;

        /// <summary>
        /// 获取Selected Fields.
        /// </summary>
        public string[] CopyFields
        {
            get
            {
                string[] array = new string[this.SelectFields.Count];
                this.SelectFields.CopyTo(array);
                return array;
            }
        }

        /// <summary>
        /// 获取或设置拆表的名称，如果设置为null则认为是默认表名称。
        /// </summary>
        public string PartialTableName
        {
            set
            {
                if (this is QueryBuilder) {
                    throw new Exception("Sorry,you can't change querybuilder's table name");
                }
                this.partialTableName = value;
            }
            get { return this.partialTableName; }
        }

        /// <summary>
        /// 是否为拆表的名称。
        /// </summary>
        public bool IsPartialTableName
        {
            get
            {
                return this.partialTableName != null && string.Compare(this.TableName, this.partialTableName, true) != 0;
            }
        }

        #region ITableSchema 成员

        /// <summary>
        /// 获取表或视图的名称。
        /// </summary>
        public abstract string TableName { get; }

        /// <summary>
        /// 获取或设置表或视图的别名。
        /// </summary>
        public virtual string Alias { get; set; }

        /// <summary>
        /// 清空所有Select字段。
        /// </summary>
        public virtual void ClearSelect()
        {
            if (this.SelectFields != null)
            {
                this.SelectFields.Clear();
            }
        }

        /// <summary>
        /// 清空所有Joins。
        /// </summary>
        public virtual void ClearJoins()
        {
            if (this.Joins != null)
            {
                this.Joins.Clear();
            }
        }

        /// <summary>
        /// 使用的SQL方言。
        /// </summary>
        public virtual ISQLDialect SQLDialect { get; set; }

        #endregion

        #region 私有函数

        /// <summary>
        /// 注册字段的别名到数据字典。
        /// </summary>
        /// <param name="field"></param>
        /// <param name="fieldAlias"></param>
        protected virtual void RegisterFieldAliases(string field,string fieldAlias)
        {
            if (this.FieldAliases == null)
            {
                this.FieldAliases = new Dictionary<string, string>();
            }
            if (!this.FieldAliases.ContainsKey(field))
            {
                this.FieldAliases.Add(field, fieldAlias);
            }
        }

        private string GetSqlFormatField(string field, string fieldAlias, bool needPrefix)
        {
            string prefix = needPrefix ? (this.Alias != null && this.Alias.Trim().Length > 0 ? string.Format("{0}.", this.Alias.Trim()) : string.Empty) : string.Empty;
            if (string.IsNullOrWhiteSpace(fieldAlias) && this.FieldAliases!=null && this.FieldAliases.ContainsKey(field))
            {
                fieldAlias = this.FieldAliases[field];
            }
            string extractFieldAlias=(fieldAlias!=null && fieldAlias.Trim().Length>0)? string.Format(" AS {0}",this.SQLDialect.QuoteField(fieldAlias.Trim())):string.Empty;
            return $"{prefix}{this.SQLDialect.QuoteField(field.Trim())}{extractFieldAlias}";
        }

        /// <summary>
        /// 添加数据表间的连接。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="clause">连接条件。</param>
        /// <param name="joinType">连接类型。</param>
        protected virtual SchemaBase AddJoin(ITableSchema target, string clause, JoinType joinType)
        {
            this.Check(target);
            this.Joins.Add(new JoinCondition(this, target, clause, joinType));
            return this;
        }

        /// <summary>
        /// 添加数据表间的连接。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField">参与连接的左边字段名。</param>
        /// <param name="rightField">参与连接的右边字段名。</param>
        /// <param name="joinType">连接类型。</param>
        protected virtual SchemaBase AddJoin(ITableSchema target, string leftField, string rightField, JoinType joinType)
        {
            return this.AddJoin(target, string.Format("{0}={1}", leftField, rightField), joinType);
        }

        /// <summary>
        /// 添加数据表间的连接。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField1">参与连接的左边字段名1。</param>
        /// <param name="rightField1">参与连接的右边字段名1。</param>
        /// <param name="leftField2">参与连接的左边字段名2。</param>
        /// <param name="rightField2">参与连接的右边字段名2。</param>
        /// <param name="joinType">连接类型。</param>
        protected virtual SchemaBase AddJoin(ITableSchema target, string leftField1, string rightField1, string leftField2, string rightField2, JoinType joinType)
        {
           return this.AddJoin(target, string.Format("{0}={1} and {2}={3}", leftField1, rightField1, leftField2, rightField2), joinType);
        }

        /// <summary>
        /// 添加数据表间的连接。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField1">参与连接的左边字段名1。</param>
        /// <param name="rightField1">参与连接的右边字段名1。</param>
        /// <param name="leftField2">参与连接的左边字段名2。</param>
        /// <param name="rightField2">参与连接的右边字段名2。</param>
        /// <param name="leftField3">参与连接的左边字段名3。</param>
        /// <param name="rightField3">参与连接的右边字段名3。</param>
        /// <param name="joinType">连接类型。</param>
        protected virtual SchemaBase AddJoin(ITableSchema target, string leftField1, string rightField1, string leftField2, string rightField2, string leftField3, string rightField3, JoinType joinType)
        {
            return this.AddJoin(target, string.Format("{0}={1} and {2}={3} and {4}={5}", leftField1, rightField1, leftField2, rightField2, leftField3, rightField3), joinType);
        }

        /// <summary>
        /// 添加数据表间的连接。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField1">参与连接的左边字段名1。</param>
        /// <param name="rightField1">参与连接的右边字段名1。</param>
        /// <param name="leftField2">参与连接的左边字段名2。</param>
        /// <param name="rightField2">参与连接的右边字段名2。</param>
        /// <param name="leftField3">参与连接的左边字段名3。</param>
        /// <param name="rightField3">参与连接的右边字段名3。</param>
        /// <param name="customFilters">自定义连接查询条件。</param>
        /// <param name="joinType">连接类型。</param>
        public virtual SchemaBase AddJoin(ITableSchema target, string leftField1, string rightField1, string leftField2, string rightField2, string leftField3, string rightField3, string customFilters, JoinType joinType)
        {
            leftField1 = leftField1.Trim();
            leftField2 = leftField2.Trim();
            leftField3 = leftField3.Trim();
            rightField1 = rightField1.Trim();
            rightField2 = rightField2.Trim();
            rightField3 = rightField3.Trim();
            customFilters = customFilters.Trim();
            StringBuilder sb = new StringBuilder();
            if (leftField1 != string.Empty && rightField1 != string.Empty)
            {
                sb.Append(string.Format("{0}={1}", leftField1, rightField1));
            }
            if (leftField2 != string.Empty && rightField2 != string.Empty)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" AND ");
                }
                sb.Append(string.Format("{0}={1}", leftField2, rightField2));
            }
            if (leftField3 != string.Empty && rightField3 != string.Empty)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" AND ");
                }
                sb.Append(string.Format("{0}={1}", leftField3, rightField3));
            }

            if (customFilters!=string.Empty)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" AND ");
                }
                sb.Append(string.Format(sb.Length==0 || (customFilters.StartsWith("(") && customFilters.EndsWith(")")) ?  "{0}" : "({0})", customFilters));
            }
           return  this.AddJoin(target,sb.ToString(), joinType);
        }

        /// <summary>
        /// 递归函数，主要是为了控制不允许出现连接形成闭环的现象，例如：连接自身，连接包括自身的子查询。
        /// 碰到这种情况，程序会抛出一个异常错误消息。
        /// </summary>
        /// <param name="item">连接或查询的目标对象。</param>
        protected void Check(ITableSchema item)
        {
            //进行连接的时候，是不允许连接自身，或者包括自身的子查询！
            if (this == item)
            {
                throw new Exception("It can't join(or be from) itself.");
            }
            if (item is QueryBuilder)
            {
                QueryBuilder qb = item as QueryBuilder;
                if (qb.FromItems.Contains(this))
                {
                    //不能够把包含自身的查询作为自己的子查询，否则构造SQL语句的时候会造成无限循环，以及堆栈溢出。
                    throw new Exception("It can't join(or be from) the sub query that contains itself.");
                }
                foreach (SchemaBase fitem in qb.FromItems)
                {
                    Check(fitem);
                }
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 抽象类<see cref="SchemaBase"/>的构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        /// <param name="dialect">数据方言。</param>
        public SchemaBase(string alias, ISQLDialect dialect)
        {
            this.SelectFields = new List<string>();
            this.Joins = new List<JoinCondition>();
            this.Alias = alias;
            this.SQLDialect = dialect ?? SQLDialectFactory.DefaultDialect;
        }
        /// <summary>
        /// 抽象类<see cref="SchemaBase"/>的构造函数。
        /// </summary>
        /// <param name="alias">表（或视图）的别名。</param>
        public SchemaBase(string alias)
            : this(alias, null)
        {
        }

        #endregion

        #region 公共函数

        internal string GetSelectClause()
        {
            StringBuilder fields = new StringBuilder();
            if (this.SelectFields != null)
            {
                for (int i = 0; i < this.SelectFields.Count; i++)
                {
                    if (i > 0)
                    {
                        fields.Append(",");
                    }
                    fields.Append(this.SelectFields[i]);
                }
            }
            return fields.ToString();
        }

        internal string GetJoinClause(out int brackets)
        {
            StringBuilder clause = new StringBuilder();
            brackets=0;
            if (this.Joins != null)
            {
                for (int i = 0; i < this.Joins.Count; i++)
                {
                    if (i > 0)
                    {
                        clause.Append(" ");
                    }
                    clause.Append(this.Joins[i].GetJoinClause());
                    if (this.SQLDialect.IsBracketJoin)
                    {
                        brackets = i+1;
                        clause.Append(")");
                    }
                }
            }
            return clause.ToString();
        }

        /// <summary>
        /// 重写了基类函数，返回构造SQL的信息。
        /// </summary>
        /// <returns>构造SQL的字符串</returns>
        public override string ToString()
        {
            return string.Format("SELECT {0} FROM {1} {2}", this.GetSelectClause(), this.QuoteName(), this.GetJoinClause(out int i));
        }

        /// <summary>
        /// 返回一个Query查询本表的查询对象。
        /// </summary>
        /// <param name="alias">子查询别名</param>
        /// <returns></returns>
        public QueryBuilder Query(string alias = null)
        {
            if (this.SelectFields.Count == 0)
            {
                this.Select();
            }
            var qb = new QueryBuilder(alias, this.SQLDialect);
            qb.From(this);
            return qb;
        }

        /// <summary>
        /// 选择表所有字段。
        /// </summary>
        /// <returns></returns>
        public SchemaBase Select()
        {
            return this.SelectExpression(string.IsNullOrWhiteSpace(this.Alias) ? "*":$"{this.Alias}.*" );
        }

        /// <summary>
        /// 根据一个字段名称选择查询列。
        /// </summary>
        /// <remarks>在这里，请不要使用表达式，如：Amount-Tax这样的表达式，会被Sql数据引擎当成关键字修饰，从而成为单独的字段名称。
        /// 如要使用表达式，请使用函数<see cref="SelectExpression"/>。
        /// 同样，也不要使用该函数，为列字段命名别名，否则也可能会被当成关键字修饰成为单独的字段，因此请使用别名参数。
        /// </remarks>
        /// <param name="field">数据表（或视图）的字段名称</param>
        public SchemaBase Select(string field)
        {
            return this.Select(field, false);
        }

        /// <summary>
        /// 根据一个字段名称选择查询列，同时指定查询列需要用表别名限定。
        /// </summary>
        /// <param name="field">数据表（或视图）的字段名称。</param>
        /// <param name="needPrefix">是否需要使用表别名限定。</param>
        public SchemaBase Select(string field, bool needPrefix)
        {
            return this.Select(field, string.Empty, needPrefix);
        }

        /// <summary>
        /// 根据一个字段名称选择查询列，同时为该列结果指定别名。
        /// </summary>
        /// <param name="field">数据表（或视图）的字段名称。</param>
        /// <param name="fieldAlias">列别名。</param>
        public SchemaBase Select(string field, string fieldAlias)
        {
            return this.Select(field, fieldAlias, false);
        }

        /// <summary>
        ///  根据一个字段名称选择查询列，同时为该列结果指定别名且要求用表别名限定该查询列。
        /// </summary>
        /// <param name="field">数据表（或视图）的字段名称。</param>
        /// <param name="fieldAlias">列别名。</param>
        /// <param name="needPrefix">是否需要使用表别名限定。</param>
        public SchemaBase Select(string field, string fieldAlias, bool needPrefix)
        {
            string item = GetSqlFormatField(field, fieldAlias, needPrefix);
            this.SelectFields.Add(item);
            return this;
        }

        /// <summary>
        /// 同时选择多个字段作为查询结果列。
        /// </summary>
        /// <param name="needPrefix">是否需要使用表别名限定。</param>
        /// <param name="fields">字段名1,字段名2......</param>
        public SchemaBase Select(bool needPrefix, params string[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] != null && fields[i].Trim().Length > 0)
                {
                    this.Select(fields[i], needPrefix);
                }
            }
            return this;
        }

        /// <summary>
        /// 选择用户表达式，可以是多个。需要使用表别名的字段（或修饰特定关键字）的情况，请程序员着情自行指定。
        /// </summary>
        /// <param name="expressions">表达式1，表达式2......</param>
        public SchemaBase SelectExpression(params string[] expressions)
        {
            for (int i = 0; i < expressions.Length; i++)
            {
                if (expressions[i] != null && expressions[i].Trim().Length > 0)
                {
                    this.SelectFields.Add(expressions[i]);
                }
            }
            return this;
        }



        /// <summary>
        /// 选择用户表达式并且指定别名。
        /// </summary>
        /// <param name="expression">表达式。</param>
        /// <param name="aliasName">结果列的别名。</param>
        public SchemaBase SelectExpression2(string expression,string aliasName)
        {
            this.SelectFields.Add(string.Format("{0} AS {1}", expression, this.SQLDialect.QuoteField(aliasName)));
            return this;
        }

        /// <summary>
        /// 自然连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="clause">连接条件子句。</param>
        public SchemaBase Join(ITableSchema target, string clause)
        {
            return this.AddJoin(target, clause, JoinType.Inner);
        }

        /// <summary>
        /// 自然连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField">参与自然连接的左边字段名。</param>
        /// <param name="rightField">参与自然连接的右边字段名。</param>
        public SchemaBase Join(ITableSchema target, string leftField, string rightField)
        {
            return this.AddJoin(target, string.Format("{0}={1}", leftField, rightField),JoinType.Inner);
        }

        /// <summary>
        /// 自然连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField1">参与自然连接的左边字段名1。</param>
        /// <param name="rightField1">参与自然连接的右边字段名1。</param>
        /// <param name="leftField2">参与自然连接的左边字段名2。</param>
        /// <param name="rightField2">参与自然连接的右边字段名2。</param>
        public SchemaBase Join(ITableSchema target, string leftField1, string rightField1, string leftField2, string rightField2)
        {
            return this.AddJoin(target, string.Format("{0}={1} and {2}={3}", leftField1, rightField1,leftField2,rightField2), JoinType.Inner);
        }

        /// <summary>
        /// 自然连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField1">参与自然连接的左边字段名1。</param>
        /// <param name="rightField1">参与自然连接的右边字段名1。</param>
        /// <param name="leftField2">参与自然连接的左边字段名2。</param>
        /// <param name="rightField2">参与自然连接的右边字段名2。</param>
        /// <param name="leftField3">参与自然连接的左边字段名3。</param>
        /// <param name="rightField3">参与自然连接的右边字段名3。</param>
        public SchemaBase Join(ITableSchema target, string leftField1, string rightField1, string leftField2, string rightField2, string leftField3, string rightField3)
        {
            return this.AddJoin(target, string.Format("{0}={1} and {2}={3} and {4}={5}", leftField1, rightField1, leftField2, rightField2,leftField3,rightField3), JoinType.Inner);
        }

        /// <summary>
        /// 左外连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="clause">连接条件子句。</param>
        public SchemaBase LeftJoin(ITableSchema target, string clause)
        {
            return this.AddJoin(target, clause, JoinType.Left);
        }

        /// <summary>
        /// 左外连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField">参与左外连接的左边字段名。</param>
        /// <param name="rightField">参与左外连接的右边字段名。</param>
        public SchemaBase LeftJoin(ITableSchema target, string leftField, string rightField)
        {
            return this.AddJoin(target, string.Format("{0}={1}", leftField, rightField), JoinType.Left);
        }

        /// <summary>
        /// 左外连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField1">参与左外连接的左边字段名1。</param>
        /// <param name="rightField1">参与左外连接的右边字段名1。</param>
        /// <param name="leftField2">参与左外连接的左边字段名2。</param>
        /// <param name="rightField2">参与左外连接的右边字段名2。</param>
        public SchemaBase LeftJoin(ITableSchema target, string leftField1, string rightField1, string leftField2, string rightField2)
        {
            return this.AddJoin(target, string.Format("{0}={1} and {2}={3}", leftField1, rightField1, leftField2, rightField2), JoinType.Left);
        }

        /// <summary>
        /// 左外连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField1">参与左外连接的左边字段名1。</param>
        /// <param name="rightField1">参与左外连接的右边字段名1。</param>
        /// <param name="leftField2">参与左外连接的左边字段名2。</param>
        /// <param name="rightField2">参与左外连接的右边字段名2。</param>
        /// <param name="leftField3">参与左外连接的左边字段名3。</param>
        /// <param name="rightField3">参与左外连接的右边字段名3。</param>
        public SchemaBase LeftJoin(ITableSchema target, string leftField1, string rightField1, string leftField2, string rightField2, string leftField3, string rightField3)
        {
            return this.AddJoin(target, string.Format("{0}={1} and {2}={3} and {4}={5}", leftField1, rightField1, leftField2, rightField2,leftField3,rightField3), JoinType.Left);
        }

        /// <summary>
        /// 右外连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="clause">连接条件子句。</param>
        public SchemaBase RightJoin(ITableSchema target, string clause)
        {
            return this.AddJoin(target, clause, JoinType.Right);
        }

        /// <summary>
        /// 右外连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField">参与右外连接的左边字段名。</param>
        /// <param name="rightField">参与右外连接的右边字段名。</param>
        public SchemaBase RightJoin(ITableSchema target, string leftField, string rightField)
        {
            return this.AddJoin(target, string.Format("{0}={1}", leftField, rightField), JoinType.Right);
        }

        /// <summary>
        /// 右外连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField1">参与右外连接的左边字段名1。</param>
        /// <param name="rightField1">参与右外连接的右边字段名1。</param>
        /// <param name="leftField2">参与右外连接的左边字段名2。</param>
        /// <param name="rightField2">参与右外连接的右边字段名2。</param>
        public SchemaBase RightJoin(ITableSchema target, string leftField1, string rightField1, string leftField2, string rightField2)
        {
            return this.AddJoin(target, string.Format("{0}={1} and {2}={3}", leftField1, rightField1, leftField2, rightField2), JoinType.Right);
        }

        /// <summary>
        /// 右外连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField1">参与右外连接的左边字段名1。</param>
        /// <param name="rightField1">参与右外连接的右边字段名1。</param>
        /// <param name="leftField2">参与右外连接的左边字段名2。</param>
        /// <param name="rightField2">参与右外连接的右边字段名2。</param>
        /// <param name="leftField3">参与右外连接的左边字段名3。</param>
        /// <param name="rightField3">参与右外连接的右边字段名3。</param>
        public SchemaBase RightJoin(ITableSchema target, string leftField1, string rightField1, string leftField2, string rightField2, string leftField3, string rightField3)
        {
            return this.AddJoin(target, string.Format("{0}={1} and {2}={3} and {4}={5}", leftField1, rightField1, leftField2, rightField2, leftField3, rightField3), JoinType.Right);
        }

        /// <summary>
        /// 全外连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="clause">连接条件子句。</param>
        public SchemaBase OuterJoin(ITableSchema target, string clause)
        {
            return this.AddJoin(target, clause, JoinType.Outer);
        }

        /// <summary>
        /// 全外连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField">参与全外连接的左边字段名。</param>
        /// <param name="rightField">参与全外连接的右边字段名。</param>
        public SchemaBase OuterJoin(ITableSchema target, string leftField, string rightField)
        {
            return this.AddJoin(target, string.Format("{0}={1}", leftField, rightField), JoinType.Outer);
        }

        /// <summary>
        /// 全外连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField1">参与全外连接的左边字段名1。</param>
        /// <param name="rightField1">参与全外连接的右边字段名1。</param>
        /// <param name="leftField2">参与全外连接的左边字段名2。</param>
        /// <param name="rightField2">参与全外连接的右边字段名2。</param>
        public SchemaBase OuterJoin(ITableSchema target, string leftField1, string rightField1, string leftField2, string rightField2)
        {
            return this.AddJoin(target, string.Format("{0}={1} and {2}={3}", leftField1, rightField1, leftField2, rightField2), JoinType.Outer);
        }

        /// <summary>
        /// 全外连接目标表（或视图）。
        /// </summary>
        /// <param name="target">目标表（或视图）对象</param>
        /// <param name="leftField1">参与全外连接的左边字段名1。</param>
        /// <param name="rightField1">参与全外连接的右边字段名1。</param>
        /// <param name="leftField2">参与全外连接的左边字段名2。</param>
        /// <param name="rightField2">参与全外连接的右边字段名2。</param>
        /// <param name="leftField3">参与全外连接的左边字段名3。</param>
        /// <param name="rightField3">参与全外连接的右边字段名3。</param>

        public SchemaBase OuterJoin(ITableSchema target, string leftField1, string rightField1, string leftField2, string rightField2, string leftField3, string rightField3)
        {
            return this.AddJoin(target, string.Format("{0}={1} and {2}={3} and {4}={5}", leftField1, rightField1, leftField2, rightField2, leftField3, rightField3), JoinType.Outer);
        }

        /// <summary>
        /// 修饰查询子句的表名称（包括为表指定别名）。
        /// </summary>
        /// <returns></returns>
        public string QuoteName()
        {
            string alias;
            alias = (this.Alias != null && this.Alias.Trim().Length > 0) ? string.Format(" {0}", this.Alias.Trim()) : string.Empty;
            return string.Format("{0}{1}", ((this is QueryBuilder) ? this.TableName : this.SQLDialect.QuoteTable( this.IsPartialTableName? this.PartialTableName: this.TableName)), alias);
        }

        /// <summary>
        /// 修饰字段（过滤关键字及按需增加表限定符）。
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="needPrefix">是否需要使用表别名限定。</param>
        /// <returns></returns>
        public string QuoteField(string field, bool needPrefix)
        {
            return this.GetSqlFormatField(field, string.Empty, needPrefix);
        }
        /// <summary>
        /// 修饰字段（过滤关键字并且增加表限定符）。
        /// </summary>
        /// <param name="field">字段名</param>
        /// <returns></returns>
        public string QuoteField(string field)
        {
            return this.QuoteField(field, true);
        }

        #endregion

    }
}
