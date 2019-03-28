using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using MasterChief.DotNet4.Utilities.Common;

namespace MasterChief.DotNet4.Utilities.Web
{
    /// <summary>
    ///     发起模拟Web请求
    /// </summary>
    public sealed class SimulateWebRequest
    {
        #region Fields

        /// <summary>
        ///     accept
        /// </summary>
        private const string Accept = "*/*";

        /// <summary>
        ///     是否允许重定向
        /// </summary>
        private const bool AllowAutoRedirect = true;

        /// <summary>
        ///     过期时间
        /// </summary>
        private const int TimeOut = 50000;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     发起Get请求
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>响应内容</returns>
        public static string Get(string url)
        {
            var builder = new StringBuilder();
            if (WebRequest.Create(url) is HttpWebRequest webRequest)
            {
                webRequest.Method = "GET";
                webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

                try
                {
                    using (var webResponse = webRequest.GetResponse() as HttpWebResponse)
                    {
                        var buffer = new byte[8192];

                        using (var stream = webResponse?.GetResponseStream())
                        {
                            var count = 0;

                            do
                            {
                                if (stream != null) count = stream.Read(buffer, 0, buffer.Length);

                                if (count != 0) builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                            } while (count > 0);

                            return builder.ToString();
                        }
                    }
                }

                finally
                {
                    webRequest.Abort();
                }
            }

            return null;
        }


