using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using MasterChief.DotNet4.WindowsAPI.Core;
using MasterChief.DotNet4.WindowsAPI.Enum;
using MasterChief.DotNet4.WindowsAPI.Model;

namespace MasterChief.DotNet4.WindowsAPI
{
    public class WindowsCore
    {
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
        ///     获取所有窗口列表
        /// </summary>
        /// <returns>窗口列表</returns>
        public static Window[] GetWindows()
        {
            var windows = new List<Window>();
            var callback = new Win32Api.EnumDesktopWindowsDelegate((hWnd, lParam) =>
            {
                windows.Add(new Window(hWnd));
                return true;
            });
            if (!Win32Api.EnumDesktopWindows(IntPtr.Zero, callback, IntPtr.Zero))
                throw new Win32ErrorCodeException("EnumDesktopWindows");
            return windows.ToArray();
        }

        /// <summary>
        ///     根据桌面名称查询窗口列表
        /// </summary>
        /// <param name="desktopName">桌面名称</param>
        /// <returns>窗口列表</returns>
        public static Window[] GetWindows(string desktopName)
        {
            var desktopHandle = Win32Api.OpenDesktop(desktopName, 0, true, WinStationAccess.GENERIC_ALL);
            if (desktopHandle == IntPtr.Zero) throw new Win32ErrorCodeException("OpenDesktop('" + desktopName + "')");

            try
            {
                var windows = new List<Window>();
                var callback = new Win32Api.EnumDesktopWindowsDelegate((hWnd, lParam) =>
                {
                    windows.Add(new Window(hWnd));
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


        /// <summary>
        ///     获取Windows Session
        /// </summary>
        /// <returns>Session集合</returns>
        public static Session[] GetSessions()
        {
            var serverHandle = Win32Api.WTSOpenServer(Environment.MachineName);
            try
            {
                var sessionInfoPtr = IntPtr.Zero;
                var sessionCount = 0;
                var hasSession =
                    Win32Api.WTSEnumerateSessions(serverHandle, 0, 1, ref sessionInfoPtr, ref sessionCount) > 0;
                if (!hasSession) return new Session[0];
                var dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
                var currentSession = sessionInfoPtr;
                var sessions = new Session[sessionCount];
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
                    sessions[i] = new Session(userName, domain, si.State, si.SessionID);

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
    }
}