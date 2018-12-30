using log4net;
using System;

namespace MasterChief.DotNet.Core.Log
{
    /// <summary>
    /// 基于Log4Net的文件日志记录
    /// </summary>
    public sealed class FileLogService : ILogService
    {
        #region Fields

        /// <summary>
        /// The debug logger name
        /// </summary>
        public const string DEBUGLoggerName = "DEBUG_FileLogger";

        /// <summary>
        /// The error logger name
        /// </summary>
        public const string ERRORLoggerName = "ERROR_FileLogger";

        /// <summary>
        /// The fatal logger name
        /// </summary>
        public const string FATALLoggerName = "FATAL_FileLogger";

        /// <summary>
        /// The information logger name
        /// </summary>
        public const string INFOLoggerName = "INFO_FileLogger";

        /// <summary>
        /// The warn logger name
        /// </summary>
        public const string WARNLoggerName = "WARN_FileLogger";

        /// <summary>
        /// The debug logger
        /// </summary>
        public static readonly ILog DebugLogger = null;

        /// <summary>
        /// The error logger
        /// </summary>
        public static readonly ILog ERRORLogger = null;

        /// <summary>
        /// The fatal logger
        /// </summary>
        public static readonly ILog FATALLogger = null;

        /// <summary>
        /// The information logger
        /// </summary>
        public static readonly ILog INFOLogger = null;

        /// <summary>
        /// The warn logger
        /// </summary>
        public static readonly ILog WARNLogger = null;

        #endregion Fields

        #region Constructors

        static FileLogService()
        {
            DebugLogger = LogManager.GetLogger(DEBUGLoggerName);
            INFOLogger = LogManager.GetLogger(INFOLoggerName);
            WARNLogger = LogManager.GetLogger(WARNLoggerName);
            ERRORLogger = LogManager.GetLogger(ERRORLoggerName);
            FATALLogger = LogManager.GetLogger(FATALLoggerName);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Debug(string message)
        {
            if (DebugLogger.IsDebugEnabled)
            {
                DebugLogger.Debug(message);
            }
        }

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Debug(string message, Exception ex)
        {
            if (DebugLogger.IsDebugEnabled)
            {
                DebugLogger.Debug(message, ex);
            }
        }

        /// <summary>
        /// Error记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Error(string message)
        {
            if (ERRORLogger.IsErrorEnabled)
            {
                ERRORLogger.Error(message);
            }
        }

        /// <summary>
        /// Error记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Error(string message, Exception ex)
        {
            if (ERRORLogger.IsErrorEnabled)
            {
                ERRORLogger.Error(message, ex);
            }
        }

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Fatal(string message)
        {
            if (FATALLogger.IsFatalEnabled)
            {
                FATALLogger.Fatal(message);
            }
        }

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Fatal(string message, Exception ex)
        {
            if (FATALLogger.IsFatalEnabled)
            {
                FATALLogger.Fatal(message, ex);
            }
        }

        /// <summary>
        /// Info记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Info(string message)
        {
            if (INFOLogger.IsInfoEnabled)
            {
                INFOLogger.Info(message);
            }
        }

        /// <summary>
        /// Info记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Info(string message, Exception ex)
        {
            if (INFOLogger.IsInfoEnabled)
            {
                INFOLogger.Info(message, ex);
            }
        }

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Warn(string message)
        {
            if (WARNLogger.IsWarnEnabled)
            {
                WARNLogger.Warn(message);
            }
        }

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Warn(string message, Exception ex)
        {
            if (WARNLogger.IsWarnEnabled)
            {
                WARNLogger.Warn(message, ex);
            }
        }

        #endregion Methods
    }
}