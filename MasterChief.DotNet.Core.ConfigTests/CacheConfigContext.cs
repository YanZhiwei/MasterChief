using MasterChief.DotNet.Core.Config;
using MasterChief.DotNet4.Utilities.WebForm.Core;
using System;
using System.Web.Caching;

namespace MasterChief.DotNet.Core.ConfigTests
{
    public sealed class CacheConfigContext : ConfigContext
    {
        public override T Get<T>(string index = null)
        {
            if (!(base.ConfigService is FileConfigService))
            {
                throw new NotSupportedException("CacheConfigContext");
            }
            string filePath = GetClusteredIndex<T>(index);
            string key = filePath;
            object cacheContent = CacheManger.Get(key);
            if (cacheContent != null)
            {
                return (T)cacheContent;
            }
            T value = base.Get<T>(index);
            CacheManger.Set(key, value, new CacheDependency(filePath));
            return value;
        }
    }
}