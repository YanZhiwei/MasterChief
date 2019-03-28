using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using MasterChief.DotNet4._5.Utilities.Model;

namespace MasterChief.DotNet4._5.Utilities.Web
{
    /// <summary>
    ///     HttpRequest辅助类
    /// </summary>
    public static class HttpRequestHelper
    {
        #region Methods

        /// <summary>
        ///     获取HttpRequest原始信息
        /// </summary>
        /// <param name="request">HttpRequestMessage</param>
        /// <returns>HttpRequest原始信息</returns>
        public static HttpRequestRaw ToRaw(this HttpRequestMessage request)
        {
            var requestRaw = new HttpRequestRaw();
            HttpRequestBasic(request, requestRaw);
            HttpRequestHeaders(request, requestRaw);
            HttpRequestBody(request, requestRaw);
            return requestRaw;
        }

        private static void HttpRequestBasic(HttpRequestMessage request, HttpRequestRaw requestRaw)
        {
            requestRaw.RequestMethod = request.Method.ToString();
            requestRaw.RequestUri = request.RequestUri.ToString();
            requestRaw.RequestVersion = $"HTTP/{request.Version}";
        }

        private static void HttpRequestBody(HttpRequestMessage request, HttpRequestRaw requestRaw)
        {
            try
            {
                string bodyString;

                using (var stream = request.Content.ReadAsStreamAsync().Result)
                {
                    if (stream.CanSeek) stream.Position = 0;

                    bodyString = request.Content.ReadAsStringAsync().Result;
                }

                requestRaw.Body = bodyString;
            }
            catch (HttpRequestException)
            {
                // ignored
            }
        }

        private static void HttpRequestHeaders(HttpRequestMessage request, HttpRequestRaw requestRaw)
        {
            requestRaw.Headers = new List<string>();
            BuilderRequestHeader(request.Headers, requestRaw);
            BuilderRequestHeader(request.Content.Headers, requestRaw);
        }

        private static void BuilderRequestHeader(HttpHeaders headers, HttpRequestRaw requestRaw)
        {
            if (headers != null)
                foreach (var item in headers)
                    requestRaw.Headers.Add(string.Format("{0}:{1}", item.Key,
                        headers.GetValues(item.Key).FirstOrDefault()));
        }

        #endregion Methods
    }
}