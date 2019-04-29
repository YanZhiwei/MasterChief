using System;
using System.Web;

namespace MasterChief.DotNet4.Utilities.WebForm.Core
{
    /// <summary>
    ///     Session 帮助类
    /// </summary>
    public static class SessionManager
    {
        #region Methods

        /// <summary>
        ///     添加Session
        ///     默认过期时间
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Add<T>(string key, T value)
        {
            Add(key, value, -1);
        }

        /// <summary>
        ///     添加Session
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="iExpires">过期时间(分钟)</param>
        public static void Add<T>(string key, T value, int iExpires)
        {
            HttpContext.Current.Session[key] = value;
            if (iExpires != -1) HttpContext.Current.Session.Timeout = iExpires;
        }

        /// <summary>
        ///     如果该键尚不存在，则使用指定函数将键/值对添加到 Session；如果该键已存在，则使用该函数更新 Session 中的键/值对。
        /// </summary>
        /// <typeparam name="TValue">泛型，数值类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="updateHanlder">更新委托</param>
        public static void AddOrUpdate<TValue>(string key, TValue value, Func<string, TValue, TValue> updateHanlder)
        {
            if (HttpContext.Current.Session[key] == null)
            {
                Add(key, value);
            }
            else
            {
                if (updateHanlder != null)
                {
                    var oldValue = (TValue) HttpContext.Current.Session[key];
                    HttpContext.Current.Session[key] = updateHanlder(key, oldValue);
                }
            }
        }

        /// <summary>
        ///     如果该键尚不存在，则使用指定函数将键/值对添加到 Session；如果该键已存在，则使用该函数更新 Session 中的键/值对。
        /// </summary>
        /// <typeparam name="TValue">泛型，数值类型.</typeparam>
        /// <param name="key">键.</param>
        /// <param name="value">值.</param>
        /// <param name="addHanlder">新增委托</param>
        /// <param name="updateHanlder">更新委托</param>
        public static void AddOrUpdate<TValue>(string key, TValue value, Action<TValue> addHanlder,
            Func<string, TValue, TValue> updateHanlder)
        {
            if (HttpContext.Current.Session[key] == null)
            {
                Add(key, value);
                addHanlder?.Invoke(value);
            }
            else
            {
                if (updateHanlder != null)
                {
                    var oldValue = (TValue) HttpContext.Current.Session[key];
                    HttpContext.Current.Session[key] = updateHanlder(key, oldValue);
                }
            }
        }

        /// <summary>
        ///     如果该键尚不存在，则使用指定函数将键/值对添加到 Session；如果该键已存在，则使用该函数更新 Session 中的键/值对。
        /// </summary>
        /// <typeparam name="TValue">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="addHanlder">新增委托</param>
        /// <param name="updateHanlder">修改委托</param>
        public static void AddOrUpdate<TValue>(string key, Func<string, TValue> addHanlder,
            Func<string, TValue, TValue> updateHanlder)
        {
            if (HttpContext.Current.Session[key] == null)
            {
                if (addHanlder != null) HttpContext.Current.Session[key] = addHanlder(key);
            }
            else
            {
                if (updateHanlder != null)
                {
                    var oldValue = (TValue) HttpContext.Current.Session[key];
                    HttpContext.Current.Session[key] = updateHanlder(key, oldValue);
                }
            }
        }

        /// <summary>
        ///     清除所有Session
        /// </summary>
        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

        /// <summary>
        ///     获取Session
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <returns>数值</returns>
        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key] == null)
                return default(T);
            return (T) HttpContext.Current.Session[key];
        }

        /// <summary>
        ///     是否存在此Session
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>是否存在</returns>
        public static bool IsExisting(string key)
        {
            if (HttpContext.Current.Session[key] != null)
                return true;
            return false;
        }

        /// <summary>
        ///     删除Session
        /// </summary>
        /// <param name="key">键</param>
        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null) HttpContext.Current.Session.Remove(key);
        }

        /// <summary>
        ///     如果该键尚不存在，则使用指定函数将键/值对添加到 Session；如果该键已存在，则使用该函数更新 Session 中的键/值对。
        /// </summary>
        /// <typeparam name="TValue">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="updateHanlder">修改委托</param>
        public static void Update<TValue>(string key, Func<string, TValue, TValue> updateHanlder)
        {
            if (HttpContext.Current.Session[key] != null)
                if (updateHanlder != null)
                {
                    var oldValue = (TValue) HttpContext.Current.Session[key];
                    HttpContext.Current.Session[key] = updateHanlder(key, oldValue);
                }
        }

        #endregion Methods
    }
}