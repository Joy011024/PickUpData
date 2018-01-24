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
using System.EnterpriseServices;
namespace HRApp.Web.Controllers
{
    [MvcActionResultHelper]
    public class AppSettingManageController : BaseController
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
        public ActionResult AppDataDialog() 
        {
            return View();
        }
        [HttpPost]
        [Description("����ϵͳӦ��")]
        public JsonResult InsertAppData(NodeRequestParam param) 
        {
            JsonData json = new JsonData();
            IAppRepository appSetRepository = new AppRepository() { SqlConnString = InitAppSetting.LogicDBConnString };
            IAppDataService appSetService = new AppDataService(appSetRepository);
            json= appSetService.Add(new AppModel()
            {
                AppName=param.Name,
                AppCode=param.Code
            });
            return Json(json);
        }
    }
}