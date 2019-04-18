using System;
using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     WinForm UI帮助类
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class UIHelper
    {
        #region Methods

        /// <summary>
        ///     控件线程安全
        /// </summary>
        /// <param name="control">Control</param>
        /// <param name="code">委托</param>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public static void UIThread(this Control control, MethodInvoker code)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(code);
                return;
            }

            code.Invoke();
        }

        /// <summary>
        ///     控件先线程安全
        ///     参考:http://www.codeproject.com/Articles/37413/A-Generic-Method-for-Cross-thread-Winforms-Access#xx3867544xx
        /// </summary>
        /// <typeparam name="T">Control</typeparam>
        /// <param name="cont">Control</param>
        /// <param name="updateUiFactory">委托</param>
        // ReSharper disable once InconsistentNaming
        public static void UIThread<T>(this T cont, Action<T> updateUiFactory)
            where T : Control
        {
            if (cont.InvokeRequired)
                cont.Invoke(new Action<T, Action<T>>(UIThread), cont, updateUiFactory);
            else
                updateUiFactory(cont);
        }

        /// <summary>
        ///     异步线程安全更新UI
        /// </summary>
        /// <typeparam name="T">Control</typeparam>
        /// <param name="cont">Control</param>
        /// <param name="updateUiFactory">委托</param>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public static void UIBeginThread<T>(this T cont, Action<T> updateUiFactory)
            where T : Control
        {
            if (cont.InvokeRequired)
                cont.BeginInvoke(new Action<T, Action<T>>(UIBeginThread), cont, updateUiFactory);
            else
                updateUiFactory(cont);
        }

        #endregion Methods
    }
}