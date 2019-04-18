using System;
using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     MenuStrip 帮助类
    /// </summary>
    public static class MenuStripHelper
    {
        #region Methods

        /// <summary>
        ///     遍历MenuStrip控件
        /// </summary>
        /// <param name="menu">MenuStrip</param>
        /// <param name="forMenuFactory">遍历的时候动作『委托』</param>
        public static void RecursiveMenuItem(this MenuStrip menu, Action<ToolStripMenuItem> forMenuFactory)
        {
            foreach (ToolStripMenuItem item in menu.Items)
            {
                if (forMenuFactory != null) forMenuFactory(item);

                RecursiveDropDownItems(item, forMenuFactory);
            }
        }

        /// <summary>
        ///     递归遍历
        /// </summary>
        /// <param name="menu">MenuStrip</param>
        /// <param name="forMenuFactory">遍历的时候动作『委托』</param>
        /// 日期：2015-10-13 13:31
        /// 备注：
        private static void RecursiveDropDownItems(ToolStripMenuItem menu, Action<ToolStripMenuItem> forMenuFactory)
        {
            for (var i = 0; i < menu.DropDownItems.Count; i++)
                if (!(menu.DropDownItems[i] is ToolStripSeparator))
                {
                    var dropItemMenu = (ToolStripMenuItem) menu.DropDownItems[i];
                    forMenuFactory?.Invoke(dropItemMenu);

                    RecursiveDropDownItems(dropItemMenu, forMenuFactory);
                }
        }

        #endregion Methods
    }
}