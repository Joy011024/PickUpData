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
    public class ReportEnumController : BaseController
    {
        //
        // GET: /ReportEnum/
        [DescriptionSort("举报对话框")]
        public ActionResult FlagReportDialog()
        {
            IAppSettingService service = IocMvcFactoryHelper.GetInterface<IAppSettingService>();
            string item=EAppSetting.ReportEnum.ToString();
            ViewData[item] = service.SelectNodeItemByParentCode(item);
            return View();
        }

    }
}
