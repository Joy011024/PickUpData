using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Net;
using CommonHelperEntity;
using Common.Data;
namespace EmailHelper
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    public class EmailService
    {
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
        public EmailService(string emailClient, string emailId, string emailKey,int? emailClientPort,bool enableSsl) 
        {
            EmailClient = emailClient;
            EmailId = emailId;
            EmailKey = emailKey;
            if (emailClientPort.HasValue) 
            {
                EmailClientPort = emailClientPort;
            }
            EnableSsl = enableSsl;
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
        public void SendEmail(string subject,string body,string emailFrom,string emailSender, string emailTo,
            string[] copySomeoneEmai,bool isHtmlFormat,MailPriority priority,List<string> fileUrls) 
        {
            if (!ValidateEmailClientParam()) 
            {
                return;
            }
            //验证邮件数据是否符合要求

            SmtpClient client = new SmtpClient() { Host =EmailClient };
            if (EmailClientPort.HasValue) 
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
                ex.AppendLog((new StringBuilder()).AppendLine(ex.ToString()).ToString(), (new CommonFormat()).DateToHourIntFormatString + FileSuffix.Log, LogPath);
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
            SmtpClient client = new SmtpClient(this.EmailClient)
            {
                Port = 0x19
            };
            if (this.EmailClientPort.HasValue)
            {
                client.Port = this.EmailClientPort.Value;
            }
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(this.EmailId, this.EmailKey);
            MailMessage message = new MailMessage
            {
                From = new MailAddress(this.EmailId)
            };
            message.To.Add(text.EmailTo);
            if (text.Mailer != null)
            {
                foreach (string str in text.Mailer)
                {
                    message.To.Add(str);
                }
            }
            message.Subject = text.EmailSubject;
            message.SubjectEncoding = Encoding.UTF8;
            message.Body = text.EmailBody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            Guid guid = Guid.NewGuid();
            client.Send(message);
        }



        

    }
}
