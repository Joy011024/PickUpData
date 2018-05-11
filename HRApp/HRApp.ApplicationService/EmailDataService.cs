using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.IApplicationService;
using EmailHelper;
using IHRApp.Infrastructure;
using HRApp.Model;
using Domain.CommonData;
namespace HRApp.ApplicationService
{
    public class EmailDataService:IEmailDataService
    {
        public IAppSettingRepository appSettingDal;
        public IEmailDataRepository emailDataDal;
        public string SqlConnString;
        public string LogPath { get; set; }
        public EmailDataService(IAppSettingRepository appSetting,IEmailDataRepository emailDal) 
        {
            appSettingDal = appSetting;
            emailDataDal = emailDal;
        }
        public bool EveryDayActiveEmailSMTP(string emailSmtpAccount, string smtpAuthorCode, string emailHost, int? emailPort, string emailTo, string EmailFrom)
        {
            EmailService es = new EmailService(emailHost, emailSmtpAccount, smtpAuthorCode, emailPort, true);
            //检查今天是否存在已发送邮件
            //如果今天一封邮件都没有进行发送，则：进行邮件发送
            string title = "Every day active email";
            string text = this.GetType().Name;
            text += "<br/>" + DateTime.Now.ToString(Common.Data.CommonFormat.DateTimeFormat);
            //查询邮件SMTP服务类别
           
            //筛选进行邮件发送触发函数

            return true;
        }


        public bool SendEmail(EmailSystemSetting setting, AppEmailData email, EnumSMTP smtpType)
        {
            if (email.EmailCreateTime.Equals(new DateTime()))
            {
                email.EmailCreateTime = DateTime.Now;
            }
            //文件内容长度选择存储形式 1.邮件文本内容 2 邮件写入的文本相对路径
            email.BodyType = EnumEmailBodyType.Body;
            email.EmailId = Guid.NewGuid();
            if (email.Body.Length > 1024)
            {//写入到文本下
                email.BodyType = EnumEmailBodyType.TextPath;
                string emailPath = LogPath + "\\" + ELogType.EmailBody.ToString();
                string file=email.EmailId.ToString()+".log";
                LoggerWriter.CreateLogFile(email.Body, emailPath, ELogType.EmailBody, file);
                email.Body = emailPath +"\\"+ file;
            }
            //将邮件内容进行存档
            bool succ= emailDataDal.SaveWaitSendEmailData(email);
            EmailService es = new EmailService(setting.EmailHost, setting.EmailAccount, setting.EmailAuthortyCode, setting.EmailHostPort, true);
            //开始进行邮件发送
            switch (smtpType)
            {
                case EnumSMTP.QQ:
                    es.SendEmail(email.Subject, email.Body, email.From, email.From, email.To, email.Mailer.ToArray(), true, System.Net.Mail.MailPriority.High, null);
                    break;
                case EnumSMTP.NETS163:
                    es.SendEmailBy163(new EmailData() {  CreateTime=email.EmailCreateTime, EmailBody=email.Body, EmailFrom=email.From, EmailSubject=email.Subject, EmailTo=email.To});
                    break;
                default:
                    break;
            }
            return true;
        }
    }
}
