using MasterChief.DotNet.Core.Contract;
using MasterChief.DotNet.Core.DapperTests.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MasterChief.DotNet.Core.DapperTests.Service
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

        List<EFSample> SqlQuery();

        bool Delete(EFSample sample);


        bool Exist<T>(Expression<Func<T, bool>> predicate) where T : ModelBase;
    }
}
