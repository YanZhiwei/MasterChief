using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MasterChief.DotNet.Core.Cache
{
    public static class CacheHelper
    {
        public static T Get<T>(this ICacheProvider cacheManager, string key, Func<T> conditions)
        {
            return Get(cacheManager, key, 60, conditions);
        }

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