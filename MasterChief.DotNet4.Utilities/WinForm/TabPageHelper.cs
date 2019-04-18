using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     TabPage 帮助类
    /// </summary>
    /// 时间：2016-01-12 17:28
    /// 备注：
    public static class TabPageHelper
    {
        #region Methods

        /// <summary>
        ///     设置Enabled属性
        /// </summary>
        /// <param name="tabpage">TabPage</param>
        /// <param name="enable">if set to <c>true</c> [enable].</param>
        /// 时间：2016-01-12 17:29
        /// 备注：
        public static void SetEnabled(this TabPage tabpage, bool enable)
        {
            ((Control) tabpage).Enabled = false;
        }

        #endregion Methods
    }
}