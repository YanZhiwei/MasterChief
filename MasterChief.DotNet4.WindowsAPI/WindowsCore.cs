using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using MasterChief.DotNet4.Utilities.Operator;
using MasterChief.DotNet4.WindowsAPI.Core;
using MasterChief.DotNet4.WindowsAPI.Enum;
using MasterChief.DotNet4.WindowsAPI.Model;

namespace MasterChief.DotNet4.WindowsAPI
{
    /// <summary>
    ///     基于Windows Api 核心操作
    /// </summary>
    public sealed class WindowsCore
    {
        #region Methods

        /// <summary>
        ///     以当前登录系统的用户角色权限启动指定的进程
        /// </summary>
        /// <param name="processPath">指定的进程(全路径)</param>
        public static void CreateProcess(string processPath)
        {
            ValidateOperator.Begin().NotNullOrEmpty(processPath, "需要运行程序全路径").CheckFileExists(processPath);
            var ppSessionInfo = IntPtr.Zero;
            var sessionCount = 0;
            var hasSession = Win32Api.WTSEnumerateSessions(IntPtr.Zero, 0, 1, ref ppSessionInfo, ref sessionCount) != 0;

            try
            {
                if (!hasSession)
                    throw new Win32ErrorCodeException("WTSEnumerateSessions==0");
                for (var count = 0; count < sessionCount; count++)
                {
                    var si = (WTS_SESSION_INFO) Marshal.PtrToStructure(
                        ppSessionInfo + count * Marshal.SizeOf(typeof(WTS_SESSION_INFO)), typeof(WTS_SESSION_INFO));

                    if (si.State != WTS_CONNECTSTATE_CLASS.WTSActive) continue;

                    if (!Win32Api.WTSQueryUserToken(si.SessionID, out var hToken)) continue;

                    var tStartUpInfo = new STARTUPINFO
                    {
                        cb = Marshal.SizeOf(typeof(STARTUPINFO))
                    };
                    var childProcStarted = Win32Api.CreateProcessAsUser(
                        hToken,
                        processPath,
                        null,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        false,
                        0,
                        null,
                        null,
                        ref tStartUpInfo,
                        out var tProcessInfo
                    );
                    if (!childProcStarted) throw new Win32ErrorCodeException($"CreateProcessAsUser({processPath})");
                    Win32Api.CloseHandle(tProcessInfo.hThread);
                    Win32Api.CloseHandle(tProcessInfo.hProcess);

                    Win32Api.CloseHandle(hToken);
                    break;
                }
            }
            finally
            {
                if (ppSessionInfo != IntPtr.Zero)
                    Win32Api.WTSFreeMemory(ppSessionInfo);
            }
        }

        /// <summary>
        ///     获取所有窗口列表
        /// </summary>
        /// <returns>窗口列表</returns>
        public static Model.WindowInfo[] GetWindows()
        {
            var windows = new List<Model.WindowInfo>();
            var callback = new Win32Api.EnumDesktopWindowsDelegate((hWnd, lParam) =>
            {
                windows.Add(new Model.WindowInfo(hWnd));
                return true;
            });
            if (!Win32Api.EnumDesktopWindows(IntPtr.Zero, callback, IntPtr.Zero))
                throw new Win32ErrorCodeException("EnumDesktopWindows");
            return windows.ToArray();
        }

        /// <summary>
        ///     根据WindowStation名称查询DesktopName列表
        /// </summary>
        /// <param name="winStationName">WindowStation名称</param>
        /// <returns>DesktopName列表</returns>
        public static string[] GetDesktopNames(string winStationName)
        {
            var stationHandle = Win32Api.OpenWindowStation(winStationName, false,
                WinStationAccess.WINSTA_ENUMDESKTOPS | WinStationAccess.WINSTA_ENUMERATE);
            if (stationHandle == IntPtr.Zero)
                throw new Win32ErrorCodeException("OpenWindowStation('" + winStationName + "')");

            try
            {
                IList<string> desktops = new List<string>();
                var callback = new Win32Api.EnumDesktopsDelegate((desktopName, lParam) =>
                {
                    desktops.Add(desktopName);
                    return true;
                });

                if (!Win32Api.EnumDesktops(stationHandle, callback, IntPtr.Zero))
                    throw new Win32ErrorCodeException("EnumDesktops('" + winStationName + "')");

                return desktops.ToArray();
            }
            finally
            {
                Win32Api.CloseWindowStation(stationHandle);
            }
        }

