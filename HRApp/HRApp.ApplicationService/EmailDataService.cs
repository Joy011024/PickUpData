using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.IApplicationService;
using EmailHelper;
using IHRApp.Infrastructure;
namespace HRApp.ApplicationService
{
    public class EmailDataService:IEmailDataService
    {
        public IAppSettingRepository appSettingDal;
        public IEmailDataRepository emailDataDal;
        
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

            throw new NotImplementedException();
        }
    }
}
