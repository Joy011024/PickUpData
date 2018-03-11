using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.CommonData;
using IHRApp.Infrastructure;
using HRApp.Infrastructure;
using HRApp.IApplicationService;
using HRApp.ApplicationService;
using HRApp.Model;
namespace HRApp.Web.Controllers
{
    public class MenuController : BaseController
    {
        //
        // GET: /Menu/
        [DescriptionSort("新增")]
        public ActionResult NewNemu()
        {
            return View();
        }
        public JsonResult SaveMenu(SampleRequestParam param) 
        {
            Common.Data.JsonData json = new Common.Data.JsonData();
            if (string.IsNullOrEmpty(param.Value))
            {
                json.Success = false;
                json.Message = AppLanguage.Lang.Tip_MenuUrlIsRequired;
                return Json(json);
            }
            IMenuRepository mr = new MenuRepository() { SqlConnString = InitAppSetting.LogicDBConnString };
            IMenuService ms = new MenuService(mr);
            json = ms.Add(new Model.Menu() { Name = param.Name, Code = param.Code, Remark = param.Desc,Url=param.Value });
            return Json(json);
        }
        [DescriptionSort("显示菜单列表")]
        public ActionResult MenuList() 
        {
            return View();
        }
        public JsonResult QueryAllMenus() 
        {
            Common.Data.JsonData json = new Common.Data.JsonData();
            IMenuRepository mr = new MenuRepository() { SqlConnString = InitAppSetting.LogicDBConnString };
            IMenuService ms = new MenuService(mr);
            json = ms.QueryAllMenu();
            return Json(json);
        }
    }
}
