using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRApp.Model;
using Domain.CommonData;
using HRApp.IApplicationService;
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
        [HttpPost]
        public JsonResult QueryDayLog(RequestParam param)
        {
            ILogDataService logService = IocMvcFactoryHelper.GetInterface<ILogDataService>();
            Common.Data.JsonData json = logService.QueryLogs(param);
            return Json(json);
        }
    }
}
