using System;
using System.Text;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     Base64帮助类
    /// </summary>
    public static class Base64Helper
    {
        #region Methods

        /// <summary>
        ///     Base64字符串解码
        /// </summary>
        /// <param name="data">Base64字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string ParseBase64String(this string data)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(data));
        }

        /// <summary>
        ///     Base64字符串译码
        /// </summary>
        /// <param name="data">字符串</param>
        /// <returns>Base64字符串</returns>
        public static string ToBase64String(this string data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }

        #endregion Methods
    }
}