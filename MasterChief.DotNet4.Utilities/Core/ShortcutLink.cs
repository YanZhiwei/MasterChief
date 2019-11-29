using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using MasterChief.DotNet4.Utilities.Models;
using MasterChief.DotNet4.Utilities.Operator;

namespace MasterChief.DotNet4.Utilities.Core
{
    /// <summary>
    ///     创建快捷方式
    /// </summary>
    public sealed class ShortcutLink
    {
        #region Methods

        /// <summary>
        ///     检查快捷方式是否已经存在
        /// </summary>
        /// <param name="shortcutFolder">快捷方式文件夹</param>
        /// <param name="name">快捷方式名称</param>
        /// <returns>是否存在</returns>
        public static bool CheckExist(string shortcutFolder, string name)
        {
            var shortcutLink = Path.Combine(shortcutFolder, $"{name}.lnk");
            return File.Exists(shortcutLink);
        }

        /// <summary>
        ///     创建当前用户桌面快捷方式
        /// </summary>
        /// <param name="name">快捷方式名称</param>
        /// <param name="programPath">程序路径</param>
        /// <param name="description">快捷方式描述</param>
        public static string CreatCurUserDesktop(string name, string programPath, string description)
        {
            var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            return Create(desktopFolder, name, programPath, description);
        }

        /// <summary>
        ///     创建当前用户开机启动项
        /// </summary>
        /// <param name="name">快捷方式名称</param>
        /// <param name="programPath">程序路径</param>
        /// <param name="description">快捷方式描述</param>
        /// <returns></returns>
        public static string CreatCurUserStartup(string name, string programPath, string description)
        {
            var startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            return Create(startupFolder, name, programPath, description);
        }

        /// <summary>
        ///     创建快捷方式
        /// </summary>
        /// <param name="shortcutFolder">快捷方式文件夹</param>
        /// <param name="name">快捷方式名称</param>
        /// <param name="programPath">程序路径</param>
        /// <param name="description">快捷方式描述</param>
        public static string Create(string shortcutFolder, string name, string programPath, string description)
        {
            ValidateOperator.Begin().NotNullOrEmpty(name, "快捷方式名称")
                .CheckFileExists(programPath)
                .CheckDirectoryExist(shortcutFolder)
                .NotNullOrEmpty(description, "快捷方式描述");
            // ReSharper disable once SuspiciousTypeConversion.Global
            var link = (IShellLink) new ShellLink();
            link.SetDescription(description);
            link.SetPath(programPath);
            // ReSharper disable once SuspiciousTypeConversion.Global
            var file = link as IPersistFile;
            var shortcutLink = Path.Combine(shortcutFolder, $"{name}.lnk");
            // ReSharper disable once PossibleNullReferenceException
            file.Save(shortcutLink, false);
            return shortcutLink;
        }

        #endregion Methods
    }
}