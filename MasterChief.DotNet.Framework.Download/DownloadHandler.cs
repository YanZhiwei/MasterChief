using MasterChief.DotNet4.Utilities.Result;
using MasterChief.DotNet4.Utilities.WebForm.Core;
using System.Web;

namespace MasterChief.DotNet.Framework.Download
{
    /// <summary>
    /// 处理文件下载
    /// </summary>
    public abstract class DownloadHandler : IHttpHandler
    {
        #region Properties

        /// <summary>
        /// IsReusable
        /// </summary>
        public bool IsReusable => false;

        #endregion Properties

        #region Methods

        /// <summary>
        /// 处理下载结果抽象方法
        /// </summary>
        /// <param name="fileName">本地文件名称</param>
        /// <param name="filePath">下载文件路径</param>
        /// <param name="err">错误信息</param>
        /// <returns>响应字符串</returns>
        public abstract string GetResult(string fileName, string filePath, string err);

        /// <summary>
        ///下载文件完成抽象方法
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="fileName">本地文件名称</param>
        /// <param name="filePath">下载文件路径</param>
        public abstract void OnDownloaded(HttpContext context, string fileName, string filePath);

        /// <summary>
        /// 处理文件下载入口
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            string downloadEncryptFileName = context.Request["fileName"];
            if (!string.IsNullOrEmpty(downloadEncryptFileName))
            {
                string downloadFileName = DownloadFileContext.Instance.DecryptFileName(downloadEncryptFileName);
                string filePath = DownloadConfigContext.DownLoadMainDirectory + downloadFileName;//HttpContext.Current.Server.MapPath("~/") + "files/" + _downloadFileName;
                FileDownloadResult result = WebDownloadFile.FileDownload(downloadFileName, filePath, DownloadConfigContext.LimitDownloadSpeedKb * 1024);

                if (result.State)
                {
                    OnDownloaded(context, downloadFileName, filePath);
                }

                context.Response.Write(GetResult(downloadFileName, filePath, result.Message));
                context.Response.End();
            }
        }

        #endregion Methods
    }
}