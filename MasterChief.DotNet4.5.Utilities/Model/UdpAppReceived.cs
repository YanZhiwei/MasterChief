using System.Net;

namespace MasterChief.DotNet4._5.Utilities.Model
{
    /// <summary>
    ///     Upd接受数据
    /// </summary>
    public struct UdpAppReceived
    {
        /// <summary>
        ///     发送者
        /// </summary>
        public IPEndPoint Sender { get; set; }

        /// <summary>
        ///     发送消息
        /// </summary>
        public string Message { get; set; }
    }
}