using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace MasterChief.DotNet.ProjectTemplate.WebApi.Component
{
    /// <summary>
    ///     Json序列化处理
    /// </summary>
    /// <seealso cref="System.Net.Http.Formatting.IContentNegotiator" />
    public class JsonContentNegotiator : IContentNegotiator
    {
        private readonly JsonMediaTypeFormatter _jsonFormatter;

        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonContentNegotiator" /> class.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        public JsonContentNegotiator(JsonMediaTypeFormatter formatter)
        {
            _jsonFormatter = formatter;
        }

        /// <summary>
        ///     通过在已为给定 request 传入的 formatters 中选择可以序列化给定 type 的对象的最适当
        ///     <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" />，来执行内容协商。
        /// </summary>
        /// <param name="type">要序列化的类型。</param>
        /// <param name="request">请求消息，其中包含用于执行协商的标头值。</param>
        /// <param name="formatters">可供选择的 <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> 对象集。</param>
        /// <returns>
        ///     包含最适当的 <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> 实例的协商结果或 null（如果没有适当的格式化程序）。
        /// </returns>
        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request,
            IEnumerable<MediaTypeFormatter> formatters)
        {
            return new ContentNegotiationResult(_jsonFormatter, new MediaTypeHeaderValue("application/json"));
        }
    }
}