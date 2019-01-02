using MasterChief.DotNet.Framework.Download;
using System.Diagnostics;
using System.Web;

namespace MasterChief.DotNet.Framework.DownloadExample.BackHandler
{
    /// <summary>
    /// FileDownloadHandler 的摘要说明
    /// </summary>
    public class FileDownloadHandler : DownloadHandler
    {
        public override string GetResult(string fileName, string filePath, string err)
        {
            return err;
        }

        public override void OnDownloaded(HttpContext context, string fileName, string filePath)
        {
            Debug.WriteLine(string.Format("文件[{0}]下载成功，映射路径：{1}", fileName, filePath));
        }
    }
}