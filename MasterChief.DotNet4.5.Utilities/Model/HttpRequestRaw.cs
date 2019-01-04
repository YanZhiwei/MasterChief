using System.Collections.Generic;

namespace MasterChief.DotNet4._5.Utilities.Model
{
    /// <summary>
    /// HttpRequest原始信息
    /// </summary>
    public class HttpRequestRaw
    {
        /// <summary>
        /// 请求URL
        /// </summary>
        public string RequestUri
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP 请求方法
        /// </summary>
        public string RequestMethod
        {
            get;
            set;
        }

        /// <summary>
        /// 请求版本
        /// </summary>
        public string RequestVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 请求头
        /// </summary>
        public List<string> Headers
        {
            get;
            set;
        }

        /// <summary>
        /// Body
        /// </summary>
        public string Body
        {
            get; set;
        }
    }
}