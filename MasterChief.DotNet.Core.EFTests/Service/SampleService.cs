using MasterChief.DotNet.Core.Contract;
using MasterChief.DotNet.Core.EF;

namespace MasterChief.DotNet.Core.EFTests.Service
{
    /// <summary>
    /// 测试数据接口
    /// </summary>
    /// <seealso cref="MasterChief.DotNet.Core.EFTests.Service.ISampleService" />
    public sealed class SampleService : ISampleService
    {
        private readonly IDatabaseContextFactory _contextFactory;

        public SampleService(IDatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// Creates the specified samle.
        /// </summary>
        /// <param name="sample">The samle.</param>
        /// <returns></returns>
        public bool Create(EFSample sample)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                IRepository<EFSample> sampleRepo = new EfRepository<EFSample>(dbcontext);
                return sampleRepo.Create(sample);
            }
        }
    }
}