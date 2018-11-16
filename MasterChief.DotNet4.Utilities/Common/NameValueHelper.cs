namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Collections.Specialized;

    /// <summary>
    /// NameValueCollection帮助类
    /// </summary>
    public static class NameValueHelper
    {
        #region Methods

        /// <summary>
        /// 针对NameValueCollection的键值判断
        /// </summary>
        /// <param name="collection">NameValueCollection</param>
        /// <param name="key">键</param>
        /// <param name="expect">期待的值</param>
        /// <returns>根据键获取的值与期待的值一致</returns>
        public static bool Check(this NameValueCollection collection, string key, string expect)
        {
            bool _result = false;

            if (collection != null && !string.IsNullOrEmpty(key))
            {
                string _value = collection[key];

                if (!string.IsNullOrEmpty(_value))
                {
                    _result = _value.Equals(expect);
                }
            }

            return _result;
        }

        /// <summary>
        /// 针对NameValueCollection的键值判断
        /// <para>eg: _modulesettings.JudgedEqual("EmergencyControl", "1", result => { return result == true ? BarItemVisibility.Always : BarItemVisibility.Never; });</para>
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="collection">NameValueCollection</param>
        /// <param name="key">键</param>
        /// <param name="expect">期待的值</param>
        /// <param name="checkedHanlder">委托</param>
        /// <returns>自定义返回类型</returns>
        public static T Check<T>(this NameValueCollection collection, string key, string expect, Func<bool, T> checkedHanlder)
        {
            T _instance = default(T);
            bool _result = Check(collection, key, expect);
            _instance = checkedHanlder(_result);
            return _instance;
        }

        /// <summary>
        /// 针对String类型的等于判断
        /// <para>eg: _modulesettings["WeekSchedule"].JudgedEqual("1", result => ItemVisibilityRule(result));</para>
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="value">需要判断的值</param>
        /// <param name="expect">期待的值</param>
        /// <param name="checkedHanlder">委托</param>
        /// <returns>自定义返回类型</returns>
        public static T Check<T>(this string value, string expect, Func<bool, T> checkedHanlder)
        {
            T _instance = default(T);
            bool _result = value.Equals(expect);
            _instance = checkedHanlder(_result);
            return _instance;
        }

        #endregion Methods
    }
}