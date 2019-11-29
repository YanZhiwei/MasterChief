using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     枚举帮助类
    /// </summary>
    public static class EnumHelper
    {
        #region Methods

        /// <summary>
        ///     判断枚举是否包括枚举常数名称
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="enumName">枚举常数名称</param>
        /// <returns>是否包括枚举常数名称</returns>
        public static bool ContainEnumName<T>(string enumName)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) return false;
            var array = Enum.GetNames(typeof(T));

            bool? any = array.Any();

            if (!(bool) any) return false;

            return array.Any(item => string.Compare(item, enumName, StringComparison.OrdinalIgnoreCase) == 0);
        }

        /// <summary>
        ///     从枚举中获取Description
        /// </summary>
        /// <param name="targetEnum">需要获取枚举描述的枚举</param>
        /// <returns>描述内容</returns>
        public static string GetDescription(this Enum targetEnum)
        {
            var fieldInfo = targetEnum.GetType().GetField(targetEnum.ToString());
            var attr = fieldInfo.GetDescriptionAttr();

            var description = attr != null && attr.Length > 0 ? attr[0].Description : targetEnum.ToString();

            return description;
        }

        /// <summary>
        ///     获取枚举常数名称
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="targetEnum">The target enum.</param>
        /// <returns>常数名称</returns>
        public static string GetName<T>(this T targetEnum)
            where T : struct, IConvertible
        {
            return Enum.GetName(typeof(T), targetEnum);
        }

        /// <summary>
        ///     根据枚举数值获取枚举常数名称
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="enumNumber">枚举数值.</param>
        /// <returns>枚举常数名称</returns>
        public static string GetName<T>(int enumNumber)
            where T : struct, IConvertible
        {
            return Enum.GetName(typeof(T), enumNumber);
        }

        /// <summary>
        ///     将枚举数值转换成数组
        ///     <code>
        /// int[] _actual = EnumHelper.GetValues'int'(typeof(AreaMode));
        /// int[] _expected = new int[] { 0, 1, 2, 4, 8, 16, 32, 59 };
        /// CollectionAssert.AreEqual(_actual, _expected);
        /// </code>
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="enumType">枚举类型</param>
        /// <returns>数组</returns>
        /// 日期：2015-09-16 14:00
        /// 备注：
        public static T[] GetValues<T>(this Type enumType)
        {
            if (!enumType.IsEnum) return null;
            var array = Enum.GetValues(enumType);
            var count = array.Length;
            var values = new T[count];

            for (var i = 0; i < count; i++) values[i] = (T) array.GetValue(i);

            return values;

        }

        /// <summary>
        ///     检查枚举是否包含
        ///     <para>
        ///         Assert.IsTrue(AreaMode.CITY.In(AreaMode.CITYTOWN, AreaMode.CITY));
        ///     </para>
        /// </summary>
        /// <param name="data">枚举</param>
        /// <param name="values">枚举</param>
        /// <returns>是否包含</returns>
        public static bool In(this Enum data, params Enum[] values)
        {
            return Array.IndexOf(values, data) != -1;
        }

        /// <summary>
        ///     检查枚举是否不包含
        ///     <para>
        ///         Assert.IsTrue(AreaMode.CITYTOWN.NotIn(AreaMode.ROAD));
        ///     </para>
        /// </summary>
        /// <param name="data">枚举</param>
        /// <param name="values">枚举</param>
        /// <returns>是否未包含</returns>
        public static bool NotIn(this Enum data, params Enum[] values)
        {
            return Array.IndexOf(values, data) == -1;
        }

        /// <summary>
        ///     根据Description获取枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="description">枚举描述</param>
        /// <param name="defaultValue">默认枚举</param>
        /// <returns>枚举</returns>
        public static T ParseEnumDescription<T>(this string description, T defaultValue)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) return defaultValue;
            var curType = typeof(T);

            foreach (var field in curType.GetFields())
            {
                var descAttr = field.GetDescriptionAttr();

                if (descAttr != null && descAttr.Length > 0)
                {
                    if (string.Compare(descAttr[0].Description, description, StringComparison.OrdinalIgnoreCase) !=
                        0) continue;
                    defaultValue = (T) field.GetValue(null);
                    break;
                }

                if (string.Compare(field.Name, description, StringComparison.OrdinalIgnoreCase) != 0) continue;
                defaultValue = (T) field.GetValue(null);
                break;
            }

            return defaultValue;
        }

        /// <summary>
        ///     将枚举常数名称转换成枚举
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="enumName">枚举常数名称</param>
        /// <returns>枚举</returns>
        public static T ParseEnumName<T>(this string enumName)
            where T : struct, IConvertible
        {
            return (T) Enum.Parse(typeof(T), enumName, true);
        }

        /// <summary>
        ///     转换KeyValuePair
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <returns>KeyValuePair</returns>
        /// 时间：2018/3/14 9:43
        /// 备注：
        public static List<KeyValuePair<int, string>> ToKeyValuePair<T>()
            where T : struct, IConvertible
        {
            var keyValues = new List<KeyValuePair<int, string>>();
            if (!typeof(T).IsEnum) return keyValues;
            keyValues.AddRange(Enum.GetNames(typeof(T)).Select(item => new KeyValuePair<int, string>((int) Enum.Parse(typeof(T), item), item)));
            return keyValues;
        }

        private static DescriptionAttribute[] GetDescriptionAttr(this FieldInfo fieldInfo)
        {
            return (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        }

        #endregion Methods
    }
}