using MasterChief.DotNet.Core.Contract;
using System;

namespace MasterChief.DotNet.Core.DapperTests
{
    public class SampleDbContextFactory : IDatabaseContextFactory
    {
        private static Lazy<IDbContext> _instance = new Lazy<IDbContext>(() => new SampleDbContext());

        public IDbContext Create()
        {
            return _instance.Value;
        }
    }
}