using System;

namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// 令牌信息
    /// </summary>
    [Serializable]
    public class TokenInfo
    {
        #region Properties

        /// <summary>
        /// 令牌
        /// </summary>
        public string Access_token
        {
            get;
            set;
        }

        /// <summary>
        /// 签名有效时间【分钟】
        /// </summary>
        public int Expires_in
        {
            get;
            set;
        }

        #endregion Properties
    }
}