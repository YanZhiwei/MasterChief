using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
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

        public static void SetFocused(IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero)
                Win32Api.SetForegroundWindow(hWnd);
        }

        public static bool IsFocused(IntPtr hWnd)
        {
            var hWndFocused = Win32Api.GetForegroundWindow();
            if (hWnd == IntPtr.Zero || hWndFocused == IntPtr.Zero) return false;
            return hWnd == hWndFocused;
        }

        public static Rectangle GetRectangle(IntPtr hWnd)
        {
            Win32Api.GetWindowRect(hWnd, out var hWndRect);

            return new Rectangle(hWndRect.X, hWndRect.Y, hWndRect.Width, hWndRect.Height);
        }

        public static Size GetSize(IntPtr hWnd)
        {
            var rec = GetRectangle(hWnd);
            var size = new Size(rec.Width, rec.Height);
            return size;
        }

        public static Point GetLocation(IntPtr hWnd)
        {
            var rec = GetRectangle(hWnd);
            var point = new Point(rec.X, rec.Y);
            return point;
        }

        public static string GetTitle(IntPtr hWnd)
        {
            const int nChars = 256;
            var builder = new StringBuilder(nChars);

            return Win32Api.GetWindowText(hWnd, builder, nChars) > 0 ? builder.ToString() : null;
        }

        public static void Normalize(IntPtr hWnd)
        {
            Win32Api.ShowWindow(hWnd, 1);
        }

        public static Bitmap Screenshot(IntPtr hWnd)
        {
            Normalize(hWnd);
            Win32Api.GetWindowRect(hWnd, out var rc);

            var bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            var gfxBmp = Graphics.FromImage(bmp);
            var hdcBitmap = gfxBmp.GetHdc();

            Win32Api.PrintWindow(hWnd, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();
            return bmp;
        }

        public static void Move(IntPtr hWnd, int x, int y)
        {
            Win32Api.SetWindowPos(hWnd, 0, x, y, 0, 0, 0x0001);
        }

        public static void Resize(IntPtr hWnd, int width, int height)
        {
            Win32Api.SetWindowPos(hWnd, 0, 0, 0, width, height, 0x002);
        }

        public static void Hide(IntPtr hWnd)
        {
            Win32Api.SetWindowPos(hWnd, 0, 0, 0, 0, 0, 0x0080);
        }

        public static void DisableCloseButton(IntPtr hWnd)
        {
            var hMenu = Win32Api.GetSystemMenu(hWnd, false);
            if (hMenu != IntPtr.Zero)
            {
                int n = Win32Api.GetMenuItemCount(hMenu);
                if (n > 0)
                {
                    Win32Api.RemoveMenu(hMenu, (uint) (n - 1), 0x400 | 0x1000);
                    Win32Api.RemoveMenu(hMenu, (uint) (n - 2), 0x400 | 0x1000);
                    Win32Api.DrawMenuBar(hWnd);
                }
            }
        }
    }
}