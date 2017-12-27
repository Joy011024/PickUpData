using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
namespace CommonHelperEntity
{
    public class EMailHelper
    {
        /// <summary>
        /// 邮件接收人列表使用到的分割符
        /// </summary>
        public const char ReceiveListSplit =';';
        /// <summary>
        /// 是否准备好邮件地址数据
        /// </summary>
        private bool PrapertyEmailAddress = false;
        public string EmailHost;
        public int? EmailServicePort;
        /// <summary>
        /// 邮件书写时间
        /// </summary>
        public DateTime WriteTime { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 邮件发送人
        /// </summary>
        public string EMailSend { get; set; }
        /// <summary>
        /// 邮件接收人
        /// </summary>
        public string Receive { get; set; }
        /// <summary>
        ///发送人的邮件地址
        /// </summary>
        public string EmailForm { get; set; }
        /// <summary>
        /// 邮件抄送人
        /// </summary>
        public string[] OtherReceiveList { get; set; }
        /// <summary>
        /// 设置定时发送邮件的时间
        /// </summary>
        public string SetSendTime { get; set; }
        /// <summary>
        /// 邮件发送内容
        /// </summary>
        public string EMailText { get; set; }
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string EMailTheme { get; set; }
        /// <summary>
        /// 附件列表
        /// </summary>
        public List<string> AttachFiles { get; set; }
        /// <summary>
        /// 是否存在附件【在发送邮件前进行检查，确保附件是否已经上传成功】
        /// </summary>
        public bool HavaFiles = false;
        public MailMessage Mail { get; set; }
        /// <summary>
        /// 是否对邮件进行socket层加密传输
        /// </summary>
        public bool MailEnableSsl = true;
        /// <summary>
        ///是否对邮件发送者进行密码验证
        /// </summary>
        public bool MailEnablePswAuthentication = false;
        /// <summary>
        /// 使用邮件发送功能的用户名
        /// </summary>
        public string UseMailOfUserName { get; set; }
        /// <summary>
        /// 使用邮件发送功能的用户密码
        /// </summary>
        public string UseMailOfUserPassword { get; set; }
        /// <summary>
        /// 邮件头部追加的内容
        /// </summary>
        public string MailHeadAppendText { get; set; }
        /// <summary>
        /// 邮件尾部追加的内容
        /// </summary>
        public string MailFootAppendText { get; set; }
        public EMailHelper() { }
        public EMailHelper(string smtpHost, int? port, Dictionary<string, string> mailAuthority)
        {
            EmailHost = smtpHost;
            if (port.HasValue)
            {
                EmailServicePort = port.Value;
            }
            if (mailAuthority.Count != 0) 
            {//是否需要邮件授权
                MailEnablePswAuthentication = true;
                KeyValuePair<string,string> keyValue= mailAuthority.FirstOrDefault();
                //如果查找到的key和value为空值则给予异常提示
                UseMailOfUserName = keyValue.Key;
                UseMailOfUserPassword = keyValue.Value;
            }
        }
        public void PrepareMail(string receiveList,string send,string subject, string document,bool isHtmlFormat,MailPriority priority) 
        {//纯邮件
            Mail = new MailMessage();
            //进行邮件人地址规则验证
            foreach (string receive in receiveList.Split(ReceiveListSplit))
            {
                Mail.To.Add(receive);
            }
            Mail.From = new MailAddress(send);
            Mail.Subject = subject;
            Mail.Sender = new MailAddress(send);
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(MailHeadAppendText)) 
            {
                sb.AppendLine(MailHeadAppendText);
            }
            sb.AppendLine(document);
            if (!string.IsNullOrEmpty(MailFootAppendText)) 
            {
                sb.AppendLine(MailFootAppendText);
            }
            Mail.Body = sb.ToString();
            Mail.IsBodyHtml = isHtmlFormat;//正文是否是html格式
            Mail.BodyEncoding = Encoding.UTF8;//正文的编码
            Mail.Priority = priority;//邮件的优先级
            PrapertyEmailAddress = true;//【标记是否准备好邮件地址数据】
        }
        public void addAttachFile(List<string> files) 
        {
            Attachment attach;
            ContentDisposition dis;
            foreach (string item in files)
            {
                attach = new Attachment(item, MediaTypeNames.Application.Octet);
                dis = attach.ContentDisposition;
                dis.CreationDate = File.GetCreationTime(item);//附件创建日期
                dis.ModificationDate =File.GetLastWriteTime(item);//附件修改日期
                dis.ReadDate = File.GetLastAccessTime(item);//附件最后读写日期
                Mail.Attachments.Add(attach);
            }
        }
        /// <summary>
        /// 是否准备好邮件的基础数据【地址，发送人，接收人数据】
        /// </summary>
        private bool HavaPrapertyEmaiAddress
        {
            get
            {//检查邮件数据【邮件主机，邮件接收人，如果邮件发送者进行密码验证则需要验证邮件发送者和邮件授权码】
                bool isSet = true;
                if(string.IsNullOrEmpty(EmailHost))
                {//邮件主机地址为空
                    return false;
                }
                if (string.IsNullOrEmpty(Receive))
                {
                    return false;
                }
                if(MailEnablePswAuthentication&&(string.IsNullOrEmpty(UseMailOfUserName)||string.IsNullOrEmpty(UseMailOfUserPassword)))
                {
                    return false;
                }
                return isSet;
            }
        }
        public void SendMail() 
        {
            if (!PrapertyEmailAddress) 
            {//前提条件没有准备齐全，拒绝发送邮件操作
                return ;
            }
            SmtpClient client = new SmtpClient();
            client.Host = EmailHost;
            if (EmailServicePort.HasValue)
            {
                client.Port = EmailServicePort.Value;
            }
            //client.UseDefaultCredentials = false;
            client.EnableSsl = MailEnableSsl;//使用安全套接字【当使用qq发送邮件时这个安全套接字的值必须是true】
            try
            {
                if (!MailEnablePswAuthentication)
                {
                    NetworkCredential nc = new NetworkCredential(UseMailOfUserName, UseMailOfUserPassword);
                    client.Credentials = nc.GetCredential(client.Host, client.Port, "NTLM");
                }
                else
                {
                    client.Credentials = new NetworkCredential(UseMailOfUserName,UseMailOfUserPassword);// ; new NetworkCredential(MailSendUserName, MailSendUserPassword);
                }
              //  client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(Mail);

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
