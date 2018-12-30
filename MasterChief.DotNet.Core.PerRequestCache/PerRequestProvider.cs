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

        public T Get<T>(string key)
        {
            IDictionary items = GetItems();
            if (items == null)
            {
                return default(T);
            }

            return (T)items[key];
        }

        public bool IsSet(string key)
        {
            IDictionary items = GetItems();
            if (items == null)
            {
                return false;
            }

            return (items[key] != null);
        }

        public void Remove(string key)
        {
            IDictionary items = GetItems();
            if (items == null)
            {
                return;
            }

            items.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            IDictionary items = GetItems();
            if (items == null)
            {
                return;
            }
            this.RemoveByPattern(pattern, items.Keys.Cast<object>().Select(p => p.ToString()));
        }

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