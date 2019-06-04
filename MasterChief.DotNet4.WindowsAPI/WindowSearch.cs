namespace MasterChief.DotNet4.WindowsAPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;

    using MasterChief.DotNet4.WindowsAPI.Core;
    using MasterChief.DotNet4.WindowsAPI.Model;

    /// <summary>
    ///     Windows 搜索
    /// </summary>
    public sealed class WindowSearch
    {
        #region Methods

        /// <summary>
        ///     根据类名查询句柄
        /// </summary>
        /// <param name="parentHwnd">父句柄</param>
        /// <param name="className">类名</param>
        /// <returns>子窗口句柄</returns>
        public static IntPtr GetChildWindowByClassName(IntPtr parentHwnd, string className)
        {
            if (parentHwnd == IntPtr.Zero)
                return IntPtr.Zero;
            var childHwnds = GetChildWindows(parentHwnd);

            foreach (var hwnd in childHwnds)
            {
                var windows = new WindowInfo(hwnd);
                var text = windows.GetClassName();
                if (string.Compare(className, text, StringComparison.OrdinalIgnoreCase) == 0) return hwnd;
            }

            return IntPtr.Zero;
        }

        /// <summary>
        ///     根据窗口标题查询句柄
        /// </summary>
        /// <param name="parentHwnd">父句柄</param>
        /// <param name="title">窗口标题</param>
        /// <returns>子窗口句柄</returns>
        public static IntPtr GetChildWindowByTitle(IntPtr parentHwnd, string title)
        {
            if (parentHwnd == IntPtr.Zero)
                return IntPtr.Zero;
            var childHwnds = GetChildWindows(parentHwnd);

            foreach (var hwnd in childHwnds)
            {
                var windows = new WindowInfo(hwnd);
                var text = windows.GetTitle();
                if (string.Compare(title, text, StringComparison.OrdinalIgnoreCase) == 0) return hwnd;
            }

            return IntPtr.Zero;
        }

        /// <summary>
        ///     根据句柄获取子句柄列表
        /// </summary>
        /// <param name="hwndParent">句柄</param>
        /// <returns>子句柄列表</returns>
        public static List<IntPtr> GetChildWindows(IntPtr hwndParent)
        {
            var childHandles = new List<IntPtr>();
            var gcChildhandles = GCHandle.Alloc(childHandles);

            try
            {
                var callback = new Win32Api.EnumWindowDelegate((hWnd, lParam) =>
                {
                    var gcChandles = GCHandle.FromIntPtr(lParam);

                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    if (gcChandles == null || gcChandles.Target == null) return false;

                    var cHandles = gcChandles.Target as List<IntPtr>;
                    cHandles?.Add(hWnd);

                    return true;
                });

                Win32Api.EnumChildWindows(hwndParent, callback, GCHandle.ToIntPtr(gcChildhandles));
            }
            finally
            {
                if (gcChildhandles.IsAllocated)
                    gcChildhandles.Free();
            }

            return childHandles.Distinct().ToList();
        }

        /// <summary>
        ///     获取所有窗口列表
        /// </summary>
        /// <returns>窗口列表</returns>
        public static WindowInfo[] GetWindows()
        {
            var windows = new List<WindowInfo>();
            var callback = new Win32Api.EnumDesktopWindowsDelegate((hWnd, lParam) =>
            {
                windows.Add(new WindowInfo(hWnd));
                return true;
            });
            if (!Win32Api.EnumDesktopWindows(IntPtr.Zero, callback, IntPtr.Zero))
                throw new Win32ErrorCodeException("EnumDesktopWindows");
            return windows.ToArray();
        }

        /// <summary>
        /// 根据类名查询句柄，不能查询子窗口
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns>句柄</returns>
        public static IntPtr GetWindowsByClassName(string className)
        {
            return Win32Api.FindWindow(className, null);
        }

        /// <summary>
        /// 根据窗口名称查询句柄，不能查询子窗口
        /// </summary>
        /// <param name="title">窗口名称</param>
        /// <returns>句柄</returns>
        public static IntPtr GetWindowsByTitle(string title)
        {
            return Win32Api.FindWindow(null, title);
        }

        #endregion Methods
    }
}