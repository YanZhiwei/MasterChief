using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MasterChief.DotNet4.Utilities.Operator;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     Process 帮助类
    /// </summary>
    public static class ProcessHelper
    {
        #region Methods

        /// <summary>
        ///     运行程序
        /// </summary>
        /// <param name="processPath">程序exe全路径</param>
        public static void Run(string processPath)
        {
            ValidateOperator.Begin()
                .NotNullOrEmpty(processPath, "需要运行的程序路径")
                .CheckFileExists(processPath);
            var fileInfo = new FileInfo(processPath);
            var process = new Process
            {
                StartInfo =
                {
                    FileName = fileInfo.Name,
                    WorkingDirectory = fileInfo.DirectoryName
                }
            };

            process.Start();
        }

        /// <summary>
        ///     判断程序是否已经运行
        /// </summary>
        /// <param name="processPath">程序exe全路径</param>
        /// <returns>是否已经运行</returns>
        public static bool IsRunning(string processPath)
        {
            ValidateOperator.Begin()
                .NotNullOrEmpty(processPath, "需要运行的程序路径")
                .CheckFileExists(processPath);
            var fileName = Path.GetFileNameWithoutExtension(processPath)?.ToLower();
            var workingDirectory = Path.GetDirectoryName(processPath);
            var processes = Process.GetProcessesByName(fileName);

            return processes.Count(c =>
                       // ReSharper disable once PossibleNullReferenceException
                       c.MainModule.FileName.StartsWith(workingDirectory, StringComparison.OrdinalIgnoreCase)) > 0;
        }

        #endregion Methods
    }
}