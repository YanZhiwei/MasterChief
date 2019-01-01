using MasterChief.DotNet.Core.Contract;
using System;
using System.Collections.Generic;
using System.Data;

namespace MasterChief.DotNet.Core.Dapper
{
    public abstract class DapperDbContextBase : IDbContext
    {
        protected readonly string _connectString = null;
        protected DapperDbContextBase(string connectString)
        {
            _connectString = connectString;
        }
        public abstract IDbConnection CreateConnection();
        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<T> ExecuteStoredProcedureList<T>(string commandText, params object[] parameters) where T : ModelBase
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
    }
}
