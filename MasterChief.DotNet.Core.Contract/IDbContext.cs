using System;
using System.Collections.Generic;

namespace MasterChief.DotNet.Core.Contract
{
    /// <summary>
    /// 数据访问上下文接口
    /// </summary>
    public interface IDbContext : IDisposable
    {
        #region Methods

        IList<T> ExecuteStoredProcedureList<T>(string commandText, params object[] parameters)
            where T : ModelBase;

        int SaveChanges();

        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
            where T : ModelBase;

        #endregion Methods
    }
}