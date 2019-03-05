using System;
using System.Collections.Generic;

namespace MasterChief.DotNet.Core.Contract
{
    /// <summary>
    /// 数据访问上下文接口
    /// </summary>
    public interface IDbContext : IDisposable, IRepository
    {
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);
    }
}