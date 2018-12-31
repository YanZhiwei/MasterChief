using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MasterChief.DotNet.Core.Contract;

namespace MasterChief.DotNet.Core.Dapper
{
    public class DapperRepository<T> : IRepository<T>
        where T : ModelBase
    {
        private readonly DapperDbContextBase _dapperDbContext = null;
        private readonly string _tableName = null;
        public DapperRepository(IDbContext dbContext)
        {
            _dapperDbContext = (DapperDbContextBase)dbContext;
        }

        public bool Create(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Create(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public bool Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
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
