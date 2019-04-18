using System;
using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     选择文件夹帮助类
    /// </summary>
    public class OpenFolderDialogHelper
    {
        #region Methods

        /// <summary>
        ///     获取选中文件夹
        /// </summary>
        /// <param name="description">说明文本.</param>
        /// <param name="showNewFolderButton">是否显示新建文件夹按钮</param>
        /// <param name="rootFolder">设置浏览的根文件夹</param>
        /// <returns>选中的文件夹</returns>
        public static string GetFolderName(string description, bool showNewFolderButton,
            Environment.SpecialFolder rootFolder)
        {
            var folderDialog = new FolderBrowserDialog();
            folderDialog.Description = description;
            folderDialog.ShowNewFolderButton = showNewFolderButton;
            folderDialog.RootFolder = rootFolder;
            var result = folderDialog.ShowDialog();

            if (result == DialogResult.OK)
                return folderDialog.SelectedPath;
            return string.Empty;
        }

        /// <summary>
        ///     获取选中文件夹
        /// </summary>
        /// <param name="description">说明文本.</param>
        /// <returns>选中文件夹</returns>
        public static string GetFolderName(string description)
        {
            return GetFolderName(description, false, Environment.SpecialFolder.Desktop);
        }

        /// <summary>
        ///     获取选中文件夹
        /// </summary>
        /// <returns>选中文件夹</returns>
        public static string GetFolderName()
        {
            return GetFolderName("请选择文件夹.", false, Environment.SpecialFolder.Desktop);
        }

        #endregion Methods
    }
}