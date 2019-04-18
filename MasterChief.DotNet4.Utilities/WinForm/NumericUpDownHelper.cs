using System;
using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     NumericUpDown 帮助类
    /// </summary>
    public static class NumericUpDownHelper
    {
        #region Methods

        /// <summary>
        ///     设置NumericUpDown值前，判断是否大于或者小于其控件本身maxvalue,minValue；
        /// </summary>
        /// <param name="numericUpDown">NumericUpDown</param>
        /// <param name="value">数值</param>
        public static void SetSafeValue(this NumericUpDown numericUpDown, decimal value)
        {
            var ctrlMax = numericUpDown.Maximum;
            var ctrMin = numericUpDown.Minimum;
            var legalMinValue = Math.Min(value, ctrlMax);
            numericUpDown.Value = Math.Max(ctrMin, legalMinValue);
        }

        #endregion Methods
    }
}