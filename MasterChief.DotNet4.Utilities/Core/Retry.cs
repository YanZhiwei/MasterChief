using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MasterChief.DotNet4.Utilities.Core
{
    /// <summary>
    ///     重试
    /// </summary>
    public sealed class Retry
    {
        #region Methods

        /// <summary>
        ///     执行重试
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="keySelector">需要执行委托</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="expectedResult">期待结果</param>
        /// <param name="maxAttemptCount">重试次数，默认三次</param>
        /// <param name="isThrowException">是否支持异常抛出</param>
        /// <returns>返回结果</returns>
        public static TResult Execute<TResult>(Func<TResult> keySelector,
            TimeSpan retryInterval,
            TResult expectedResult,
            int maxAttemptCount = 3,
            bool isThrowException = false
        )
        {
            TResult actualResult = default;
            var exceptions = new List<Exception>();

            for (var i = 0; i < maxAttemptCount; i++)
                try
                {
                    if (i > 0)
                        Thread.Sleep(retryInterval);
                    actualResult = keySelector();
                    if (actualResult.Equals(expectedResult)) return actualResult;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }

            if (isThrowException && exceptions.Any())
                throw new AggregateException(exceptions);
            return actualResult;
        }

        #endregion Methods
    }
}