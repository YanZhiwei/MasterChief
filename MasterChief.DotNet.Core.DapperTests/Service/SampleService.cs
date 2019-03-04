using MasterChief.DotNet.Core.Contract;
using MasterChief.DotNet.Core.DapperTests.Model;
using System;

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

        public EFSample Get(Guid id)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.Get<EFSample>(id);
            }
        }
    }
}