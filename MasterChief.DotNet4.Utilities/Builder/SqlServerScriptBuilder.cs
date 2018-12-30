namespace MasterChief.DotNet4.Utilities.Builder
{
    using Common;
    using Manager;
    using MasterChief.DotNet4.Utilities.Operator;
    using System.Collections;
    using System.Text;

    /// <summary>
    /// Sql Server 脚本创建
    /// </summary>
    /// 时间：2016-01-07 10:12
    /// 备注：
    public class SqlServerScriptBuilder
    {
        #region Fields

        /// <summary>
        /// 主键
        /// </summary>
        /// 时间：2016-01-07 10:14
        /// 备注：
        public readonly string PrimaryKey;

        /// <summary>
        /// 表名
        /// </summary>
        /// 时间：2016-01-07 10:14
        /// 备注：
        public readonly string TableName;

        #endregion Fields

        #region Constructors

        ///// <summary>
        ///// 查询时候需要显示的字段
        ///// </summary>
        ///// 时间：2016-01-07 11:06
        ///// 备注：
        //public HashSet<string> SelectedFields { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">主键</param>
        /// 时间：2016-01-07 10:15
        /// 备注：
        public SqlServerScriptBuilder(string tableName, string primaryKey)
        {
            ValidateOperator.Begin().NotNullOrEmpty(tableName, "表名").NotNullOrEmpty(primaryKey, "主键");
            TableName = tableName;
            PrimaryKey = primaryKey;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 删除语句
        /// </summary>
        /// <returns>sql脚本</returns>
        /// 时间：2016-01-07 11:36
        /// 备注：
        public string Delete()
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@{1}", TableName, PrimaryKey.ToLower());
            return sql.Trim();
        }

        /// <summary>
        /// 删除语句
        /// </summary>
        /// <param name="sqlWhere">条件键值</param>
        /// <returns>sql脚本</returns>
        /// 时间：2016-01-07 11:41
        /// 备注：
        public string Delete(Hashtable sqlWhere)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, CreateWhereSql(sqlWhere));
            return sql.Trim();
        }

        /// <summary>
        /// 插入语句
        /// </summary>
        /// <param name="insertFields">插入键值</param>
        /// <returns>sql脚本</returns>
        /// 时间：2016-01-07 11:21
        /// 备注：
        public string Insert(Hashtable insertFields)
        {
            string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", TableName, CreateInsertNameSql(insertFields), CreateInsertValueSql(insertFields));
            return sql.Trim();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="columns">需要查询的列『eg: name,age,address』</param>
        /// <returns>sql脚本</returns>
        /// 时间：2016-01-07 10:30
        /// 备注：
        public string Select(string columns)
        {
            string sql = string.Format("select {0} from {1}", columns, TableName);
            return sql.Trim();
        }

        /// <summary>
        /// 查询所有列
        /// </summary>
        /// <returns>sql脚本</returns>
        /// 时间：2016-01-07 10:23
        /// 备注：
        public string SelectAllColumns()
        {
            string sql = string.Format("select * from {0}", TableName);
            return sql.Trim();
        }

        /// <summary>
        /// 查询所有列
        /// </summary>
        /// <param name="sqlWhere">查询键值</param>
        /// <returns></returns>
        /// 时间：2016-01-07 11:11
        /// 备注：
        public string SelectAllColumns(Hashtable sqlWhere)
        {
            string sql = SelectAllColumns();
            return string.Format("{0} where ({1})", sql, CreateWhereSql(sqlWhere)).Trim();
        }

        /// <summary>
        /// 带条件查询
        /// </summary>
        /// <param name="columns">需要显示列</param>
        /// <param name="sqlWhere">条件键值</param>
        /// <returns>sql脚本</returns>
        /// 时间：2016-01-07 10:33
        /// 备注：
        public string SelectWhere(string columns, Hashtable sqlWhere)
        {
            string sql = string.Format("select {0} from {1} where ({2})", columns, TableName, CreateWhereSql(sqlWhere));
            return sql.Trim();
        }

        /// <summary>
        /// 更新语句
        /// </summary>
        /// <param name="updateFields">更新键值</param>
        /// <param name="sqlWhere">条件键值</param>
        /// <returns>sql脚本</returns>
        /// 时间：2016-01-07 11:28
        /// 备注：
        public string Update(Hashtable updateFields, Hashtable sqlWhere)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, CreateUpdateSql(updateFields), CreateWhereSql(sqlWhere));
            return sql.Trim();
        }

        private static StringBuilder CreateInsertNameSql(Hashtable sqlWhere)
        {
            StringBuilder builder = new StringBuilder();

            foreach (DictionaryEntry de in sqlWhere)
            {
                string key = de.Key.ToString().ToLower();
                builder.AppendFormat("{0},", key);
            }

            builder = builder.RemoveLast(",");
            return builder;
        }

        private static StringBuilder CreateInsertValueSql(Hashtable sqlWhere)
        {
            StringBuilder builder = new StringBuilder();

            foreach (DictionaryEntry de in sqlWhere)
            {
                string key = de.Key.ToString().ToLower();
                builder.AppendFormat("@{0},", key);
            }

            builder = builder.RemoveLast(",");
            return builder;
        }

        private static StringBuilder CreateUpdateSql(Hashtable sqlWhere)
        {
            StringBuilder builder = new StringBuilder();

            foreach (DictionaryEntry de in sqlWhere)
            {
                string key = de.Key.ToString().ToLower();
                builder.AppendFormat("{0}=@{1}, ", key, key);
            }

            builder = builder.RemoveLast(",");
            return builder;
        }

        private static StringBuilder CreateWhereSql(Hashtable sqlWhere)
        {
            StringBuilder builder = new StringBuilder();

            foreach (DictionaryEntry de in sqlWhere)
            {
                string key = de.Key.ToString().ToLower();
                builder.AppendFormat("{0}=@{1} and ", key, key);
            }

            builder = builder.RemoveLast("and");
            return builder;
        }

        #endregion Methods
    }
}