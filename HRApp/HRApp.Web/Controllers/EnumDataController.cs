using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRApp.IApplicationService;
using HRApp.Model;
using Infrastructure.ExtService;
using Domain.CommonData;
namespace HRApp.Web.Controllers
{
    public class EnumDataController : Controller
    {
        //
        // GET: /EnumData/

        public ActionResult EnumList()
        {
            EAppSetting app = EAppSetting.AccountComplaintsType;
            SaveEnum(new SampleRequestParam());
            return View();
        }
        public ActionResult EnumDataDialog() 
        {
            return View();
        }
        [HttpPost]
        [Description("存储")]
        public JsonResult SaveEnum(SampleRequestParam param) 
        {
            Common.Data.JsonData json = new Common.Data.JsonData() { Result=true};
            int ev = 0;
            if (!int.TryParse(param.Value, out ev))
            {
                json.Message = AppLanguage.Lang.Tip_EnumValueIsDigit;
                return Json(json);
            }
            IEnumDataService enumBll = IocMvcFactoryHelper.GetInterface<IEnumDataService>();
            json= enumBll.Add(new EnumData()
            {
                Name = param.Name,
                Code = param.Code,
                Value = ev,
                Remark = param.Desc
            });
            return Json(json);
        }
        public JsonResult ChangeMenuStatue(int id,bool isStop) 
        {
            Common.Data.JsonData json = new Common.Data.JsonData();

            return Json(json);
        }
    }
}
