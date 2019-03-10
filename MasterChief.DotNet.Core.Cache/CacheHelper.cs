using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MasterChief.DotNet.Core.Cache
{
    /// <summary>
    /// 缓存辅助类
    /// </summary>
    public static class CacheHelper
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="cacheManager">ICacheProvider</param>
        /// <param name="key">键</param>
        /// <param name="conditions">条件委托</param>
        /// <returns>缓存</returns>
        public static T Get<T>(this ICacheProvider cacheManager, string key, Func<T> conditions)
        {
            return Get(cacheManager, key, 60, conditions);
        }

        /// <summary>
        /// 获取缓存.
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="cacheManager">ICacheProvider.</param>
        /// <param name="key">键</param>
        /// <param name="dependFile">文件依赖</param>
        /// <param name="conditions">条件委托</param>
        /// <returns>缓存</returns>
        public static T Get<T>(this ICacheProvider cacheManager, string key, string dependFile, Func<T> conditions)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }
            T result = conditions();
            if (File.Exists(dependFile))
            {
                cacheManager.Set(key, result, dependFile);
            }

            return result;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="cacheManager">ICacheProvider</param>
        /// <param name="key">键</param>
        /// <param name="cacheTime">缓存时间，单位分钟</param>
        /// <param name="conditions">条件委托</param>
        /// <returns>缓存</returns>
        public static T Get<T>(this ICacheProvider cacheManager, string key, int cacheTime, Func<T> conditions)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }

            T result = conditions();
            if (cacheTime > 0)
            {
                cacheManager.Set(key, result, cacheTime);
            }

            return result;
        }

        /// <summary>
        /// 根据正则表达式移除缓存
        /// </summary>
        /// <param name="cacheManager">ICacheProvider</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="keys">要移除的缓存key</param>
        public static void RemoveByPattern(this ICacheProvider cacheManager, string pattern, IEnumerable<string> keys)
        {
            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (string key in keys.Where(p => regex.IsMatch(p.ToString())).ToList())
            {
                cacheManager.Remove(key);
            }
        }
    }
}