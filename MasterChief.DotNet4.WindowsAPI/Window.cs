using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Threading;
using MasterChief.DotNet4.WindowsAPI.Core;
using MasterChief.DotNet4.WindowsAPI.Enum;

namespace MasterChief.DotNet4.WindowsAPI
{
    /// <summary>
    ///     Window 操作
    /// </summary>
    public sealed class Window
    {
        #region Methods

        /// <summary>
        ///     关闭窗口
        /// </summary>
        /// <param name="hWnd">句柄</param>
        public static void Close(IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero)
                Win32Api.SendMessage(hWnd, (int) Wm.WM_SYSCOMMAND, (int) SysCommands.SC_CLOSE, null);
        }

        /// <summary>
        ///     设置关闭按钮不可用
        /// </summary>
        /// <param name="hWnd">句柄</param>
        public static void DisableCloseButton(IntPtr hWnd)
        {
            var hMenu = Win32Api.GetSystemMenu(hWnd, false);
            if (hMenu != IntPtr.Zero)
            {
                var n = Win32Api.GetMenuItemCount(hMenu);
                if (n > 0)
                {
                    Win32Api.RemoveMenu(hMenu, (uint) (n - 1), 0x400 | 0x1000);
                    Win32Api.RemoveMenu(hMenu, (uint) (n - 2), 0x400 | 0x1000);
                    Win32Api.DrawMenuBar(hWnd);
                }
            }
        }

        /// <summary>
        ///     获取坐标
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <returns>坐标</returns>
        public static Point GetLocation(IntPtr hWnd)
        {
            var rec = GetRectangle(hWnd);
            var point = new Point(rec.X, rec.Y);
            return point;
        }

        /// <summary>
        ///     获取矩形
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <returns>矩形</returns>
        public static Rectangle GetRectangle(IntPtr hWnd)
        {
            Win32Api.GetWindowRect(hWnd, out var hWndRect);

            return new Rectangle(hWndRect.X, hWndRect.Y, hWndRect.Width, hWndRect.Height);
        }

        /// <summary>
        ///     获取大小
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <returns>大小</returns>
        public static Size GetSize(IntPtr hWnd)
        {
            var rec = GetRectangle(hWnd);
            var size = new Size(rec.Width, rec.Height);
            return size;
        }

        /// <summary>
        ///     获取Title
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <returns>Title</returns>
        public static string GetTitle(IntPtr hWnd)
        {
            const int nChars = 256;
            var builder = new StringBuilder(nChars);

            return Win32Api.GetWindowText(hWnd, builder, nChars) > 0 ? builder.ToString() : null;
        }

        /// <summary>
        ///     隐藏
        /// </summary>
        /// <param name="hWnd">句柄</param>
        public static void Hide(IntPtr hWnd)
        {
            Win32Api.SetWindowPos(hWnd, 0, 0, 0, 0, 0, 0x0080);
        }

        /// <summary>
        ///     获取当前活动窗口，最前窗口，由此可判断一个窗口是否最前
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <returns>是否当前系统中被激活的窗口</returns>
        public static bool IsFocused(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero) return false;
            var hWndFocused = Win32Api.GetForegroundWindow();
            if (hWndFocused == IntPtr.Zero) return false;
            return hWnd == hWndFocused;
        }

        /// <summary>
        ///     根据句柄判断是否是个窗口
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <returns>是否是个窗口</returns>
        public static bool IsWindow(IntPtr hWnd)
        {
            return Win32Api.IsWindow(hWnd);
        }

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
        ///     移动
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public static void Move(IntPtr hWnd, int x, int y)
        {
            Win32Api.SetWindowPos(hWnd, 0, x, y, 0, 0, 0x0001);
        }

        /// <summary>
        ///     调整大小
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        public static void Resize(IntPtr hWnd, int width, int height)
        {
            Win32Api.SetWindowPos(hWnd, 0, 0, 0, width, height, 0x002);
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
        ///     截图
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Screenshot(IntPtr hWnd)
        {
            Restore(hWnd);
            Win32Api.GetWindowRect(hWnd, out var rc);

            var bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            var gfxBmp = Graphics.FromImage(bmp);
            var hdcBitmap = gfxBmp.GetHdc();

            Win32Api.PrintWindow(hWnd, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();
            return bmp;
        }

        /// <summary>
        ///     设置该窗口句柄被激活并成为当前窗口
        /// </summary>
        /// <param name="hWnd">句柄</param>
        public static void SetFocused(IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero)
                if (!Win32Api.SetForegroundWindow(hWnd))
                    AttachedThreadInputAction(
                        () =>
                        {
                            Win32Api.BringWindowToTop(hWnd);
                            Win32Api.ShowWindow(hWnd, SwShow);
                        });
        }


        /// <summary>
        ///     根据句柄判断一个窗口是否可见
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <returns>窗口是否可见</returns>
        public bool IsVisible(IntPtr hWnd)
        {
            return Win32Api.IsWindowVisible(hWnd);
        }

        private static void AttachedThreadInputAction(Action action)
        {
            var foreThread = Win32Api.GetWindowThreadProcessId(Win32Api.GetForegroundWindow(),
                IntPtr.Zero);
            var appThread = Win32Api.GetCurrentThreadId();
            var threadsAttached = false;
            try
            {
                threadsAttached =
                    foreThread == appThread ||
                    Win32Api.AttachThreadInput(foreThread, appThread, true);
                if (threadsAttached) action();
                else throw new ThreadStateException("AttachThreadInput failed.");
            }
            finally
            {
                if (threadsAttached)
                    Win32Api.AttachThreadInput(foreThread, appThread, false);
            }
        }

        private const uint SwShow = 5;

        /// <summary>
        ///     强制窗口前置
        /// </summary>
        /// <param name="hWnd">句柄</param>
        public static void ForceForeground(IntPtr hWnd)
        {
            var foreThread = Win32Api.GetWindowThreadProcessId(Win32Api.GetForegroundWindow(),
                IntPtr.Zero);
            var appThread = Win32Api.GetCurrentThreadId();

            if (foreThread != appThread)
            {
                Win32Api.AttachThreadInput(foreThread, appThread, true);
                Win32Api.BringWindowToTop(hWnd);
                Win32Api.ShowWindow(hWnd, SwShow);
                Win32Api.AttachThreadInput(foreThread, appThread, false);
            }
            else
            {
                Win32Api.BringWindowToTop(hWnd);
                Win32Api.ShowWindow(hWnd, SwShow);
            }
        }

        #endregion Methods
    }
}