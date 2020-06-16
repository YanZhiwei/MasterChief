using System.Globalization;
using System.Reflection;
using System.Resources;

namespace MasterChief.DotNet4._5.Utilities.Core
{
    /// <summary>
    ///     本地化支持
    /// </summary>
    public class LocalizedResource
    {
        /// <summary>
        ///     The resource
        /// </summary>
        public readonly ResourceManager Resource;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LocalizedResource" /> class.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        public LocalizedResource(string resourceName) : this(resourceName, Assembly.GetExecutingAssembly())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LocalizedResource" /> class.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAssembly">The resource assembly.</param>
        public LocalizedResource(string resourceName, Assembly resourceAssembly)
        {
            Resource = new ResourceManager(resourceName, resourceAssembly);
        }

        /// <summary>
        ///     获取本地化文本
        /// </summary>
        /// <param name="key">资源Key</param>
        /// <returns>本地化文本</returns>
        public virtual string GetString(string key)
        {
            if (string.IsNullOrEmpty(key)) return key;
            var result = Resource.GetString(key, CultureInfo.DefaultThreadCurrentCulture);
            return string.IsNullOrEmpty(result) ? key : result;
            //2
        }
    }
}