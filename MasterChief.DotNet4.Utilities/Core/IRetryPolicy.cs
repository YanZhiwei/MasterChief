using System;

namespace MasterChief.DotNet4.Utilities.Core
{
    /// <summary>
    /// 重试接口
    /// </summary>
    internal interface IRetryPolicy
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
        /// <returns>返回结果</returns>
        TResult Execute<TResult>(Func<TResult> keySelector,
            TimeSpan retryInterval,
            TResult expectedResult,
            int maxAttemptCount = 3,
            bool isThrowException = false);

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
        TResult Execute<TResult>(Func<TResult> keySelector,
            TimeSpan retryInterval,
            Predicate<TResult> expectedResult,
            int maxAttemptCount = 3,
            bool isThrowException = false
        );

        /// <summary>
        ///     设置具体错误时候重试
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="keySelector">需要执行委托</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="specialError">具体错误条件成立</param>
        /// <param name="maxAttemptCount">重试次数，默认三次</param>
        /// <param name="isThrowException">是否支持异常抛出</param>
        /// <returns>返回结果</returns>
        TResult ExecuteWhen<TResult>(Func<TResult> keySelector,
            TimeSpan retryInterval,
            Predicate<TResult> specialError,
            int maxAttemptCount = 3,
            bool isThrowException = false
        );
    }
}