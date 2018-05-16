using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRApp.Web.Controllers
{
    public class AppAccountManageController : Controller
    {
        //
        // GET: /AppAccountManage/
        [Description("账号服务列表")]
        public ActionResult AccountList()
        {
            return View();
        }
        [Description("注册账号")]
        public ActionResult SignInAccount() 
        {
            return View();
        }
    }
}
