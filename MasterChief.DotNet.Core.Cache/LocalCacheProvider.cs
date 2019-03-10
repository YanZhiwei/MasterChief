namespace MasterChief.DotNet.Core.Cache
{
    using MasterChief.DotNet4.Utilities.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Caching;

    /// <summary>
    /// 本地内存缓存
    /// </summary>
    /// <seealso cref="MasterChief.DotNet.Core.Cache.ICacheProvider" />
    public class LocalCacheProvider : ICacheProvider
    {
        #region Fields

        /// <summary>
        /// ObjectCache
        /// </summary>
        /// <value>
        /// The cache.
        /// </value>
        protected ObjectCache Cache => MemoryCache.Default;

        #endregion Fields

        #region Methods

        /// <summary>
        /// 根据Key获取缓存
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>
        /// 缓存
        /// </returns>
        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        /// <summary>
        /// 是否设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>
        ///   <c>true</c> if the specified key is set; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 根据正则表达式移除缓存
        /// </summary>
        /// <param name="pattern">移除缓存</param>
        public void RemoveByPattern(string pattern)
        {
            this.RemoveByPattern(pattern, Cache.Select(p => p.Key));
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">值</param>
        /// <param name="cacheTime">过期时间，单位分钟</param>
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

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">值</param>
        /// <param name="dependFile">文件依赖</param>
        /// <exception cref="FileNotFoundException"></exception>
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