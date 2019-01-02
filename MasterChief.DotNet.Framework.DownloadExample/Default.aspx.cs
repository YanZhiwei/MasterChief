using MasterChief.DotNet.Framework.Download;
using System;

namespace MasterChief.DotNet.Framework.DownloadExample
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = DownloadFileContext.Instance.EncryptFileName("VIDEO0015.mp4");
            link.NavigateUrl = "~/download.aspx?fileName=" + url;
        }
    }
}