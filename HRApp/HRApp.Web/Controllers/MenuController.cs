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
        public JsonResult SaveMenu(SampleRequestParam param) 
        {
            Common.Data.JsonData json = new Common.Data.JsonData();
            if (string.IsNullOrEmpty(param.Value))
            {
                json.Success = false;
                json.Message = AppLanguage.Lang.Tip_MenuUrlIsRequired;
                return Json(json);
            }
            IMenuService ms = IocMvcFactoryHelper.GetInterface<IMenuService>();
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
            Common.Data.JsonData json = new Common.Data.JsonData() { Result=true};
            IMenuService ms = IocMvcFactoryHelper.GetInterface<IMenuService>();
            List<UinMenuObjcet> menus = ms.QueryMenusByAuthor(string.Empty)
                .Select(s=>s.MapObject<Menu,UinMenuObjcet>()).ToList();//这个参数后期修改为当前操作用户
            Menu uin = menus.Where(s => s.Code == "UinImageGroupService")
                .Select(s => s.MapObject<UinMenuObjcet, Menu>())
                .FirstOrDefault();
            //对于查询出的菜单列表进行用户查询限定组装
            if (uin != null)
            { //是否查询举报列表
                UinMenuObjcet ui= menus.Where(s => s.Code == "UinImageGroupService").FirstOrDefault();
                ui.Childerns = new List<Menu>();
                IAppSettingService appSettingService = IocMvcFactoryHelper.GetInterface<IAppSettingService>();
                List<CategoryItems> reports = appSettingService.SelectNodeItemByParentCode(EAppSetting.ReportEnum.ToString());//查询举报分类集合
                //将该菜单变为子节点菜单
                if (reports.Count > 0)
                {
                    UinMenuObjcet first = uin.MapObject<Menu, UinMenuObjcet>();
                    first.ParentCode = first.Code;
                    ui.Childerns.Add(first);
                    foreach (CategoryItems item in reports)
                    {
                        UinMenuObjcet copy = uin.MapObject<Menu, UinMenuObjcet>();//进行一个实体Copy,避免引用类型更改集合中数据项
                        copy.Url = copy.Url + "?" + EAppSetting.ReportEnum.ToString() + "=" + item.ItemValue;
                        copy.Name += " - " + item.Name;
                        copy.IsChild = true;
                        copy.ParentCode = uin.Code;
                        copy.Code = uin.Code + "." + item.Code.ToString();
                        copy.ParetnId = copy.Id;
                        ui.Childerns.Add(copy);
                    }
                }
            }
            json.Data = menus;
            json.Success = true;
            return Json(json);
        }
    }
}
