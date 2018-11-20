namespace MasterChief.DotNet4.Utilities.WebForm.Core
{
    using System;
    using System.Web;
    using System.Web.Caching;

    /// <summary>
    /// 缓存帮助类
    /// </summary>
    public static class CacheManger
    {
        #region Properties

        /*
         *ASP.NET支持二种缓存项的过期策略：绝对过期和滑动过期。
         *    1. 绝对过期，这个容易理解：就是在缓存放入Cache时，指定一个具体的时间。当时间到达指定的时间的时，缓存项自动从Cache中移除。
         *    2. 滑动过期：某些缓存项，我们可能只希望在有用户在访问时，就尽量保留在缓存中，只有当一段时间内用户不再访问该缓存项时，才移除它， 这样可以优化内存的使用，因为这种策略可以保证缓存的内容都是【很热门】的。 操作系统的内存以及磁盘的缓存不都是这样设计的吗？而这一非常有用的特性，Cache也为我们准备好了，只要在将缓存项放入缓存时， 指定一个滑动过期时间就可以实现了。
         *以上二个选项分别对应Add, Insert方法中的DateTime absoluteExpiration, TimeSpan slidingExpiration这二个参数。
         *注意：这二个参数都是成对使用的，但不能同时指定它们为一个【有效】值，最多只能一个参数值有效。 当不使用另一个参数项时，请用Cache类定义二个static readonly字段赋值。
         *如果都使用Noxxxxx这二个选项，那么缓存项就一直保存在缓存中。（或许也会被移除）
         */

        /// <summary>
        /// AppPrefix
        /// </summary>
        public static string AppPrefix
        {
            get
            {
                return AppDomain.CurrentDomain.Id.ToString();
            }
        }

        /// <summary>
        /// Cache对象
        /// </summary>
        public static Cache Cache
        {
            get
            {
                if (null != HttpContext.Current)
                {
                    return HttpContext.Current.Cache;
                }
                else
                {
                    return HttpRuntime.Cache;
                }
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 是否已缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否已缓存</returns>
        public static bool Contain(string key)
        {
            return Cache.Get(AppPrefix + key) != null;
        }

        /// <summary>
        /// 获取缓存内容
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static object Get(string key)
        {
            return Cache.Get(AppPrefix + key);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key">键</param>
        public static void Remove(string key)
        {
            Cache.Remove(AppPrefix + key);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set(string key, object value)
        {
            Cache.Insert(AppPrefix + key, value);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheDependency">
        /// 所插入对象的文件依赖项或缓存键依赖项。当任何依赖项更改时，该对象即无效，并从缓存中移除。如果没有依赖项，则此参数包含 nullNothingnullptrnull 引用。
        /// </param>
        public static void Set(string key, object value, CacheDependency cacheDependency)
        {
            Cache.Insert(AppPrefix + key, value, cacheDependency);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheDependency">
        /// 所插入对象的文件依赖项或缓存键依赖项。当任何依赖项更改时，该对象即无效，并从缓存中移除。如果没有依赖项，则此参数包含 nullNothingnullptrnull 引用
        /// </param>
        /// <param name="dt">
        /// 所插入对象将过期并被从缓存中移除的时间。若要避免可能出现的本地时间方面的问题（如从标准时间更改为夏时制），请对此参数值使用 UtcNow，不要使用 Now。如果使用绝对过期，则
        /// slidingExpiration 参数必须为 NoSlidingExpiration
        /// </param>
        public static void Set(string key, object value, CacheDependency cacheDependency, DateTime dt)
        {
            Cache.Insert(AppPrefix + key, value, cacheDependency, dt, TimeSpan.Zero);
        }

        /// <summary>
        /// 缓存写入，包括分钟，是否绝对过期及缓存过期的回调
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="minutes">缓存分钟</param>
        /// <param name="isAbsoluteExpiration">是否绝对过期</param>
        /// <param name="onRemoveCallback">缓存过期回调</param>
        public static void Set(string key, object value, int minutes, bool isAbsoluteExpiration, CacheItemRemovedCallback onRemoveCallback)
        {
            key = AppPrefix + key;
            if (isAbsoluteExpiration)
                HttpRuntime.Cache.Insert(key, value, null, DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration, CacheItemPriority.Normal, onRemoveCallback);
            else
                HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minutes), CacheItemPriority.Normal, onRemoveCallback);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheDependency">
        /// 所插入对象的文件依赖项或缓存键依赖项。当任何依赖项更改时，该对象即无效，并从缓存中移除。如果没有依赖项，则此参数包含 nullNothingnullptrnull 引用
        /// </param>
        /// <param name="ts">
        /// 最后一次访问所插入对象时与该对象过期时之间的时间间隔。如果该值等效于 20 分钟，则对象在最后一次被访问 20 分钟之后将过期并被从缓存中移除。如果使用可调过期，则
        /// absoluteExpiration 参数必须为 NoAbsoluteExpiration。
        /// </param>
        public static void Set(string key, object value, CacheDependency cacheDependency, TimeSpan ts)
        {
            Cache.Insert(AppPrefix + key, value, cacheDependency, Cache.NoAbsoluteExpiration, ts);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheDependency">
        /// 所插入对象的文件依赖项或缓存键依赖项。当任何依赖项更改时，该对象即无效，并从缓存中移除。如果没有依赖项，则此参数包含 nullNothingnullptrnull 引用
        /// </param>
        /// <param name="dt">
        /// 所插入对象将过期并被从缓存中移除的时间。若要避免可能出现的本地时间方面的问题（如从标准时间更改为夏时制），请对此参数值使用 UtcNow，不要使用 Now。如果使用绝对过期，则
        /// slidingExpiration 参数必须为 NoSlidingExpiration
        /// </param>
        /// <param name="ts">
        /// 最后一次访问所插入对象时与该对象过期时之间的时间间隔。如果该值等效于 20 分钟，则对象在最后一次被访问 20 分钟之后将过期并被从缓存中移除。如果使用可调过期，则
        /// absoluteExpiration 参数必须为 NoAbsoluteExpiration。
        /// </param>
        public static void Set(string key, object value, CacheDependency cacheDependency, DateTime dt, TimeSpan ts)
        {
            Cache.Insert(AppPrefix + key, value, cacheDependency, dt, ts);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheDependency">
        /// 该项的文件依赖项或缓存键依赖项。当任何依赖项更改时，该对象即无效，并从缓存中移除。如果没有依赖项，则此参数包含 null。
        /// </param>
        /// <param name="absoluteExpiration">
        /// 所插入对象将过期并被从缓存中移除的时间。若要避免可能出现的本地时间方面的问题（如从标准时间更改为夏时制），请对此参数值使用 UtcNow，不要使用 Now。如果使用绝对过期，则
        /// slidingExpiration 参数必须为 NoSlidingExpiration
        /// </param>
        /// <param name="slidingExpiration">
        /// 最后一次访问所插入对象时与该对象到期时之间的时间间隔。如果该值等效于 20 分钟，则对象在最后一次被访问 20 分钟之后将到期并被从缓存中移除。如果使用可调到期，则
        /// absoluteExpiration 参数必须为 NoAbsoluteExpiration。
        /// </param>
        /// <param name="onUpdate">在从缓存中移除对象时将调用的委托（如果提供）。当从缓存中删除应用程序的对象时，可使用它来通知应用程序。</param>
        public static void Set(string key, object value, CacheDependency cacheDependency, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemRemovedCallback onUpdate)
        {
            Cache.Insert(AppPrefix + key, value, cacheDependency, absoluteExpiration, slidingExpiration, CacheItemPriority.Normal, onUpdate);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheDependency">
        /// 该项的文件依赖项或缓存键依赖项。当任何依赖项更改时，该对象即无效，并从缓存中移除。如果没有依赖项，则此参数包含 null。
        /// </param>
        /// <param name="absoluteExpiration">
        /// 所插入对象将过期并被从缓存中移除的时间。若要避免可能出现的本地时间方面的问题（如从标准时间更改为夏时制），请对此参数值使用 UtcNow，不要使用 Now。如果使用绝对过期，则
        /// slidingExpiration 参数必须为 NoSlidingExpiration
        /// </param>
        /// <param name="slidingExpiration">
        /// 最后一次访问所插入对象时与该对象到期时之间的时间间隔。如果该值等效于 20 分钟，则对象在最后一次被访问 20 分钟之后将到期并被从缓存中移除。如果使用可调到期，则
        /// absoluteExpiration 参数必须为 NoAbsoluteExpiration。
        /// </param>
        /// <param name="priority">优先级</param>
        /// <param name="onUpdate">在从缓存中移除对象时将调用的委托（如果提供）。当从缓存中删除应用程序的对象时，可使用它来通知应用程序。</param>
        public static void Set(string key, object value, CacheDependency cacheDependency, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onUpdate)
        {
            Cache.Insert(AppPrefix + key, value, cacheDependency, absoluteExpiration, slidingExpiration, priority, onUpdate);
        }

        /// <summary>
        /// 如果该键尚不存在，则使用指定函数将键/值对添加到 Session；如果该键已存在，则使用该函数更新 Session 中的键/值对。
        /// </summary>
        /// <typeparam name="TValue">泛型，数值类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="updateFactory">更新委托</param>
        public static void AddOrUpdate<TValue>(string key, TValue value, Func<string, TValue, TValue> updateFactory)
        {
            if (!Contain(key))
            {
                Set(key, value, 120, true, null);
            }
            else
            {
                if (updateFactory != null)
                {
                    TValue _oldValue = (TValue)Get(key);
                    Set(key, updateFactory(key, _oldValue), 120, true, null);
                }
            }
        }

        ///// <summary>
        ///// 缓存处理策略
        ///// </summary>
        ///// <param name="key">键</param>
        ///// <param name="value">值</param>
        ///// <param name="reason">原因</param>
        ///// 时间：2016-05-26 9:53 备注：
        //private static void ItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        //{
        //    if (CacheItemRemovedReason.Expired == reason)//缓存过期
        //    {
        //    }
        //}

        /// <summary>
        /// 如果该键尚不存在，则使用指定函数将键/值对添加到 Session；如果该键已存在，则使用该函数更新 Session 中的键/值对。
        /// </summary>
        /// <typeparam name="TValue">泛型，数值类型.</typeparam>
        /// <param name="key">键.</param>
        /// <param name="value">值.</param>
        /// <param name="addHanlder">新增委托</param>
        /// <param name="updateHanlder">更新委托</param>
        public static void AddOrUpdate<TValue>(string key, TValue value, Action<TValue> addHanlder, Func<string, TValue, TValue> updateHanlder)
        {
            if (!Contain(key))
            {
                Set(key, value, 120, true, null);
                if (addHanlder != null)
                {
                    addHanlder(value);
                }
            }
            else
            {
                if (updateHanlder != null)
                {
                    TValue _oldValue = (TValue)Get(key);
                    Set(key, updateHanlder(key, _oldValue), 120, true, null);
                }
            }
        }

        /// <summary>
        /// 如果该键尚不存在，则使用指定函数将键/值对添加到 Session；如果该键已存在，则使用该函数更新 Session 中的键/值对。
        /// </summary>
        /// <typeparam name="TValue">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="addHanlder">新增委托</param>
        /// <param name="updateHanlder">修改委托</param>
        public static void AddOrUpdate<TValue>(string key, Func<string, TValue> addHanlder, Func<string, TValue, TValue> updateHanlder)
        {
            if (!Contain(key))
            {
                if (addHanlder != null)
                {
                    Set(key, addHanlder(key), 120, true, null);
                }
            }
            else
            {
                if (updateHanlder != null)
                {
                    TValue _oldValue = (TValue)Get(key);
                    Set(key, updateHanlder(key, _oldValue), 120, true, null);
                }
            }
        }

        #endregion Methods
    }
}