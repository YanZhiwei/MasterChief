using System;
using System.Threading;
using System.Threading.Tasks;

namespace MasterChief.DotNet4._5.Utilities.Core
{
    /// <summary>
    /// 基于Task的定期任务
    /// </summary>
    public static class PeriodicTask
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="action">业务逻辑</param>
        /// <param name="period">间隔周期</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public static async Task Run(Action action, TimeSpan period, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(period, cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                {
                    action();
                }
            }
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="action">业务逻辑</param>
        /// <param name="period">间隔周期</param>
        /// <returns>Task</returns>
        public static Task Run(Action action, TimeSpan period)
        {
            return Run(action, period, CancellationToken.None);
        }
    }
}