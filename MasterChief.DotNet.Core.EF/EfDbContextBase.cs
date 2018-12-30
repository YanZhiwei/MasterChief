using MasterChief.DotNet.Core.Contract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace MasterChief.DotNet.Core.EF
{
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
        public EfDbContextBase(DbConnection dbConnection)
            : base(dbConnection, true)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
        }

        #endregion Constructors

        #region Methods

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            TransactionalBehavior transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;
            int result = parameters != null ? Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters) : Database.ExecuteSqlCommand(transactionalBehavior, sql);
            if (timeout.HasValue)
            {
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            return result;
        }

        public IList<T> ExecuteStoredProcedureList<T>(string commandText, params object[] parameters)
            where T : ModelBase
        {
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    DbParameter paramter = parameters[i] as DbParameter;
                    if (paramter == null)
                    {
                        throw new ArgumentException("不支持的参数类型。");
                    }

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + paramter.ParameterName;
                    if (paramter.Direction == ParameterDirection.InputOutput || paramter.Direction == ParameterDirection.Output)
                    {
                        commandText += " output";
                    }
                }
            }

            List<T> result = Database.SqlQuery<T>(commandText, parameters).ToList();

            bool acd = Configuration.AutoDetectChangesEnabled;
            try
            {
                Configuration.AutoDetectChangesEnabled = false;

                for (int i = 0; i < result.Count; i++)
                {
                    result[i] = AttachEntityToContext(result[i]);
                }
            }
            finally
            {
                Configuration.AutoDetectChangesEnabled = acd;
            }

            return result;
        }

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : ModelBase
        {
            return Database.SqlQuery<T>(sql, parameters);
        }

        protected virtual T AttachEntityToContext<T>(T entity)
            where T : ModelBase
        {
            T alreadyAttached = Set<T>().Local.FirstOrDefault(x => x.ID.Equals(entity.ID));
            if (alreadyAttached == null)
            {
                Set<T>().Attach(entity);
                return entity;
            }
            return alreadyAttached;
        }

        #endregion Methods
    }
}