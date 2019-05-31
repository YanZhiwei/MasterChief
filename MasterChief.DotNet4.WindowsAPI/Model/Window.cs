using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterChief.DotNet4.WindowsAPI.Core;

namespace MasterChief.DotNet4.WindowsAPI.Model
{
    /// <summary>
    ///     窗口句柄信息
    /// </summary>
    public sealed class Window
    {
        /// <summary>
        ///     句柄
        /// </summary>
        public readonly IntPtr HWnd;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="hWnd">句柄信息</param>
        public Window(IntPtr hWnd)
        {
            HWnd = hWnd;
        }

        /// <summary>
        ///     获取ClassName
        /// </summary>
        /// <param name="throwError">是否抛出异常</param>
        /// <returns>ClassName</returns>
        public string GetClassName(bool throwError = false)
        {
            var buffer = new StringBuilder(256);
            var charCount = Win32Api.GetClassName(HWnd, buffer, buffer.Capacity);
            if (charCount != 0)
                return buffer.ToString();
            if (throwError) throw new Win32ErrorCodeException("GetClassName(hWnd=" + HWnd + ")");
            return string.Empty;
        }

        /// <summary>
        ///     获取标题
        /// </summary>
        /// <param name="throwError">是否抛出异常</param>
        /// <returns>标题</returns>
        public string GetTitle(bool throwError = false)
        {
            const int count = 256;
            var buffer = new StringBuilder(count);
            if (Win32Api.GetWindowText(HWnd, buffer, count) > 0)
                return buffer.ToString();
            if (throwError) throw new Win32ErrorCodeException("GetWindowText(hWnd=" + HWnd + ")");
            return string.Empty;
        }

        /// <summary>
        ///     窗口是否可见
        /// </summary>
        /// <returns>是否可见</returns>
        public bool IsVisible()
        {
            return Win32Api.IsWindowVisible(HWnd);
        }
    }
}
