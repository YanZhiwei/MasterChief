using System;

namespace MasterChief.DotNet.ProjectTemplate.WebApi.Model
{
    /// <summary>
    ///     验证用户信息
    /// </summary>
    [Serializable]
    public sealed class IdentityUser
    {
        /// <summary>
        ///     用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        ///     用户密码
        /// </summary>
        public string Password { get; set; }
    }
}