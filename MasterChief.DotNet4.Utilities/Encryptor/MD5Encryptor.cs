namespace MasterChief.DotNet4.Utilities.Encryptor
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// MD5加密帮助类
    /// </summary>
    /// 时间：2015-11-06 9:22
    /// 备注：
    public static class MD5Encryptor
    {
        #region Methods

        /// <summary>
        /// 验证随机加密的MD5
        /// </summary>
        /// <param name="data">需要判断的字符串</param>
        /// <param name="rmd5">MD5 GUID</param>
        /// <returns>是否相等</returns>
        /// 时间：2015-11-06 9:24
        /// 备注：
        public static bool EqualsRandomMD5(this string data, Guid rmd5)
        {
            byte[] md5Array = rmd5.ToByteArray();
            byte randomKey = md5Array[0];
            using (MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider())
            {
                data += randomKey;
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                byte[] hash = md5Provider.ComputeHash(bytes);

                for (int i = 1; i < 16; i++)
                {
                    if (hash[i] != md5Array[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// 生成随机加密的
        /// </summary>
        /// <param name="data">需要加密字符串</param>
        /// <returns>MD5加密Guid</returns>
        /// 时间：2015-11-06 9:20
        /// 备注：
        public static Guid ToRandomMD5(this string data)
        {
            using (MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider())
            {
                //生成256以内的随机数
                byte randomKey = (byte)Math.Abs(new object().GetHashCode() % 256);
                data += randomKey;
                byte[] array = Encoding.UTF8.GetBytes(data);
                byte[] hash = md5Provider.ComputeHash(array);
                hash[0] = randomKey;
                return new Guid(hash);
            }
        }

        /// <summary>
        /// 获取MD5加密字符串
        /// <para>eg:StringHelper.Encrypt("YanZhiwei");==>"b07ec574a666d8e7582885ce334b4d00"</para>
        /// </summary>
        /// <param name="data">需要加密的字符串</param>
        /// <returns>MD5加密的字符串</returns>
        public static string Encrypt(string data)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] array = Encoding.UTF8.GetBytes(data);
            byte[] hash = md5.ComputeHash(array);
            md5.Clear();
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X").PadLeft(2, '0'));
            }

            return builder.ToString().ToLower();
        }

        #endregion Methods
    }
}