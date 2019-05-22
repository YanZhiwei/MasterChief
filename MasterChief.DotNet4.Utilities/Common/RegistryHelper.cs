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

        #endregion Methods
    }
}