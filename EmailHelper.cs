using System;
using System.Configuration;
using System.Net.Mail;

namespace EmailTest.CommonHelper
{
    public class EmailHelper
    {
        private readonly MailMessage _mailMessage;   //主要处理发送邮件的内容（如：收发人地址、标题、主体、图片等等）
        private SmtpClient _smtpClient; //主要处理用smtp方式发送此邮件的配置信息（如：邮件服务器、发送端口号、验证方式等等）
        private readonly int _senderPort;   //发送邮件所用的端口号（htmp协议默认为25）
        private readonly string _senderServerHost;    //发件箱的邮件服务器地址（IP形式或字符串形式均可）
        private readonly string _senderPassword;    //发件箱的密码
        private readonly string _senderUsername;   //发件箱的用户名（即@符号前面的字符串，例如：hello@163.com，用户名为：hello）
        private readonly bool _enableSsl;    //是否对邮件内容进行socket层加密传输
        private readonly bool _enablePwdAuthentication;  //是否对发件人邮箱进行密码验证

        readonly string _emailServer = ConfigurationManager.AppSettings["EmailServer"].ToString();
        readonly string _emailAdmin = ConfigurationManager.AppSettings["EmailAdmin"].ToString();
        readonly string _emailAdminPwd = ConfigurationManager.AppSettings["EmailAdminPWD"].ToString();
        int _emailSendTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["EmailSendTimeOut"].ToString());
        readonly string _emailSendFrom = ConfigurationManager.AppSettings["EmailSendFrom"].ToString();

        ///<summary>
        /// 构造函数
        ///</summary>
        ///<param name="server">发件箱的邮件服务器地址</param>
        ///<param name="toMail">收件人地址（可以是多个收件人，程序中是以";"进行区分的）</param>
        ///<param name="fromMail">发件人地址</param>
        ///<param name="subject">邮件标题</param>
        ///<param name="emailBody">邮件内容（可以以html格式进行设计）</param>
        ///<param name="username">发件箱的用户名（即@符号前面的字符串，例如：hello@163.com，用户名为：hello）</param>
        ///<param name="password">发件人邮箱密码</param>
        ///<param name="port">发送邮件所用的端口号（htmp协议默认为25）</param>
        ///<param name="sslEnable">true表示对邮件内容进行socket层加密传输，false表示不加密</param>
        ///<param name="pwdCheckEnable">true表示对发件人邮箱进行密码验证，false表示不对发件人邮箱进行密码验证</param>
        public EmailHelper(string server, string toMail, string fromMail, string subject, string emailBody, string username, string password, string port, bool sslEnable, bool pwdCheckEnable)
        {
            try
            {
                _mailMessage = new MailMessage();
                _mailMessage.To.Add(toMail);
                _mailMessage.From = new MailAddress(fromMail);
                _mailMessage.Subject = subject;
                _mailMessage.Body = emailBody;
                _mailMessage.IsBodyHtml = true;
                _mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                _mailMessage.Priority = MailPriority.Normal;
                _senderServerHost = server;
                _senderUsername = username;
                _senderPassword = password;
                _senderPort = Convert.ToInt32(port);
                _enableSsl = sslEnable;
                _enablePwdAuthentication = pwdCheckEnable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        ///<summary>
        /// 邮件的发送
        ///</summary>
        public void Send()
        {
            try
            {
                if (_smtpClient != null)
                {
                    _smtpClient = new SmtpClient();
                    //mSmtpClient.Host = "smtp." + mMailMessage.From.Host;
                    _smtpClient.Host = _senderServerHost;
                    _smtpClient.Port = _senderPort;
                    _smtpClient.UseDefaultCredentials = false;
                    _smtpClient.EnableSsl = _enableSsl;
                    if (_enablePwdAuthentication)
                    {
                        System.Net.NetworkCredential nc = new System.Net.NetworkCredential(_senderUsername, _senderPassword);
                        //mSmtpClient.Credentials = new System.Net.NetworkCredential(this.mSenderUsername, this.mSenderPassword);
                        //NTLM: Secure Password Authentication in Microsoft Outlook Express
                        _smtpClient.Credentials = nc.GetCredential(_smtpClient.Host, _smtpClient.Port, "NTLM");
                    }
                    else
                    {
                        _smtpClient.Credentials = new System.Net.NetworkCredential(_senderUsername, _senderPassword);
                    }
                    _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    _smtpClient.Send(_mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void SendEmail(string subject, string mailBody, params string[] toUserAddresss)
        {
            SmtpClient client = new SmtpClient(_emailServer);
            client.Credentials = new System.Net.NetworkCredential(_emailAdmin, _emailAdminPwd);
            //MailAddress SendFromAddr = new MailAddress(EmailSendFrom);
            //MailAddress ToUserAddr = new MailAddress(ToUserAddress);
            MailMessage message = new MailMessage();
            message.From = new MailAddress(_emailSendFrom);
            foreach (var addr in toUserAddresss)
            {
                message.To.Add(new MailAddress(addr));
            }

            message.Subject = subject;
            message.Body = mailBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            //message.To.Add()

            // client.Timeout = EMailSendTimeOut;
            client.Send(message);
        }

        public EmailHelper()
        {

        }
    }
}