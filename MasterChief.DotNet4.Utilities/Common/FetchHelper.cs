using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    /// URL 帮助类
    /// </summary>
    public static class FetchHelper
    {
        #region Methods

        /// <summary>
        /// 获取当前页面的Url
        /// </summary>
        public static string CurrentUrl => HttpContext.Current.Request.Url.ToString();

        /// <summary>
        /// 获取当前页面的主域
        /// </summary>
        public static string ServerDomain
        {
            get
            {
                string host = HttpContext.Current.Request.Url.Host.ToLower();
                string[] hostArray = host.Split(new char[] { '.' });

                if ((hostArray.Length < 3) || CheckHelper.IsIp4Address(host))
                {
                    return host;
                }

                string actualHost = host.Remove(0, host.IndexOf(".", StringComparison.OrdinalIgnoreCase) + 1);

                if ((actualHost.StartsWith("com.", StringComparison.OrdinalIgnoreCase) || actualHost.StartsWith("net.", StringComparison.OrdinalIgnoreCase)) || (actualHost.StartsWith("org.", StringComparison.OrdinalIgnoreCase) || actualHost.StartsWith("gov.", StringComparison.OrdinalIgnoreCase)))
                {
                    return host;
                }

                return actualHost;
            }
        }

        /// <summary>
        /// 获取访问用户的IP
        /// </summary>
        public static string UserIp
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    switch (result)
                    {
                        case null:
                        case "":
                            result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                            break;
                    }

                    if (result == "::1")
                    {
                        result = "127.0.0.1";
                    }
                    else if (!CheckHelper.IsIp4Address(result))
                    {
                        return "Unknown";
                    }

                    return result;
                }
                return "Unknown";
            }
        }

        /// <summary>
        /// 取得网站根目录的物理路径
        /// </summary>
        /// <returns>网站根目录的物理路径</returns>
        public static string GetRootPath()
        {
            string website = string.Empty;
            HttpContext context = HttpContext.Current;

            if (context != null)
            {
                website = context.Server.MapPath("~");
            }
            else
            {
                website = AppDomain.CurrentDomain.BaseDirectory;

                if (Regex.Match(website, @"\\$", RegexOptions.Compiled).Success)
                {
                    website = website.Substring(0, website.Length - 1);
                }
            }

            return website;
        }

        /// <summary>
        /// 取得网站的根目录的URL
        /// </summary>
        /// <returns>网站的根目录的URL</returns>
        public static string GetRootURI()
        {
            string website = string.Empty;
            HttpContext context = HttpContext.Current;
            HttpRequest request;

            if (context != null)
            {
                request = context.Request;
                string _urlAuthority = request.Url.GetLeftPart(UriPartial.Authority);

                if (request.ApplicationPath == null || request.ApplicationPath == "/")
                {
                    website = _urlAuthority;
                }
                else
                {
                    website = _urlAuthority + request.ApplicationPath;
                }
            }

            return website;
        }

        /// <summary>
        /// 取得网站的根目录的URL
        /// </summary>
        /// <param name="reguest">HttpRequest</param>
        /// <returns>网站的根目录的URL</returns>
        public static string GetRootURI(HttpRequest reguest)
        {
            string website = string.Empty;

            if (reguest != null)
            {
                string authority = reguest.Url.GetLeftPart(UriPartial.Authority);

                if (reguest.ApplicationPath == null || reguest.ApplicationPath == "/")
                {
                    website = authority;
                }
                else
                {
                    website = authority + reguest.ApplicationPath;
                }
            }

            return website;
        }

        /// <summary>
        /// 将虚拟路径映射到物理磁盘路径。
        /// <code>
        ///  ~/bin==>c:\inetpub\wwwroot\bin
        /// </code>
        /// </summary>
        /// <param name="path">虚拟路径</param>
        /// <returns>物理磁盘路径</returns>
        public static string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                return HostingEnvironment.MapPath(path);
            }

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }

        /// <summary>
        /// 获取表单Post过来的值
        /// </summary>
        /// <param name="name">参数</param>
        /// <returns>参数数值</returns>
        public static string Post(string name)
        {
            string formValue = HttpContext.Current.Request.Form[name];
            return ((formValue == null)
                ? string.Empty
                : formValue.Trim());
        }

        #endregion Methods
    }
}
