using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRApp.Web.Controllers
{
    public class RoleDataManageController : Controller
    {
        //
        // GET: /RoleDataManage/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RoleDialog() 
        {
            return View();
        }
    }
}
