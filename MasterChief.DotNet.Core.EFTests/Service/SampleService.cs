using MasterChief.DotNet.Core.Contract;
using MasterChief.DotNet.Core.Contract.Helper;
using System;
using System.Linq;

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

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public EFSample Get(Guid id)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.Get<EFSample>(id);
            }
        }

        public PagedList<EFSample> GetByPage(int pageIndex, int PageSize)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.Query<EFSample>().OrderByDescending(ent => ent.CreateTime).ToPagedList(pageIndex, PageSize);
            }
        }
    }
}