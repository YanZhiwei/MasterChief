using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     Notepad 帮助类
    /// </summary>
    public class NotepadHelper
    {
        #region Methods

        /// <summary>
        ///     打开记事本，并往记事本写入内容
        /// </summary>
        /// <param name="message">记事本内容</param>
        /// <param name="title">记事本名称</param>
        public static void ShowMessage(string message, string title)
        {
            var notepad = Process.Start(new ProcessStartInfo("notepad.exe"));
            if (notepad != null)
            {
                notepad.WaitForInputIdle();
                if (!string.IsNullOrEmpty(title)) SetWindowText(notepad.MainWindowHandle, title);

                if (string.IsNullOrEmpty(message))
                {
                    return;
                }

                var child = FindWindowEx(notepad.MainWindowHandle, new IntPtr(0), "Edit", null);
                SendMessage(child, 0x000C, 0, message);
            }
        }

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
            string lpszWindow);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        [DllImport("user32.dll", EntryPoint = "SetWindowText")]
        private static extern int SetWindowText(IntPtr hWnd, string text);

        #endregion Methods
    }
}