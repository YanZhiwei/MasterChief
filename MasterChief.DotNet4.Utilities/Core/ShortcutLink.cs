using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using MasterChief.DotNet4.Utilities.Model;
using MasterChief.DotNet4.Utilities.Operator;

namespace MasterChief.DotNet4.Utilities.Core
{
    /// <summary>
    ///     创建快捷方式
    /// </summary>
    public sealed class ShortcutLink
    {
        /// <summary>
        ///     创建用户桌面快捷方式
        /// </summary>
        /// <param name="desktopPath">存储桌面上的文件对象的目录</param>
        /// <param name="name">快捷方式名称</param>
        /// <param name="programPath">程序路径</param>
        /// <param name="description">快捷方式描述</param>
        public static void CreatUserDesktop(string desktopPath, string name, string programPath, string description)
        {
            ValidateOperator.Begin().NotNullOrEmpty(name, "快捷方式名称")
                .CheckFileExists(programPath)
                .CheckDirectoryExist(desktopPath)
                .NotNullOrEmpty(description, "快捷方式描述");
            var link = (IShellLink) new ShellLink();
            link.SetDescription(description);
            link.SetPath(programPath);
            var file = link as IPersistFile;
            file.Save(Path.Combine(desktopPath, $"{name}.lnk"), false);
        }

        /// <summary>
        ///     创建当前用户桌面快捷方式
        /// </summary>
        /// <param name="name">快捷方式名称</param>
        /// <param name="programPath">程序路径</param>
        /// <param name="description">快捷方式描述</param>
        public static void CreatCurrentUserDesktop(string name, string programPath, string description)
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            CreatUserDesktop(desktopPath, name, programPath, description);
        }
    }
}