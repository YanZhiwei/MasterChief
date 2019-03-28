namespace MasterChief.DotNet4.Utilities.Manager
{
    using Model;
    using System.Net.Mail;
    using System.Text;

    /// <summary>
    /// SmtpClient 帮助类
    /// </summary>
    public class SmtpClientManager
    {
        #region Fields

        /// <summary>
        /// 附件
        /// </summary>
        public readonly string[] AttachmentsPathList;

        /// <summary>
        /// 正文是否是html格式
        /// </summary>
        public readonly bool IsbodyHtml;

        /// <summary>
        /// 正文
        /// </summary>
        public readonly string MailBody;

        /// <summary>
        /// 抄送
        /// </summary>
        public readonly string[] MailCcList;

        /// <summary>
        /// 标题
        /// </summary>
        public readonly string MailSubject;

        /// <summary>
        /// 收件人
        /// </summary>
        public readonly string[] MailToList;

        /// <summary>
        /// 发送者昵称
        /// </summary>
        public readonly string NickName;

        /// <summary>
        /// 优先级别
        /// </summary>
        public readonly MailPriority Priority;

        /// <summary>
        /// SMTP服务器
        /// </summary>
        public readonly SmtpServer StmpServer;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stmpserver">SMTP服务器</param>
        /// <param name="nickname">发送者昵称</param>
        /// <param name="mailsubject">标题</param>
        /// <param name="mailbody">正文</param>
        /// <param name="mailTolist">收件人</param>
        /// <param name="mailCclist">抄送</param>
        /// <param name="attachmentsPathlist">附件</param>
        /// <param name="isbodyhtml">正文是否是html格式</param>
        /// <param name="mailPriority">优先级别</param>
        public SmtpClientManager(SmtpServer stmpserver, string nickname, string mailsubject, string mailbody, string[] mailTolist, string[] mailCclist, string[] attachmentsPathlist, bool isbodyhtml, MailPriority mailPriority)
        {
            StmpServer = stmpserver;
            NickName = nickname;
            MailSubject = mailsubject;
            MailBody = mailbody;
            MailToList = mailTolist;
            MailCcList = mailCclist;
            AttachmentsPathList = attachmentsPathlist;
            IsbodyHtml = isbodyhtml;
            Priority = mailPriority;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stmpserver">SMTP服务器</param>
        /// <param name="nickname">发送者昵称</param>
        /// <param name="mailsubject">标题</param>
        /// <param name="mailbody">正文</param>
        /// <param name="mailTolist">收件人</param>
        public SmtpClientManager(SmtpServer stmpserver, string nickname, string mailsubject, string mailbody, string[] mailTolist)
        : this(stmpserver, nickname, mailsubject, mailbody, mailTolist, null, null, true, MailPriority.Normal)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stmpserver">SMTP服务器</param>
        /// <param name="nickname">发送者昵称</param>
        /// <param name="mailsubject">标题</param>
        /// <param name="mailbody">正文</param>
        /// <param name="mailTolist">收件人</param>
        /// <param name="mailPriority">优先级别</param>
        public SmtpClientManager(SmtpServer stmpserver, string nickname, string mailsubject, string mailbody, string[] mailTolist, MailPriority mailPriority)
        : this(stmpserver, nickname, mailsubject, mailbody, mailTolist, null, null, true, mailPriority)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stmpserver">SMTP服务器</param>
        /// <param name="nickname">发送者昵称</param>
        /// <param name="mailsubject">标题</param>
        /// <param name="mailbody">正文</param>
        /// <param name="mailTolist">收件人</param>
        /// <param name="attachmentsPathlist">附件</param>
        public SmtpClientManager(SmtpServer stmpserver, string nickname, string mailsubject, string mailbody, string[] mailTolist, string[] attachmentsPathlist)
        : this(stmpserver, nickname, mailsubject, mailbody, mailTolist, null, attachmentsPathlist, true, MailPriority.Normal)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <returns>发送返回状态</returns>
        public void Send()
        {
            MailAddress mailAddress = new MailAddress(StmpServer.SendMail, NickName);
            MailMessage mailMessage = new MailMessage();
            InitBasicInfo(mailAddress, mailMessage);
            InitSendMailList(mailMessage);
            InitSendCcList(mailMessage);
            AttachFile(mailMessage);
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new System.Net.NetworkCredential(StmpServer.SendMail, StmpServer.SendMailPasswrod);//设置SMTP邮件服务器
            smtpClient.Host = StmpServer.Host;
            smtpClient.Send(mailMessage);
        }

        /// <summary>
        /// 添加邮件附件信息
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <returns></returns>
        private void AttachFile(MailMessage mailMessage)
        {
            if(AttachmentsPathList != null && AttachmentsPathList.Length > 0)
            {
                Attachment attachFile;

                foreach(string path in AttachmentsPathList)
                {
                    attachFile = new Attachment(path);
                    mailMessage.Attachments.Add(attachFile);
                }
            }
        }

        /// <summary>
        /// 初始化邮件基本信息
        /// </summary>
        /// <param name="mailAddress">MailAddress</param>
        /// <param name="mailMessage">mailMessage</param>
        private void InitBasicInfo(MailAddress mailAddress, MailMessage mailMessage)
        {
            //发件人地址
            mailMessage.From = mailAddress;
            //电子邮件的标题
            mailMessage.Subject = MailSubject;
            //电子邮件的主题内容使用的编码
            mailMessage.SubjectEncoding = Encoding.UTF8;
            //电子邮件正文
            mailMessage.Body = MailBody;
            //电子邮件正文的编码
            mailMessage.BodyEncoding = Encoding.UTF8;
            //邮件优先级
            mailMessage.Priority = Priority;
            //是否HTML格式
            mailMessage.IsBodyHtml = IsbodyHtml;
        }

        /// <summary>
        /// 初始化发送邮件抄送集合
        /// </summary>
        /// <param name="mailMessage">MailMessage</param>
        private void InitSendCcList(MailMessage mailMessage)
        {
            if(MailCcList != null)
            {
                for(int i = 0; i < MailCcList.Length; i++)
                {
                    mailMessage.CC.Add(MailCcList[i]);
                }
            }
        }

        /// <summary>
        /// 初始化发送邮件集合
        /// </summary>
        /// <param name="mailMessage">MailMessage</param>
        private void InitSendMailList(MailMessage mailMessage)
        {
            if(MailToList != null)
            {
                for(int i = 0; i < MailToList.Length; i++)
                {
                    mailMessage.To.Add(MailToList[i]);
                }
            }
        }

        #endregion Methods
    }
}