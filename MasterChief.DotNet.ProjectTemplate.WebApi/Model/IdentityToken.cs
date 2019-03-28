using System;

namespace MasterChief.DotNet.ProjectTemplate.WebApi.Model
{
    /// <summary>
    ///     令牌信息
    /// </summary>
    [Serializable]
    public class IdentityToken
    {
        #region Properties

        /// <summary>
        ///     令牌
        /// </summary>
        public string AccessToken { get; set; }


        /// <summary>
        ///     签名有效时间【分钟】
        /// </summary>
        public int ExpiresIn { get; set; }

        #endregion Properties
    }
}