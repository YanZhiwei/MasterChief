using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using MasterChief.DotNet.Core.EFTests.Model;

namespace MasterChief.DotNet.Core.EFTests.Service
{
    /// <summary>
    /// 测试数据接口
    /// </summary>
    public interface ISampleService
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="samle">EFSample</param>
        /// <returns></returns>
        bool Create(EfSample samle);

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        EfSample GetFirstOrDefault(Expression<Func<EfSample, bool>> predicate = null);

        /// <summary>
        ///根据主键查询
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        EfSample GetByKeyId(Guid id);

        /// <summary>
        /// 条件查询集合
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        List<EfSample> GetList(Expression<Func<EfSample, bool>> predicate = null);

        /// <summary>
        /// 添加判断是否存在
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        bool Exist(Expression<Func<EfSample, bool>> predicate = null);

        /// <summary>
        /// 脚本查询
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameter">DbParameter</param>
        /// <returns></returns>
        List<EfSample> SqlQuery(string sql, DbParameter[] parameter);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <returns></returns>
        bool Update(EfSample sample);

        /// <summary>
        /// 事务成功
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="sample2">The sample2.</param>
        /// <returns></returns>
        bool CreateWithTransaction(EfSample sample, EfSample sample2);
    }
}