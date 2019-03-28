using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text.RegularExpressions;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     AD域帮助类
    /// </summary>
    public class AdDomainHelper
    {
        #region Constructors

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="domain">域名称</param>
        /// <param name="userName">用户名称</param>
        /// <param name="userPassword">用户密码</param>
        public AdDomainHelper(string domain, string userName, string userPassword)
        {
            UserName = userName;
            UserPassword = userPassword;
            AdDomian = domain;
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     域名称
        /// </summary>
        public readonly string AdDomian;

        /// <summary>
        ///     用户名称
        /// </summary>
        public readonly string UserName;

        /// <summary>
        ///     用户密码
        /// </summary>
        public readonly string UserPassword;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     取用户所对应的用户组
        /// </summary>
        /// <returns>集合</returns>
        public List<string> GetGroups()
        {
            var groups = new List<string>();
            try
            {
                var dEntity = new DirectoryEntry($"LDAP://{AdDomian}", UserName, UserPassword);
                dEntity.RefreshCache();
                var dSearcher = new DirectorySearcher(dEntity);
                dSearcher.PropertiesToLoad.Add("memberof");
                dSearcher.Filter = $"sAMAccountName={UserName}";
                var searchResult = dSearcher.FindOne();

                if (searchResult != null)
                    foreach (var group in searchResult.Properties["memberof"])
                    {
                        var match = Regex.Match(group.ToString().Trim(), @"CN=\s*(?<g>\w*)\s*.");
                        groups.Add(match.Groups["g"].Value);
                    }
            }
            catch (Exception)
            {
                groups = null;
            }

            return groups;
        }

        /// <summary>
        ///     登陆域
        /// </summary>
        /// <returns>登陆是否成功</returns>
        public bool Login()
        {
            bool result;

            try
            {
                var dEntity = new DirectoryEntry($"LDAP://{AdDomian}", UserName, UserPassword);
                dEntity.RefreshCache();
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        #endregion Methods
    }
}