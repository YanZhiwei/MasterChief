using System;
using System.Collections.Generic;
using System.Management;
using MasterChief.DotNet4.Utilities.Model;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     Windows 账户辅助类
    /// </summary>
    public static class Win32UserAccountHelper
    {
        /// <summary>
        ///     获取本机用户列表
        /// </summary>
        /// <returns>用户列表</returns>
        public static List<Win32UserAccount> GetUsers()
        {
            return GetUsers(Environment.MachineName);
        }

        /// <summary>
        ///     获取指定机器用户列表
        /// </summary>
        /// <param name="domain">机器名称</param>
        /// <returns>用户列表</returns>
        public static List<Win32UserAccount> GetUsers(string domain)
        {
            var users = new List<Win32UserAccount>();
            var query = new SelectQuery("Win32_UserAccount", $"Domain='{domain}'");
            var searcher = new ManagementObjectSearcher(query);
            var searcherResult = searcher.Get();
            foreach (var item in searcherResult)
            {
                var account = new Win32UserAccount(
                    item["Name"].ToStringOrDefault(),
                    item["FullName"].ToStringOrDefault(),
                    item["AccountType"].ToStringOrDefault(),
                    item["Description"].ToStringOrDefault(),
                    item["Caption"].ToStringOrDefault(),
                    item["Domain"].ToStringOrDefault(),
                    item["Disabled"].ToBooleanOrDefault(),
                    item["LocalAccount"].ToBooleanOrDefault(),
                    item["Status"].ToStringOrDefault(),
                    item["Lockout"].ToBooleanOrDefault());
                users.Add(account);
            }

            return users;
        }
    }
}