namespace MasterChief.DotNet4.Utilities.Common
{
    using System;

    /// <summary>
    /// Double 帮助类
    /// </summary>
    public static class Double
    {
        #region Methods

        /// <summary>
        /// 计算百分比(保持两位小数)
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="total">The total.</param>
        /// <returns>double</returns>
        /// 日期：2015-09-16 13:57
        /// 备注：
        public static double CalcPercentage(double value, double total)
        {
            return Math.Round(100 * value / total, 2);
        }

        /// <summary>
        /// 转换成钱表示形式（保持两位小数）
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>double</returns>
        /// 日期：2015-09-16 13:57
        /// 备注：
        public static double ToMoney(this double data)
        {
            return Math.Round(data, 2);
        }

        #endregion Methods
    }
}