using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.CommonData;
using Common.Data;
using HRApp.Model;
using IHRApp.Infrastructure;
using HRApp.Infrastructure;
using HRApp.IApplicationService;
using HRApp.ApplicationService;
namespace HRApp.Web.Controllers
{
    [Description("拼音维护")]
    public class SpellNameController : Controller
    {
        //
        // GET: /SpellName/
        [Description("生僻字维护")]
        public ActionResult SpecialSpellNameDialog()
        {
            string test = "正则@表达式&*只能输入中,；:=+文和字母zhongguo1949垚";
            //CommonCallService.TextConvertSpellName(test);
            return View();
        }
        [Description("保存生僻字")]
        public JsonResult SaveSpecialSpellName(NodeRequestParam param) 
        {
            JsonData json = new JsonData();
            if (string.IsNullOrEmpty(param.Name))
            { 
           
            }
            ISpecialSpellNameRepository spellRepository = new SpecialSpellNameRepository() { SqlConnString = InitAppSetting.LogicDBConnString };
            ISpecialSpellNameService appSetService = new SpecialSpellNameService(spellRepository);
            json = appSetService.Add(new SpecialSpellName()
            {
                Name = param.Name[0],//只读取第一个字符
                Code = param.Code
            });
            return Json(json);
        }
    }
}
