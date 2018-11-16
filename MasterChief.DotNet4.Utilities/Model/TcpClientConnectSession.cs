namespace MasterChief.DotNet4.Utilities.Model
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using MasterChief.DotNet4.Utilities.Enum;

    /// <summary>
    /// 自定义Socket对象
    /// </summary>
    public class TcpClientConnectSession
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public TcpClientConnectSession()
        {
        }

        /// <summary>
        /// 创建Sockets对象
        /// </summary>
        /// <param name="ip">Ip地址</param>
        /// <param name="client">TcpClient</param>
        /// <param name="ns">承载客户端Socket的网络流</param>
        public TcpClientConnectSession(IPEndPoint ip, TcpClient client, NetworkStream ns)
        {
            Ip = ip;
            Client = client;
            SkStream = ns;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 客户端主通信程序
        /// </summary>
        public TcpClient Client
        {
            get; set;
        }

        /// <summary>
        /// 客户端退出标识.如果服务端发现此标识为true,那么认为客户端下线
        /// 客户端接收此标识时,认为客户端异常.
        /// </summary>
        public bool ClientDispose
        {
            get; set;
        }

        /// <summary>
        /// 异常枚举
        /// </summary>
        public TcpOperateEventCode Code
        {
            get; set;
        }

        /// <summary>
        /// 发生异常时不为null.
        /// </summary>
        public Exception ex
        {
            get; set;
        }

        /// <summary>
        /// 当前IP地址,端口号
        /// </summary>
        public IPEndPoint Ip
        {
            get; set;
        }

        /// <summary>
        /// 新客户端标识.如果推送器发现此标识为true,那么认为是客户端上线
        /// 仅服务端有效
        /// </summary>
        public bool NewClientFlag
        {
            get; set;
        }

        /// <summary>
        /// 异步接收后包的大小
        /// </summary>
        public int Offset
        {
            get; set;
        }

        /// <summary>
        /// 承载客户端Socket的网络流
        /// </summary>
        public NetworkStream SkStream
        {
            get; set;
        }

        /// <summary>
        /// 扩展对象
        /// </summary>
        public object Tag
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 关闭终端连接
        /// </summary>
        /// 时间：2015-12-04 10:03
        /// 备注：
        public void Close()
        {
            if (Client != null)
            {
                try
                {
                    Client.Client.Shutdown(SocketShutdown.Both);
                    Client.Close();
                    Client = null;
                    ClientDispose = true;
                    if (SkStream != null)
                    {
                        SkStream.Flush();
                        SkStream.Close();
                        SkStream.Dispose();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        #endregion Methods
    }
}