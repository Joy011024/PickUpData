using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DataHelp;
using Domain.CommonData;
using Infrastructure.ExtService;
namespace HRApp.Web.Controllers
{
    [MvcActionResultHelper]
    public class BaseController : Controller
    {
        //
        // GET: /Base/

       

    }
    public class MvcActionResultHelperAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {//此处理并非是客户端直接传递过来的参数，而是经过代码处理后的结果
            RequestContext request = filterContext.RequestContext;
            HttpContextBase hcb = request.HttpContext;//HTTP请求相关参数
            //是否开启日志记录
            if (InitAppSetting.OpenDebugType)
            {
                string json = hcb.ConvertJson();
                LoggerWriter.CreateLogFile(json, new AppDirHelper().GetAppDir(AppCategory.WebApp) + "/" + ELogType.DebugData, ELogType.DebugData);
            }
            IDictionary<string, object> apiParamList = filterContext.ActionParameters;//进入接口传递的参数
            RouteData rd = filterContext.RouteData;
            if (apiParamList.Count == 0)
            {
                return;
            }
            base.OnActionExecuting(filterContext);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            RequestContext request = filterContext.RequestContext;

            base.OnResultExecuting(filterContext);
        }
    }
}
