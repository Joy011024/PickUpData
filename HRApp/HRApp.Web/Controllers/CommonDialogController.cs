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
        public ActionResult SampleNoteDataDialog() 
        {
            return View();
        }
        public ActionResult CommonValueDialog()
        { 
            return View();
        }
        public ActionResult CommonSelectDialog(string sltEnum,string id) 
        {//公用的选择框的对话框窗体
            BaseRequestParam param = new BaseRequestParam();
            //EMenuType
            return View();
        }
        public ActionResult CommonTreeDialog() 
        {
            return View();
        }
    }
}
