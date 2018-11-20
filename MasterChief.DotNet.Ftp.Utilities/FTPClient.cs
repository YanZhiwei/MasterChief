namespace MasterChief.DotNet.Ftp.Utilities
{
    using MasterChief.DotNet4.Utilities.Operator;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.FtpClient;

    /// <summary>
    /// Ftp 客户端
    /// </summary>
    /// 时间：2016-04-26 8:58
    /// 备注：
    public class FTPClient
    {
        #region Fields

        /// <summary>
        /// 服务器IP
        /// </summary>
        public readonly string ServerHost;

        /// <summary>
        /// 用户名称
        /// </summary>
        public readonly string UserName;

        /// <summary>
        /// 用户密码
        /// </summary>
        public readonly string UserPassword;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serverHost">服务器IP</param>
        /// <param name="userName">用户名</param>
        /// <param name="userPassword">密码</param>
        /// 时间：2016-04-26 9:01
        /// 备注：
        public FTPClient(string serverHost, string userName, string userPassword)
        {
            ValidateOperator.Begin().NotNullOrEmpty(serverHost, "服务器IP").NotNullOrEmpty(UserName, "用户名").NotNullOrEmpty(userPassword, "用户密码");
            UserName = userName;
            UserPassword = userPassword;
            ServerHost = serverHost;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// FTP下载文件
        /// </summary>
        /// <param name="serverDire">服务器路径，eg："/Serverpath/"</param>
        /// <param name="localDire">本地保存文件夹</param>
        public bool DownloadFile(string serverDire, string localDire)
        {
            ValidateOperator.Begin()
                .NotNullOrEmpty(serverDire, "服务器路径，eg：'/Serverpath/'")
                .NotNullOrEmpty(localDire, "本地保存路径")
                .CheckDirectoryExist(localDire);

            List<string> downloadFiles = null;
            using (FtpClient ftpClient = CreateFtpClient())
            {
                downloadFiles = DownloadFromFtpSvr(ftpClient, serverDire, localDire);
            }

            return CheckFtpDownFiles(downloadFiles, localDire);
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns>登陆是否成功</returns>
        public bool Login()
        {
            using (FtpClient ftp = new FtpClient())
            {
                ftp.Host = ServerHost;
                ftp.Credentials = new NetworkCredential(UserName, UserPassword);
                ftp.Connect();
                return ftp.IsConnected;
            }
        }

        private bool CheckFtpDownFiles(List<string> downloadFiles, string localDire)
        {
            string[] localfiles = Directory.GetFiles(localDire);
            int checkSum = 0;

            foreach (string local in localfiles)
            {
                if (Array.Exists(localfiles,
            s => s.Equals(local, StringComparison.InvariantCultureIgnoreCase)))
                {
                    checkSum++;
                }
            }

            return checkSum == downloadFiles.Count;
        }

        private FtpClient CreateFtpClient()
        {
            FtpClient ftpClient = new FtpClient
            {
                Host = ServerHost,
                Credentials = new NetworkCredential(UserName, UserPassword)
            };
            ftpClient.Connect();
            return ftpClient;
        }

        private List<string> DownloadFromFtpSvr(FtpClient ftpClient, string serverpath, string localDire)
        {
            List<string> downloadFiles = new List<string>();
            foreach (FtpListItem ftpItem in ftpClient.GetListing(serverpath, FtpListOption.Modify | FtpListOption.Size))
            {
                string descPath = string.Format(@"{0}\{1}", localDire, ftpItem.Name);
                if (File.Exists(descPath))
                {
                    File.Decrypt(descPath);
                }

                using (Stream ftpStream = ftpClient.OpenRead(ftpItem.FullName))
                {
                    using (FileStream fileStream = File.Create(descPath, (int)ftpStream.Length))
                    {
                        byte[] buffer = new byte[200 * 1024];
                        int count;

                        while ((count = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, count);
                        }
                    }
                    downloadFiles.Add(ftpItem.Name);
                }
            }
            return downloadFiles;
        }

        #endregion Methods
    }
}