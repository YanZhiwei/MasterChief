using System;
using System.Runtime.InteropServices;
using System.Text;
using MasterChief.DotNet4.Utilities.Enum;
using MasterChief.DotNet4.Utilities.Model;

namespace MasterChief.DotNet4.Utilities.Core
{
    /// <summary>
    ///     Windows API
    /// </summary>
    public sealed class Win32Api
    {
        public delegate bool EnumDelegate(IntPtr hWnd, int lParam);


        public delegate bool EnumDesktopsDelegate(string desktop, IntPtr lParam);


        public delegate bool EnumDesktopWindowsDelegate(IntPtr hWnd, IntPtr lParam);


        public delegate bool EnumWindowStationsDelegate(string windowsStation, IntPtr lParam);

        [DllImport("wtsapi32.dll")]
        public static extern IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] string pServerName);

        [DllImport("wtsapi32.dll")]
        public static extern void WTSCloseServer(IntPtr hServer);

        [DllImport("wtsapi32.dll")]
        public static extern int WTSEnumerateSessions(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.U4)] int reserved,
            [MarshalAs(UnmanagedType.U4)] int version,
            ref IntPtr ppSessionInfo,
            [MarshalAs(UnmanagedType.U4)] ref int pCount);

        [DllImport("wtsapi32.dll")]
        public static extern void WTSFreeMemory(IntPtr pMemory);

        [DllImport("Wtsapi32.dll")]
        public static extern bool WTSQuerySessionInformation(
            IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out IntPtr ppBuffer, out uint pBytesReturned);

        [DllImport("user32.dll")]
        public static extern bool EnumWindowStations(EnumWindowStationsDelegate lpEnumFunc,
            IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowText",
            ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd,
            StringBuilder lpWindowText, int nMaxCount);


        [DllImport("user32.dll")]
        public static extern bool EnumDesktops(IntPtr hwinsta, EnumDesktopsDelegate
            lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetThreadDesktop(uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetThreadDesktop(IntPtr hDesktop);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr OpenWindowStation(
            [MarshalAs(UnmanagedType.LPTStr)] string winStationName,
            [MarshalAs(UnmanagedType.Bool)] bool inherit,
            WinStationAccess access
        );

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetProcessWindowStation(IntPtr hWinSta);

        [DllImport("user32.dll")]
        public static extern bool CloseWindowStation(
            IntPtr winStation
        );


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr OpenDesktop(
            [MarshalAs(UnmanagedType.LPTStr)] string desktopName,
            uint flags,
            bool inherit,
            WinStationAccess access
        );

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool CloseDesktop(
            IntPtr hDesktop
        );

        [DllImport("user32.dll")]
        public static extern bool SwitchDesktop(IntPtr hDesktop);

        [DllImport("user32.dll")]
        public static extern bool EnumDesktopWindows(
            IntPtr hDesktop,
            EnumDesktopWindowsDelegate lpEnumFunc,
            IntPtr lParam
        );

        [DllImport("user32", SetLastError = true)]
        private static extern IntPtr GetProcessWindowStation();


        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowThreadProcessId(
            IntPtr hWnd,
            out IntPtr processId
        );

        [DllImport("NetAPI32.dll", CharSet = CharSet.Unicode)]
        public static extern int NetLocalGroupGetMembers(
            [MarshalAs(UnmanagedType.LPWStr)] string servername,
            [MarshalAs(UnmanagedType.LPWStr)] string localgroupname,
            int level,
            out IntPtr bufptr,
            int prefmaxlen,
            out int entriesread,
            out int totalentries,
            ref int resume_handle);


        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("ADVAPI32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CreateProcessAsUser(IntPtr hToken, string lpApplicationName, string lpCommandLine,
            IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, uint dwCreationFlags, string lpEnvironment, string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("WTSAPI32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool WTSQueryUserToken(int sessionId, out IntPtr token);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern bool WTSSendMessage(IntPtr hServer, int sessionId, string pTitle, int titleLength,
            string pMessage, int messageLength, int style, int timeout, out int pResponse, bool bWait);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int WTSGetActiveConsoleSessionId();
    }
}