using System;

namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public sealed class UserInfo
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password
        {
            get;
            set;
        }
    }
}