namespace MasterChief.DotNet4.Utilities.Common
{
    using System;

    /// <summary>
    /// 排序辅助类
    /// </summary>
    public static class SortHelper
    {
        #region Methods

        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <typeparam name="T">泛型，需实现IComparable</typeparam>
        /// <param name="array">泛型数组</param>
        public static void BubbleSort<T>(this T[] array)
        where T : IComparable<T>
        {
            if (array != null)
            {
                int _length = array.Length;

                for (int i = 0; i <= _length - 2; i++)
                {
                    for (int j = _length - 1; j >= 1; j--)
                    {
                        if (array[j].CompareTo(array[j - 1]) < 0)
                        {
                            T _temp = array[j];
                            array[j] = array[j - 1];
                            array[j - 1] = _temp;
                        }
                    }
                }
            }
        }

        #endregion Methods
    }
}