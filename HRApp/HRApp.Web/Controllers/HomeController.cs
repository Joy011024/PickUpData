using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRApp.Model;
using Infrastructure.MsSqlService.SqlHelper;
using EmailHelper;
using Infrastructure.ExtService;
using Domain.CommonData;
namespace HRApp.Web.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            TestEmail();
            return View();
        }
        public ActionResult UILayout() 
        {
            return View();
        }
        void TestEmail() 
        {
            string text = "使用代码进行邮箱测试功能";
            text += "<br/>邮件创建时间 ：" + DateTime.Now.ToString(Common.Data.CommonFormat.DateTimeFormat);
            for (int i = 0; i < 10; i++)
            {
                text += "<br/> Guid" + (i+1) + "= " + Guid.NewGuid().ToString().ToUpper();
            }
            string logDir = new AppDirHelper().GetAppDir(AppCategory.WebApp);
            string day = DateTime.Now.ToString(Common.Data.CommonFormat.DateIntFormat) + ".log"; 
            try 
            {
                Dictionary<string, string> emailSetting = InitAppSetting.AppSettingItemsInDB;
                string type = emailSetting[EAppSetting.SMTP.ToString()];
                string sendBy = emailSetting[EAppSetting.SystemEmailSendBy.ToString()];
                string authorCode = emailSetting[EAppSetting.SystemEmailSMPTAuthor.ToString()];
                string smtpClient = emailSetting[EAppSetting.SMTPClient.ToString()];
                EmailService es = new EmailService(smtpClient, sendBy, authorCode, 25, true);
                EmailData email = new EmailData()
                {
                    EmailTo = "158055983@qq.com",
                    EmailSubject = "主题_测试163邮件",
                    EmailBody = text,
                    CreateTime = DateTime.Now,
                    EmailFrom = sendBy
                };
                es.SendEmailBy163(email);
                LoggerWriter.CreateLogFile("进行163发送邮件", logDir, ELogType.EmailLog, day);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                LoggerWriter.CreateLogFile(msg, logDir, ELogType.EmailLog, day);
            }
        }
    }
}
