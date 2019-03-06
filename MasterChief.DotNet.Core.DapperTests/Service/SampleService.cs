using MasterChief.DotNet.Core.Contract;
using MasterChief.DotNet.Core.DapperTests.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace MasterChief.DotNet.Core.DapperTests.Service
{
    public sealed class SampleService : ISampleService
    {
        private readonly IDatabaseContextFactory _contextFactory = null;

        public SampleService(IDatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public bool Create(EFSample samle)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                dbcontext.BeginTransaction();

                bool result = dbcontext.Create<EFSample>(samle);
                dbcontext.Rollback();
                return result;
            }
        }

        public EFSample Get(Guid id)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.Get<EFSample>(id);
            }
        }

        public List<EFSample> SqlQuery()
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                string sql = @"SELECT
            *
            from
            EFSample
            WHERE
            UserName like @UserName
            and Available = @Available
            order by
            CreateTime DESC";

                DbParameter[] parameter = {
                    new SqlParameter(){ ParameterName="@UserName", Value="%ef%" },
                    new SqlParameter(){ ParameterName="@Available", Value=true }
                };
                return dbcontext.SqlQuery<EFSample>(sql, parameter).ToList();
            }
        }
    }
}