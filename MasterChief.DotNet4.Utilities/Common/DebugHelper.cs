namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Debug 帮助类
    /// </summary>
    public class DebugHelper
    {
        #region Methods

        /// <summary>
        /// 输出到控制台
        /// </summary>
        public static void ConsoleOutput()
        {
            TextWriterTraceListener _listener = new TextWriterTraceListener(Console.Out);
            Debug.Listeners.Add(_listener);
        }

        /// <summary>
        /// 输出到文件
        /// </summary>
        /// <param name="path">路径</param>
        public static void FileOutput(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = DateTime.Now.ToString("yyyyMMdd") + ".log";
            }

            TextWriterTraceListener _listener = new TextWriterTraceListener(File.CreateText(path));
            Debug.Listeners.Add(_listener);
        }

        /// <summary>
        /// 输出到文件【文件名称：yyyyMMdd】
        /// </summary>
        public static void FileOutput()
        {
            FileOutput(string.Empty);
        }

        #endregion Methods
    }
}