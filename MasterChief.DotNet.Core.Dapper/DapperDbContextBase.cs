namespace MasterChief.DotNet.Core.Dapper
{
    using global::Dapper;
    using global::Dapper.Contrib.Extensions;
    using MasterChief.DotNet.Core.Contract;
    using MasterChief.DotNet.Core.Dapper.Helper;
    using MasterChief.DotNet4.Utilities.Common;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// 基于Dapper的DbContext
    /// </summary>
    /// <seealso cref="MasterChief.DotNet.Core.Contract.IDbContext" />
    public abstract class DapperDbContextBase : IDbContext
    {
        #region Fields

        /// <summary>
        /// 连接字符串
        /// </summary>
        protected readonly string _connectString = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        protected DapperDbContextBase(string connectString)
        {
            _connectString = connectString;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="entity">需要操作的实体类</param>
        /// <returns>操作是否成功</returns>
        public bool Create<T>(T entity)
            where T : ModelBase
        {
            using (IDbConnection connection = CreateConnection())
            {
                List<T> data = new List<T>() { entity };
                // insert single data always return 0 but the data is inserted in database successfully
                //https://github.com/StackExchange/Dapper/issues/587
                return connection.Insert(data) > 0;
            }
        }

        /// <summary>
        /// 创建记录集合
        /// </summary>
        /// <returns>操作是否成功.</returns>
        /// <param name="entities">实体类集合.</param>
        public bool Create<T>(IEnumerable<T> entities)
            where T : ModelBase
        {
            bool result = false;
            using (IDbConnection connection = CreateConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (T item in entities)
                        {
                            connection.Insert(item, transaction);
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 创建数据库连接IDbConnection
        /// </summary>
        /// <returns></returns>
        public abstract IDbConnection CreateConnection();

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns>操作是否成功</returns>
        /// <param name="entity">需要操作的实体类.</param>
        public bool Delete<T>(T entity)
            where T : ModelBase
        {
            using (IDbConnection connection = CreateConnection())
            {
                return connection.Delete(entity);
            }
        }

        /// <summary>
        /// 条件删除记录
        /// </summary>
        /// <returns>操作是否成功</returns>
        /// <param name="entities">需要操作的集合.</param>
        public bool Delete<T>(IEnumerable<T> entities)
            where T : ModelBase
        {
            bool result = false;
            using (IDbConnection connection = CreateConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (T item in entities)
                        {
                            connection.Delete(item, transaction);
                        }
                        transaction.Commit();
                        result = true;
                    }
                    catch (Exception)
                    {
                        result = false;
                        transaction.Rollback();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 条件判断是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        /// <param name="predicate">判断条件委托</param>
        public bool Exist<T>(Expression<Func<T, bool>> predicate = null)
            where T : ModelBase
        {
            string tableName = GetTableName<T>();
            QueryResult queryResult = DynamicQuery.GetDynamicQuery(tableName, predicate);
            using (IDbConnection connection = CreateConnection())
            {
                object result = connection.ExecuteScalar(queryResult.Sql, (object)queryResult.Param);
                return result.ToInt32OrDefault(0) > 0;
            }
        }

        /// <summary>
        /// 根据id获取记录
        /// </summary>
        /// <returns>记录</returns>
        /// <param name="id">id.</param>
        public T Get<T>(object id)
            where T : ModelBase
        {
            using (IDbConnection connection = CreateConnection())
            {
                return connection.Get<T>(id);
            }
        }

        /// <summary>
        /// 条件获取记录集合
        /// </summary>
        /// <returns>集合</returns>
        /// <param name="predicate">筛选条件.</param>
        public List<T> Get<T>(Expression<Func<T, bool>> predicate = null)
            where T : ModelBase
        {
            string tableName = GetTableName<T>();
            QueryResult queryResult = DynamicQuery.GetDynamicQuery(tableName, predicate);
            using (IDbConnection connection = CreateConnection())
            {
                return connection.Query<T>(queryResult.Sql, (T)queryResult.Param).ToList();
            }
        }

        /// <summary>
        /// 条件获取记录第一条或者默认
        /// </summary>
        /// <returns>记录</returns>
        /// <param name="predicate">筛选条件.</param>
        public T GetFirstOrDefault<T>(Expression<Func<T, bool>> predicate = null)
            where T : ModelBase
        {
            string tableName = GetTableName<T>();
            QueryResult queryResult = DynamicQuery.GetDynamicQuery(tableName, predicate);
            using (IDbConnection connection = CreateConnection())
            {
                return connection.QuerySingle<T>(queryResult.Sql, (T)queryResult.Param);
            }
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <returns>IQueryable</returns>
        /// <param name="predicate">筛选条件.</param>
        public IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate = null)
            where T : ModelBase
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            using (IDbConnection connection = CreateConnection())
            {
                return connection.Query<T>(sql, parameters);
            }
        }

        /// <summary>
        /// 根据记录
        /// </summary>
        /// <returns>操作是否成功.</returns>
        /// <param name="entity">实体类记录.</param>
        public bool Update<T>(T entity)
            where T : ModelBase
        {
            using (IDbConnection connection = CreateConnection())
            {
                return connection.Update(entity);
            }
        }

        private string GetTableName<T>()
            where T : ModelBase
        {
            TableAttribute tableCfgInfo = AttributeHelper.Get<T, TableAttribute>();
            return tableCfgInfo != null ? tableCfgInfo.Name.Trim() : typeof(T).Name;
        }

        #endregion Methods
    }
}