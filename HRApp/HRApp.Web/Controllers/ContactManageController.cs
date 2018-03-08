﻿using HRApp.ApplicationService;
using HRApp.IApplicationService;
using HRApp.Infrastructure;
using IHRApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRApp.Model;
namespace HRApp.Web.Controllers
{
    public class ContactManageController : Controller
    {
        //
        // GET: /ContactManage/
        [Domain.CommonData.DescriptionSort("添加联系人对话框")]
        public ActionResult AddContacterDialog()
        {
            IAppSettingRepository appSetRepository = new AppSettingRepository() { SqlConnString = InitAppSetting.LogicDBConnString };
            IAppSettingService appSetService = new AppSettingService(appSetRepository);
            ViewData["ParentNode"] = appSetService.SelectNodeItemByParentCode("LianXiRenLeiXingGuanLi");
            return View();
        }
        public JsonResult SaveContacter(SampleRequestParam param) 
        {
            Common.Data.JsonData json = new Common.Data.JsonData();
            ContactData contacter = new ContactData() 
            {
                ContactName=param.Name,
                ContactTypeId=string.IsNullOrEmpty(param.Code)?-1: int.Parse(param.Code),
                Value=param.Value,
                Desc = param.Desc
            };
            IContactDataRepository contactRep = new ContactDataRepository() { SqlConnString = InitAppSetting.LogicDBConnString };
            IContactDataService contactService = new ContactDataService(contactRep);
            json = contactService.Add(contacter);
            return Json(json);
        }
    }
}
