namespace MasterChief.DotNet.Framework.Download
{
    using MasterChief.DotNet4.Utilities.Common;

    /// <summary>
    /// 文件下载的配置
    /// </summary>
    internal static class DownloadConfigContext
    {
        #region Fields

        /// <summary>
        /// 文件下载配置
        /// </summary>
        public static DownloadConfig downloadConfig = CachedConfigContext.Instance.DownloadConfig;

        /// <summary>
        /// 文件下载的文件夹目录
        /// </summary>
        public static string DownLoadMainDirectory => downloadConfig.DownLoadMainDirectory;

        /// <summary>
        /// 限制的下载速度Kb
        /// </summary>
        public static ulong LimitDownloadSpeedKb => downloadConfig.LimitDownloadSpeedKb;

        private static readonly object syncRoot = new object();

        private static string fileNameEncryptorKey = null;
        private static byte[] _fileNameEncryptorIv = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 下载文件名称加密偏移向量
        /// </summary>
        public static byte[] FileNameEncryptorIv
        {
            get
            {
                if (_fileNameEncryptorIv == null)
                {
                    lock (syncRoot)
                    {
                        if (_fileNameEncryptorIv == null)
                        {
                            _fileNameEncryptorIv = ByteHelper.ParseHexString(downloadConfig.FileNameEncryptorIvHexString);
                        }
                    }
                }

                return _fileNameEncryptorIv;
            }
        }

        /// <summary>
        /// 下载文件名称加密Key
        /// </summary>
        public static string FileNameEncryptorKey
        {
            get
            {
                if (string.IsNullOrEmpty(fileNameEncryptorKey))
                {
                    lock (syncRoot)
                    {
                        if (string.IsNullOrEmpty(fileNameEncryptorKey))
                        {
                            fileNameEncryptorKey = downloadConfig.FileNameEncryptorKey ?? "dotnetDownloadHanlder";
                        }
                    }
                }

                return fileNameEncryptorKey;
            }
        }

        #endregion Properties
    }
}