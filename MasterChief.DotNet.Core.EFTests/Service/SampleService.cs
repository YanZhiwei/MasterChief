using MasterChief.DotNet.Core.Contract;

namespace MasterChief.DotNet.Core.EFTests.Service
{
    /// <summary>
    /// 测试数据接口
    /// </summary>
    /// <seealso cref="MasterChief.DotNet.Core.EFTests.Service.ISampleService" />
    public sealed class SampleService : ISampleService
    {
        private readonly IDatabaseContextFactory _contextFactory = null;

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
                return dbcontext.Create<EFSample>(sample);
            }
        }
    }
}