        /// <summary>
        ///     获取Windows Session
        /// </summary>
        /// <returns>Session集合</returns>
        public static SessionInfo[] GetSessions()
        {
            var serverHandle = Win32Api.WTSOpenServer(Environment.MachineName);
            try
            {
                var sessionInfoPtr = IntPtr.Zero;
                var sessionCount = 0;
                var hasSession =
                    Win32Api.WTSEnumerateSessions(serverHandle, 0, 1, ref sessionInfoPtr, ref sessionCount) > 0;
                if (!hasSession) return new SessionInfo[0];
                var dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
                var currentSession = sessionInfoPtr;
                var sessions = new SessionInfo[sessionCount];
                for (var i = 0; i < sessionCount; i++)
                {
                    var si = (WTS_SESSION_INFO) Marshal.PtrToStructure(currentSession, typeof(WTS_SESSION_INFO));
                    currentSession += dataSize;

                    Win32Api.WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSUserName,
                        out var userPtr, out _);
                    Win32Api.WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSDomainName,
                        out var domainPtr, out _);
                    var userName = Marshal.PtrToStringAnsi(userPtr);
                    var domain = Marshal.PtrToStringAnsi(domainPtr);
                    sessions[i] = new SessionInfo(userName, domain, si.State, si.SessionID);

                    Win32Api.WTSFreeMemory(userPtr);
                    Win32Api.WTSFreeMemory(domainPtr);
                }

                Win32Api.WTSFreeMemory(sessionInfoPtr);
                return sessions;
            }
            finally
            {
                if (serverHandle != IntPtr.Zero)
                    Win32Api.WTSCloseServer(serverHandle);
            }
        }

        /// <summary>
        ///     根据桌面名称查询窗口列表
        /// </summary>
        /// <param name="desktopName">桌面名称</param>
        /// <returns>窗口列表</returns>
        public static Model.WindowInfo[] GetWindows(string desktopName)
        {
            var desktopHandle = Win32Api.OpenDesktop(desktopName, 0, true, WinStationAccess.GENERIC_ALL);
            if (desktopHandle == IntPtr.Zero) throw new Win32ErrorCodeException("OpenDesktop('" + desktopName + "')");

            try
            {
                var windows = new List<Model.WindowInfo>();
                var callback = new Win32Api.EnumDesktopWindowsDelegate((hWnd, lParam) =>
                {
                    windows.Add(new Model.WindowInfo(hWnd));
                    return true;
                });

                if (!Win32Api.EnumDesktopWindows(desktopHandle, callback, IntPtr.Zero))
                    throw new Win32ErrorCodeException("EnumDesktopWindows('" + desktopName + "')");

                return windows.ToArray();
            }
            finally
            {
                Win32Api.CloseDesktop(desktopHandle);
            }
        }

        /// <summary>
        ///     获取WindowStation名称列表
        /// </summary>
        /// <returns>WindowStation名称列表</returns>
        public static string[] GetWindowStationNames()
        {
            return GetWindowStationNames(IntPtr.Zero);
        }

        /// <summary>
        ///     根据用户Token查询WindowStation名称列表
        /// </summary>
        /// <param name="userPtr">用户Token句柄</param>
        /// <returns>WindowStation名称列表</returns>
        public static string[] GetWindowStationNames(IntPtr userPtr)
        {
            IList<string> winStations = new List<string>();
            var callback = new Win32Api.EnumWindowStationsDelegate((windowStation, lParam) =>
            {
                winStations.Add(windowStation);
                return true;
            });

            if (!Win32Api.EnumWindowStations(callback, userPtr))
                throw new Win32ErrorCodeException("EnumWindowStations");
            return winStations.ToArray();
        }

        #endregion Methods
    }
}