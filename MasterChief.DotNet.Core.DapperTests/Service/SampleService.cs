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
        private readonly IDatabaseContextFactory _contextFactory;

        public SampleService(IDatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="sample">EFSample</param>
        /// <returns></returns>
        public bool Create(EfSample sample)
        {
            using (IDbContext context = _contextFactory.Create())
            {
                return context.Create(sample);
            }
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public EfSample GetFirstOrDefault(Expression<Func<EfSample, bool>> predicate = null)
        {
            using (IDbContext context = _contextFactory.Create())
            {
                return context.GetFirstOrDefault(predicate);
            }
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public EfSample GetByKeyId(Guid id)
        {
            using (IDbContext context = _contextFactory.Create())
            {
                return context.GetByKeyId<EfSample>(id);
            }
        }

        /// <summary>
        /// 条件查询集合
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public List<EfSample> GetList(Expression<Func<EfSample, bool>> predicate = null)
        {
            using (IDbContext context = _contextFactory.Create())
            {
                return context.GetList(predicate);
            }
        }

        /// <summary>
        /// 添加判断是否存在
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public bool Exist(Expression<Func<EfSample, bool>> predicate = null)
        {
            using (IDbContext context = _contextFactory.Create())
            {
                return context.Exist(predicate);
            }
        }

        /// <summary>
        /// 脚本查询
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameter">DbParameter[]</param>
        /// <returns></returns>
        public List<EfSample> SqlQuery(string sql, DbParameter[] parameter)
        {
            using (IDbContext context = _contextFactory.Create())
            {
                return context.SqlQuery<EfSample>(sql, parameter)?.ToList();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <returns></returns>
        public bool Update(EfSample sample)
        {
            using (IDbContext context = _contextFactory.Create())
            {
                return context.Update(sample);
            }
        }

        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="sample2">The sample2.</param>
        /// <returns></returns>
        public bool CreateWithTransaction(EfSample sample, EfSample sample2)
        {
            bool result;
            using (IDbContext context = _contextFactory.Create())
            {
                try
                {
                    context.BeginTransaction();//开启事务
                    context.Create(sample);
                    context.Create(sample2);
                    context.Commit();
                    result = false;
                }
                catch (Exception)
                {
                    context.Rollback();
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        public bool Delete(EfSample sample)
        {
            using (IDbContext context = _contextFactory.Create())
            {
                return context.Delete(sample);
            }
        }
    }
}