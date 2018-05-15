using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRApp.Model;
using HRApp.IApplicationService;
namespace HRApp.Web.Controllers
{
    public class EmailDataController : BaseController
    {
        //
        // GET: /EmailData/

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult CallEmailService(SendEmailData email) 
        {
            Common.Data.JsonData json = new Common.Data.JsonData();
            //查询邮件账户
            EmailSystemSetting setting=new EmailSystemSetting();
            IEmailDataService es = IocMvcFactoryHelper.GetInterface<IEmailDataService>();
            //es.SendEmail(setting,email)
            json.AttachData = email;
            return Json(json);
        }
        public ActionResult SendEmailDialog()
        {
            return View();
        }
    }
}
