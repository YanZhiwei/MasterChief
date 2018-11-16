using System.Net.Sockets;

namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// Socket连接信息
    /// </summary>
    /// 时间：2016/6/7 13:25
    /// 备注：
    public class SocketConnectionInfo
    {
        /// <summary>
        /// 缓冲大小
        /// </summary>
        public const int BufferSize = 1048576;
        
        /// <summary>
        ///Socket对象
        /// </summary>
        public Socket Socket
        {
            get;
            set;
        }
        
        /// <summary>
        /// 缓冲
        /// </summary>
        public byte[] Buffer;
        
        /// <summary>
        /// 读取字节
        /// </summary>
        public int BytesRead
        {
            get;
            set;
        }
    }
}