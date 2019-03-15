namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Linq;

    /// <summary>
    /// 特性辅助类
    /// </summary>
    public static class AttributeHelper
    {
        #region Methods

        /// <summary>
        /// 获取自定义Attribute
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="A">泛型</typeparam>
        /// <returns>未获取到则返回NULL</returns>
        /// 时间：2016-01-12 15:22
        /// 备注：
        public static A Get<T, A>()
            where T : class
            where A : Attribute
        {
            Type modelType = typeof(T);

            object[] modelAttrs = modelType.GetCustomAttributes(typeof(A), true);

            return modelAttrs?.Any() ?? false ? modelAttrs.FirstOrDefault() as A : null;
        }

        #endregion Methods
    }
}