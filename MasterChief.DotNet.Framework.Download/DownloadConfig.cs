using System;
using System.Xml.Serialization;

namespace MasterChief.DotNet.Framework.Download
{
    /// <summary>
    /// 文件下载配置
    /// </summary>
    [Serializable]
    public class DownloadConfig
    {
        #region Properties

        /// <summary>
        /// 文件加密的偏移向量,十六进制
        /// </summary>
        [XmlAttribute("FileNameEncryptorIv")]
        public string FileNameEncryptorIvHexString
        {
            get;
            set;
        }

        /// <summary>
        /// 文件加密的Key，勿包括特殊字符
        /// </summary>
        [XmlAttribute("FileNameEncryptorKey")]
        public string FileNameEncryptorKey
        {
            get;
            set;
        }

        /// <summary>
        /// 限制的下载速度Kb
        /// </summary>
        [XmlAttribute("LimitDownloadSpeedKb")]
        public ulong LimitDownloadSpeedKb
        {
            get;
            set;
        }

        /// <summary>
        /// 文件下载的文件夹目录
        /// </summary>
        [XmlAttribute("DownLoadMainDirectory")]
        public string DownLoadMainDirectory
        {
            get;
            set;
        }

        #endregion Properties
    }
}