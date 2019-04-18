using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     Tooltip 帮助类
    /// </summary>
    public static class TooltipHelper
    {
        #region Methods

        /// <summary>
        ///     ToolTip——Error
        /// </summary>
        /// <typeparam name="T">Control</typeparam>
        /// <param name="t">Control</param>
        /// <param name="tooltip">ToolTip</param>
        /// <param name="titile">标题</param>
        /// <param name="content">内容</param>
        public static void ShowErrorToolTip<T>(this T t, ToolTip tooltip, string titile, string content)
            where T : Control
        {
            t.ShowBaseToolTip(tooltip, ToolTipIcon.Error, titile, content);
        }

        /// <summary>
        ///     ToolTip——Info
        /// </summary>
        /// <typeparam name="T">Control</typeparam>
        /// <param name="t">Control</param>
        /// <param name="tooltip">ToolTip</param>
        /// <param name="titile">标题</param>
        /// <param name="content">内容</param>
        public static void ShowInfoToolTip<T>(this T t, ToolTip tooltip, string titile, string content)
            where T : Control
        {
            t.ShowBaseToolTip(tooltip, ToolTipIcon.Info, titile, content);
        }

        /// <summary>
        ///     ToolTip——Normal
        /// </summary>
        /// <typeparam name="T">Control</typeparam>
        /// <param name="t">Control</param>
        /// <param name="tooltip">ToolTip</param>
        /// <param name="content">内容</param>
        public static void ShowNormalToolTip<T>(this T t, ToolTip tooltip, string content)
            where T : Control
        {
            t.ShowBaseToolTip(tooltip, ToolTipIcon.None, string.Empty, content);
        }

        /// <summary>
        ///     为控件提供Tooltip
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="tip">ToolTip</param>
        /// <param name="message">提示消息</param>
        public static void ShowTooltip(this Control control, ToolTip tip, string message)
        {
            var mousePoint = Control.MousePosition;
            var x = control.PointToClient(mousePoint).X;
            var y = control.PointToClient(mousePoint).Y;
            tip.Show(message, control, x, y);
            tip.Active = true;
        }

        /// <summary>
        ///     为控件提供Tooltip
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="tip">ToolTip</param>
        /// <param name="message">提示消息</param>
        /// <param name="durationTime">保持提示的持续时间</param>
        public static void ShowTooltip(this Control control, ToolTip tip, string message, int durationTime)
        {
            var mousePoint = Control.MousePosition;
            var x = control.PointToClient(mousePoint).X;
            var y = control.PointToClient(mousePoint).Y;
            tip.Show(message, control, x, y, durationTime);
            tip.Active = true;
        }

        /// <summary>
        ///     为控件提供Tooltip
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="tip">ToolTip</param>
        /// <param name="message">提示消息</param>
        /// <param name="xoffset">水平偏移量</param>
        /// <param name="yoffset">垂直偏移量</param>
        public static void ShowTooltip(this Control control, ToolTip tip, string message, int xoffset, int yoffset)
        {
            var mousePoint = Control.MousePosition;
            var x = control.PointToClient(mousePoint).X;
            var y = control.PointToClient(mousePoint).Y;
            tip.Show(message, control, x + xoffset, y + yoffset);
            tip.Active = true;
        }

        /// <summary>
        ///     为控件提供Tooltip
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="tip">ToolTip</param>
        /// <param name="message">提示消息</param>
        /// <param name="xoffset">水平偏移量</param>
        /// <param name="yoffset">垂直偏移量</param>
        /// <param name="durationTime">保持提示的持续时间</param>
        public static void ShowTooltip(this Control control, ToolTip tip, string message, int xoffset, int yoffset,
            int durationTime)
        {
            var mousePoint = Control.MousePosition;
            var x = control.PointToClient(mousePoint).X;
            var y = control.PointToClient(mousePoint).Y;
            tip.Show(message, control, x + xoffset, y + yoffset, durationTime);
            tip.Active = true;
        }

        /// <summary>
        ///     ToolTip——Warning
        /// </summary>
        /// <typeparam name="T">Control</typeparam>
        /// <param name="t">Control</param>
        /// <param name="tooltip">ToolTip</param>
        /// <param name="titile">标题</param>
        /// <param name="content">内容</param>
        public static void ShowWarningToolTip<T>(this T t, ToolTip tooltip, string titile, string content)
            where T : Control
        {
            t.ShowBaseToolTip(tooltip, ToolTipIcon.Warning, titile, content);
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private static ToolTip ShowBaseToolTip<T>(this T t, ToolTip tooltip, ToolTipIcon toolTipIcon, string titile,
            string content)
            where T : Control
        {
            tooltip.Active = false;
            tooltip.Active = true;
            tooltip.ToolTipIcon = toolTipIcon;
            tooltip.ToolTipTitle = titile;
            tooltip.IsBalloon = true;
            tooltip.Show(content, t, 3000);
            return tooltip;
        }

        #endregion Methods
    }
}