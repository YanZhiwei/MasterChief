using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     弹出文件对话框 帮助类
    /// </summary>
    public class OpenFileDialogHelper
    {
        #region Methods

        /// <summary>
        ///     弹出文件对话框，获取打开的文件名，该重载方法未设置过滤器，显示全部文件。如果未打开文件，则返回""。
        /// </summary>
        /// <param name="title">在文件对话框上显示的标题</param>
        /// <returns>选中文件路径</returns>
        /// 日期：2015-10-13 13:35
        /// 备注：
        public static string GetFileName(string title)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = title,
                // ReSharper disable once LocalizableElement
                Filter = "全部(*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                return openFileDialog.FileName;
            return string.Empty;
        }

        /// <summary>
        ///     弹出文件对话框，获取打开的文件名。如果未打开文件，则返回"";
        /// </summary>
        /// <param name="title">在文件对话框上显示的标题</param>
        /// <param name="filter">
        ///     条件过滤，只显示指定后缀的文件名。
        ///     范例1：全部(*.*)|*.*,
        ///     范例2：数据库脚本文件(*.sql)|*.sql|文本文件(*.txt)|*.txt
        /// </param>
        /// <returns>选中文件路径</returns>
        public static string GetFileName(string title, string filter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                return openFileDialog.FileName;
            return string.Empty;
        }

        /// <summary>
        ///     弹出文件对话框，获取所有打开的文件名，该重载方法未设置过滤器，显示全部文件。如果未打开文件，则返回""。
        /// </summary>
        /// <param name="title">在文件对话框上显示的标题</param>
        /// <returns>选中多个文件路径</returns>
        public static string[] GetFileNames(string title)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = title,
                // ReSharper disable once LocalizableElement
                Filter = "全部(*.*)|*.*",
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                return openFileDialog.FileNames;
            return new[] {string.Empty};
        }

        /// <summary>
        ///     弹出文件对话框，获取所有打开的文件名。如果未打开文件，则返回"";
        /// </summary>
        /// <param name="title">在文件对话框上显示的标题</param>
        /// <param name="filter">
        ///     条件过滤，只显示指定后缀的文件名。
        ///     范例1：全部(*.*)|*.*,
        ///     范例2：数据库脚本文件(*.sql)|*.sql|文本文件(*.txt)|*.txt
        /// </param>
        /// <returns>选中多个文件路径</returns>
        public static string[] GetFileNames(string title, string filter)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = title;
            openFileDialog.Filter = filter;
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                return openFileDialog.FileNames;
            return new[] {string.Empty};
        }

        #endregion Methods
    }
}