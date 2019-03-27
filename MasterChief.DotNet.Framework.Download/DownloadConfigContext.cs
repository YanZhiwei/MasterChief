using MasterChief.DotNet4.Utilities.Common;

namespace MasterChief.DotNet.Framework.Download
{
    /// <summary>
    ///     文件下载的配置
    /// </summary>
    internal static class DownloadConfigContext
    {
        #region Fields

        /// <summary>
        ///     文件下载配置
        /// </summary>
        public static DownloadConfig DownloadConfig = CachedConfigContext.Instance.DownloadConfig;

        /// <summary>
        ///     文件下载的文件夹目录
        /// </summary>
        public static string DownLoadMainDirectory => DownloadConfig.DownLoadMainDirectory;

        /// <summary>
        ///     限制的下载速度Kb
        /// </summary>
        public static ulong LimitDownloadSpeedKb => DownloadConfig.LimitDownloadSpeedKb;

        private static readonly object SyncRoot = new object();

        private static string _fileNameEncryptorKey;
        private static byte[] _fileNameEncryptorIv;

        #endregion Fields

        #region Properties

        /// <summary>
        ///     下载文件名称加密偏移向量
        /// </summary>
        public static byte[] FileNameEncryptorIv
        {
            get
            {
                if (_fileNameEncryptorIv == null)
                    lock (SyncRoot)
                    {
                        if (_fileNameEncryptorIv == null)
                            _fileNameEncryptorIv = DownloadConfig.FileNameEncryptorIvHexString.ParseHexString();
                    }

                return _fileNameEncryptorIv;
            }
        }

        /// <summary>
        ///     下载文件名称加密Key
        /// </summary>
        public static string FileNameEncryptorKey
        {
            get
            {
                if (string.IsNullOrEmpty(_fileNameEncryptorKey))
                    lock (SyncRoot)
                    {
                        if (string.IsNullOrEmpty(_fileNameEncryptorKey))
                            _fileNameEncryptorKey = DownloadConfig.FileNameEncryptorKey ?? "dotnetDownloadHanlder";
                    }

                return _fileNameEncryptorKey;
            }
        }

        #endregion Properties
    }
}