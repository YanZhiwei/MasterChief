namespace MasterChief.DotNet4.Utilities.Common
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// 模糊替换
    /// </summary>
    public static class FuzzyHelper
    {
        #region Methods

        /// <summary>
        /// 模糊替换邮箱
        /// <para>eg:churenyouzi@outlook.com==>ch*********@outlook.com</para>
        /// </summary>
        /// <param name="input">电子邮箱</param>
        /// <returns>模糊替换后的邮箱</returns>
        public static string FuzzyEmail(this string input)
        {
            return Regex.Replace(input, @"(?<=\S{2}\S*?)[^@\s](?=\S*?@\S+)", "*");
        }

        /// <summary>
        /// 模糊替换手机号码
        /// <para>eg:18501600110==>185****0110</para>
        /// </summary>
        /// <param name="input">手机号码</param>
        /// <returns>模糊替换后的手机号码</returns>
        public static string FuzzyMobieNumber(this string input)
        {
            return Regex.Replace(input, @"(?<=\S{3}\S*?).+?(?=\S*?\S{4})", "*");
        }

        /// <summary>
        /// 模糊替换用户名称
        /// <para>eg:朱重八==>朱***八</para>
        /// </summary>
        /// <param name="input">用户名</param>
        /// <returns>模糊替换后的用户名称</returns>
        public static string FuzzyUserName(this string input)
        {
            return Regex.Replace(input, @"(?<=\S{1}\S*?).+?(?=\S*?\S{1})", "*").Replace("*", string.Empty).Insert(1, "***"); ;
        }

        #endregion Methods
    }
}