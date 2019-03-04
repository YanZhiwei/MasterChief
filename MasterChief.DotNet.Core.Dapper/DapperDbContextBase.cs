using Dapper;
using Dapper.Contrib.Extensions;
using MasterChief.DotNet.Core.Contract;
using MasterChief.DotNet.Core.Dapper.Helper;
using MasterChief.DotNet4.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace MasterChief.DotNet.Core.Dapper
{
    public abstract class DapperDbContextBase : IDbContext
    {
        protected readonly string _connectString = null;

        protected DapperDbContextBase(string connectString)
        {
            _connectString = connectString;
        }

        public bool Create<T>(T entity) where T : ModelBase
        {
            using (IDbConnection connection = CreateConnection())
            {
                return connection.Insert(entity) > 0;
            }
        }

        public bool Create<T>(IEnumerable<T> entities) where T : ModelBase
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

        public abstract IDbConnection CreateConnection();

        public bool Delete<T>(T entity) where T : ModelBase
        {
            using (IDbConnection connection = CreateConnection())
            {
                return connection.Delete(entity);
            }
        }

        public bool Delete<T>(IEnumerable<T> entities) where T : ModelBase
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

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public bool Exist<T>(Expression<Func<T, bool>> predicate = null) where T : ModelBase
        {
            string tableName = GetTableName<T>();
            QueryResult queryResult = DynamicQuery.GetDynamicQuery(tableName, predicate);
            using (IDbConnection connection = CreateConnection())
            {
                object result = connection.ExecuteScalar(queryResult.Sql, (object)queryResult.Param);
                return result.ToInt32OrDefault(0) > 0;
            }
        }

        private string GetTableName<T>() where T : ModelBase
        {
            TableAttribute tableCfgInfo = AttributeHelper.Get<T, TableAttribute>();
            return tableCfgInfo != null ? tableCfgInfo.Name.Trim() : typeof(T).Name;
        }

        public T Get<T>(object id) where T : ModelBase
        {
            using (IDbConnection connection = CreateConnection())
            {
                return connection.Get<T>(id);
            }
        }

        public List<T> Get<T>(Expression<Func<T, bool>> predicate = null) where T : ModelBase
        {
            string tableName = GetTableName<T>();
            QueryResult queryResult = DynamicQuery.GetDynamicQuery(tableName, predicate);
            using (IDbConnection connection = CreateConnection())
            {
                return connection.Query<T>(queryResult.Sql, (T)queryResult.Param).ToList();
            }
        }

        public T GetFirstOrDefault<T>(Expression<Func<T, bool>> predicate = null) where T : ModelBase
        {
            string tableName = GetTableName<T>();
            QueryResult queryResult = DynamicQuery.GetDynamicQuery(tableName, predicate);
            using (IDbConnection connection = CreateConnection())
            {
                return connection.QuerySingle<T>(queryResult.Sql, (T)queryResult.Param);
            }
        }

        public bool Update<T>(T entity) where T : ModelBase
        {
            using (IDbConnection connection = CreateConnection())
            {
                return connection.Update(entity);
            }
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate = null) where T : ModelBase
        {
            throw new NotImplementedException();
        }
    }
}