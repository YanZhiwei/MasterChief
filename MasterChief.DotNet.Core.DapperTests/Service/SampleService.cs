using MasterChief.DotNet.Core.Contract;
using MasterChief.DotNet.Core.DapperTests.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace MasterChief.DotNet.Core.DapperTests.Service
{
    /// <summary>
    /// SampleService
    /// </summary>
    /// <seealso cref="MasterChief.DotNet.Core.DapperTests.Service.ISampleService" />
    public class SampleService : ISampleService
    {
        private readonly IDatabaseContextFactory _contextFactory = null;

        public SampleService(IDatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="samle">EFSample</param>
        /// <returns></returns>
        public bool Create(EFSample samle)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.Create<EFSample>(samle);
            }
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public EFSample GetFirstOrDefault(Expression<Func<EFSample, bool>> predicate = null)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.GetFirstOrDefault<EFSample>(predicate);
            }
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public EFSample GetByKeyID(Guid id)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.GetByKeyID<EFSample>(id);
            }
        }

        /// <summary>
        /// 条件查询集合
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public List<EFSample> GetList(Expression<Func<EFSample, bool>> predicate = null)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.GetList<EFSample>(predicate);
            }
        }

        /// <summary>
        /// 添加判断是否存在
        /// </summary>
        /// <typeparam name="EFSample">The type of the f sample.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public bool Exist(Expression<Func<EFSample, bool>> predicate = null)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.Exist<EFSample>(predicate);
            }
        }

        /// <summary>
        /// 脚本查询
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        public List<EFSample> SqlQuery(string sql, DbParameter[] parameter)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.SqlQuery<EFSample>(sql, parameter)?.ToList();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <returns></returns>
        public bool Update(EFSample sample)
        {
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                return dbcontext.Update(sample);
            }
        }

        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="sample2">The sample2.</param>
        /// <returns></returns>
        public bool CreateWithTransaction(EFSample sample, EFSample sample2)
        {
            bool result = true;
            using (IDbContext dbcontext = _contextFactory.Create())
            {
                try
                {
                    dbcontext.BeginTransaction();//开启事务
                    dbcontext.Create(sample);
                    dbcontext.Create(sample2);
                    dbcontext.Commit();
                }
                catch (Exception)
                {
                    dbcontext.Rollback();
                    result = false;
                }
            }

            return result;
        }
    }
}