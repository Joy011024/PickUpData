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
using Infrastructure.ExtService;
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
        public JsonResult SaveMenu(EnumRequestParam param) 
        {
            Common.Data.JsonData json = new Common.Data.JsonData() { Result=true};
            IMenuService ms = IocMvcFactoryHelper.GetInterface<IMenuService>();
            Menu m = new Model.Menu()
            {
                Name = param.Name,
                Code = param.Code,
                Remark = param.Desc,
                Url = param.Value,
                MenuType = (short)EMenuType.MenuNode.GetHashCode()
            };
            string et = param.EnumMember;
            if (string.IsNullOrEmpty(et))
            {
                m.MenuType = (short)EMenuType.MenuRoot.GetHashCode();
            }
            else 
            {
                //提取转换
                EMenuType t;
                if (!Enum.TryParse(et, out t)) 
                {
                    json.Message = AppLanguage.Lang.Tip_MenuTypeValidInMenu;
                    return Json(json);
                }
                m.MenuType = (short)t.GetHashCode();
            }
            if (m.MenuType==(short)EMenuType.MenuNode.GetHashCode()&& string.IsNullOrEmpty(param.Value))
            {
                json.Success = false;
                json.Message = AppLanguage.Lang.Tip_MenuUrlIsRequired;
                return Json(json);
            }
            json = ms.Add(m);
            return Json(json);
        }
        [DescriptionSort("显示菜单列表")]
        public ActionResult MenuList() 
        {
            Common.Data.JsonData json = new Common.Data.JsonData();
            List<UinMenuObjcet> menu = QueryAllMenuData();
            json.Data = menu;
            json.Total = menu.Count;
            return View(json);
        }
        public JsonResult QueryAllMenus() 
        {
            Common.Data.JsonData json = new Common.Data.JsonData() { Result=true};
            json.Data = QueryAllMenuData();
            json.Success = true;
            return Json(json);
        }
        [HttpPost]
        [DescriptionSort("修改菜单的类型")]
        public JsonResult ChangeMenuType(int id,int type) 
        {
            Common.Data.JsonData json = new Common.Data.JsonData() { Result=true};
            IMenuService menuService = IocMvcFactoryHelper.GetInterface<IMenuService>();
            try
            {
                json.Success = menuService.ChangeMenuType(id, type);
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return Json(json);
        }
        [DescriptionSort("更改状态")]
        public JsonResult ChangeMenuStatue(int id, bool operate)
        {
            Common.Data.JsonData json = new Common.Data.JsonData() { Result = true };
            IMenuService menuService = IocMvcFactoryHelper.GetInterface<IMenuService>();
            try
            {
                json.Success = menuService.ChangeMenuStatue(id, operate);
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return Json(json);
        }
    }
}
