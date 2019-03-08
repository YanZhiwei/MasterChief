using MasterChief.DotNet.Core.DapperTests.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;

namespace MasterChief.DotNet.Core.DapperTests.Service
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
        bool Create(EFSample samle);

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        EFSample GetFirstOrDefault(Expression<Func<EFSample, bool>> predicate = null);

        /// <summary>
        ///根据主键查询
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        EFSample GetByKeyID(Guid id);

        /// <summary>
        /// 条件查询集合
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        List<EFSample> GetList(Expression<Func<EFSample, bool>> predicate = null);

        /// <summary>
        /// 添加判断是否存在
        /// </summary>
        /// <typeparam name="EFSample">The type of the f sample.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        bool Exist(Expression<Func<EFSample, bool>> predicate = null);

        /// <summary>
        /// 脚本查询
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        List<EFSample> SqlQuery(string sql, DbParameter[] parameter);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <returns></returns>
        bool Update(EFSample sample);
    }
}