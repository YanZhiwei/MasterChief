using MasterChief.DotNet4.Utilities.Common;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace MasterChief.DotNet4.Utilities.Web
{
    /// <summary>
    /// 发起模拟Web请求
    /// </summary>
    public sealed class SimulateWebRequest
    {
        #region Fields

        /// <summary>
        /// accept
        /// </summary>
        private const string accept = "*/*";

        /// <summary>
        /// 是否允许重定向
        /// </summary>
        private const bool allowAutoRedirect = true;

        /// <summary>
        /// contentType
        /// </summary>
        private const string contentType = "application/x-www-form-urlencoded";

        /// <summary>
        /// 过期时间
        /// </summary>
        private const int timeOut = 50000;

        #endregion Fields

        #region Methods

        /// <summary>
        /// 发起Get请求
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>响应内容</returns>
        public static string Get(string url)
        {
            StringBuilder _responeBuilder = new StringBuilder();
            HttpWebRequest _webRequest = WebRequest.Create(url) as HttpWebRequest;
            _webRequest.Method = "GET";
            _webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            try
            {
                using (HttpWebResponse webResponse = _webRequest.GetResponse() as HttpWebResponse)
                {
                    byte[] _responeBuffer = new byte[8192];

                    using (Stream responeStream = webResponse.GetResponseStream())
                    {
                        int _count = 0;

                        do
                        {
                            _count = responeStream.Read(_responeBuffer, 0, _responeBuffer.Length);

                            if (_count != 0)
                            {
                                _responeBuilder.Append(Encoding.UTF8.GetString(_responeBuffer, 0, _count));
                            }
                        }
                        while (_count > 0);

                        return _responeBuilder.ToString();
                    }
                }
            }

            finally
            {
                if (_webRequest != null)
                {
                    _webRequest.Abort();
                }
            }
        }



        /// <summary>
        /// 发起Post请求
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="header">Headers</param>
        /// <returns>结果</returns>
        public static string Post(string url, NameValueCollection header)
        {

            HttpWebRequest _webRequest = WebRequest.Create(url) as HttpWebRequest;
            _webRequest.Method = "POST";
            _webRequest.Timeout = timeOut;
            _webRequest.AllowAutoRedirect = allowAutoRedirect;
            _webRequest.ServicePoint.ConnectionLimit = int.MaxValue;
            _webRequest.ContentLength = 0;

            if (header != null)
            {
                _webRequest.Headers.Add(header);
            }

            try
            {
                using (HttpWebResponse webResponse = (HttpWebResponse)_webRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        string _responeString = reader.ReadToEnd();
                        return _responeString;
                    }
                }
            }
            finally
            {
                if (_webRequest != null)
                {
                    _webRequest.Abort();
                }
            }
        }

        /// <summary>
        /// 适用于大文件的上传
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="file">文件路径</param>
        /// <param name="postData">参数</param>
        /// <param name="completePercentFacotry">上传完成百分比 委托</param>
        /// <returns>响应内容</returns>
        public static string UploadFile(string url, string file, NameValueCollection postData, Action<decimal> completePercentFacotry)
        {
            return UploadFile(url, file, postData, Encoding.UTF8, completePercentFacotry);
        }

        /// <summary>
        /// 适用于大文件的上传
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="file">文件路径</param>
        /// <param name="postData">参数</param>
        /// <param name="encoding">编码</param>
        /// <param name="completePercentFacotry">上传完成百分比 委托</param>
        /// <returns>响应内容</returns>
        public static string UploadFile(string url, string file, NameValueCollection postData, Encoding encoding, Action<decimal> completePercentFacotry)
        {
            return UploadFile(url, new string[] { file }, postData, encoding, completePercentFacotry);
        }

        /// <summary>
        /// 适用于大文件的上传
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="files">文件路径</param>
        /// <param name="postData">参数</param>
        /// <param name="completePercentFacotry">上传完成百分比 委托</param>
        /// <returns>响应内容</returns>
        public static string UploadFile(string url, string[] files, NameValueCollection postData, Action<decimal> completePercentFacotry)
        {
            return UploadFile(url, files, postData, Encoding.UTF8, completePercentFacotry);
        }


        /// <summary>
        /// 适用于大文件的上传
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="files">文件路径</param>
        /// <param name="postData">参数</param>
        /// <param name="encoding">编码</param>
        /// <param name="completePercentFacotry">上传完成百分比 委托</param>
        /// <returns>响应内容</returns>
        public static string UploadFile(string url, string[] files, NameValueCollection postData, Encoding encoding, Action<decimal> completePercentFacotry)
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
            string _boundarynumber = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] _boundarybuffer = Encoding.ASCII.GetBytes("\r\n--" + _boundarynumber + "\r\n");
            byte[] _allRequestbuffer = Encoding.ASCII.GetBytes("\r\n--" + _boundarynumber + "--\r\n");
            HttpWebRequest _webRequest = CreateUploadFileWebRequest(url, _boundarynumber);

            try
            {
                using (Stream requestStream = _webRequest.GetRequestStream())
                {
                    BuilderUploadFilePostParamter(requestStream, _boundarybuffer, postData, encoding);
                    FetchUploadFiles(requestStream, _boundarybuffer, files, encoding, _allRequestbuffer, completePercentFacotry);
                }

                using (HttpWebResponse response = (HttpWebResponse)_webRequest.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                    {
                        return stream.ReadToEnd();
                    }
                }
            }

            finally
            {
                if (_webRequest != null)
                {
                    _webRequest.Abort();
                }

            }
        }

        private static void BuilderUploadFilePostParamter(Stream requestStream, byte[] boundarybuffer, NameValueCollection postData, Encoding encoding)
        {
            string _formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

            if (postData != null)
            {
                foreach (string key in postData.Keys)
                {
                    requestStream.Write(boundarybuffer, 0, boundarybuffer.Length);
                    string _formitem = string.Format(_formdataTemplate, key, postData[key]);
                    byte[] _formitembuffer = encoding.GetBytes(_formitem);
                    requestStream.Write(_formitembuffer, 0, _formitembuffer.Length);
                }
            }
        }

        private static HttpWebRequest CreateUploadFileWebRequest(string url, string boundarynumber)
        {
            HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(url);
            _request.ContentType = "multipart/form-data; boundary=" + boundarynumber;
            _request.Method = "POST";
            _request.KeepAlive = true;
            _request.Accept = accept;
            _request.Timeout = timeOut;
            _request.AllowAutoRedirect = allowAutoRedirect;
            _request.Credentials = CredentialCache.DefaultCredentials;
            return _request;
        }

        private static void FetchUploadFiles(Stream requestStream, byte[] boundarybuffer, string[] files, Encoding encoding, byte[] allRequestBuffer, Action<decimal> completePercentFacotry)
        {
            string _headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
            byte[] _buffer = new byte[4096];
            int _offset = 0;
            int _completeOffset = 0;
            DateTime _uploadFilesStartTime = DateTime.Now;

            for (int i = 0; i < files.Length; i++)
            {
                requestStream.Write(boundarybuffer, 0, boundarybuffer.Length);
                string _header = string.Format(_headerTemplate, "file" + i, Path.GetFileName(files[i]));
                byte[] _headerbytes = encoding.GetBytes(_header);
                requestStream.Write(_headerbytes, 0, _headerbytes.Length);

                using (FileStream fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                {
                    while ((_offset = fileStream.Read(_buffer, 0, _buffer.Length)) != 0)
                    {
                        _completeOffset = _completeOffset + _offset;
                        requestStream.Write(_buffer, 0, _offset);

                        if ((DateTime.Now - _uploadFilesStartTime).TotalMilliseconds >= 10 || _completeOffset == fileStream.Length)
                        {
                            decimal _percent = DecimalHelper.CalcPercentage(_completeOffset, fileStream.Length);
                            completePercentFacotry(_percent);
                            _uploadFilesStartTime = DateTime.Now;
                        }
                    }
                }
            }

            requestStream.Write(allRequestBuffer, 0, allRequestBuffer.Length);
        }

        #endregion Methods
    }
}

