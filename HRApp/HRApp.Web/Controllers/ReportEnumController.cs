using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.CommonData;
using HRApp.IApplicationService;
using HRApp.Model;
namespace HRApp.Web.Controllers
{
    public class ReportEnumController : MVCMediatorControl
    {
        //
        // GET: /ReportEnum/
        [DescriptionSort("举报对话框 usingParentWinSelfTool=true使用父窗体自定义工具栏")]
        public ActionResult FlagReportDialog(bool usingParentWinSelfTool,Guid? optionId,string table)
        {
            IAppSettingService service = IocMvcFactoryHelper.GetInterface<IAppSettingService>();
            string item=EAppSetting.ReportEnum.ToString();
            ViewData[item] = service.SelectNodeItemByParentCode(item);
            ViewData["usingParentWinSelfTool"] = usingParentWinSelfTool;
            ViewData["optionId"] = optionId;
            ViewData["optionTable"] = table;
            return View();
        }

    }
}
