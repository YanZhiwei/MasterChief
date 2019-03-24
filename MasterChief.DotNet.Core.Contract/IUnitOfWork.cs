using System.Data;

namespace MasterChief.DotNet.Core.Contract
{
    /// <summary>
    ///     业务单元操作接口
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        ///     获取 是否开启事务提交
        /// </summary>
        bool TransactionEnabled { get; }

        /// <summary>
        ///     显式开启数据上下文事务
        /// </summary>
        /// <param name="isolationLevel">指定连接的事务锁定行为</param>
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);

        /// <summary>
        ///     提交当前上下文的事务更改
        /// </summary>
        void Commit();

        /// <summary>
        ///     显式回滚事务，仅在显式开启事务后有用
        /// </summary>
        void Rollback();
    }
}