namespace MasterChief.DotNet.Core.EFTests.Service
{
    using MasterChief.DotNet.Core.Contract;
    using MasterChief.DotNet.Core.Contract.Helper;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;

    /// <summary>
    /// 测试数据接口
    /// </summary>
    /// <seealso cref="MasterChief.DotNet.Core.EFTests.Service.ISampleService" />
    public sealed class SampleService : ISampleService
    {
        #region Fields

        private readonly IDatabaseContextFactory _contextFactory = null;

        #endregion Fields

        #region Constructors

        public SampleService(IDatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        #endregion Constructors

        #region Methods

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

        #endregion Methods
    }
}