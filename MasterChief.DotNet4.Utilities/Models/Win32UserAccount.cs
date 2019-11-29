namespace MasterChief.DotNet4.Utilities.Models
{
    /// <summary>
    ///     Windows 账户信息
    /// </summary>
    public sealed class Win32UserAccount
    {
        /// <summary>
        ///     账户类型
        /// </summary>
        public readonly string AccountType;

        /// <summary>
        ///     标题
        /// </summary>
        public readonly string Caption;

        /// <summary>
        ///     描述
        /// </summary>
        public readonly string Description;

        /// <summary>
        ///     是否可用
        /// </summary>
        public readonly bool Disabled;

        /// <summary>
        ///     Domain
        /// </summary>
        public readonly string Domain;

        /// <summary>
        ///     全名
        /// </summary>
        public readonly string FullName;

        /// <summary>
        ///     Local Account
        /// </summary>
        public readonly bool LocalAccount;

        /// <summary>
        ///     Lockout
        /// </summary>
        public readonly bool Lockout;

        /// <summary>
        ///     账户名称
        /// </summary>
        public readonly string Name;

        /// <summary>
        ///     状态
        /// </summary>
        public readonly string Status;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="name">账户名称</param>
        /// <param name="fullName">全名</param>
        /// <param name="accountType">账户类型</param>
        /// <param name="description">描述</param>
        /// <param name="caption">标题</param>
        /// <param name="domian">domian</param>
        /// <param name="disabled">是否可用</param>
        /// <param name="localAccount">是否本地账户</param>
        /// <param name="status">账户</param>
        /// <param name="lockout">lockout</param>
        public Win32UserAccount(string name, string fullName, string accountType, string description,
            string caption, string domian, bool disabled, bool localAccount, string status, bool lockout)
        {
            Name = name;
            Caption = caption;
            FullName = fullName;
            AccountType = accountType;
            Description = description;
            Domain = domian;
            Disabled = disabled;
            LocalAccount = localAccount;
            Status = status;
            Lockout = lockout;
        }
    }
}