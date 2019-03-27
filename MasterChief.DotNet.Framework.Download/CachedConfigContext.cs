using System;
using System.Web.Caching;
using MasterChief.DotNet.Core.Config;
using MasterChief.DotNet4.Utilities.WebForm.Core;

namespace MasterChief.DotNet.Framework.Download
{
    /// <summary>
    ///     CachedConfigContext
    /// </summary>
    public sealed class CachedConfigContext : ConfigContext
    {
        #region Methods

        /// <summary>
        ///     重写基类的取配置，加入缓存机制
        /// </summary>
        public override T Get<T>(string index = null)
        {
            if (!(ConfigService is FileConfigService)) throw new NotSupportedException("CacheConfigContext");
            string fileName = GetClusteredIndex<T>(index),
                key = fileName;
            var content = CacheManger.Get(key);

            if (content != null)
            {
                return (T) content;
            }

            var value = base.Get<T>(index);
            CacheManger.Set(key, value, new CacheDependency(fileName));
            return value;
        }

        #endregion Methods

        #region Fields

        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<CachedConfigContext> _instance
            = new Lazy<CachedConfigContext>(() => new CachedConfigContext());

        /// <summary>
        /// CachedConfigContext
        /// </summary>
        public static CachedConfigContext Instance => _instance.Value;

        /// <summary>
        ///     文件下载配置项
        /// </summary>
        public DownloadConfig DownloadConfig => Get<DownloadConfig>();

        #endregion Fields
    }
}