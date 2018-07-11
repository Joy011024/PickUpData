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
        public ILogDataRepository logDal;
        public string SqlConnString { get; set; }
        public string LogPath { get; set; }
        /// <summary>
        /// 是否进行日志写入
        /// </summary>
        bool WriteLog { get { return !string.IsNullOrEmpty(LogPath); } }
        public EmailDataService(IAppSettingRepository appSetting,IEmailDataRepository emailDal,ILogDataRepository logRepository) 
        {
            appSettingDal = appSetting;
            emailDataDal = emailDal;
            logDal = logRepository;
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
            DateTime appTime = Domain.GlobalModel.AppRunData.RunTime;//程序启动时的时间
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
            DateTime now = DateTime.Now;
            string logFile = now.ToString(Common.Data.CommonFormat.DateIntFormat) + ".log";
            string mailTo = email.To+";";
            if (email.Mailer != null)
            {
                mailTo += string.Join(";", email.Mailer);
            }
            try
            {
                //将邮件内容进行存档
                bool succ = emailDataDal.SaveWaitSendEmailData(email);
                logDal.WriteLog(ELogType.EmailLog, string.Format("email data into db ,email Account=【{0}】",email.From)
                    , ELogType.EmailLog.ToString(), succ);
                //判断是否需要进行此刻邮件发送
                if (email.SendTime.HasValue && email.SendTime.Value > DateTime.Now)
                {//定时发送
                    logDal.WriteLog(ELogType.EmailBody,
                    string.Format("Save email to DB:【{0}】", mailTo)
                , ELogType.EmailBody.ToString(), true);
                    return true;
                }
                EmailService es = new EmailService(setting.EmailHost, setting.EmailAccount, setting.EmailAuthortyCode, setting.EmailHostPort, true) { LogPath=LogPath};
                //开始进行邮件发送
                switch (smtpType)
                {
                    case EnumSMTP.QQ:
                        es.SendEmail(email.Subject, email.Body, email.From, email.From, email.To, 
                            (email.Mailer == null ? null : email.Mailer.ToArray()),
                            true, System.Net.Mail.MailPriority.High, null);
                        break;
                    case EnumSMTP.NETS163:
                        es.SendEmailBy163(new EmailData() { CreateTime = email.EmailCreateTime, EmailBody = email.Body, EmailFrom = email.From, EmailSubject = email.Subject, EmailTo = email.To,Mailer=email.Mailer });
                        break;
                    default:
                        break;
                }
                logDal.WriteLog(ELogType.EmailBody,
                    string.Format("Send email to user:【{0}】" ,mailTo)
                , ELogType.EmailBody.ToString(), true);
                //进行日志存储
                if (WriteLog)
                {
                    LoggerWriter.CreateLogFile(string.Format("send email 【Success】-{0} - {1}" , now.ToString()), 
                        LogPath, ELogType.EmailLog, logFile, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                logDal.WriteLog(ELogType.EmailBody,
                    string.Format("Send email to user:【{0}】\r\n error msg=【{1}】", mailTo, msg)
                    ,ELogType.EmailBody.ToString(), false);
                if (WriteLog)
                    LoggerWriter.CreateLogFile(string.Format("send email 【Error】 -{0}- msg: {}", now.ToString(), msg),
                    LogPath, ELogType.EmailLog, logFile, true);
                return false;  
            }
        }


        public List<EmailAccount> QueryEmailAccountInDB()
        {
            DateTime appTime = Domain.GlobalModel.AppRunData.RunTime;//程序启动时的时间
            List<EmailAccount> account = new List<EmailAccount>();
            foreach (var item in emailDataDal.QueryEmailAccounts())
            {
                EmailAccount acc = item;
                account.Add(acc);
            }
            return account;
        }
    }
}
