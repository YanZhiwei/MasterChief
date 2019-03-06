using System;

namespace MasterChief.DotNet.Core.Contract
{
    /// <summary>
    /// 数据访问层异常类
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class DataAccessException : Exception
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataAccessException()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        public DataAccessException(string message) : base(CustomizeExceptionMessage(message, null))
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="inner">内置异常</param>
        public DataAccessException(string message, Exception inner) : base(CustomizeExceptionMessage(message, inner), inner)
        {
        }

        private static string CustomizeExceptionMessage(string message, Exception inner)
        {
            if (string.IsNullOrEmpty(message) && inner != null)
            {
                message = inner.Message;
            }
            else if (string.IsNullOrEmpty(message))
            {
                message = "未知数据访问层异常，详情请查看日志信息。";
            }
            return string.Format("数据访问层异常：{0}", message);
        }

        #endregion Constructors
    }
}
