using System;

namespace MasterChief.DotNet.Core.Contract
{
    /// <summary>
    /// 数据访问上下文接口
    /// </summary>
    public interface IDbContext : IDisposable, IRepository
    {

    }
}