using System.Diagnostics;
using System.Web;
using MasterChief.DotNet.Framework.Download;

namespace MasterChief.DotNet.Framework.DownloadExample.BackHandler
{
    /// <summary>
    ///     FileDownloadHandler 的摘要说明
    /// </summary>
    public class FileDownloadHandler : DownloadHandler, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var fileName = context.Request["fileName"];
            StartDownloading(context, fileName);

        }

        public bool IsReusable { get; }

        public override void OnDownloadFailed(HttpContext context, string fileName, string filePath, string ex)
        {
            context.Response.Write(ex);
        }

        public override void OnDownloadSucceed(HttpContext context, string fileName, string filePath)
        {
            string result = $"文件[{fileName}]下载成功，映射路径：{filePath}";
            context.Response.Write(result);
        }
    }
}