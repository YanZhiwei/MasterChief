namespace MasterChief.DotNet4.Utilities.Encryptor
{
    using MasterChief.DotNet4.Utilities.Operator;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// SHA256 加密
    /// </summary>
    /// 时间：2016/9/22 10:08
    /// 备注：
    public static class SHA256Encryptor
    {
        #region Methods

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="secretKey">加密密钥</param>
        /// <param name="encryptString">需要加密的字符串</param>
        /// <returns></returns>
        /// 时间：2016/9/22 10:13
        /// 备注：
        public static string Encrypt(string secretKey, string encryptString)
        {
            ValidateOperator.Begin().NotNullOrEmpty(secretKey, "加密密钥").NotNullOrEmpty(encryptString, "需要加密处理的字符串");
            byte[] keyData = Encoding.UTF8.GetBytes(secretKey);
            byte[] plainData = Encoding.UTF8.GetBytes(encryptString);
            using (HMACSHA256 sha256 = new HMACSHA256(keyData))
            {
                StringBuilder builder = new StringBuilder();
                byte[] hash = sha256.ComputeHash(plainData);

                foreach (byte item in hash)
                {
                    builder.Append(string.Format("{0:x2}", item));
                }

                return builder.ToString();
            }
        }

        #endregion Methods
    }
}