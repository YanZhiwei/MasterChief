using MasterChief.DotNet4.Utilities.DesignPattern;
using MasterChief.DotNet4.Utilities.Encryptor;
using System.Security.Cryptography;
using System.Web;

namespace MasterChief.DotNet.Framework.Download
{
    /// <summary>
    /// 下载文件加密解密辅助类
    /// </summary>
    public class DownloadFileHelper
    {
        private static AESEncryptor _fileEncryptor = null;

        /// <summary>
        /// 获取对象实例
        /// </summary>
        public static DownloadFileHelper Instance => Singleton<DownloadFileHelper>.Instance;

        static DownloadFileHelper()
        {
            Aes aes = AESEncryptor.CreateAES(DownloadConfigContext.FileNameEncryptorKey);
            _fileEncryptor = new AESEncryptor(aes.Key, DownloadConfigContext.FileNameEncryptorIv);
        }

        /// <summary>
        /// 加密下载文件
        /// </summary>
        /// <param name="fileName">需要下载文件名称</param>
        /// <returns>加密后的文件</returns>
        public string EncryptFileName(string fileName)
        {
            return _fileEncryptor.Encrypt(HttpUtility.UrlEncode(fileName));
        }

        /// <summary>
        /// 解密下载文件
        /// </summary>
        /// <param name="encryptFileName">加密下载文件字符串</param>
        /// <returns>原始下载文件名称</returns>
        internal string DecryptFileName(string encryptFileName)
        {
            return _fileEncryptor.Decrypt(encryptFileName);
        }
    }
}