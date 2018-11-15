namespace MasterChief.DotNet4.Utilities.Common
{
    using System.Collections;

    /// <summary>
    /// 集合 辅助类
    /// </summary>
    public static class CollectionHelper
    {
        #region Methods

        /// <summary>
        ///  判断集合是否NULL或含有元素
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   是否NULL或含有元素
        /// </returns>
        public static bool IsNullOrEmpty(this IEnumerable source)
        {
            return source?.GetEnumerator().MoveNext() == false;
        }

        #endregion Methods
    }
}