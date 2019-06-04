using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using MasterChief.DotNet4.WindowsAPI.Enum;
using MasterChief.DotNet4.WindowsAPI.Model;

namespace MasterChief.DotNet4.WindowsAPI.Core
{
    /// <summary>
    ///     Windows API
    /// </summary>
    public sealed class Win32Api
    {
        #region Delegates

        internal delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        internal delegate bool EnumDesktopsDelegate(string desktop, IntPtr lParam);

        internal delegate bool EnumDesktopWindowsDelegate(IntPtr hWnd, IntPtr lParam);

        internal delegate bool EnumWindowDelegate(IntPtr hwnd, IntPtr lParam);

        internal delegate bool EnumWindowStationsDelegate(string windowsStation, IntPtr lParam);

        #endregion Delegates

        #region Methods

        [DllImport("user32.dll")]
        internal static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern void keybd_event(int bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        /// <summary>
        ///     该函数合成键盘事件和鼠标事件，用来模拟鼠标或者键盘操作。
        ///     事件将被插入在鼠标或者键盘处理队列里面。
        ///     mouse_event()在windows后期版本中逐渐被SendInPut()取代
        /// </summary>
        /// <param name="nInputs">插入事件的个数</param>
        /// <param name="pInputs">该数组中的每个元素代表一个将要插入到线程事件中去的键盘或鼠标事</param>
        /// <param name="cbSize">INPUT结构的大小</param>
        /// <returns>成功插入操作事件的个数，如果插入出错可以利用GetLastError来查看错误类型</returns>
        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray)] [In]
            INPUT[] pInputs,
            int cbSize);

        [DllImport("user32.dll")]
        internal static extern bool CloseDesktop(
            IntPtr hDesktop
        );

        [DllImport("user32.dll")]
        internal static extern void mouse_event(MouseFlags dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern bool CloseHandle(IntPtr handle);

        [DllImport("user32.dll")]
        internal static extern bool CloseWindowStation(
            IntPtr winStation
        );

        [DllImport("ADVAPI32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool CreateProcessAsUser(IntPtr hToken, string lpApplicationName, string lpCommandLine,
            IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, uint dwCreationFlags, string lpEnvironment, string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("user32.dll")]
        internal static extern bool DrawMenuBar(IntPtr hWnd);


        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, EnumWindowDelegate lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool EnumDesktops(IntPtr hwinsta, EnumDesktopsDelegate
            lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool EnumDesktopWindows(
            IntPtr hDesktop,
            EnumDesktopWindowsDelegate lpEnumFunc,
            IntPtr lParam
        );

        [DllImport("user32.dll")]
        internal static extern bool EnumWindowStations(EnumWindowStationsDelegate lpEnumFunc,
            IntPtr lParam);

        /// <summary>
        ///     索处理顶级窗口的类名和窗口名称匹配指定的字符串。这个函数不搜索子窗口。
        /// </summary>
        /// <param name="className">窗体的类名</param>
        /// <param name="captionName">窗体的标题</param>
        /// <returns>返回窗体句柄</returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        internal static extern IntPtr FindWindow(string className, string captionName);

        /// <summary>
        ///     根据句柄判断是否是个窗口
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <returns>是否是个窗口</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        /// <summary>
        ///     获取主窗体下子控件的句柄
        ///     类名如果为[NULL]，则通过标题查找
        /// </summary>
        /// <param name="parentHandle">主窗体句柄</param>
        /// <param name="childHandle">子控件句柄</param>
        /// <param name="className">类名</param>
        /// <param name="captionName">标题</param>
        /// <returns>返回控件句柄</returns>
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Auto)]
        internal static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childHandle, string className,
            string captionName);

        [DllImport("user32.dll")]
        internal static extern IntPtr FindWindowEx(IntPtr parentHandle, int childAfter, string className,
            int windowTitle);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        internal static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetThreadDesktop(uint dwThreadId);

        [DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll", EntryPoint = "GetWindowText",
            ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int GetWindowText(IntPtr hWnd,
            StringBuilder lpWindowText, int nMaxCount);


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWindowVisible(IntPtr hWnd);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr OpenDesktop(
            [MarshalAs(UnmanagedType.LPTStr)] string desktopName,
            uint flags,
            bool inherit,
            WinStationAccess access
        );

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr OpenWindowStation(
            [MarshalAs(UnmanagedType.LPTStr)] string winStationName,
            [MarshalAs(UnmanagedType.Bool)] bool inherit,
            WinStationAccess access
        );

        [DllImport("user32.dll")]
        internal static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [DllImport("user32.dll")]
        internal static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SendMessage(IntPtr hWnd, int wmUser, int wParam, [Out] StringBuilder windowText);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);


        [DllImport("user32.dll")]
        internal static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cX, int cY,
            int wFlags);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);


        [DllImport("wtsapi32.dll")]
        internal static extern void WTSCloseServer(IntPtr hServer);

        [DllImport("wtsapi32.dll")]
        internal static extern int WTSEnumerateSessions(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.U4)] int reserved,
            [MarshalAs(UnmanagedType.U4)] int version,
            ref IntPtr ppSessionInfo,
            [MarshalAs(UnmanagedType.U4)] ref int pCount);

        [DllImport("wtsapi32.dll")]
        internal static extern void WTSFreeMemory(IntPtr pMemory);


        [DllImport("wtsapi32.dll")]
        internal static extern IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] string pServerName);

        [DllImport("Wtsapi32.dll")]
        internal static extern bool WTSQuerySessionInformation(
            IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out IntPtr ppBuffer, out uint pBytesReturned);

        [DllImport("WTSAPI32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool WTSQueryUserToken(int sessionId, out IntPtr token);

        #endregion Methods
    }
}