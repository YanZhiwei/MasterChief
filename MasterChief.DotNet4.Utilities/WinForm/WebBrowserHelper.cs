using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     WebBrowser 帮助类
    /// </summary>
    public static class WebBrowserHelper
    {
        #region Methods

        /// <summary>
        ///     从WebBrowser中获取CookieContainer
        /// </summary>
        /// <param name="webBrowser">WebBrowser对象</param>
        /// <returns>CookieContainer</returns>
        public static CookieContainer GetCookieContainer(this WebBrowser webBrowser)
        {
            var cookieContainer = new CookieContainer();

            var cookieString = webBrowser.Document?.Cookie;
            if (string.IsNullOrEmpty(cookieString)) return cookieContainer;

            var cookies = cookieString.Split(';');
            foreach (var itemCookie in cookies)
            {
                var cookieNameValue = itemCookie.Split('=');
                if (cookieNameValue.Length != 2) continue;

                var cookie = new Cookie(cookieNameValue[0].Trim(), cookieNameValue[1].Trim());
                cookieContainer.Add(cookie);
            }

            return cookieContainer;
        }

        /// <summary>
        ///     WebBrowser添加cookie
        /// </summary>
        /// <param name="webBrowser">WebBrowser</param>
        /// <param name="url">url</param>
        /// <param name="cookie">cookie</param>
        public static void InsertCookie(this WebBrowser webBrowser, string url, Cookie cookie)
        {
            InternetSetCookie(url, cookie.Name, cookie.Value);
        }

        /// <summary>
        ///     Internets the set cookie.
        /// </summary>
        /// <param name="urlName">Name of the URL.</param>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <param name="cookieData">The cookie data.</param>
        /// <returns>设置是否成功</returns>
        /// 日期：2015-10-13 13:44
        /// 备注：
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string urlName, string cookieName, string cookieData);

        #endregion Methods
    }
}