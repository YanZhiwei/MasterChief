namespace MasterChief.DotNet.Core.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// 标准仓储接口
    /// </summary>
    /// <typeparam name="T">ModelBase</typeparam>
    public interface IRepository<T>
        where T : ModelBase
    {
        #region Methods

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns>操作是否成功</returns>
        /// <param name="entity">需要操作的实体类.</param>
        bool Delete(T entity);

        /// <summary>
        /// 条件删除记录
        /// </summary>
        /// <returns>操作是否成功</returns>
        /// <param name="entities">需要操作的集合.</param>
        bool Delete(IEnumerable<T> entities);

        /// <summary>
        /// 条件判断是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        /// <param name="predicate">判断条件委托</param>
        bool Exist(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// 根据id获取记录
        /// </summary>
        /// <returns>记录</returns>
        /// <param name="id">id.</param>
        T Get(object id);

        /// <summary>
        /// 条件获取记录集合
        /// </summary>
        /// <returns>集合</returns>
        /// <param name="predicate">筛选条件.</param>
        List<T> Get(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// 条件获取记录第一条或者默认
        /// </summary>
        /// <returns>记录</returns>
        /// <param name="predicate">筛选条件.</param>
        T GetFirstOrDefault(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// 创建一条记录
        /// </summary>
        /// <returns>操作是否成功.</returns>
        /// <param name="entity">实体类记录.</param>
        bool Create(T entity);

        /// <summary>
        /// 创建记录集合
        /// </summary>
        /// <returns>操作是否成功.</returns>
        /// <param name="entities">实体类集合.</param>
        bool Create(IEnumerable<T> entities);

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <returns>IQueryable</returns>
        /// <param name="predicate">筛选条件.</param>
        IQueryable<T> Query(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// 根据记录
        /// </summary>
        /// <returns>操作是否成功.</returns>
        /// <param name="entity">实体类记录.</param>
        bool Update(T entity);

        #endregion Methods
    }
}