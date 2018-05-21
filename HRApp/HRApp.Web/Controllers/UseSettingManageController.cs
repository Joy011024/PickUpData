using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRApp.Web.Controllers
{
    public class UseSettingManageController : Controller
    {
        //
        // GET: /UseSettingManage/

        public ActionResult UseSettingLsit()
        {
            return View();
        }
        public ActionResult UseSettingDialog() 
        {
            return View();
        }
    }
}
