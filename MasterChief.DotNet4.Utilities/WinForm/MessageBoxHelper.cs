using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     Message 帮助类
    /// </summary>
    public class MessageBoxHelper
    {
        #region Methods

        /// <summary>
        ///     错误信息
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        /// <returns>DialogResult</returns>
        /// 日期：2015-10-13 13:33
        /// 备注：
        public static DialogResult ShowError(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        ///     错误信息
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>DialogResult</returns>
        /// 日期：2015-10-13 13:33
        /// 备注：
        public static DialogResult ShowError(string message)
        {
            return ShowError(message, "错误");
        }

        /// <summary>
        ///     一般提示信息
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>DialogResult</returns>
        /// 日期：2015-10-13 13:33
        /// 备注：
        public static DialogResult ShowInfo(string message)
        {
            return ShowInfo(message, "提示");
        }

        /// <summary>
        ///     一般提示信息
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        /// <returns>DialogResult</returns>
        /// 日期：2015-10-13 13:33
        /// 备注：
        public static DialogResult ShowInfo(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        ///     警告信息
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        /// <returns>DialogResult</returns>
        /// 日期：2015-10-13 13:33
        /// 备注：
        public static DialogResult ShowWarning(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        ///     警告信息
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>DialogResult</returns>
        /// 日期：2015-10-13 13:33
        /// 备注：
        public static DialogResult ShowWarning(string message)
        {
            return ShowWarning(message, "警告");
        }

        /// <summary>
        ///     显示询问用户信息，并显示错误标志
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>DialogResult</returns>
        /// 日期：2015-10-13 13:33
        /// 备注：
        public static DialogResult ShowYesNoAndError(string message)
        {
            // ReSharper disable once LocalizableElement
            return MessageBox.Show(message, "错误", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
        }

        /// <summary>
        ///     显示询问用户信息，并显示提示标志
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>DialogResult</returns>
        /// 日期：2015-10-13 13:33
        /// 备注：
        public static DialogResult ShowYesNoAndTips(string message)
        {
            // ReSharper disable once LocalizableElement
            return MessageBox.Show(message, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        /// <summary>
        ///     显示询问用户信息，并显示警告标志
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>DialogResult</returns>
        /// 日期：2015-10-13 13:34
        /// 备注：
        public static DialogResult ShowYesNoAndWarning(string message)
        {
            // ReSharper disable once LocalizableElement
            return MessageBox.Show(message, "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        /// <summary>
        ///     显示询问用户信息，并显示提示标志
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>DialogResult</returns>
        /// 日期：2015-10-13 13:34
        /// 备注：
        public static DialogResult ShowYesNoCancelAndInfo(string message)
        {
            // ReSharper disable once LocalizableElement
            return MessageBox.Show(message, "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
        }

        #endregion Methods
    }
}