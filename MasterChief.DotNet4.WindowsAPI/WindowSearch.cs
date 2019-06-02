using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using MasterChief.DotNet4.WindowsAPI.Core;

namespace MasterChief.DotNet4.WindowsAPI
{
    /// <summary>
    ///     Windows 搜索
    /// </summary>
    public sealed class WindowSearch
    {
        #region Methods

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
        public static Model.Window[] GetWindows()
        {
            var windows = new List<Model.Window>();
            var callback = new Win32Api.EnumDesktopWindowsDelegate((hWnd, lParam) =>
            {
                windows.Add(new Model.Window(hWnd));
                return true;
            });
            if (!Win32Api.EnumDesktopWindows(IntPtr.Zero, callback, IntPtr.Zero))
                throw new Win32ErrorCodeException("EnumDesktopWindows");
            return windows.ToArray();
        }

        #endregion Methods
    }
}