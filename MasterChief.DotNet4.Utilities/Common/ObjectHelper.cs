using System.Collections;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     Object 辅助类
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        ///     判断对象是否是泛型List
        /// </summary>
        /// <param name="value">对象实例</param>
        /// <returns>
        ///     否是泛型List
        /// </returns>
        public static bool IsCollection(this object value)
        {
            return value is IEnumerable;
        }
    }
}