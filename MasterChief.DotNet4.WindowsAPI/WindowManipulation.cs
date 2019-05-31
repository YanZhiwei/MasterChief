using System;
using MasterChief.DotNet4.WindowsAPI.Core;
using MasterChief.DotNet4.WindowsAPI.Enum;

namespace MasterChief.DotNet4.WindowsAPI
{
    /// <summary>
    ///     Window 操控
    /// </summary>
    public sealed class WindowManipulation
    {
        /// <summary>
        ///     最大化窗口
        /// </summary>
        /// <param name="hWnd">句柄</param>
        public static void Maximize(IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero)
            {
                Win32Api.SendMessage(hWnd, (int) Wm.WM_SYSCOMMAND, (int) SysCommands.SC_MINIMIZE, null);
                Win32Api.SendMessage(hWnd, (int) Wm.WM_SYSCOMMAND, (int) SysCommands.SC_MAXIMIZE, null);
            }
        }

        /// <summary>
        ///     最小化窗口
        /// </summary>
        /// <param name="hWnd">句柄</param>
        public static void Minimize(IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero)
                Win32Api.SendMessage(hWnd, (int) Wm.WM_SYSCOMMAND, (int) SysCommands.SC_MINIMIZE, null);
        }

        /// <summary>
        ///     还原窗口
        /// </summary>
        /// <param name="hWnd">句柄</param>
        public static void Restore(IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero)
                Win32Api.ShowWindow(hWnd, 1);
        }

        /// <summary>
        ///     激活窗口
        /// </summary>
        /// <param name="hWnd">句柄</param>
        public static void Activate(IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero)
                Win32Api.SendMessage(hWnd, (int) Wm.WM_ACTIVATE, WA.WA_CLICKACTIVE, 0);
        }

        /// <summary>
        ///     关闭窗口
        /// </summary>
        /// <param name="hWnd">句柄</param>
        public static void CloseWindow(IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero)
                Win32Api.SendMessage(hWnd, (int) Wm.WM_SYSCOMMAND, (int) SysCommands.SC_CLOSE, null);
        }
    }
}