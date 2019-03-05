namespace MasterChief.DotNet.Core.EF
{
    using MasterChief.DotNet.Core.Contract;
    using MasterChief.DotNet.Core.EF.Helper;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// 实现Repository通用泛型数据访问模式
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public abstract class EfDbContextBase : DbContext, IDbContext
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">dbConnection</param>
        protected EfDbContextBase(DbConnection dbConnection)
            : base(dbConnection, true)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns>操作是否成功</returns>
        /// <param name="entity">需要操作的实体类.</param>
        public bool Create<T>(T entity)
            where T : ModelBase
        {
            bool result = false;
            try
            {
                Entry<T>(entity).State = EntityState.Added;
                result = SaveChanges() > 0;
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
        public bool Create<T>(IEnumerable<T> entities)
            where T : ModelBase
        {
            bool result = false;
            try
            {
                foreach (T entity in entities)
                {
                    Entry<T>(entity).State = EntityState.Added;
                }

                result = SaveChanges() > 0;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
            return result;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns>操作是否成功</returns>
        /// <param name="entity">需要操作的实体类.</param>
        public bool Delete<T>(T entity)
            where T : ModelBase
        {
            bool result = false;
            try
            {
                Entry<T>(entity).State = EntityState.Deleted;
                result = SaveChanges() > 0;
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
        public bool Delete<T>(IEnumerable<T> entities)
            where T : ModelBase
        {
            bool result = false;
            try
            {
                foreach (T entity in entities)
                {
                    Entry<T>(entity).State = EntityState.Deleted;
                }

                result = SaveChanges() > 0;
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
        public bool Exist<T>(Expression<Func<T, bool>> predicate = null)
            where T : ModelBase
        {
            return predicate == null ? Set<T>().Any() : Set<T>().Any(predicate);
        }

        /// <summary>
        /// 根据id获取记录
        /// </summary>
        /// <returns>记录</returns>
        /// <param name="id">id.</param>
        public T Get<T>(object id)
            where T : ModelBase
        {
            return Set<T>().Find(id);
        }

        /// <summary>
        /// 条件获取记录集合
        /// </summary>
        /// <returns>集合</returns>
        /// <param name="predicate">筛选条件.</param>
        public List<T> Get<T>(Expression<Func<T, bool>> predicate = null)
            where T : ModelBase
        {
            IQueryable<T> query = Set<T>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.ToList();
        }

        /// <summary>
        /// 条件获取记录第一条或者默认
        /// </summary>
        /// <returns>记录</returns>
        /// <param name="predicate">筛选条件.</param>
        public T GetFirstOrDefault<T>(Expression<Func<T, bool>> predicate = null)
            where T : ModelBase
        {
            IQueryable<T> query = Set<T>();

            return query.FirstOrDefault(predicate);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <returns>IQueryable</returns>
        /// <param name="predicate">筛选条件.</param>
        public IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate = null)
            where T : ModelBase
        {
            IQueryable<T> query = Set<T>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query;
        }

        /// <summary>
        /// 根据记录
        /// </summary>
        /// <returns>操作是否成功.</returns>
        /// <param name="entity">实体类记录.</param>
        public bool Update<T>(T entity)
            where T : ModelBase
        {
            bool result = false;
            try
            {
                DbSet<T> set = Set<T>();
                set.Attach(entity);
                Entry<T>(entity).State = EntityState.Modified;
                result = SaveChanges() > 0;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
            return result;
        }

        #endregion Methods
    }
}