using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using MasterChief.DotNet4.Utilities.Enums;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     Application 帮助类
    /// </summary>
    public static class ApplicationHelper
    {
        #region Methods

        /// <summary>
        ///     设置程序唯一实例运行.
        /// </summary>
        /// <param name="oneInstancaAction">委托，参数：是否是唯一实例</param>
        public static void ApplyOnlyOneInstance(Action<bool> oneInstancaAction)
        {
            // ReSharper disable once UnusedVariable
            var mutex = new Mutex(false, Assembly.GetCallingAssembly().FullName, out var onlyRun);
            oneInstancaAction(onlyRun);
        }

        /// <summary>
        ///     捕获异常
        ///     <para>在应用程序的主入口点Main方法使用</para>
        /// </summary>
        /// <param name="capturedFAction">The captured hanlder.</param>
        public static void CapturedException(Action<Exception, ExceptionType> capturedFAction)
        {
            try
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += (sender, e) => { capturedFAction(e.Exception, ExceptionType.Thread); };
                AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
                {
                    var ex = (Exception) e.ExceptionObject;
                    capturedFAction(ex, ExceptionType.Unhandled);
                };
            }
            catch (Exception ex)
            {
                capturedFAction(ex, ExceptionType.Unhandled);
            }
        }

        /// <summary>
        ///     捕获退出
        /// </summary>
        /// <param name="mainForm">主窗口</param>
        /// <param name="closingFunc">窗体正在关闭事件</param>
        public static void CapturedExit(Form mainForm, Func<bool> closingFunc)
        {
            mainForm.FormClosing += (sender, e) =>
            {
                if (closingFunc != null)
                    e.Cancel = !closingFunc();
                else
                    e.Cancel = true;
            };
            mainForm.FormClosed += (sender, e) =>
            {
                Environment.Exit(Environment.ExitCode);
                var runMainForm = sender as Form;
                runMainForm.Dispose();
                runMainForm.Close();
            };
        }

        /// <summary>
        ///     获取exe执行文件夹路径
        /// </summary>
        /// <returns>exe执行文件夹路径</returns>
        public static string GetExecuteDirectory()
        {
            return Path.GetDirectoryName(Application.ExecutablePath);
        }

        /// <summary>
        ///     全屏
        /// </summary>
        /// <param name="form">Form</param>
        public static void ToFullScreen(Form form)
        {
            ToFullScreen(form, 0);
        }

        /// <summary>
        ///     全屏
        /// </summary>
        /// <param name="form">Form</param>
        /// <param name="screen">0：主屏;1,2分屏</param>
        public static void ToFullScreen(Form form, int screen)
        {
            form.WindowState = FormWindowState.Maximized;
            form.StartPosition = FormStartPosition.Manual;
            form.Bounds = Screen.AllScreens[screen].Bounds;
        }

        /// <summary>
        ///     该方法只是暂时的将应用程序占用的内存移至虚拟内存，一旦应用程序被激活或者有操作请求时，这些内存又会被重新占用
        /// </summary>
        /// <param name="maxSize">最大虚拟内存，单位字节</param>
        /// <param name="minSize">最小虚拟内存，单位字节</param>
        public static void SetWorkingSet(int maxSize, int minSize)
        {
            var process = Process.GetCurrentProcess();
            process.MaxWorkingSet = (IntPtr) maxSize;
            process.MinWorkingSet = (IntPtr) minSize;
        }

        #endregion Methods
    }
}