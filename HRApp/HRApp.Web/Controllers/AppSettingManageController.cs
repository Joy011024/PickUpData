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
        [Description("�б�ҳ")]
        public ActionResult AppSettingList() 
        {
            return View();
        }
        public JsonResult QueryRootAppSetting()
        {
            Common.Data.JsonData json = new JsonData() { Result=true};
            json.Data = QueryAppSettingList(InitAppSetting.DefaultAppsettingRootCode);
            json.Success = true;
            return Json(json);
        }
        public ActionResult AppSettingDialog(int id=0)
        {
            CategoryItems item = new CategoryItems();
            IAppSettingService appService = IocMvcFactoryHelper.GetInterface<IAppSettingService>();
            if (id > 0)
            { //��ѯ���༭������
                item = appService.Get(id);
            }
            ViewData[ParamNameTemplate.AppSettingParentNode] =
                (item.Id > 0&&item.ParentCode!=InitAppSetting.DefaultAppsettingRootCode) ?
                appService.QueryNodes(item.ParentCode)
                :QueryAppSettingList(InitAppSetting.DefaultAppsettingRootCode);
            return View(item);
        }
        [HttpPost]
        public JsonResult QueryAllAppSettingData() 
        {
            JsonData json = new JsonData() { Result=true};
            try
            {
                json.Data = QueryAllAppSetting();
                json.Success = true;
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return Json(json);
        }
        public JsonResult SaveAppSetting(NodeRequestParam param)
        {
            IAppSettingService appSetService = IocMvcFactoryHelper.GetInterface<IAppSettingService>();
            JsonData json = appSetService.Add(new CategoryItems()
            {
                ItemDesc = param.Desc,
                Code=param.Code,
                ParentCode=param.ParentCode,
                Name=param.Name,
                ParentId=param.ParentId,
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
            IAppDataService appSetService = IocMvcFactoryHelper.GetInterface<IAppDataService>();
            json= appSetService.Add(new AppModel()
            {
                AppName=param.Name,
                AppCode=param.Code
            });
            return Json(json);
        }
        [Description("���ݸ��ڵ��ѯ�ڵ��б�")]
        [HttpPost]
        public JsonResult QueryNodes(string parenCode)
        {
            Common.Data.JsonData json = new JsonData();
            IAppSettingService appSetService = IocMvcFactoryHelper.GetInterface<IAppSettingService>();
            json = appSetService.SelectNodesByParent(parenCode);
            return Json(json);
        }
        [Description("���ݼ����Ĺؼ��ֲ�ѯ�����б�")]
        public JsonResult QueryNodesByIndex(string keySpell)
        {
            Common.Data.JsonData json = new JsonData() { Result=true};
            IAppSettingService appService= IocMvcFactoryHelper.GetInterface<IAppSettingService>();
            List<CategoryItems> data= appService.QueryNodes(keySpell);
            json.Data = data;
            json.Total = data.Count;
            json.Success = true;
            return Json(json);
        }
    }
}