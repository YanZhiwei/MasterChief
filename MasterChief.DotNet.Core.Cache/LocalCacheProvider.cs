namespace MasterChief.DotNet.Core.Cache
{
    using MasterChief.DotNet4.Utilities.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Caching;

    public class LocalCacheProvider : ICacheProvider
    {
        #region Fields

        protected ObjectCache Cache => MemoryCache.Default;

        #endregion Fields

        #region Methods

        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        public bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            this.RemoveByPattern(pattern, Cache.Select(p => p.Key));
        }

        public void Set(string key, object data, int cacheTime)
        {
            if (!CheckCacheData(data))
            {
                return;
            }

            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime)
            };
            Cache.Add(new CacheItem(key, data), policy);
        }

        public void Set(string key, object data, string dependFile)
        {
            if (!CheckCacheData(data))
            {
                return;
            }
            if (!File.Exists(dependFile))
            {
                throw new FileNotFoundException(dependFile);
            }
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string>() { dependFile }));
            Cache.Add(new CacheItem(key, data), policy);
        }

        private bool CheckCacheData(object data)
        {
            if (data == null)
            {
                return false;
            }
            if (data.IsCollection() && IEnumerableHelper.IsNullOrEmpty((IEnumerable)data))
            {
                return false;
            }
            return true;
        }

        #endregion Methods
    }
}