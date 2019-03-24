using System;
using log4net;

namespace MasterChief.DotNet.Core.Log
{
    /// <summary>
    ///     基于Log4Net的文件日志记录
    /// </summary>
    public sealed class FileLogService : ILogService
    {
        #region Constructors

        static FileLogService()
        {
            DebugLogger = LogManager.GetLogger(DebugLoggerName);
            InfoLogger = LogManager.GetLogger(InfoLoggerName);
            WarnLogger = LogManager.GetLogger(WarnLoggerName);
            ErrorLogger = LogManager.GetLogger(ErrorLoggerName);
            FatalLogger = LogManager.GetLogger(FatalLoggerName);
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     The debug logger name
        /// </summary>
        public const string DebugLoggerName = "DEBUG_FileLogger";

        /// <summary>
        ///     The error logger name
        /// </summary>
        public const string ErrorLoggerName = "ERROR_FileLogger";

        /// <summary>
        ///     The fatal logger name
        /// </summary>
        public const string FatalLoggerName = "FATAL_FileLogger";

        /// <summary>
        ///     The information logger name
        /// </summary>
        public const string InfoLoggerName = "INFO_FileLogger";

        /// <summary>
        ///     The warn logger name
        /// </summary>
        public const string WarnLoggerName = "WARN_FileLogger";

        /// <summary>
        ///     The debug logger
        /// </summary>
        public static readonly ILog DebugLogger;

        /// <summary>
        ///     The error logger
        /// </summary>
        public static readonly ILog ErrorLogger;

        /// <summary>
        ///     The fatal logger
        /// </summary>
        public static readonly ILog FatalLogger;

        /// <summary>
        ///     The information logger
        /// </summary>
        public static readonly ILog InfoLogger;

        /// <summary>
        ///     The warn logger
        /// </summary>
        public static readonly ILog WarnLogger;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Debug记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Debug(string message)
        {
            if (DebugLogger.IsDebugEnabled) DebugLogger.Debug(message);
        }

        /// <summary>
        ///     Debug记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Debug(string message, Exception ex)
        {
            if (DebugLogger.IsDebugEnabled) DebugLogger.Debug(message, ex);
        }

        /// <summary>
        ///     Error记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Error(string message)
        {
            if (ErrorLogger.IsErrorEnabled) ErrorLogger.Error(message);
        }

        /// <summary>
        ///     Error记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Error(string message, Exception ex)
        {
            if (ErrorLogger.IsErrorEnabled) ErrorLogger.Error(message, ex);
        }

        /// <summary>
        ///     Fatal记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Fatal(string message)
        {
            if (FatalLogger.IsFatalEnabled) FatalLogger.Fatal(message);
        }

        /// <summary>
        ///     Fatal记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Fatal(string message, Exception ex)
        {
            if (FatalLogger.IsFatalEnabled) FatalLogger.Fatal(message, ex);
        }

        /// <summary>
        ///     Info记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Info(string message)
        {
            if (InfoLogger.IsInfoEnabled) InfoLogger.Info(message);
        }

        /// <summary>
        ///     Info记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Info(string message, Exception ex)
        {
            if (InfoLogger.IsInfoEnabled) InfoLogger.Info(message, ex);
        }

        /// <summary>
        ///     Warn记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Warn(string message)
        {
            if (WarnLogger.IsWarnEnabled) WarnLogger.Warn(message);
        }

        /// <summary>
        ///     Warn记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Warn(string message, Exception ex)
        {
            if (WarnLogger.IsWarnEnabled) WarnLogger.Warn(message, ex);
        }

        #endregion Methods
    }
}