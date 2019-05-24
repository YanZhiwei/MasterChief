using System;
using MasterChief.DotNet4.Utilities.Operator;
using Microsoft.Win32;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     注册表辅助类
    /// </summary>
    public sealed class RegistryHelper
    {
        #region Fields

        private static readonly string _regWinLogonPath = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon";

        #endregion Fields

        #region Methods

        /// <summary>
        ///     禁止Windows免密登录
        /// </summary>
        public static void DisableDefaultLogin()
        {
            var subKey = Registry.LocalMachine.CreateSubKey
                (_regWinLogonPath);
            if (subKey == null)
                throw new ApplicationException($"访问获取注册表:{_regWinLogonPath}失败。");
            using (subKey)
            {
                subKey.DeleteValue("DefaultUserName", false);
                subKey.DeleteValue("DefaultPassword", false);
                subKey.DeleteValue("AutoAdminLogon", false);
            }
        }

        /// <summary>
        ///     设置Windows 免密登录
        /// </summary>
        /// <param name="userName">账户名称</param>
        /// <param name="password">账户密码</param>
        public static void EnableDefaultLogin(string userName, string password)
        {
            ValidateOperator.Begin()
                .NotNullOrEmpty(userName, "Windows 免密登录账户名称");

            var subKey = Registry.LocalMachine.CreateSubKey
                (_regWinLogonPath);
            if (subKey == null)
                throw new ApplicationException($"访问获取注册表:{_regWinLogonPath}失败。");
            using (subKey)
            {
                subKey.SetValue("AutoAdminLogon", "1");
                subKey.SetValue("DefaultUserName", userName);
                subKey.SetValue("DefaultPassword", password);
            }
        }

        /// <summary>
        ///     设置程序开机启动_注册表形式
        /// </summary>
        /// <param name="path">需要开机启动的exe路径</param>
        /// <param name="keyName">注册表中键值名称</param>
        /// <param name="set">true设置开机启动，false取消开机启动</param>
        public static void StartupSet(string path, string keyName, bool set)
        {
            /*
             * 知识：
             * 1.管理员权限问题
             *   在打开的工程中，看下Properties 下面是否有app.manifest 这个文件，如果没有，右击工程在菜单中选择“属性”
             *   选中"Security"，在界面中勾选"Enable ClickOnce Security Settings"后，在Properties下就有自动生成app.manifest文件。
             *   打开app.manifest文件，将<requestedExecutionLevel level="asInvoker" uiAccess="false" />改为
             *   <requestedExecutionLevel level="requireAdministrator" uiAccess="false" />
             *   然后在"Security"中再勾去"Enable ClickOnce Security Settings"后，重新编译即可。
             * 参考:
             * 1. http://syxc.iteye.com/blog/673972
             * 2. http://zouqinghua11111.blog.163.com/blog/static/67997654201242334620628/
             * 3. http://stackoverflow.com/questions/5089601/run-the-application-at-windows-startup
             */
            using (var registry = Registry.LocalMachine)
            {
                var subKey = registry.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

                if (set)
                {
                    subKey?.SetValue(keyName, path);
                }
                else
                {
                    var value = subKey?.GetValue(keyName);

                    if (value != null) subKey.DeleteValue(keyName);
                }
            }
        }

        #endregion Methods
    }
}