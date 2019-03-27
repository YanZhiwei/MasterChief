using System;
using System.Web;
using MasterChief.DotNet4.Utilities.WebForm.Core;

namespace MasterChief.DotNet.Framework.Download
{
    /// <summary>
    ///     处理文件下载
    /// </summary>
    public abstract class DownloadHandler
    {
        #region Methods

        /// <summary>
        ///     下载失败
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="fileName">本地文件名称</param>
        /// <param name="filePath">下载文件路径</param>
        /// <param name="ex">错误信息</param>
        public abstract void OnDownloadFailed(HttpContext context, string fileName, string filePath, string ex);

        /// <summary>
        ///     下载成功
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="fileName">本地文件名称</param>
        /// <param name="filePath">下载文件路径</param>
        public abstract void OnDownloadSucceed(HttpContext context, string fileName, string filePath);


        /// <summary>
        ///     开始下载
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="fileName"></param>
        public virtual void StartDownloading(HttpContext context, string fileName)
        {
            fileName = fileName?.Trim();
            if (string.IsNullOrEmpty(fileName))
            {
                OnDownloadFailed(context, fileName, string.Empty, "请求下载文件非法.");
                return;
            }

            try
            {
                var downloadFileName = DownloadFileContext.Instance.DecryptFileName(fileName);
                var filePath =
                    DownloadConfigContext.DownLoadMainDirectory +
                    downloadFileName; //HttpContext.Current.Server.MapPath("~/") + "files/" + _downloadFileName;
                var result = WebDownloadFile.FileDownload(context, downloadFileName, filePath,
                    DownloadConfigContext.LimitDownloadSpeedKb * 1024);

                if (result.State)
                    OnDownloadSucceed(context, downloadFileName, filePath);
                else
                    OnDownloadFailed(context, downloadFileName, filePath, result.Message);
            }
            catch (Exception ex)
            {
                OnDownloadFailed(context, fileName, string.Empty, ex.Message);
            }
            finally
            {
                context.Response.End();
            }
        }

        #endregion Methods
    }
}