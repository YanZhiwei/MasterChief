using MasterChief.DotNet.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MasterChief.DotNet.Core.EFTests.Service
{
    /// <summary>
    /// 测试数据接口
    /// </summary>
    public interface ISampleService
    {
        /// <summary>
        /// Creates the specified samle.
        /// </summary>
        /// <param name="samle">The samle.</param>
        /// <returns></returns>
        bool Create(EFSample samle);

        EFSample Get(Guid id);

        PagedList<EFSample> GetByPage(int pageIndex, int PageSize);

        List<EFSample> SqlQuery();

        bool Exist<T>(Expression<Func<T, bool>> predicate) where T : ModelBase;
    }
}
