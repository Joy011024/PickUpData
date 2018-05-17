using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRApp.Web.Controllers
{
    public class CommonDialogController : Controller
    {
        //
        // GET: /CommonDialog/
        [Description("公用对话框")]
        public ActionResult SampleDataDialog()
        {
            return View();
        }

    }
}
