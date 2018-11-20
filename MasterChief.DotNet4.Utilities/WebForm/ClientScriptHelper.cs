namespace MasterChief.DotNet4.Utilities.WebForm
{
    using System;
    using System.Web;
    using System.Web.UI;

    /// <summary>
    /// JavaScript 操作类
    /// </summary>
    public static class ClientScriptHelper
    {
        #region Methods

        /// <summary>
        /// JS弹出对消息话框
        /// </summary>
        /// <param name="message">要显示的消息</param>
        public static void Alert(string message)
        {
            Page page = HttpContext.Current.Handler as Page;
            page.ClientScript.RegisterStartupScript(page.GetType(), string.Format("Js_{0}", Guid.NewGuid().ToString()), string.Format("<script>alert(\"{0}\")</script>", message));
        }

        /// <summary>
        /// JS弹出消息并跳转
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="url">跳转的连接</param>
        public static void AlertAndRedirect(string message, string url)
        {
            Alert(message);
            JumpToSpecifiedUrl(url);
        }

        /// <summary>
        /// JS跳转指定URL
        /// </summary>
        /// <param name="url">要转到的URL</param>
        public static void JumpToSpecifiedUrl(string url)
        {
            RegisterScript(string.Format("location.href='{0}'", url));
        }

        /// <summary>
        /// JS刷新页面
        /// </summary>
        public static void Refresh()
        {
            RegisterScript(string.Format("location.reload();"));
        }

        /// <summary>
        /// 向客户端写入js脚本
        /// </summary>
        /// <param name="script">js内容</param>
        public static void RegisterScript(string script)
        {
            Page page = HttpContext.Current.Handler as Page;
            page.ClientScript.RegisterStartupScript(page.GetType(), string.Format("Js_{0}", Guid.NewGuid().ToString()), string.Format("<script>{0}</script>", script));
        }

        #endregion Methods
    }
}