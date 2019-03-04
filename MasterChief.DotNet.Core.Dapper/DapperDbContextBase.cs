using MasterChief.DotNet.Core.Contract;
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
            throw new NotImplementedException();
        }

        public bool Create<T>(IEnumerable<T> entities) where T : ModelBase
        {
            throw new NotImplementedException();
        }

        public abstract IDbConnection CreateConnection();

        public bool Delete<T>(T entity) where T : ModelBase
        {
            throw new NotImplementedException();
        }

        public bool Delete<T>(IEnumerable<T> entities) where T : ModelBase
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public IList<T> ExecuteStoredProcedureList<T>(string commandText, params object[] parameters) where T : ModelBase
        {
            throw new NotImplementedException();
        }

        public bool Exist<T>(Expression<Func<T, bool>> predicate = null) where T : ModelBase
        {
            throw new NotImplementedException();
        }

        public T Get<T>(object id) where T : ModelBase
        {
            throw new NotImplementedException();
        }

        public List<T> Get<T>(Expression<Func<T, bool>> predicate = null) where T : ModelBase
        {
            throw new NotImplementedException();
        }

        public T GetFirstOrDefault<T>(Expression<Func<T, bool>> predicate = null) where T : ModelBase
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate = null) where T : ModelBase
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : ModelBase
        {
            throw new NotImplementedException();
        }

        public bool Update<T>(T entity) where T : ModelBase
        {
            throw new NotImplementedException();
        }
    }
}