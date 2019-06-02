using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using MasterChief.DotNet4.Utilities.Common;
using MasterChief.DotNet4.WindowsAPI.Core;
using MasterChief.DotNet4.WindowsAPI.Model;

namespace MasterChief.DotNet4.WindowsAPI
{
    /// <summary>
    ///     键盘鼠标操作
    /// </summary>
    public sealed class MouseKeyboard
    {
        #region Methods

        /// <summary>
        ///     在当前位置单击鼠标左键。
        /// </summary>
        public static void LeftClick()
        {
            LeftClick(Cursor.Position.X, Cursor.Position.Y);
        }

        /// <summary>
        ///     在当前位置单击鼠标左键。
        /// </summary>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        public static void LeftClick(int x, int y)
        {
            LeftClick(x, y, new Random().Next(20, 30));
        }

        /// <summary>
        ///     在当前位置单击鼠标左键。
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        public static void LeftClick(IntPtr hWnd, int x, int y)
        {
            LeftClick(hWnd, x, y, RandomHelper.NextNumber(10, 30));
        }

        /// <summary>
        ///     在当前位置单击鼠标左键。
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        /// <param name="delay">按下鼠标与放开鼠标之间延迟时间【毫秒】。</param>
        public static void LeftClick(IntPtr hWnd, int x, int y, int delay)
        {
            var point = ToWindowCoordinates(hWnd, x, y);
            LeftClick(point.X, point.Y, delay);
        }

        /// <summary>
        ///     在当前位置单击鼠标左键
        /// </summary>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        /// <param name="delay">按下鼠标与放开鼠标之间延迟时间【毫秒】</param>
        public static void LeftClick(int x, int y, int delay)
        {
            Move(x, y);
            LeftDown();
            Thread.Sleep(delay);
            LeftUp();
        }

        /// <summary>
        ///     鼠标左键按下
        /// </summary>
        public static void LeftDown()
        {
            LeftDown(Cursor.Position.X, Cursor.Position.Y);
        }

        /// <summary>
        ///     鼠标左键按下
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        public static void LeftDown(IntPtr hWnd, int x, int y)
        {
            var point = ToWindowCoordinates(hWnd, x, y);
            LeftDown(point.X, point.Y);
        }

        /// <summary>
        ///     鼠标左键按下
        /// </summary>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        public static void LeftDown(int x, int y)
        {
            Move(x, y);
            MouseSendInput(0x0002);
        }

        /// <summary>
        ///     鼠标左键放开
        /// </summary>
        public static void LeftUp()
        {
            LeftUp(Cursor.Position.X, Cursor.Position.Y);
        }

        /// <summary>
        ///     鼠标左键放开
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        public static void LeftUp(IntPtr hWnd, int x, int y)
        {
            var point = ToWindowCoordinates(hWnd, x, y);
            LeftUp(point.X, point.Y);
        }

        /// <summary>
        ///     鼠标左键放开
        /// </summary>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        public static void LeftUp(int x, int y)
        {
            Move(x, y);
            MouseSendInput(0x0004);
        }

        /// <summary>
        ///     移动.
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        public static void Move(IntPtr hWnd, int x, int y)
        {
            var point = ToWindowCoordinates(hWnd, x, y);
            Move(point.X, point.Y);
        }

        /// <summary>
        ///     移动
        /// </summary>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        public static void Move(int x, int y)
        {
            Cursor.Position = new Point(x, y);
        }

        /// <summary>
        ///     右击
        /// </summary>
        public static void RightClick()
        {
            RightClick(Cursor.Position.X, Cursor.Position.Y);
        }

        /// <summary>
        ///     右击
        /// </summary>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        public static void RightClick(int x, int y)
        {
            RightClick(x, y, RandomHelper.NextNumber(10, 30));
        }

        /// <summary>
        ///     右击
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        public static void RightClick(IntPtr hWnd, int x, int y)
        {
            RightClick(hWnd, x, y, RandomHelper.NextNumber(10, 30));
        }

        /// <summary>
        ///     右击
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        /// <param name="delay">按下鼠标与放开鼠标之间延迟时间【毫秒】</param>
        public static void RightClick(IntPtr hWnd, int x, int y, int delay)
        {
            var point = ToWindowCoordinates(hWnd, x, y);
            RightClick(point.X, point.Y, delay);
        }

        /// <summary>
        ///     右击
        /// </summary>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        /// <param name="delay">按下鼠标与放开鼠标之间延迟时间【毫秒】</param>
        public static void RightClick(int x, int y, int delay)
        {
            Move(x, y);
            RightDown();
            Thread.Sleep(delay);
            RightUp();
        }

        /// <summary>
        ///     鼠标右键按下
        /// </summary>
        public static void RightDown()
        {
            RightDown(Cursor.Position.X, Cursor.Position.Y);
        }

        /// <summary>
        ///     鼠标右键按下
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        public static void RightDown(IntPtr hWnd, int x, int y)
        {
            var point = ToWindowCoordinates(hWnd, x, y);
            RightDown(point.X, point.Y);
        }

        /// <summary>
        ///     鼠标右键按下
        /// </summary>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        public static void RightDown(int x, int y)
        {
            Move(x, y);
            MouseSendInput(0x0008);
        }

        /// <summary>
        ///     鼠标右键放开
        /// </summary>
        public static void RightUp()
        {
            RightUp(Cursor.Position.X, Cursor.Position.Y);
        }

        /// <summary>
        ///     鼠标右键放开
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        public static void RightUp(IntPtr hWnd, int x, int y)
        {
            var point = ToWindowCoordinates(hWnd, x, y);
            RightUp(point.X, point.Y);
        }

        /// <summary>
        ///     鼠标右键放开
        /// </summary>
        /// <param name="x">x坐标.</param>
        /// <param name="y">y坐标.</param>
        public static void RightUp(int x, int y)
        {
            Move(x, y);
            MouseSendInput(0x0010);
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