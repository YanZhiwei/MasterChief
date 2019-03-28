using System;
using System.Diagnostics;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     Process 帮助类
    /// </summary>
    public static class ProcessHelper
    {
        #region Methods

        /// <summary>
        ///     动态执行一系列控制台命令
        ///     <para>eg: ProcessHelper.ExecBatCommand(cmd =></para>
        ///     <para>{</para>
        ///     <para>cmd("ipconfig");</para>
        ///     <para>cmd("getmac");</para>
        ///     <para>cmd("exit 0");</para>
        ///     <para> });</para>
        /// </summary>
        /// <param name="keySelector">委托 </param>
        public static void ExecBatCommand(Action<Action<string>> keySelector)
        {
            Process process = null;

            try
            {
                process = new Process
                {
                    StartInfo =
                    {
                        FileName = "cmd.exe",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.Start();
                using (var writer = process.StandardInput)
                {
                    writer.AutoFlush = true;
                    process.BeginOutputReadLine();
                    // ReSharper disable once AccessToDisposedClosure
                    keySelector?.Invoke(value => { writer.WriteLine(value); });
                }

                process.WaitForExit();
            }
            finally
            {
                if (process != null && !process.HasExited) process.Kill();

                process?.Close();
            }
        }

        #endregion Methods
    }
}