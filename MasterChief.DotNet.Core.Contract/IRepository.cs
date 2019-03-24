namespace MasterChief.DotNet.Core.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// 标准仓储接口
    /// </summary>
    public interface IRepository
    {
        #region Methods

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns>操作是否成功</returns>
        /// <param name="entity">需要操作的实体类.</param>
        bool Delete<T>(T entity) where T : ModelBase;

        /// <summary>
        /// 条件判断是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        /// <param name="predicate">判断条件委托</param>
        bool Exist<T>(Expression<Func<T, bool>> predicate = null) where T : ModelBase;

        /// <summary>
        /// 根据id获取记录
        /// </summary>
        /// <returns>记录</returns>
        /// <param name="id">id.</param>
        T GetByKeyId<T>(object id) where T : ModelBase;

        /// <summary>
        /// 条件获取记录集合
        /// </summary>
        /// <returns>集合</returns>
        /// <param name="predicate">筛选条件.</param>
        List<T> GetList<T>(Expression<Func<T, bool>> predicate = null) where T : ModelBase;

        /// <summary>
        /// 条件获取记录第一条或者默认
        /// </summary>
        /// <returns>记录</returns>
        /// <param name="predicate">筛选条件.</param>
        T GetFirstOrDefault<T>(Expression<Func<T, bool>> predicate = null) where T : ModelBase;

        /// <summary>
        /// 创建一条记录
        /// </summary>
        /// <returns>操作是否成功.</returns>
        /// <param name="entity">实体类记录.</param>
        bool Create<T>(T entity) where T : ModelBase;

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <returns>IQueryable</returns>
        /// <param name="predicate">筛选条件.</param>
        IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate = null) where T : ModelBase;

        /// <summary>
        /// 根据记录
        /// </summary>
        /// <returns>操作是否成功.</returns>
        /// <param name="entity">实体类记录.</param>
        bool Update<T>(T entity) where T : ModelBase;

        #endregion Methods
    }
}