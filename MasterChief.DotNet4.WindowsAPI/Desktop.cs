using System.Drawing;
using System.Windows.Forms;
using MasterChief.DotNet4.WindowsAPI.Core;

namespace MasterChief.DotNet4.WindowsAPI
{
    /// <summary>
    ///     Desktop 操作
    /// </summary>
    public sealed class Desktop
    {
        /// <summary>
        ///     截图
        /// </summary>
        /// <param name="screen">Screen</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Screenshot(Screen screen)
        {
            var bmpScreenshot = new Bitmap(screen.Bounds.Width, screen.Bounds.Height);
            var g = Graphics.FromImage(bmpScreenshot);
            g.CopyFromScreen(0, 0, 0, 0, screen.Bounds.Size);
            g.Dispose();
            return bmpScreenshot;
        }

        /// <summary>
        ///     主屏幕截图
        /// </summary>
        /// <returns>Bitmap</returns>
        private static Bitmap Screenshot()
        {
            return Screenshot(Screen.PrimaryScreen);
        }

        /// <summary>
        ///     隐藏任务栏
        /// </summary>
        public static void HideTaskBar()
        {
            var hWndDesktop = Win32Api.GetDesktopWindow();
            var hWndStartButton = Win32Api.FindWindowEx(hWndDesktop, 0, "button", 0);
            var hWndTaskBar = Win32Api.FindWindowEx(hWndDesktop, 0, "Shell_TrayWnd", 0);
            Win32Api.SetWindowPos(hWndStartButton, 0, 0, 0, 0, 0, 0x0080);
            Win32Api.SetWindowPos(hWndTaskBar, 0, 0, 0, 0, 0, 0x0080);
        }

        /// <summary>
        ///     显示任务栏
        /// </summary>
        public static void ShowTaskBar()
        {
            var hWndDesktop = Win32Api.GetDesktopWindow();
            var hWndStartButton = Win32Api.FindWindowEx(hWndDesktop, 0, "button", 0);
            var hWndTaskBar = Win32Api.FindWindowEx(hWndDesktop, 0, "Shell_TrayWnd", 0);
            Win32Api.SetWindowPos(hWndStartButton, 0, 0, 0, 0, 0, 0x0040);
            Win32Api.SetWindowPos(hWndTaskBar, 0, 0, 0, 0, 0, 0x0040);
        }
    }
}