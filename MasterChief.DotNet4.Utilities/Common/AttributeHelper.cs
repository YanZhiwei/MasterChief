using System;
using System.Linq;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     特性辅助类
    /// </summary>
    public static class AttributeHelper
    {
        #region Methods

        /// <summary>
        ///     获取自定义Attribute
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="A">泛型</typeparam>
        /// <returns>未获取到则返回NULL</returns>
        /// 时间：2016-01-12 15:22
        /// 备注：
        // ReSharper disable once InconsistentNaming
        public static A Get<T, A>()
            where T : class
            where A : System.Attribute
        {
            var modelType = typeof(T);

            var modelAttrs = modelType.GetCustomAttributes(typeof(A), true);

            return (bool) modelAttrs?.Any() ? modelAttrs.FirstOrDefault() as A : null;
        }

        #endregion Methods
    }
}