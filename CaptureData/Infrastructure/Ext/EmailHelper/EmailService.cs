using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Net;
using Common.Data;
using Domain.CommonData;
namespace EmailHelper
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    public class EmailService
    {
        class SmtpEmailKey
        {
            public string emailSmtpAccount;
            public string EmailKey;
            public string EmailClient;
            public int? EmailClientPort;
            public bool enableSsl;
        }
        public string EmailClient { get; private set; }//对应的邮件服务
        public int? EmailClientPort { get; set; }//邮件相关端口
        public string EmailId { get;private set; }//邮件发送使用的账户（系统级别）
        public string EmailKey { get;private set; }//开通发送邮件服务的账户密码
        public bool ValidateSuccess { get; private set; }//验证条件是否通过
        public string BodyHeadText { get; set; }//邮件中前置添加内容
        public string BodyFeetText { get; set; }//邮件尾部内容
        /// <summary>
        /// 使用安全套接字【当使用qq发送邮件时这个安全套接字的值必须是true】
        /// </summary>
        public bool EnableSsl { get; set; }
        /// <summary>
        /// 在进行邮件发送时是否需要对邮件发送者（系统）进行密码验证
        /// </summary>
        public bool MailEnablePswAuthentication { get; set; }
        public string LogPath { get; set; }
        SmtpEmailKey ek;
        public EmailService(string emailClient, string emailSmtpAccount, string emailKey, int? emailClientPort, bool enableSsl) 
        {
            EmailClient = emailClient;
            EmailId = emailSmtpAccount;
            EmailKey = emailKey;
            if (emailClientPort.HasValue) 
            {
                EmailClientPort = emailClientPort;
            }
            if (ek == null)
            {
                ek = new SmtpEmailKey();
            }
            //新调整业务->直接设置邮件接收的基础配置【后期直接提供邮件的内容，邮件的接收人即可】
            EnableSsl = enableSsl;
            ek.EmailClient = emailClient;
            ek.emailSmtpAccount = emailSmtpAccount;
            ek.EmailKey = emailKey;
            ek.enableSsl =enableSsl;
            ek.EmailClientPort = emailClientPort;
        }
        public bool ValidateEmailClientParam() 
        {
            ValidateSuccess = true;
            if (string.IsNullOrEmpty(EmailClient)|| string.IsNullOrEmpty(EmailId) || string.IsNullOrEmpty(EmailKey)) 
            {
                ValidateSuccess = false;
            }
            return ValidateSuccess;
        }
        private void ValidateEmailRequestParam() 
        {
            
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="emailFrom"></param>
        /// <param name="emailSender"></param>
        /// <param name="emailTo"></param>
        /// <param name="copySomeoneEmai"></param>
        /// <param name="isHtmlFormat"></param>
        /// <param name="priority"></param>
        /// <param name="fileUrls"></param>
        public void SendEmail(string subject,string body,string emailFrom,string emailSender, string emailTo,
            string[] copySomeoneEmai,bool isHtmlFormat,MailPriority priority,List<string> fileUrls) 
        {
            if (!ValidateEmailClientParam()) 
            {
                return;
            }
            //验证邮件数据是否符合要求

            SmtpClient client = new SmtpClient() { Host =EmailClient };
            if (EmailClientPort.HasValue&&EmailClientPort.Value>0) 
            {
                client.Port = EmailClientPort.Value;
            }
            client.EnableSsl = EnableSsl;
           // client.UseDefaultCredentials = false;
            try
            {
                if (!MailEnablePswAuthentication)
                {
                    NetworkCredential nc = new NetworkCredential(EmailId, EmailKey);
                    client.Credentials = nc.GetCredential(client.Host, client.Port, "NTLM");
                }
                else
                {
                    client.Credentials = new NetworkCredential(EmailId, EmailKey);
                } //  client.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(emailFrom);//发信人地址
                mail.Sender = new MailAddress(emailSender);//发件人地址
                mail.To.Add(emailTo);
                if (copySomeoneEmai != null)
                {
                    foreach (var item in copySomeoneEmai)
                    {
                        mail.To.Add(item);
                    }
                }
                mail.Subject = subject;
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(BodyHeadText))
                {
                    sb.AppendLine(BodyHeadText);
                }
                sb.AppendLine(body);
                if (!string.IsNullOrEmpty(BodyFeetText))
                {
                    sb.AppendLine(BodyFeetText);
                }
                mail.Body = sb.ToString();
                mail.IsBodyHtml = isHtmlFormat;
                mail.BodyEncoding = Encoding.UTF8;
                mail.Priority = priority;
                PreperAttachFile(fileUrls, ref mail);
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(LogPath))
                    LoggerWriter.CreateLogFile(ex.ToString(), LogPath, ELogType.ErrorLog);
            }
        }

        /// <summary>
        /// 准备附件
        /// </summary>
        /// <param name="fileUrls"></param>
        /// <param name="mail"></param>
        public void PreperAttachFile(List<string> fileUrls,ref MailMessage mail) 
        {
            if (fileUrls == null || fileUrls.Count == 0) { return; }
            Attachment attach = null;
            foreach (string url in fileUrls)
            {
                attach=new Attachment(url,MediaTypeNames.Application.Octet);
                ContentDisposition dis = attach.ContentDisposition;
                dis.CreationDate = File.GetCreationTime(url);//创建时间
                dis.ModificationDate = File.GetLastWriteTime(url);//最后更新时间
                dis.ReadDate = File.GetLastAccessTime(url);//最后读写时间
                mail.Attachments.Add(attach);
            }
        }
        public void SendEmailBy163(EmailData text) 
        {
            SmtpClient smtp = new SmtpClient(EmailClient);
            smtp.Port = 25;
            if (EmailClientPort.HasValue)
            {
                smtp.Port = EmailClientPort.Value;
            }
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential(EmailId, EmailKey);
            MailMessage msg = new MailMessage();
            msg.From =new MailAddress( EmailId);
            msg.To.Add(text.EmailTo);
            if (text.Mailer != null)
            {
                foreach (var item in text.Mailer)
                {//抄送人
                    msg.To.Add(item);
                }
            }
            msg.Subject = text.EmailSubject;//邮件标题
            msg.SubjectEncoding = Encoding.UTF8;
            msg.Body = text.EmailBody;//邮件内容
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.High;//邮件优先级
            Guid gid=Guid.NewGuid();
            smtp.Send(msg);
            /*
             * smtp.SendAsync(msg,gid );
             现在无法开始异步操作。异步操作只能在异步处理程序或模块中开始，或在页生存期中的特定事件过程中开始。如果此异常在执行 Page 时发生，请确保 Page 标记为 <%@ Page Async="true" %>。此异常也可能表明试图调用“异步无效”方法，在 ASP.NET 请求处理内一般不支持这种方法。相反，该异步方法应该返回一个任务，而调用方应该等待该任务。
             */
        }
    }
}
