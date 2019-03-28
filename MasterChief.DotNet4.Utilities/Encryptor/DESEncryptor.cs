namespace MasterChief.DotNet4.Utilities.Encryptor
{
    using Common;
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// DES(Data Encryption Standard)
    /// DES使用的密钥key为8字节，初始向量IV也是8字节。
    /// </summary>
    public class DesEncryptor
    {
        #region Fields

        /// <summary>
        /// 默认加密Key
        /// </summary>
        private const string DefaultKey = "MasterChief.DotNet4.Utilities";

        /// <summary>
        /// 默认向量
        /// </summary>
        private static readonly byte[] DefaultIv = { 0x21, 0x45, 0x65, 0x87, 0x09, 0xBA, 0xDC, 0xEF };

        /// <summary>
        /// The DES
        /// </summary>
        private DES _desProvider;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        public DesEncryptor(byte[] key, byte[] iv)
        {
            _desProvider = new DESCryptoServiceProvider
            {
                Key = key,
                IV = iv
            };
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// 时间：2016-01-14 13:23
        /// 备注：
        public DesEncryptor(string key, byte[] iv)
        {
            _desProvider = new DESCryptoServiceProvider();
            key = key.Substring(0, 8);
            key = key.PadRight(8, ' ');
            _desProvider.Key = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            _desProvider.IV = iv;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 生成DES
        /// </summary>
        /// <returns>DES</returns>
        public static DES CreateDes()
        {
            return CreateDes(string.Empty);
        }

        /// <summary>
        /// 根据KEY生成DES
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>DES</returns>
        public static DES CreateDes(string key)
        {
            DES desProvider = new DESCryptoServiceProvider();
            DESCryptoServiceProvider desCryptoService = (DESCryptoServiceProvider)DES.Create();

            if (!string.IsNullOrEmpty(key))
            {
                MD5 md5Provider = new MD5CryptoServiceProvider();
                desProvider.Key = ArrayHelper.Copy(md5Provider.ComputeHash(Encoding.UTF8.GetBytes(key)), 0, 8);
            }
            else
            {
                desProvider.Key = desCryptoService.Key;
            }

            desProvider.IV = desProvider.IV;
            return desProvider;
        }

        /// <summary>
        /// 采用默认向量，Key解密
        /// </summary>
        /// <param name="text">需要解密的字符串.</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string text)
        {
            DesEncryptor desEncryptor = new DesEncryptor(DefaultKey, DefaultIv);
            return desEncryptor.DecryptString(text);
        }

        /// <summary>
        /// 采用默认向量,KEY加密
        /// </summary>
        /// <param name="text">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string text)
        {
            DesEncryptor desEncryptor = new DesEncryptor(DefaultKey, DefaultIv);
            return desEncryptor.EncryptString(text);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="text">需要解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public string DecryptString(string text)
        {
            byte[] decryptedData = Convert.FromBase64String(text);
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    CryptoStream cryptoStream = new CryptoStream(ms, _desProvider.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(decryptedData, 0, decryptedData.Length);
                    cryptoStream.FlushFinalBlock();
                }
                catch
                {
                    return "N/A";
                }

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="text">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public string EncryptString(string text)
        {
            byte[] encryptedData = Encoding.UTF8.GetBytes(text);
            using (MemoryStream ms = new MemoryStream())
            {
                CryptoStream cryptoStream = new CryptoStream(ms, _desProvider.CreateEncryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(encryptedData, 0, encryptedData.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        #endregion Methods
    }
}