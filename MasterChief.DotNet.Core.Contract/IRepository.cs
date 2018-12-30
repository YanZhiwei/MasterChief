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
        #region Properties

        IQueryable<T> Table
        {
            get;
        }

        IQueryable<T> TableNoTracking
        {
            get;
        }

        #endregion Properties

        #region Methods

        bool Delete(T entity);

        bool Delete(IEnumerable<T> entities);

        bool Exist(Expression<Func<T, bool>> keySelector = null);

        T Get(object id);

        List<T> Get(Expression<Func<T, bool>> keySelector = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes);

        T GetFirstOrDefault(Expression<Func<T, bool>> keySelector = null, params Expression<Func<T, object>>[] includes);

        bool Insert(T entity);

        bool Insert(IEnumerable<T> entities);

        IQueryable<T> Query(Expression<Func<T, bool>> keySelector = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        bool Update(T entity);

        #endregion Methods
    }
}