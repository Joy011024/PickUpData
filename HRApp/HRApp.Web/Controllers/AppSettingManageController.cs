using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Data;
namespace HRApp.Web.Controllers
{
    public class AppSettingManageController : Controller
    {
        //
        // GET: /AppSettingManage/

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult SaveAppSetting(NodeRequestParam param)
        {
            JsonData json = new JsonData();

            return Json(json);
        }
    }
}