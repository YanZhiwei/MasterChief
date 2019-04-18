using System;
using System.Text;
using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     CheckBox 帮助类
    /// </summary>
    public static class CheckBoxHelper
    {
        #region Methods

        /// <summary>
        ///     转译checkbox的状态
        /// </summary>
        /// <param name="checkStateFactory">委托</param>
        /// <param name="checks">CheckBox</param>
        /// <returns>转译后的checkbox的状态</returns>
        public static string TranCheckState(Func<CheckState, string> checkStateFactory, params CheckBox[] checks)
        {
            var builder = new StringBuilder();
            if (checks != null && checks.Length > 0)
                foreach (var ck in checks)
                    builder.Append(checkStateFactory(ck.CheckState));
            return builder.ToString();
        }

        #endregion Methods
    }
}