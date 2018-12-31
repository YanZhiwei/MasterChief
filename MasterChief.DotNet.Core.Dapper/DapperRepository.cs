using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Dapper.Contrib.Extensions;
using MasterChief.DotNet.Core.Contract;
using MasterChief.DotNet4.Utilities.Common;

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
            DapperTableNameAttribute tableCfgInfo = AttributeHelper.Get<T, DapperTableNameAttribute>();
            _tableName = tableCfgInfo != null ? tableCfgInfo.TableName.Trim() : typeof(T).Name;

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
            throw new NotImplementedException();

        }

        public T Get(object id)
        {
            throw new NotImplementedException();
        }

        public List<T> Get(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            throw new NotImplementedException();
        }

        public bool Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
