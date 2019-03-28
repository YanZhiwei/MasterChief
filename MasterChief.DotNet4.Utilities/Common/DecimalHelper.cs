using System;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     Decimal 帮助类
    /// </summary>
    public static class DecimalHelper
    {
        #region Methods

        /// <summary>
        ///     计算百分比(保持两位小数)
        /// </summary>
        /// <param name="value">分子</param>
        /// <param name="total">分母</param>
        /// <returns>百分比</returns>
        public static decimal CalcPercentage(decimal value, decimal total)
        {
            return Math.Round(100 * value / total, 2);
        }

        /// <summary>
        ///     转换成钱表示形式（保持两位小数）
        /// </summary>
        /// <param name="data">需要处理的decimal</param>
        /// <returns>保持两位小数</returns>
        public static decimal ToMoney(this decimal data)
        {
            return Math.Round(data, 2);
        }

        /// <summary>
        ///     处理decimal，规则如下：
        ///     1.55.55 output 55.55
        ///     2.55.00 output 55
        /// </summary>
        /// <param name="value">需要处理的decimal</param>
        /// <returns>处理后的decimal</returns>
        public static decimal CustomizedDecimal(decimal value)
        {
            return decimal.Parse(string.Format("{0:0.00}", value).Replace(".00", "")).ToDecimalOrDefault(value);
        }

        #endregion Methods
    }
}