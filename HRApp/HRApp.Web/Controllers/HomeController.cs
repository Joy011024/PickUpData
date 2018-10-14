using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Infrastructure.MsSqlService.SqlHelper;
using EmailHelper;
using Infrastructure.ExtService;
using Domain.CommonData;
using HRApp.Model;
using HRApp.IApplicationService;
namespace HRApp.Web.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                RouteValueDictionary route = RouteData.Values;
                if (!route.ContainsKey("id"))
                {
                    string tocken = new TockenHelper().GenerateTocken();
                    route.Add("id", tocken);
                }
                ViewData["id"] = route["id"];
            }
            else
            {
                //检测是否过期
                ViewData["id"] = id;
            }
            return View();
        }
        public ActionResult UILayout() 
        {
            return View();
        }
        void TestEmail() 
        {
            string text = "使用代码进行邮箱测试功能";
            string time = DateTime.Now.ToString(Common.Data.CommonFormat.DateTimeFormat);
            text += "<br/>邮件创建时间 ：" + time;
            for (int i = 0; i < 10; i++)
            {
                text += string.Format("<br/> Guid{0}={1}", (i+1) , Guid.NewGuid().ToString().ToUpper());
            }
            string logDir = InitAppSetting.LogPath;
            string day = DateTime.Now.ToString(Common.Data.CommonFormat.DateIntFormat) + ".log"; 
            try 
            {
                Dictionary<string, string> emailSetting = InitAppSetting.AppSettingItemsInDB;
                string type = emailSetting[EAppSetting.SMTP.ToString()];
                string sendBy = emailSetting[EAppSetting.SystemEmailSendBy.ToString()];
                string authorCode = emailSetting[EAppSetting.SystemEmailSMPTAuthor.ToString()];
                string smtpClient = emailSetting[EAppSetting.SMTPClient.ToString()];
                EnumSMTP smtp;
                Enum.TryParse(type, out smtp);
                AppEmailData emailData = new AppEmailData()
                {
                    EmailCreateTime = DateTime.Now,
                    To = "158055983@qq.com",
                    Subject = "HrApp主题_测试163邮件",
                    From = sendBy,
                    Body = text
                };
                EmailSystemSetting setting = new Model.EmailSystemSetting()
                {
                    EmailHostPort =smtp==EnumSMTP.NETS163? 25:587,
                    EmailAuthortyCode = authorCode,
                    EmailAccount = sendBy,
                    EmailHost = smtpClient
                };
                
                #region 直接发送，不存储【测试可用】
                /*
                EmailData email = new EmailData()
                {
                    EmailTo = "158055983@qq.com",
                    EmailSubject = "主题_测试163邮件",
                    EmailBody = text,
                    CreateTime = DateTime.Now,
                    EmailFrom = sendBy
                };
                EmailService es = new EmailService(smtpClient, sendBy, authorCode, 25, true);
                es.SendEmailBy163(email);
                 * */
                #endregion
                IEmailDataService eds = IocMvcFactoryHelper.GetInterface<IEmailDataService>();
                eds.SendEmail(setting, emailData, smtp);
                LoggerWriter.CreateLogFile(time + "\t[OK]进行163发送邮件", logDir, ELogType.EmailLog, day, true);
            }
            catch (Exception ex)
            {
                string msg = time + "\t[ Error]" + ex.Message;
                LoggerWriter.CreateLogFile(msg, logDir, ELogType.EmailLog, day,true);
            }
        }
        public void Test()
        {
            long l = 0xE0 | 0x0D;
            string result= Convert.ToString(l, 16);
        }

        public ActionResult TreeTemplateView() 
        {
            return View();
        }
    }
}
