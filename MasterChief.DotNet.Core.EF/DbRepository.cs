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
    public class DbRepository<T> : IRepository<T>
        where T : ModelBase
    {
        #region Fields

        public virtual IQueryable<T> Table => Entities;
        public virtual IQueryable<T> TableNoTracking => Entities.AsNoTracking();

        protected virtual IDbSet<T> Entities => _dbContext.Set<T>();

        private readonly DbContext _dbContext;

        #endregion Fields

        #region Constructors

        public DbRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Constructors

        #region Methods

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

        public bool Exist(Expression<Func<T, bool>> keySelector = null)
        {
            return keySelector == null ? _dbContext.Set<T>().Any() : _dbContext.Set<T>().Any(keySelector);
        }

        public List<T> Get(Expression<Func<T, bool>> keySelector = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            foreach (Expression<Func<T, object>> include in includes)
            {
                query = query.Include(include);
            }
            if (keySelector != null)
            {
                query = query.Where(keySelector);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.ToList();
        }

        public T Get(object id)
        {
            T finded = _dbContext.Set<T>().Find(id);
            if (finded != null)
            {
                _dbContext.Entry(finded).State = EntityState.Detached;
            }
            return finded;
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> keySelector = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            foreach (Expression<Func<T, object>> include in includes)
            {
                query = query.Include(include);
            }

            return query.FirstOrDefault(keySelector);
        }

        public bool Insert(T entity)
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

        public bool Insert(IEnumerable<T> entities)
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

        public IQueryable<T> Query(Expression<Func<T, bool>> keySelector = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (keySelector != null)
            {
                query = query.Where(keySelector);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }

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