using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MasterChief.DotNet4.Utilities.Core;

namespace MasterChief.DotNet4.Utilities.Policy
{
    /// <summary>
    ///     重试机制
    /// </summary>
    public sealed class RetryPolicy : IRetryPolicy
    {
        /// <summary>
        ///     执行重试
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="keySelector">需要执行委托</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="expectedResult">期待结果</param>
        /// <param name="maxAttemptCount">重试次数，默认三次</param>
        /// <param name="isThrowException">是否支持异常抛出</param>
        /// <returns>
        ///     返回结果
        /// </returns>
        /// <exception cref="AggregateException"></exception>
        public TResult Execute<TResult>(Func<TResult> keySelector, TimeSpan retryInterval, TResult expectedResult,
            int maxAttemptCount = 3,
            bool isThrowException = false)
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

        /// <summary>
        ///     执行重试
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="keySelector">需要执行委托</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="expectedResult">期待结果</param>
        /// <param name="maxAttemptCount">重试次数，默认三次</param>
        /// <param name="isThrowException">是否支持异常抛出</param>
        /// <returns>
        ///     返回结果
        /// </returns>
        /// <exception cref="AggregateException"></exception>
        public TResult Execute<TResult>(Func<TResult> keySelector, TimeSpan retryInterval,
            Predicate<TResult> expectedResult, int maxAttemptCount = 3,
            bool isThrowException = false)
        {
            var actualResult = default(TResult);
            var exceptions = new List<Exception>();

            for (var i = 0; i < maxAttemptCount; i++)
                try
                {
                    if (i > 0)
                        Thread.Sleep(retryInterval);
                    actualResult = keySelector();
                    if (expectedResult(actualResult)) return actualResult;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }

            if (isThrowException && exceptions.Any())
                throw new AggregateException(exceptions);
            return actualResult;
        }

        /// <summary>
        ///     设置具体错误时候重试
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="keySelector">需要执行委托</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="specialError">具体错误条件成立</param>
        /// <param name="maxAttemptCount">重试次数，默认三次</param>
        /// <param name="isThrowException">是否支持异常抛出</param>
        /// <returns>
        ///     返回结果
        /// </returns>
        public TResult ExecuteWhen<TResult>(Func<TResult> keySelector, TimeSpan retryInterval,
            Predicate<TResult> specialError, int maxAttemptCount = 3,
            bool isThrowException = false)
        {
            var actualResult = default(TResult);
            Exception occurException = null;
            var count = 0;
            do
            {
                try
                {
                    if (count > 0)
                        Thread.Sleep(retryInterval);
                    actualResult = keySelector();
                    if (specialError(actualResult))
                        count++;
                    else
                        count = int.MaxValue;
                }
                catch (Exception ex)
                {
                    occurException = ex;
                    count = int.MaxValue;
                }
            } while (count < maxAttemptCount);

            if (isThrowException && occurException != null)
                throw occurException;
            return actualResult;
        }
    }
}