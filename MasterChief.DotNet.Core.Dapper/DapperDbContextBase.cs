using System;
using System.Collections.Generic;
using System.Data;
using MasterChief.DotNet.Core.Contract;

namespace MasterChief.DotNet.Core.Dapper
{
    public class DapperDbContextBase : IDbContext
    {

        protected readonly IDbConnection _dbConnection;
        protected DapperDbContextBase(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

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
