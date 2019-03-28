using System.Collections.Generic;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     Dictionary 帮助类
    /// </summary>
    /// 创建时间:2015-05-27 9:50
    /// 备注说明:
    /// <c>null</c>
    public static class DictionaryHelper
    {
        #region Methods

        /// <summary>
        ///     创建或修改
        /// </summary>
        /// <typeparam name="TKey">泛型</typeparam>
        /// <typeparam name="TValue">泛型</typeparam>
        /// <param name="dict">Dictionary</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>更新Dictionary</returns>
        public static Dictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key,
            TValue value)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, value);
            else
                dict[key] = value;

            return dict;
        }

        /// <summary>
        ///     添加字典
        /// </summary>
        /// <typeparam name="TKey">泛型</typeparam>
        /// <typeparam name="TValue">泛型</typeparam>
        /// <param name="dict">Dictionary</param>
        /// <param name="dicList">Dictionary集合</param>
        /// <param name="replaceExisted">是否更新已经存在键值</param>
        /// <returns>更新后的Dictionary</returns>
        public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dict,
            IEnumerable<KeyValuePair<TKey, TValue>> dicList, bool replaceExisted)
        {
            foreach (var item in dicList)
                if (!dict.ContainsKey(item.Key) || replaceExisted)
                    dict[item.Key] = item.Value;

            return dict;
        }

        /// <summary>
        ///     取值
        /// </summary>
        /// <typeparam name="TKey">泛型</typeparam>
        /// <typeparam name="TValue">泛型</typeparam>
        /// <param name="dict">Dictionary</param>
        /// <param name="key">键</param>
        /// <param name="defaultValue">若键不存在返回默认数值</param>
        /// <returns>键对应的值</returns>
        /// 创建时间:2015-05-27 9:49
        /// 备注说明:
        /// <c>null</c>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        /// <summary>
        ///     添加
        /// </summary>
        /// <typeparam name="TKey">泛型</typeparam>
        /// <typeparam name="TValue">泛型</typeparam>
        /// <param name="dict">Dictionary</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>更新Dictionary</returns>
        /// 创建时间:2015-05-27 9:46
        /// 备注说明:
        /// <c>null</c>
        public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key,
            TValue value)
        {
            if (!dict.ContainsKey(key)) dict.Add(key, value);

            return dict;
        }

        #endregion Methods
    }
}