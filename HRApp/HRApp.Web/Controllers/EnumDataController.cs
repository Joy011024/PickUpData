﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRApp.Web.Controllers
{
    public class EnumDataController : Controller
    {
        //
        // GET: /EnumData/

        public ActionResult EnumList()
        {
            return View();
        }
        public ActionResult EnumDataDialog() 
        {
            return View();
        }
    }
}