        /// <summary>
        ///     发起Post请求
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="header">Headers</param>
        /// <returns>结果</returns>
        public static string Post(string url, NameValueCollection header)
        {
            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            // ReSharper disable once PossibleNullReferenceException
            webRequest.Method = "POST";
            webRequest.Timeout = TimeOut;
            webRequest.AllowAutoRedirect = AllowAutoRedirect;
            webRequest.ServicePoint.ConnectionLimit = int.MaxValue;
            webRequest.ContentLength = 0;

            if (header != null) webRequest.Headers.Add(header);

            try
            {
                using (var webResponse = (HttpWebResponse) webRequest.GetResponse())
                {
                    using (var reader =
                        new StreamReader(webResponse.GetResponseStream() ?? throw new InvalidOperationException(),
                            Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            finally
            {
                webRequest.Abort();
            }
        }

        /// <summary>
        ///     适用于大文件的上传
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="file">文件路径</param>
        /// <param name="postData">参数</param>
        /// <param name="completePercentFacotry">上传完成百分比 委托</param>
        /// <returns>响应内容</returns>
        public static string UploadFile(string url, string file, NameValueCollection postData,
            Action<decimal> completePercentFacotry)
        {
            return UploadFile(url, file, postData, Encoding.UTF8, completePercentFacotry);
        }

        /// <summary>
        ///     适用于大文件的上传
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="file">文件路径</param>
        /// <param name="postData">参数</param>
        /// <param name="encoding">编码</param>
        /// <param name="completePercentFacotry">上传完成百分比 委托</param>
        /// <returns>响应内容</returns>
        public static string UploadFile(string url, string file, NameValueCollection postData, Encoding encoding,
            Action<decimal> completePercentFacotry)
        {
            return UploadFile(url, new[] {file}, postData, encoding, completePercentFacotry);
        }

        /// <summary>
        ///     适用于大文件的上传
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="files">文件路径</param>
        /// <param name="postData">参数</param>
        /// <param name="completePercentFacotry">上传完成百分比 委托</param>
        /// <returns>响应内容</returns>
        public static string UploadFile(string url, string[] files, NameValueCollection postData,
            Action<decimal> completePercentFacotry)
        {
            return UploadFile(url, files, postData, Encoding.UTF8, completePercentFacotry);
        }


        /// <summary>
        ///     适用于大文件的上传
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="files">文件路径</param>
        /// <param name="postData">参数</param>
        /// <param name="encoding">编码</param>
        /// <param name="completePercentFacotry">上传完成百分比 委托</param>
        /// <returns>响应内容</returns>
        public static string UploadFile(string url, string[] files, NameValueCollection postData, Encoding encoding,
            Action<decimal> completePercentFacotry)
        {
            // 使用HttpWebRequest上传大文件时，服务端配置中需要进行以下节点配置：
            // < system.web >
            //< compilation debug = "true" targetFramework = "4.0" />
            //   < httpRuntime maxRequestLength = "100000000" executionTimeout = "600" ></ httpRuntime >
            //  </ system.web >
            //    < system.webServer >
            //      < security >
            //        < requestFiltering >
            //          < !--这个节点直接决定了客户端文件上传最大值-- >
            //          < requestLimits maxAllowedContentLength = "2147483647" />
            //         </ requestFiltering >
            //       </ security >
            //     </ system.webServer >
            //   否则会出现服务端返回404错误。
            var boundarynumber = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            var boundarybuffer = Encoding.ASCII.GetBytes("\r\n--" + boundarynumber + "\r\n");
            var allRequestbuffer = Encoding.ASCII.GetBytes("\r\n--" + boundarynumber + "--\r\n");
            var webRequest = CreateUploadFileWebRequest(url, boundarynumber);

            try
            {
                using (var requestStream = webRequest.GetRequestStream())
                {
                    BuilderUploadFilePostParamter(requestStream, boundarybuffer, postData, encoding);
                    FetchUploadFiles(requestStream, boundarybuffer, files, encoding, allRequestbuffer,
                        completePercentFacotry);
                }

                using (var response = (HttpWebResponse) webRequest.GetResponse())
                {
                    using (var stream = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
                    {
                        return stream.ReadToEnd();
                    }
                }
            }

            finally
            {
                webRequest?.Abort();
            }
        }

        private static void BuilderUploadFilePostParamter(Stream requestStream, byte[] boundarybuffer,
            NameValueCollection postData, Encoding encoding)
        {
            var _formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

            if (postData != null)
                foreach (string key in postData.Keys)
                {
                    requestStream.Write(boundarybuffer, 0, boundarybuffer.Length);
                    var formitem = string.Format(_formdataTemplate, key, postData[key]);
                    var formitembuffer = encoding.GetBytes(formitem);
                    requestStream.Write(formitembuffer, 0, formitembuffer.Length);
                }
        }

        private static HttpWebRequest CreateUploadFileWebRequest(string url, string boundarynumber)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundarynumber;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Accept = Accept;
            request.Timeout = TimeOut;
            request.AllowAutoRedirect = AllowAutoRedirect;
            request.Credentials = CredentialCache.DefaultCredentials;
            return request;
        }

        private static void FetchUploadFiles(Stream requestStream, byte[] boundarybuffer, string[] files,
            Encoding encoding, byte[] allRequestBuffer, Action<decimal> completePercentFacotry)
        {
            var _headerTemplate =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
            var buffer = new byte[4096];
            int _offset;
            var completeOffset = 0;
            var uploadFilesStartTime = DateTime.Now;

            for (var i = 0; i < files.Length; i++)
            {
                requestStream.Write(boundarybuffer, 0, boundarybuffer.Length);
                var header = string.Format(_headerTemplate, "file" + i, Path.GetFileName(files[i]));
                var headerbytes = encoding.GetBytes(header);
                requestStream.Write(headerbytes, 0, headerbytes.Length);

                using (var fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                {
                    while ((_offset = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        completeOffset = completeOffset + _offset;
                        requestStream.Write(buffer, 0, _offset);

                        if ((DateTime.Now - uploadFilesStartTime).TotalMilliseconds >= 10 ||
                            completeOffset == fileStream.Length)
                        {
                            var percent = DecimalHelper.CalcPercentage(completeOffset, fileStream.Length);
                            completePercentFacotry(percent);
                            uploadFilesStartTime = DateTime.Now;
                        }
                    }
                }
            }

            requestStream.Write(allRequestBuffer, 0, allRequestBuffer.Length);
        }

        #endregion Methods
    }
}