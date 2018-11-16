using System;

namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// APP 信息
    /// </summary>
    public sealed class AppInfo
    {
        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AppInfo()
        {
            SignatureExpiredMinutes = 10;
            TokenExpiredDay = 7;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 辨识ID
        /// </summary>
        public Guid AppId
        {
            get;
            set;
        }

        /// <summary>
        /// APP加密建
        /// </summary>
        public string AppSecret
        {
            get;
            set;
        }

        /// <summary>
        /// 令牌共享Key
        /// </summary>
        public string SharedKey
        {
            get;
            set;
        }

        /// <summary>
        /// 签名过期时间 【分钟】
        /// </summary>
        public int SignatureExpiredMinutes
        {
            get;
            set;
        }

        /// <summary>
        /// 令牌过期时间 【天】
        /// </summary>
        public int TokenExpiredDay
        {
            get;
            set;
        }

        #endregion Properties
    }
}