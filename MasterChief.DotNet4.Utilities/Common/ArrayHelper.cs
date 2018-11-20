namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Array 辅助类
    /// </summary>
    public static class ArrayHelper
    {
        #region Methods

        /// <summary>
        /// 向数组添加一个元素
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="source">原始数组</param>
        /// <param name="item">元素</param>
        /// <returns>新的数组</returns>
        public static T[] Add<T>(this T[] source, T item)
        {
            int count = source.Length;
            Array.Resize<T>(ref source, count + 1);
            source[count] = item;
            return source;
        }

        /// <summary>
        /// 字符串数值忽略大小写包含判断
        /// </summary>
        /// <param name="sourceArray">需要操作的数组</param>
        /// <param name="compareStringItem">包含判断的字符串</param>
        /// <returns>是否包含在内</returns>
        public static bool ContainIgnoreCase(this string[] sourceArray, string compareStringItem)
        {
            bool result = false;

            foreach (string item in sourceArray)
            {
                if (item.CompareIgnoreCase(compareStringItem))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 复制数组
        /// <para>
        /// eg: CollectionAssert.AreEqual(new int[3] { 1, 2, 3 }, ArrayHelper.Copy(new int[5] { 1,
        ///     2, 3, 4, 5 }, 0, 3));
        /// </para>
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sourceArray">需要操作数组</param>
        /// <param name="startIndex">复制起始索引，从零开始</param>
        /// <param name="endIndex">复制结束索引</param>
        /// <returns>数组</returns>
        public static T[] Copy<T>(T[] sourceArray, int startIndex, int endIndex)
        {
            int len = endIndex - startIndex;
            T[] destination = new T[len];
            Array.Copy(sourceArray, startIndex, destination, 0, len);
            return destination;
        }

        /// <summary>
        /// 像数组添加数组
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="source">原始数组</param>
        /// <param name="target">添加数组</param>
        /// <returns>新的数组</returns>
        public static T[] AddRange<T>(this T[] source, T[] target)
        {
            int count = source.Length;
            int targetCount = target.Length;
            Array.Resize<T>(ref source, count + targetCount);
            target.CopyTo(source, count);
            return source;
        }

        /// <summary>
        /// 判断数组是否是空还是NULL
        /// </summary>
        /// <param name="source">原始数组</param>
        public static bool IsNullOrEmpty(this Array source)
        {
            if (source == null || source.Length == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 将array转为具体List对象集合
        /// </summary>
        /// <param name="data">Array</param>
        /// <returns>List对象集合</returns>
        public static List<T> ToList<T>(this Array data)
        {
            return data.Cast<T>().ToList<T>();
        }

        #endregion Methods
    }
}