namespace MasterChief.DotNet.Core.PerRequestCache
{
    using MasterChief.DotNet.Core.Cache;
    using System;
    using System.Collections;
    using System.Linq;
    using System.Web;

    /// <summary>
    ///一个HttpRequest中的各个单元需要处理相同或类似的数据。
    ///如果数据的生存期只是一个请求，就可以考虑使用HttpContext. Items作为短期的高速缓存。
    /// </summary>
    public class PerRequestProvider : ICacheProvider
    {
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
            IDictionary items = GetItems();
            if (items == null)
            {
                return default(T);
            }

            return (T)items[key];
        }

        /// <summary>
        /// 是否设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>
        /// <c>true</c> if the specified key is set; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSet(string key)
        {
            IDictionary items = GetItems();
            if (items == null)
            {
                return false;
            }

            return (items[key] != null);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            IDictionary items = GetItems();
            if (items == null)
            {
                return;
            }

            items.Remove(key);
        }

        /// <summary>
        /// 根据正则表达式移除缓存
        /// </summary>
        /// <param name="pattern">移除缓存</param>
        public void RemoveByPattern(string pattern)
        {
            IDictionary items = GetItems();
            if (items == null)
            {
                return;
            }
            this.RemoveByPattern(pattern, items.Keys.Cast<object>().Select(p => p.ToString()));
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">值</param>
        /// <param name="cacheTime">过期时间，单位分钟</param>
        public void Set(string key, object data, int cacheTime)
        {
            IDictionary items = GetItems();
            if (items == null)
            {
                return;
            }

            if (data != null)
            {
                if (items.Contains(key))
                {
                    items[key] = data;
                }
                else
                {
                    items.Add(key, data);
                }
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">值</param>
        /// <param name="dependFile">文件依赖</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Set(string key, object data, string dependFile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <returns>IDictionary</returns>
        protected virtual IDictionary GetItems()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Items;
            }

            return null;
        }

        #endregion Methods
    }
}