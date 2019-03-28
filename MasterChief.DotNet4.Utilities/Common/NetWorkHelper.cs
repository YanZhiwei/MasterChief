using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     NetWork帮助类
    /// </summary>
    public static class NetWorkHelper
    {
        #region Methods

        /// <summary>
        ///     获取本机名
        /// </summary>
        /// <returns>本机名</returns>
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }

        /// <summary>
        ///     获取本地Ip4地址集合
        /// </summary>
        /// <returns>本地Ip4地址集合</returns>
        public static List<IPAddress> GetLocalIp4Address()
        {
            var addresses = new List<IPAddress>();
            var hostEntry = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in hostEntry.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    addresses.Add(ip);

            return addresses;
        }

        /// <summary>
        ///     根据网卡类型来获取mac地址
        /// </summary>
        /// <param name="networkType">网卡类型</param>
        /// <param name="getMacAddrFactory">格式化获取到的mac地址</param>
        /// <returns>获取到的mac地址</returns>
        public static string GetMacAddress(NetworkInterfaceType networkType, Func<string, string> getMacAddrFactory)
        {
            var macAddr = string.Empty;
            var allnetworks = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var adapter in allnetworks)
                if (adapter.NetworkInterfaceType == networkType)
                {
                    macAddr = adapter.GetPhysicalAddress().ToString();

                    if (!string.IsNullOrEmpty(macAddr)) break;
                }

            if (getMacAddrFactory != null) macAddr = getMacAddrFactory(macAddr);

            return macAddr;
        }

        /// <summary>
        ///     根据网卡类型以及网卡状态获取mac地址
        /// </summary>
        /// <param name="networkType">网卡类型</param>
        /// <param name="status">网卡状态</param>
        /// Up 网络接口已运行，可以传输数据包。
        /// Down 网络接口无法传输数据包。
        /// Testing 网络接口正在运行测试。
        /// Unknown 网络接口的状态未知。
        /// Dormant 网络接口不处于传输数据包的状态；它正等待外部事件。
        /// NotPresent 由于缺少组件（通常为硬件组件），网络接口无法传输数据包。
        /// LowerLayerDown 网络接口无法传输数据包，因为它运行在一个或多个其他接口之上，而这些“低层”接口中至少有一个已关闭。
        /// <param name="getMacAddrFactory">格式化获取到的mac地址</param>
        /// <returns>获取到的mac地址</returns>
        public static string GetMacAddress(NetworkInterfaceType networkType, OperationalStatus status,
            Func<string, string> getMacAddrFactory)
        {
            var macAddr = string.Empty;
            var allnetworks = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var adapter in allnetworks)
                if (adapter.NetworkInterfaceType == networkType)
                {
                    if (adapter.OperationalStatus != status) continue;

                    macAddr = adapter.GetPhysicalAddress().ToString();

                    if (!string.IsNullOrEmpty(macAddr)) break;
                }

            if (getMacAddrFactory != null) macAddr = getMacAddrFactory(macAddr);

            return macAddr;
        }

        /// <summary>
        ///     获取读到的第一个mac地址
        /// </summary>
        /// <param name="getMacAddrFactory">委托</param>
        /// <returns>mac地址</returns>
        public static string GetMacAddress(Func<string, string> getMacAddrFactory)
        {
            var macAddr = string.Empty;
            var allnetworks = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var adapter in allnetworks)
            {
                macAddr = adapter.GetPhysicalAddress().ToString();

                if (!string.IsNullOrEmpty(macAddr)) break;
            }

            if (getMacAddrFactory != null) macAddr = getMacAddrFactory(macAddr);

            return macAddr;
        }

        /// <summary>
        ///     通过ARP方式获取MAC地址
        /// </summary>
        /// <param name="ip">当前IP地址</param>
        /// <returns>MAC地址</returns>
        /// 时间：2016/7/27 13:42
        /// 备注：
        public static string GetMacAddressByArp(string ip)
        {
            var builder = new StringBuilder();

            try
            {
                var ipAddress = inet_addr(ip);
                var macInfo = new long();
                long length = 6;
                SendARP(ipAddress, 0, ref macInfo, ref length);
                var temp = Convert.ToString(macInfo, 16).PadLeft(12, '0').ToUpper();
                var x = 12;

                for (var i = 0; i < 6; i++)
                {
                    if (i == 5)
                        builder.Append(temp.Substring(x - 2, 2));
                    else
                        builder.Append(temp.Substring(x - 2, 2) + "-");

                    x -= 2;
                }

                return builder.ToString();
            }
            catch
            {
                return builder.ToString();
            }
        }

        /// <summary>
        ///     将字符串IP转换为IPAddress对象，若转换失败，则返回NULL
        /// </summary>
        /// <param name="data">The ip address.</param>
        /// <returns>若转换失败，则返回NULL</returns>
        public static IPAddress ParseIpString(this string data)
        {
            if (!IPAddress.TryParse(data, out var ipAddress)) return null;

            return ipAddress;
        }

        /// <summary>
        ///     指定指定端口是否已经被占用的代码
        /// </summary>
        /// <param name="port">端口号</param>
        /// <returns>是否已经占用</returns>
        /// 日期：2015-10-09 16:16
        /// 备注：
        public static bool PortInUse(int port)
        {
            var inUse = false;
            var iPGlobal = IPGlobalProperties.GetIPGlobalProperties();
            var iPEndPoints = iPGlobal.GetActiveTcpListeners();

            foreach (var endPoint in iPEndPoints)
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }

            return inUse;
        }

        [DllImport("Ws2_32.dll")]
        private static extern int inet_addr(string ip);

        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(int dest, int host, ref long mac, ref long length);

        #endregion Methods
    }
}