using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Data;
using HRApp.ApplicationService;
using HRApp.Infrastructure;
using HRApp.IApplicationService;
using IHRApp.Infrastructure;
using HRApp.Model;
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
            IAppSettingRepository appSetRepository = new AppSettingRepository() { SqlConnString = InitAppSetting.LogicDBConnString };
            IAppSettingService appSetService = new AppSettingService(appSetRepository);
            JsonData json = appSetService.Add(new CategoryItems()
            {
                ItemDesc = param.Desc,
                Code=param.Code,
                ParentCode=param.ParentCode,
                Name=param.Name,
                ItemValue=param.Value
            });
            return Json(json);
        }
    }
}