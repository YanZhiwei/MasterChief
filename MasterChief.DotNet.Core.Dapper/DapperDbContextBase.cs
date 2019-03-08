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
    using System.Data.SqlClient;
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
        /// 当前数据库连接
        /// </summary>
        public IDbConnection CurrentConnection => TransactionEnabled ? CurrentTransaction.Connection : CreateConnection();

        /// <summary>
        /// 获取 是否开启事务提交
        /// </summary>
        public bool TransactionEnabled => CurrentTransaction != null;

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

        #region Properties

        /// <summary>
        ///获取 是否开启事务提交
        /// </summary>
        public IDbTransaction CurrentTransaction
        {
            get; private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 显式开启数据上下文事务
        /// </summary>
        /// <param name="isolationLevel">指定连接的事务锁定行为</param>
        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            if (!TransactionEnabled)
            {
                CurrentTransaction = CreateConnection().BeginTransaction(isolationLevel);
            }
        }

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        /// <exception cref="DataAccessException">提交数据更新时发生异常：" + msg</exception>
        public void Commit()
        {
            if (TransactionEnabled)
            {
                try
                {
                    CurrentTransaction.Commit();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException is SqlException)
                    {
                        SqlException sqlEx = ex.InnerException.InnerException as SqlException;
                        string msg = DataBaseHelper.GetSqlExceptionMessage(sqlEx.Number);
                        throw new DataAccessException("提交数据更新时发生异常：" + msg, sqlEx);
                    }
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="entity">需要操作的实体类</param>
        /// <returns>操作是否成功</returns>
        public bool Create<T>(T entity)
            where T : ModelBase
        {
            // insert single data always return 0 but the data is inserted in database successfully
            //https://github.com/StackExchange/Dapper/issues/587
            List<T> data = new List<T>() { entity };
            return CurrentConnection.Insert(data, CurrentTransaction) > 0;
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
        /// 执行与释放或重置非托管资源关联的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Dispose();
                CurrentTransaction = null;
            }

            if (CurrentConnection != null)
            {
                CurrentConnection.Dispose();
            }
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

            object result = CurrentConnection.ExecuteScalar(queryResult.Sql, (object)queryResult.Param, CurrentTransaction);
            return result != null;
        }

        /// <summary>
        /// 根据id获取记录
        /// </summary>
        /// <returns>记录</returns>
        /// <param name="id">id.</param>
        public T Get<T>(object id)
            where T : ModelBase
        {
            return CurrentConnection.Get<T>(id, CurrentTransaction);
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

            return CurrentConnection.Query<T>(queryResult.Sql, (object)queryResult.Param, CurrentTransaction).ToList();
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

            return CurrentConnection.QueryFirst<T>(queryResult.Sql, (object)queryResult.Param, CurrentTransaction);
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

        /// <summary>
        /// 显式回滚事务，仅在显式开启事务后有用
        /// </summary>
        public void Rollback()
        {
            if (TransactionEnabled)
            {
                CurrentTransaction.Rollback();
            }
        }

        public IEnumerable<T> SqlQuery<T>(string sql, IDbDataParameter[] parameters)
        {
            DapperParameter dataParameters = CreateParameter(parameters);
            return CurrentConnection.Query<T>(sql, dataParameters, CurrentTransaction);
        }

        /// <summary>
        /// 根据记录
        /// </summary>
        /// <returns>操作是否成功.</returns>
        /// <param name="entity">实体类记录.</param>
        public bool Update<T>(T entity)
            where T : ModelBase
        {
            return CurrentConnection.Update(entity, CurrentTransaction);
        }

        private DapperParameter CreateParameter(IDbDataParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return null;
            }

            DapperParameter dataParameters = new DapperParameter();
            foreach (IDbDataParameter paramter in parameters)
            {
                dataParameters.Add(paramter);
            }
            return dataParameters;
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