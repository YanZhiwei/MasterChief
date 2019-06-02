using MasterChief.DotNet4.WindowsAPI.Enum;

namespace MasterChief.DotNet4.WindowsAPI.Model
{
    /// <summary>
    ///     Windows Session 信息
    /// </summary>
    public sealed class SessionInfo
    {
        /// <summary>
        ///     状态
        /// </summary>
        public readonly WTS_CONNECTSTATE_CLASS ConnectionState;

        /// <summary>
        ///     Domain
        /// </summary>
        public readonly string Domain;

        /// <summary>
        ///     SessionId
        /// </summary>
        public readonly int SessionId;

        /// <summary>
        ///     用户名称
        /// </summary>
        public readonly string UserName;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="domain">Domain</param>
        /// <param name="connectionState">状态</param>
        /// <param name="sessionId">SessionId</param>
        public SessionInfo(string userName, string domain, WTS_CONNECTSTATE_CLASS connectionState, int sessionId)
        {
            UserName = userName;
            Domain = domain;
            ConnectionState = connectionState;
            SessionId = sessionId;
        }
    }
}