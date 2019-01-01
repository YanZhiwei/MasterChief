using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Dapper.Contrib.Extensions;
using MasterChief.DotNet.Core.Contract;
using MasterChief.DotNet.Core.Dapper.Helper;
using MasterChief.DotNet4.Utilities.Common;
using global::Dapper;
namespace MasterChief.DotNet.Core.Dapper
{
    public class DapperRepository<T> : IRepository<T>
        where T : ModelBase
    {
        protected readonly DapperDbContextBase _dapperDbContext = null;
        protected readonly string _tableName = null;
        public DapperRepository(IDbContext dbContext)
        {
            _dapperDbContext = (DapperDbContextBase)dbContext;
            TableAttribute tableCfgInfo = AttributeHelper.Get<T, TableAttribute>();
            _tableName = tableCfgInfo != null ? tableCfgInfo.Name.Trim() : typeof(T).Name;

        }

        public bool Create(T entity)
        {
            using (IDbConnection connection = _dapperDbContext.CreateConnection())
            {
                return connection.Insert(entity) > 0;
            }

        }

        public bool Create(IEnumerable<T> entities)
        {
            bool result = false;
            using (IDbConnection connection = _dapperDbContext.CreateConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in entities)
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

        public bool Delete(T entity)
        {
            using (IDbConnection connection = _dapperDbContext.CreateConnection())
            {

                return connection.Delete(entity);
            }
        }

        public bool Delete(IEnumerable<T> entities)
        {
            bool result = false;
            using (IDbConnection connection = _dapperDbContext.CreateConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in entities)
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

        public bool Exist(Expression<Func<T, bool>> predicate = null)
        {
            QueryResult queryResult = DynamicQuery.GetDynamicQuery(_tableName, predicate);
            using (IDbConnection connection = _dapperDbContext.CreateConnection())
            {
                var result = connection.ExecuteScalar(queryResult.Sql, (object)queryResult.Param);
                return result.ToInt32OrDefault(0) > 0;
            }
        }

        public T Get(object id)
        {
            using (IDbConnection connection = _dapperDbContext.CreateConnection())
            {
                return connection.Get<T>(id);
            }
        }

        public List<T> Get(Expression<Func<T, bool>> predicate = null)
        {

            QueryResult queryResult = DynamicQuery.GetDynamicQuery(_tableName, predicate);
            using (IDbConnection connection = _dapperDbContext.CreateConnection())
            {
                return connection.Query<T>(queryResult.Sql, (T)queryResult.Param).ToList();
            }
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate = null)
        {
            QueryResult queryResult = DynamicQuery.GetDynamicQuery(_tableName, predicate);
            using (IDbConnection connection = _dapperDbContext.CreateConnection())
            {
                return connection.QuerySingle<T>(queryResult.Sql, (T)queryResult.Param);
            }
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public bool Update(T entity)
        {
            using (IDbConnection connection = _dapperDbContext.CreateConnection())
            {
                return connection.Update(entity);
            }
        }
    }
}
