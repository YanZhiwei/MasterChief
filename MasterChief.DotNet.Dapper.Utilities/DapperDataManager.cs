using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace MasterChief.DotNet.Dapper.Utilities
{
    /// <summary>
    ///     Dapper 数据库操作帮助类，默认是sql Server
    /// </summary>
    /// 时间：2016-01-19 16:21
    /// 备注：
    public abstract class DapperDataManager
    {
        #region Fields

        /// <summary>
        ///     连接字符串
        /// </summary>
        public readonly string ConnectString;

        #endregion Fields

        #region Constructors

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        /// 时间：2016-01-19 16:21
        /// 备注：
        protected DapperDataManager(string connectString)
        {
            ConnectString = connectString;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     创建SqlConnection连接对象，需要打开
        /// </summary>
        /// <returns>IDbConnection</returns>
        /// 时间：2016-01-19 16:22
        /// 备注：
        public abstract IDbConnection CreateConnection();

        /// <summary>
        ///     ExecuteDataTable
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>DataTable</returns>
        /// 时间：2016-01-19 16:22
        /// 备注：
        public virtual DataTable ExecuteDataTable<T>(string sql, T parameters)
            where T : class
        {
            using (var connection = CreateConnection())
            {
                var table = new DataTable();
                table.Load(connection.ExecuteReader(sql, parameters));
                return table;
            }
        }

        /// <summary>
        ///     ExecuteDataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <returns>DataTable</returns>
        /// 时间:2016/10/15 20:07
        /// 备注:
        public virtual DataTable ExecuteDataTable(string sql)
        {
            using (var connection = CreateConnection())
            {
                var table = new DataTable();
                table.Load(connection.ExecuteReader(sql));
                return table;
            }
        }

        /// <summary>
        ///     ExecuteNonQuery
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>影响行数</returns>
        /// 时间：2016-01-19 16:23
        /// 备注：
        public virtual int ExecuteNonQuery<T>(string sql, T parameters)
            where T : class
        {
            using (var connection = CreateConnection())
            {
                return connection.Execute(sql, parameters);
            }
        }

        /// <summary>
        ///     ExecuteNonQuery
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>影响行数</returns>
        public virtual int ExecuteNonQuery<T>(string sql, List<T> parameters)
        {
            var result = 0;
            using (var connection = CreateConnection())
            {
                if (connection.State != ConnectionState.Open) connection.Open();

                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        result += connection.Execute(sql, parameters, tran);
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        result = 0;
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     ExecuteNonQuery
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <returns>影响行数</returns>
        public virtual int ExecuteNonQuery(string sql)
        {
            using (var connection = CreateConnection())
            {
                return connection.Execute(sql);
            }
        }

        /// <summary>
        ///     ExecuteReader
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>IDataReader</returns>
        /// 时间：2016-01-19 16:24
        /// 备注：
        public virtual IDataReader ExecuteReader<T>(string sql, T parameters)
            where T : class
        {
            var connection = CreateConnection();

            return connection.ExecuteReader(sql, parameters);
        }

        /// <summary>
        ///     ExecuteReader
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <returns>IDataReader</returns>
        public virtual IDataReader ExecuteReader(string sql)
        {
            var connection = CreateConnection();
            return connection.ExecuteReader(sql);
        }

        /// <summary>
        ///     ExecuteScalar
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>返回对象</returns>
        /// 时间：2016-01-19 16:25
        /// 备注：
        public virtual object ExecuteScalar<T>(string sql, T parameters)
            where T : class
        {
            using (var connection = CreateConnection())
            {
                return connection.ExecuteScalar(sql, parameters, null, null, null);
            }
        }

        /// <summary>
        ///     ExecuteScalar
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <returns>返回对象</returns>
        public virtual object ExecuteScalar(string sql)
        {
            using (var connection = CreateConnection())
            {
                return connection.ExecuteScalar(sql, null, null, null, null);
            }
        }

        /// <summary>
        ///     返回实体类
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>实体类</returns>
        /// 时间：2016-01-19 16:25
        /// 备注：
        public virtual T Query<T>(string sql, T parameters)
            where T : class
        {
            using (var connection = CreateConnection())
            {
                return connection.Query<T>(sql, parameters).FirstOrDefault();
            }
        }

        /// <summary>
        ///     返回集合
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>集合</returns>
        /// 时间：2016-01-19 16:25
        /// 备注：
        public virtual List<T> QueryList<T>(string sql, T parameters)
            where T : class
        {
            using (var connection = CreateConnection())
            {
                return connection.Query<T>(sql, parameters).ToList();
            }
        }

        #endregion Methods
    }
}