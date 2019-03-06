using System;
using System.Collections.Generic;
using System.Data;

namespace MasterChief.DotNet.Core.Contract
{
    /// <summary>
    /// 数据访问上下文接口
    /// </summary>
    public interface IDbContext : IDisposable, IRepository, IUnitOfWork
    {
        IEnumerable<T> SqlQuery<T>(string sql, IDbDataParameter[] parameters);
    }
}