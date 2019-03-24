namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Process 帮助类
    /// </summary>
    public static class ProcessHelper
    {
        #region Methods

        /// <summary>
        /// 动态执行一系列控制台命令
        /// <para>eg: ProcessHelper.ExecBatCommand(cmd =></para>
        /// <para>{</para>
        /// <para>cmd("ipconfig");</para>
        /// <para>cmd("getmac");</para>
        /// <para>cmd("exit 0");</para>
        /// <para> });</para>
        /// </summary>
        /// <param name="keySelector">委托 </param>
        public static void ExecBatCommand(Action<Action<string>> keySelector)
        {
            Process process = null;

            try
            {
                process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.Start();
                using (StreamWriter writer = process.StandardInput)
                {
                    writer.AutoFlush = true;
                    process.BeginOutputReadLine();
                    keySelector?.Invoke(value => writer.WriteLine(value));
                }
                process.WaitForExit();
            }
            finally
            {
                if (process != null && !process.HasExited)
                {
                    process.Kill();
                }

                if (process != null)
                {
                    process.Close();
                }
            }
        }

        #endregion Methods
    }
}