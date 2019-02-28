using MasterChief.DotNet.Core.Contract;

namespace MasterChief.DotNet.Core.EFTests
{
    public sealed class SampleDbContextFactory : IDatabaseContextFactory
    {
        public IDbContext Create()
        {
            return new SampleDbContext();
        }
    }
}