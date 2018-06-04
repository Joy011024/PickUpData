using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRApp.IApplicationService;
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
        [HttpPost]
        [Description("存储")]
        public JsonResult SaveEnum(SampleRequestParam param) 
        {
            Common.Data.JsonData json = new Common.Data.JsonData();
            IEnumDataService enumBll = IocMvcFactoryHelper.GetInterface<IEnumDataService>();
            return Json(json);
        }
    }
}
