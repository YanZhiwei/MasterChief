using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using MasterChief.DotNet4.Utilities.Operator;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     Process 帮助类
    /// </summary>
    public static class ProcessHelper
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hwnd);

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
            //Cherry-pick
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

        /// <summary>
        ///     判断程序是否已经运行
        /// </summary>
        /// <param name="processPath">程序exe全路径</param>
        /// <param name="owner">进程拥有者</param>
        /// <returns>
        ///     <c>true</c> 是否已经运行 <c>false</c>.
        /// </returns>
        public static bool IsRunning(string processPath, string owner)
        {
            //Cherry-pick 2
            ValidateOperator.Begin()
                .NotNullOrEmpty(processPath, "需要运行的程序路径")
                .NotNullOrEmpty(owner, "进程拥有者")
                .CheckFileExists(processPath);
            var result = false;
            var fileName = Path.GetFileNameWithoutExtension(processPath)?.ToLower();
            var workingDirectory = Path.GetDirectoryName(processPath);
            var processes = Process.GetProcessesByName(fileName);
            if (processes?.Any() ?? false)
                foreach (var process in processes)
                {
                    var procOwner = GetOwner(process);
                    if (string.IsNullOrEmpty(procOwner)) continue;
                    if (string.Compare(procOwner, owner, StringComparison.OrdinalIgnoreCase) < 0) continue;
                    if (process.MainModule.FileName.StartsWith(workingDirectory, StringComparison.OrdinalIgnoreCase))
                    {
                        result = true;
                        break;
                    }
                }

            return result;
        }

        /// <summary>
        ///     获取进程所有者
        /// </summary>
        /// <param name="process">Process</param>
        /// <returns>进程所有者</returns>
        private static string GetOwner(Process process)
        {
            var processHandle = IntPtr.Zero;
            try
            {
                OpenProcessToken(process.Handle, 8, out processHandle);
                var wi = new WindowsIdentity(processHandle);
                var user = wi.Name;
                return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\", StringComparison.Ordinal) + 1) : user;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (processHandle != IntPtr.Zero) CloseHandle(processHandle);
            }
        }

        /// <summary>
        /// 获取当前进程版本信息
        /// </summary>
        /// <returns>FileVersionInfo</returns>
        public static FileVersionInfo GetCurrentVersion()
        {
            var fileName = Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
            return string.IsNullOrEmpty(fileName) ? null : FileVersionInfo.GetVersionInfo(fileName);
        }

        #endregion Methods
    }
}