namespace MasterChief.DotNet4.Utilities.Common
{
    using System;

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

        #endregion Methods
    }
}