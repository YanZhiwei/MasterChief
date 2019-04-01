using System;
using MasterChief.DotNet4.Utilities.Result;

namespace MasterChief.DotNet.ProjectTemplate.WebApi.Result
{
    /// <summary>
    ///     WebApi 结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public sealed class ApiResult<T> : BasicResult<T>
    {
        #region Properties

        /// <summary>
        ///     操作结果
        /// </summary>
        public bool State { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="data">返回数据</param>
        /// <param name="state">操作结果</param>
        public ApiResult(string message, T data, bool state)
            : base(message, data)
        {
            State = state;
        }

        /// <summary>
        ///     默认无参构造函数
        /// </summary>
        public ApiResult()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     失败结果
        /// </summary>
        /// <param name="message">失败内容</param>
        /// <returns>OperatedResult</returns>
        public static ApiResult<T> Fail(string message)
        {
            var failResult = new ApiResult<T>(message, default, false);
            return failResult;
        }

        /// <summary>
        ///     成功结果
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <param name="data">成功时候需要返回的数据对象</param>
        /// <returns>OperatedResult</returns>
        public static ApiResult<T> Success(string message, T data)
        {
            var successResult = new ApiResult<T>(message, data, true);
            return successResult;
        }

        /// <summary>
        ///     成功结果
        /// </summary>
        /// <param name="data">成功时候需要返回的数据对象</param>
        /// <returns>OperatedResult</returns>
        public static ApiResult<T> Success(T data)
        {
            var successResult = new ApiResult<T>(null, data, true);
            return successResult;
        }

        #endregion Methods
    }

    /// <summary>
    ///     WebApi 结果
    /// </summary>
    [Serializable]
    public sealed class ApiResult : BasicResult<string>
    {
        #region Constructors

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="data">返回数据</param>
        /// <param name="state">操作结果</param>
        public ApiResult(string message, string data, bool state)
            : base(message, data)
        {
            State = state;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     操作结果
        /// </summary>
        public bool State { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     失败结果
        /// </summary>
        /// <param name="message">失败内容</param>
        /// <returns>OperatedResult</returns>
        public static ApiResult Fail(string message)
        {
            var failResult = new ApiResult(message, null, false);
            return failResult;
        }

        /// <summary>
        ///     成功结果
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <param name="data">成功时候需要返回的数据对象</param>
        /// <returns>OperatedResult</returns>
        public static ApiResult Success(string message, string data)
        {
            var successResult = new ApiResult(message, data, true);
            return successResult;
        }

        /// <summary>
        ///     成功结果
        /// </summary>
        /// <param name="data">成功时候需要返回的数据对象</param>
        /// <returns>OperatedResult</returns>
        public static ApiResult Success(string data)
        {
            var successResult = new ApiResult(null, data, true);
            return successResult;
        }

        /// <summary>
        ///     成功结果
        /// </summary>
        /// <returns>OperatedResult</returns>
        public static ApiResult Success()
        {
            var successResult = new ApiResult(null, null, true);
            return successResult;
        }

        #endregion Methods
    }
}