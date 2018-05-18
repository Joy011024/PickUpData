using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRApp.Model;
using HRApp.IApplicationService;
namespace HRApp.Web.Controllers
{
    public class OrganizationManageController : Controller
    {
        //
        // GET: /OrganizationManage/
        [Description("组织机构列表")]
        public ActionResult OrganzeListManage()
        {
            return View();
        }
        [Description("组织机构维护对话框")]
        public ActionResult OrganzeDialog() 
        {
            return View();
        }
        public JsonResult SaveCompany(BaseRequestParam param) 
        {
            IOrganizationService organzeService= IocMvcFactoryHelper.GetInterface<IOrganizationService>();
            Common.Data.JsonData json = organzeService.Add(new Organze() { Name = param.Name, Code = param.Code });
            return Json(json);
        }
    }
}
