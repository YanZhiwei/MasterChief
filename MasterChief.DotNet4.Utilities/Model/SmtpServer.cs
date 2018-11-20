namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// SMTP服务器 实体类
    /// </summary>
    public class SmtpServer
    {
        #region Fields

        /// <summary>
        /// 服务器
        /// </summary>
        public readonly string Host;

        /// <summary>
        /// 发送邮箱
        /// </summary>
        public readonly string SendMail;

        /// <summary>
        /// 发送邮箱密码
        /// </summary>
        public readonly string SendMailPasswrod;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">服务器</param>
        /// <param name="sendmail">发送邮箱</param>
        /// <param name="sendPossword">发送邮箱密码</param>
        public SmtpServer(string host, string sendmail, string sendPossword)
        {
            Host = host;
            SendMail = sendmail;
            SendMailPasswrod = sendPossword;
        }

        #endregion Constructors
    }
}