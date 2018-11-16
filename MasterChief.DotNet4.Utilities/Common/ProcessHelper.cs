namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Process 帮助类
    /// </summary>
    public class ProcessHelper
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
        /// <param name="execFactory">委托 </param>
        public static void ExecBatCommand(Action<Action<string>> execFactory)
        {
            Process _process = null;

            try
            {
                _process = new Process();
                _process.StartInfo.FileName = "cmd.exe";
                _process.StartInfo.UseShellExecute = false;
                _process.StartInfo.CreateNoWindow = true;
                _process.StartInfo.RedirectStandardInput = true;
                _process.StartInfo.RedirectStandardOutput = true;
                _process.StartInfo.RedirectStandardError = true;
                _process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                _process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);
                _process.Start();
                using (StreamWriter writer = _process.StandardInput)
                {
                    writer.AutoFlush = true;
                    _process.BeginOutputReadLine();
                    execFactory(value => writer.WriteLine(value));
                }
                _process.WaitForExit();
            }
            finally
            {
                if (_process != null && !_process.HasExited)
                {
                    _process.Kill();
                }

                if (_process != null)
                {
                    _process.Close();
                }
            }
        }

        #endregion Methods
    }
}