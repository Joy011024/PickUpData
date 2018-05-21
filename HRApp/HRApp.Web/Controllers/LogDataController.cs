using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRApp.Web.Controllers
{
    public class LogDataController : Controller
    {
        //
        // GET: /LogData/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LogDataList() 
        {
            return View();
        }
    }
}
