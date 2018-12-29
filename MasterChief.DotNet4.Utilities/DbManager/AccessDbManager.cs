namespace MasterChief.DotNet4.Utilities.DbManager
{
    using MasterChief.DotNet4.Utilities.Manager;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.OleDb;

    /// <summary>
    /// Access 数据访问操作类
    /// </summary>
    public sealed class AccessDataOperator : IDbManager
    {
        #region Fields

        /// <summary>
        /// 连接字符串
        /// </summary>
        private readonly string _connectString = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"> access路径 </param>
        public AccessDataOperator(string path)
        {
            CheckedAccessDBPath(path);
            _connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path">access路径</param>
        /// <param name="password">access密码</param>
        public AccessDataOperator(string path, string password)
        {
            CheckedAccessDBPath(path);
            ValidateManager.Begin().NotNullOrEmpty(password, "Access数据库密码");
            _connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Jet OLEDB:Database Password= " + password;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// ExecuteDataTable
        /// </summary>
        /// <param name="sql">读取sql语句</param>
        /// <param name="parameters">OleDbParameter参数；eg: new OleDbParameter("@categoryName","Test2")</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql, DbParameter[] parameters)
        {
            CheckedSql(sql);
            using (OleDbConnection sqlcon = new OleDbConnection(_connectString))
            {
                using (OleDbCommand sqlcmd = new OleDbCommand(sql, sqlcon))
                {
                    if (parameters != null)
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    using (OleDbDataAdapter sqldap = new OleDbDataAdapter(sqlcmd))
                    {
                        DataTable table = new DataTable();
                        sqldap.Fill(table);
                        return table;
                    }
                }
            }
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql">查询，修改，删除sql语句</param>
        /// <param name="parameters">OleDbParameter参数；eg: new OleDbParameter("@categoryName","Test2")</param>
        /// <returns>操作影响行数</returns>
        public int ExecuteNonQuery(string sql, DbParameter[] parameters)
        {
            CheckedSql(sql);
            int affectedRows = -1;
            using (OleDbConnection sqlcon = new OleDbConnection(_connectString))
            {
                sqlcon.Open();
                using (OleDbCommand sqlcmd = new OleDbCommand(sql, sqlcon))
                {
                    if (parameters != null)
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    affectedRows = sqlcmd.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="sql">读取sql语句</param>
        /// <param name="parameters">OleDbParameter参数；eg: new OleDbParameter("@categoryName","Test2")</param>
        /// <returns>IDataReader</returns>
        public IDataReader ExecuteReader(string sql, DbParameter[] parameters)
        {
            CheckedSql(sql);
            OleDbConnection sqlcon = new OleDbConnection(_connectString);
            using (OleDbCommand sqlcmd = new OleDbCommand(sql, sqlcon))
            {
                if (parameters != null)
                {
                    sqlcmd.Parameters.AddRange(parameters);
                }

                sqlcon.Open();
                return sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql">查询第一行第一列数据值</param>
        /// <param name="parameters">OleDbParameter参数；eg: new OleDbParameter("@categoryName","Test2")</param>
        /// <returns>Object</returns>
        public object ExecuteScalar(string sql, DbParameter[] parameters)
        {
            CheckedSql(sql);
            using (OleDbConnection sqlcon = new OleDbConnection(_connectString))
            {
                using (OleDbCommand sqlcmd = new OleDbCommand(sql, sqlcon))
                {
                    if (parameters != null)
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    sqlcon.Open();
                    return sqlcmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// ExecuteReader 存储过程
        /// </summary>
        /// <param name="proName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>IDataReader</returns>
        public IDataReader StoreExecuteDataReader(string proName, DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        private void CheckedAccessDBPath(string path)
        {
            ValidateManager.Begin().NotNullOrEmpty(path, "Access数据库路径").CheckFileExists(path);
        }

        private void CheckedSql(string sql)
        {
            ValidateManager.Begin().NotNullOrEmpty(sql, "SQL语句");
        }

        #endregion Methods
    }
}