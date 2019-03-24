using System;
using log4net;
using MasterChief.DotNet.Core.Log;

namespace MasterChief.DotNet.Core.KafkaLog
{
    /// <summary>
    ///     Kafka日志服务
    /// </summary>
    public sealed class KafkaLogService : ILogService
    {
        #region Constructors

        /// <summary>
        ///     Initializes the <see cref="FileLogService" /> class.
        /// </summary>
        static KafkaLogService()
        {
            KafkaLogger = LogManager.GetLogger(KafkaLoggerName);
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     Kafka logger name
        /// </summary>
        public const string KafkaLoggerName = "KafkaLogger";

        /// <summary>
        ///     Kafka logger
        /// </summary>
        public static readonly ILog KafkaLogger;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Debug记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Debug(string message)
        {
            if (KafkaLogger.IsDebugEnabled) KafkaLogger.Debug(message);
        }

        /// <summary>
        ///     Debug记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Debug(string message, Exception ex)
        {
            if (KafkaLogger.IsDebugEnabled) KafkaLogger.Debug(message, ex);
        }

        /// <summary>
        ///     Error记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Error(string message)
        {
            if (KafkaLogger.IsErrorEnabled) KafkaLogger.Error(message);
        }

        /// <summary>
        ///     Error记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Error(string message, Exception ex)
        {
            if (KafkaLogger.IsErrorEnabled) KafkaLogger.Error(message, ex);
        }

        /// <summary>
        ///     Fatal记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Fatal(string message)
        {
            if (KafkaLogger.IsFatalEnabled) KafkaLogger.Fatal(message);
        }

        /// <summary>
        ///     Fatal记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Fatal(string message, Exception ex)
        {
            if (KafkaLogger.IsFatalEnabled) KafkaLogger.Fatal(message, ex);
        }

        /// <summary>
        ///     Info记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Info(string message)
        {
            if (KafkaLogger.IsInfoEnabled) KafkaLogger.Info(message);
        }

        /// <summary>
        ///     Info记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Info(string message, Exception ex)
        {
            if (KafkaLogger.IsInfoEnabled) KafkaLogger.Info(message, ex);
        }

        /// <summary>
        ///     Warn记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Warn(string message)
        {
            if (KafkaLogger.IsWarnEnabled) KafkaLogger.Warn(message);
        }

        /// <summary>
        ///     Warn记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Warn(string message, Exception ex)
        {
            if (KafkaLogger.IsWarnEnabled) KafkaLogger.Warn(message, ex);
        }

        #endregion Methods
    }
}