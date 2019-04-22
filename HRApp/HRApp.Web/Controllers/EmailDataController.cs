using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRApp.Model;
using HRApp.IApplicationService;
namespace HRApp.Web.Controllers
{
    public class EmailDataController : MVCMediatorControl
    {
        //
        // GET: /EmailData/

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult CallEmailService(SendEmailData email) 
        {
            Common.Data.JsonData json = new Common.Data.JsonData() { Result=true};
            //查询邮件账户
            EmailSystemSetting setting = RefreshAppSetting.GetSystemEmailAccount();
            IEmailDataService es = IocMvcFactoryHelper.GetInterface<IEmailDataService>();
            es.LogPath = InitAppSetting.LogPath;
            AppEmailData data = new AppEmailData()
            {
                Body = email.Body,
                From = setting.EmailAccount,
                Mailer = email.Mailer,
                To = email.To,
                Subject=email.Subject
            };
            if (!string.IsNullOrEmpty(email.SendTime))
            {
                data.SendTime = Convert.ToDateTime(email.SendTime);
            }
            json.Success= es.SendEmail(setting, data, setting.Smtp);
            json.AttachData = email;
            return Json(json);
        }
        public ActionResult SendEmailDialog()
        {
            return View();
        }
    }
}
