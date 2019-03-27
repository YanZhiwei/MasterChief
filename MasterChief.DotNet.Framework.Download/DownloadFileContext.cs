using System;
using System.Web;
using MasterChief.DotNet4.Utilities.Encryptor;

namespace MasterChief.DotNet.Framework.Download
{
    /// <summary>
    ///     下载文件加密解密辅助类
    /// </summary>
    public class DownloadFileContext
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public DownloadFileContext()
        {
            var aes = AESEncryptor.CreateAES(DownloadConfigContext.FileNameEncryptorKey);
            _fileEncryptor = new AESEncryptor(aes.Key, DownloadConfigContext.FileNameEncryptorIv);
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        /// DownloadFileContext
        /// </summary>
        public static DownloadFileContext Instance => _instance.Value;

        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<DownloadFileContext> _instance =
            new Lazy<DownloadFileContext>(() => new DownloadFileContext());

        private static AESEncryptor _fileEncryptor;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     加密下载文件
        /// </summary>
        /// <param name="fileName">需要下载文件名称</param>
        /// <returns>加密后的文件</returns>
        public string EncryptFileName(string fileName)
        {
            return HttpUtility.UrlEncode(_fileEncryptor.Encrypt(fileName));
        }

        /// <summary>
        ///     解密下载文件
        /// </summary>
        /// <param name="encryptFileName">加密下载文件字符串</param>
        /// <returns>原始下载文件名称</returns>
        internal string DecryptFileName(string encryptFileName)
        {
            return _fileEncryptor.Decrypt(encryptFileName);
        }

        #endregion Methods
    }
}