using System;
using System.Collections.Specialized;
using System.Web;
using MasterChief.DotNet4.Utilities.Common;

namespace MasterChief.DotNet4.Utilities.WebForm.Core
{
    /// <summary>
    ///     cookie 帮助类
    /// </summary>
    public class CookieManger
    {
        #region Methods

        /// <summary>
        ///     设置cookie （24小时过期）
        /// </summary>
        /// <param name="cookiename">键</param>
        /// <param name="cookievalue">值</param>
        public static void AddValue(string cookiename, NameValueCollection cookievalue)
        {
            AddValue(cookiename, cookievalue, DateTime.Now.AddDays(1));
        }

        /// <summary>
        ///     设置cookie
        /// </summary>
        /// <param name="cookiename">键.</param>
        /// <param name="cookievalue">值</param>
        /// <param name="expires">过期时间</param>
        public static void AddValue(string cookiename, NameValueCollection cookievalue, DateTime expires)
        {
            var cookie = new HttpCookie(cookiename);

            foreach (string key in cookievalue) cookie.Values[key] = cookievalue[key];

            cookie.Expires = expires;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        ///     取Cookie
        /// </summary>
        /// <param name="name">键</param>
        /// <returns>值</returns>
        public static HttpCookie Get(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        /// <summary>
        ///     取Cookie值
        /// </summary>
        /// <param name="name">键</param>
        /// <returns>值</returns>
        public static string GetValue(string name)
        {
            var httpCookie = Get(name);

            if (httpCookie != null)
                return httpCookie.Value;
            return string.Empty;
        }

        /// <summary>
        ///     移除Cookie
        /// </summary>
        /// <param name="name">cookie键</param>
        public static void Remove(string name)
        {
            Remove(Get(name));
        }

        /// <summary>
        ///     Removes the specified cookie.
        /// </summary>
        /// <param name="cookie">HttpCookie</param>
        public static void Remove(HttpCookie cookie)
        {
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now;
                Save(cookie, 0);
            }
        }

        /// <summary>
        ///     保存Cookie
        /// </summary>
        /// <param name="name">键</param>
        /// <param name="value">值</param>
        /// <param name="expiresHours">小时</param>
        public static void Save(string name, string value, int expiresHours = 0)
        {
            var httpCookie = Get(name);

            if (httpCookie == null) httpCookie = Set(name);

            httpCookie.Value = value;
            Save(httpCookie, expiresHours);
        }

        /// <summary>
        ///     保存Cookie
        /// </summary>
        /// <param name="cookie">HttpCookie</param>
        /// <param name="expiresHours">小时</param>
        public static void Save(HttpCookie cookie, int expiresHours)
        {
            var domain = FetchHelper.ServerDomain;
            var urlHost = HttpContext.Current.Request.Url.Host.ToLower();

            if (domain != urlHost) cookie.Domain = domain;

            if (expiresHours > 0) cookie.Expires = DateTime.Now.AddHours(expiresHours);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        private static HttpCookie Set(string name)
        {
            return new HttpCookie(name);
        }

        #endregion Methods
    }
}