namespace MasterChief.DotNet.Framework.Download
{
    using MasterChief.DotNet.Core.Config;
    using MasterChief.DotNet4.Utilities.WebForm.Core;
    using System;
    using System.Web.Caching;

    /// <summary>
    /// CachedConfigContext
    /// </summary>
    public sealed class CachedConfigContext : ConfigContext
    {
        #region Fields

        private static readonly Lazy<CachedConfigContext> _instance
          = new Lazy<CachedConfigContext>(() => new CachedConfigContext());

        public static CachedConfigContext Instance => _instance.Value;

        /// <summary>
        /// 文件下载配置项
        /// </summary>
        public DownloadConfig DownloadConfig => Get<DownloadConfig>();

        #endregion Fields

        #region Methods

        /// <summary>
        /// 重写基类的取配置，加入缓存机制
        /// </summary>
        public override T Get<T>(string index = null)
        {
            if (!(base.ConfigService is FileConfigService))
            {
                throw new NotSupportedException("CacheConfigContext");
            }
            string fileName = GetClusteredIndex<T>(index),
                   key = fileName;
            object content = CacheManger.Get(key);

            if (content != null)
            {
                return (T)content;
            }
            else
            {
                T _value = base.Get<T>(index);
                CacheManger.Set(key, _value, new CacheDependency(fileName));
                return _value;
            }
        }

        #endregion Methods
    }
}