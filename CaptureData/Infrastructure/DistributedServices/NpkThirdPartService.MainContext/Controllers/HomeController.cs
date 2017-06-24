using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Data;
using Domain.RequestParam;
namespace NpkThirdPartService.MainContext.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult NewNPK(NPKRequestData param) 
        {
            JsonData json = new JsonData();
            return Json(json);
        }
        public JsonResult SubmitNpkFile() 
        {
            HttpFileCollectionBase files = Request.Files;//查看是否存在被提交的文件
            JsonData json = new JsonData();
            return Json(json);
        }
    }
}
