using System;

namespace MasterChief.DotNet.Core.Log
{
    /// <summary>
    /// 日志记录接口
    /// </summary>
    public interface ILogService
    {
        #region Methods

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <param name="message">日志信息</param>
        void Debug(string message);

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Debug(string message, Exception ex);

        /// <summary>
        /// Error记录
        /// </summary>
        /// <param name="message">日志信息</param>
        void Error(string message);

        /// <summary>
        /// Error记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Error(string message, Exception ex);

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <param name="message">日志信息</param>
        void Fatal(string message);

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Fatal(string message, Exception ex);

        /// <summary>
        /// Info记录
        /// </summary>
        /// <param name="message">日志信息</param>
        void Info(string message);

        /// <summary>
        /// Info记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Info(string message, Exception ex);

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <param name="message">日志信息</param>
        void Warn(string message);

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Warn(string message, Exception ex);

        #endregion Methods
    }
}