using MasterChief.DotNet.Core.Contract;
using MasterChief.DotNet.Core.DapperTests.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

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
                return dbcontext.Create<EFSample>(samle);
            }
        }

        public bool Delete(EFSample sample)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.Delete<EFSample>(sample);
            }
        }

        public bool Exist<T>(Expression<Func<T, bool>> predicate = null)
            where T : ModelBase
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.Exist<T>(predicate);
            }
        }

        public bool Exist(string name)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.Exist<EFSample>(ent => ent.UserName == name);
            }
        }

        public EFSample Get(Guid id)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.Get<EFSample>(id);
            }
        }

        public EFSample Get(string name)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.GetFirstOrDefault<EFSample>(ent => ent.UserName == name);
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

        public bool Update(EFSample sample)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.Update(sample);
            }
        }
    }
}