namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// 客户端请求信息
    /// </summary>
    /// 时间：2016/7/27 14:29
    /// 备注：
    public class RequestClientInfo
    {
        /// <summary>
        /// 浏览器名称以及版本
        /// </summary>
        public string BrowserVersion { get; set; }

        /// <summary>
        /// 计算机名称
        /// </summary>
        public string ComputerName { get; set; }

        /// <summary>
        /// Ip地址
        /// </summary>
        public string Ip4Address { get; set; }

        /// <summary>
        /// 操作系统名称以及版本
        /// </summary>
        public string OSVersion { get; set; }
    }
}