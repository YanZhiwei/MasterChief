using System;
using System.Collections.Generic;
using System.IO;
using log4net.Appender;
using log4net.Core;

namespace MasterChief.DotNet.Core.Log
{
    /// <summary>
    ///     自动删除多少天日志 RollingFileAppender
    /// </summary>
    /// <seealso cref="log4net.Appender.RollingFileAppender" />
    public class AutoDeleteFileAppender : RollingFileAppender
    {
        #region Properties

        /// <summary>
        ///     最多保留多少天日志
        /// </summary>
        public int MaxNumberOfDays { get; set; }

        #endregion Properties

        #region Fields

        /// <summary>
        /// appender 类型
        /// </summary>
        public static Type DeclaringType { get; } = typeof(AutoDeleteFileAppender);

        private string _baseDirectory = string.Empty;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Performs any required rolling before outputting the next event
        /// </summary>
        /// <remarks>
        ///     Handles append time behavior for RollingFileAppender.  This checks
        ///     if a roll over either by date (checked first) or time (checked second)
        ///     is need and then appends to the file last.
        /// </remarks>
        protected override void AdjustFileBeforeAppend()
        {
            var now = DateTime.Now;
            int curHour = now.Hour, curMinu = now.Minute, curSec = now.Second;

            if (curHour == 23 && curMinu == 59 && curSec == 59) //每天执行一次
                if (MaxNumberOfDays > 0 && Directory.Exists(_baseDirectory))
                    RemoveOldLogFiles();

            base.AdjustFileBeforeAppend();
        }

        /// <summary>
        ///     Creates and opens the file for logging.  If <see cref="P:log4net.Appender.RollingFileAppender.StaticLogFileName" />
        ///     is false then the fully qualified name is determined and used.
        /// </summary>
        /// <param name="fileName">the name of the file to open</param>
        /// <param name="append">true to append to existing file</param>
        /// <remarks>
        ///     This method will ensure that the directory structure
        ///     for the <paramref name="fileName" /> specified exists.
        /// </remarks>
        protected override void OpenFile(string fileName, bool append)
        {
            _baseDirectory = Path.GetDirectoryName(fileName);

            base.OpenFile(fileName, append);
        }

        /// <summary>
        /// 删除老旧日志文件
        /// </summary>
        protected void RemoveOldLogFiles()
        {
            try
            {
                var logFiles = Directory.GetFiles(_baseDirectory, "*.log", SearchOption.AllDirectories);
                var fiLogList = new List<FileInfo>();
                foreach (var file in logFiles) fiLogList.Add(new FileInfo(file));

                foreach (var item in fiLogList)
                    if (item.LastAccessTime < DateTime.Now.AddDays(-MaxNumberOfDays))
                        DeleteFile(item.FullName);
            }
            catch (Exception ex)
            {
                ErrorHandler.Error(string.Format("删除{0}天日志发生错误", MaxNumberOfDays), ex, ErrorCode.GenericFailure);
            }
        }

        #endregion Methods
    }
}