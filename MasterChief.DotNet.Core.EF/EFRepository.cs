namespace MasterChief.DotNet.Core.EF
{
    using MasterChief.DotNet.Core.Contract;
    using MasterChief.DotNet.Core.EF.Helper;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// EF 仓储实现
    /// </summary>
    public class EfRepository : IRepository
    {
        #region Fields

        protected virtual IDbSet<T> Entities => _dbContext.Set<T>();

        protected readonly DbContext _dbContext;

        #endregion Fields

        #region Constructors

        public EfRepository(IDbContext dbContext)
        {
            _dbContext = (DbContext)dbContext;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns>操作是否成功</returns>
        /// <param name="entity">需要操作的实体类.</param>
        public bool Delete(T entity)
        {
            bool result = false;
            try
            {
                _dbContext.Entry<T>(entity).State = EntityState.Deleted;
                result = _dbContext.SaveChanges() > 0;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
            return result;
        }

        /// <summary>
        /// 条件删除记录
        /// </summary>
        /// <returns>操作是否成功</returns>
        /// <param name="entities">需要操作的集合.</param>
        public bool Delete(IEnumerable<T> entities)
        {
            bool result = false;
            try
            {
                foreach (T entity in entities)
                {
                    _dbContext.Entry<T>(entity).State = EntityState.Deleted;
                }

                result = _dbContext.SaveChanges() > 0;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
            return result;
        }

        /// <summary>
        /// 条件判断是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        /// <param name="predicate">判断条件委托</param>
        public bool Exist(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? _dbContext.Set<T>().Any() : _dbContext.Set<T>().Any(predicate);
        }

        /// <summary>
        /// 条件获取记录集合
        /// </summary>
        /// <returns>集合</returns>
        /// <param name="predicate">筛选条件.</param>
        public List<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            //foreach (Expression<Func<T, object>> include in includes)
            //{
            //    query = query.Include(include);
            //}
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            //if (orderBy != null)
            //{
            //    query = orderBy(query);
            //}
            return query.ToList();
        }

        /// <summary>
        /// 根据id获取记录
        /// </summary>
        /// <returns>记录</returns>
        /// <param name="id">id.</param>
        public T Get(object id)
        {
            T finded = _dbContext.Set<T>().Find(id);
            if (finded != null)
            {
                _dbContext.Entry(finded).State = EntityState.Detached;
            }
            return finded;
        }

        /// <summary>
        /// 条件获取记录第一条或者默认
        /// </summary>
        /// <returns>记录</returns>
        /// <param name="predicate">筛选条件.</param>
        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            //foreach (Expression<Func<T, object>> include in includes)
            //{
            //    query = query.Include(include);
            //}

            return query.FirstOrDefault(predicate);
        }

        /// <summary>
        /// 创建一条记录
        /// </summary>
        /// <returns>操作是否成功.</returns>
        /// <param name="entity">实体类记录.</param>
        public bool Create(T entity)
        {
            bool result = false;
            try
            {
                _dbContext.Set<T>().Add(entity);
                _dbContext.Entry<T>(entity).State = EntityState.Added;
                result = _dbContext.SaveChanges() > 0;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
            return result;
        }

        /// <summary>
        /// 创建记录集合
        /// </summary>
        /// <returns>操作是否成功.</returns>
        /// <param name="entities">实体类集合.</param>
        public bool Create(IEnumerable<T> entities)
        {
            bool result = false;
            try
            {
                foreach (T entity in entities)
                {
                    _dbContext.Entry<T>(entity).State = EntityState.Added;
                }

                result = _dbContext.SaveChanges() > 0;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
            return result;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <returns>IQueryable</returns>
        /// <param name="predicate">筛选条件.</param>
        public IQueryable<T> Query(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            //if (orderBy != null)
            //{
            //    query = orderBy(query);
            //}

            return query;
        }

        /// <summary>
        /// 根据记录
        /// </summary>
        /// <returns>操作是否成功.</returns>
        /// <param name="entity">实体类记录.</param>
        public bool Update(T entity)
        {
            bool result = false;
            try
            {
                if (_dbContext.Entry<T>(entity).State == EntityState.Detached)
                {
                    Detached(entity);
                }
                _dbContext.Set<T>().Attach(entity);
                _dbContext.Entry<T>(entity).State = EntityState.Modified;
                result = _dbContext.SaveChanges() > 0;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
            return result;
        }

        private bool Detached(T entity)
        {
            ObjectContext objectContext = ((IObjectContextAdapter)_dbContext).ObjectContext;
            ObjectSet<T> entitySet = objectContext.CreateObjectSet<T>();
            EntityKey entityKey = objectContext.CreateEntityKey(entitySet.EntitySet.Name, entity);
            bool exists = objectContext.TryGetObjectByKey(entityKey, out object foundSet);
            if (exists)
            {
                objectContext.Detach(foundSet);
            }
            return exists;
        }

        #endregion Methods
    }
}