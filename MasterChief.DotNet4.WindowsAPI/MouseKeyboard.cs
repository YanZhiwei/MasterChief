using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using MasterChief.DotNet4.WindowsAPI.Core;
using MasterChief.DotNet4.WindowsAPI.Enum;
using MasterChief.DotNet4.WindowsAPI.Model;

namespace MasterChief.DotNet4.WindowsAPI
{
    /// <summary>
    ///     键盘鼠标操作
    /// </summary>
    public sealed class MouseKeyboard
    {
        #region Fields

        private const uint KeyeventfExtendedkey = 0x0001;
        private const uint KeyeventfKeyup = 0x0002;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     鼠标左键点击
        /// </summary>
        /// <param name="hWnd">句柄</param>
        public static void LeftClick(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return;
            var oldPos = Cursor.Position;
            var hWndPoint = new Point(0, 0);
            Win32Api.ClientToScreen(hWnd, ref hWndPoint);
            Cursor.Position = new Point(hWndPoint.X, hWndPoint.Y);
            var mouseDown = new INPUT {Type = 0};

            mouseDown.Data.Mouse.Flags = 0x0002;

            var mouseUp = new INPUT {Type = 0};

            mouseUp.Data.Mouse.Flags = 0x0004;

            var inputs = new[] {mouseDown, mouseUp};
            Win32Api.SendInput((uint) inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
            Cursor.Position = oldPos;
        }

        /// <summary>
        ///     粘贴
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="delay">延时间隔【毫秒】</param>
        public static void Paste(string text, int delay)
        {
            Paste(text, delay, true);
        }

        /// <summary>
        ///     粘贴
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="delay">延时间隔【毫秒】</param>
        /// <param name="backup">是否备份</param>
        public static void Paste(string text, int delay, bool backup)
        {
            if (string.IsNullOrEmpty(text)) return;

            string backupText = null;

            if (backup)
            {
                backupText = Clipboard.GetText();

                Thread.Sleep(delay);
            }

            Clipboard.SetText(text);

            Thread.Sleep(delay);

            PressKey('V', false, false, true);

            Thread.Sleep(delay);

            if (backup) Clipboard.SetText(backupText);
        }

        /// <summary>
        ///     按键
        /// </summary>
        /// <param name="key">按键</param>
        /// <param name="shift">是否shift</param>
        /// <param name="alt">是否alt</param>
        /// <param name="ctrl">是否ctrl</param>
        public static void PressKey(int key, bool shift, bool alt, bool ctrl)
        {
            PressKey(key, shift, alt, ctrl, 0);
        }

        /// <summary>
        ///     按键
        /// </summary>
        /// <param name="key">按键</param>
        /// <param name="shift">是否shift</param>
        /// <param name="alt">是否alt</param>
        /// <param name="ctrl">是否ctrl</param>
        /// <param name="delay">延时间隔【毫秒】</param>
        public static void PressKey(int key, bool shift, bool alt, bool ctrl, int delay)
        {
            byte bScan = 0x45;
            if (key == VIRTUALKEY.VK_SNAPSHOT) bScan = 0;
            if (key == VIRTUALKEY.VK_SPACE) bScan = 39;

            if (alt) Win32Api.keybd_event(VIRTUALKEY.VK_MENU, 0, 0, 0);
            if (ctrl) Win32Api.keybd_event(VIRTUALKEY.VK_LCONTROL, 0, 0, 0);
            if (shift) Win32Api.keybd_event(VIRTUALKEY.VK_LSHIFT, 0, 0, 0);

            Win32Api.keybd_event(key, bScan, KeyeventfExtendedkey, 0);

            if (delay > 0) Thread.Sleep(delay);

            Win32Api.keybd_event(key, bScan, KeyeventfExtendedkey | KeyeventfKeyup, 0);

            if (shift) Win32Api.keybd_event(VIRTUALKEY.VK_LSHIFT, 0, KeyeventfKeyup, 0);
            if (ctrl) Win32Api.keybd_event(VIRTUALKEY.VK_LCONTROL, 0, KeyeventfKeyup, 0);
            if (alt) Win32Api.keybd_event(VIRTUALKEY.VK_MENU, 0, KeyeventfKeyup, 0);
        }


        /// <summary>
        ///     打字输入
        /// </summary>
        /// <param name="text">文本</param>
        public static void Type(string text)
        {
            Type(text, 50);
        }

        /// <summary>
        ///     打字输入
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="delay">延迟【毫秒】</param>
        public static void Type(string text, int delay)
        {
            foreach (var c in text)
            {
                var shift = char.IsUpper(c);

                int key;
                if (char.IsLetterOrDigit(c))
                {
                    key = char.ToUpper(c);
                }
                else
                {
                    if (c == '.' || c == ',') key = VIRTUALKEY.VK_DECIMAL;
                    else continue;
                }

                PressKey(key, shift, false, false);


                Thread.Sleep(delay);
            }
        }

        /// <summary>
        ///     滚动
        /// </summary>
        /// <param name="scrollValue">滚动数值</param>
        public static void Scroll(int scrollValue)
        {
            Win32Api.mouse_event(MouseFlags.Scroll, 0, 0, scrollValue, IntPtr.Zero);
        }


        private static void MouseSendInput(uint flag)
        {
            var inputMouseDown = new INPUT
            {
                Type = 0
            };
            inputMouseDown.Data.Mouse.Flags = flag;
            Win32Api.SendInput(1, ref inputMouseDown, Marshal.SizeOf(new INPUT()));
        }

        private static Point ToWindowCoordinates(IntPtr hWnd, int x, int y)
        {
            Win32Api.GetWindowRect(hWnd, out var hWndRect);
            var point = new Point(hWndRect.X + x, hWndRect.Y + y);
            return point;
        }

        #endregion Methods
    }
}