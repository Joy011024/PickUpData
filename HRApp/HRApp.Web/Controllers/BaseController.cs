using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DataHelp;
using Domain.CommonData;
using Infrastructure.ExtService;
using Newtonsoft.Json;
using System.Text;
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
                StringBuilder sb = new StringBuilder();
                HttpRequestBase req = hcb.Request;//包含请求头相关信息的实体
                string webBrowser = req.UserAgent;
                sb.AppendLine("UserAgent=\t"+webBrowser);
                string[] userLanguage = req.UserLanguages;//用户使用的语言
                sb.AppendLine("UserLanguages =\t" + string.Join("|", userLanguage));
                string url= req.Url.ToString();
                sb.AppendLine("Url=\t" + url);
                string method= req.RequestType;//HTTP请求形式
                sb.AppendLine("RequestType=\t" + method);
                string httpMethod= req.HttpMethod;
                sb.AppendLine("HttpMethod=\t" + httpMethod);
                //req.Form //请求的表单
                //req.Headers //请求头
                HttpCookieCollection cookies = req.Cookies;//请求的cookie
                if (cookies.Count > 0)
                {
                    sb.AppendLine("Cookies=");
                    foreach (HttpCookie item in cookies)
                    {
                        sb.AppendLine(item.ConvertJson()+"\r\n");
                    }
                }
                string mimeType = req.ContentType;//文件传输类型
                sb.AppendLine("ContentType=\t" + mimeType);
                System.Text.Encoding userEncoding= req.ContentEncoding;
                sb.AppendLine("ContentEncoding=\t" + userEncoding.ToString());
                HttpBrowserCapabilitiesBase browserInfo = req.Browser;
                sb.AppendLine("Browser=\t" + browserInfo.Browser + "\t" );
                for (int i = 0; i < browserInfo.Browsers.Count; i++)
                {
                    sb.Append(browserInfo.Browsers[i].ToString()+"\t");
                }
                sb.AppendLine("\r\nMobileDeviceModel=\t" + browserInfo.MobileDeviceModel);
                sb.AppendLine("Platform=\t"+browserInfo.Platform);
                string[] browserSupperMimeType = req.AcceptTypes;//HTTP预返回前端支持的文件格式
                sb.AppendLine("\nAcceptTypes=\t" + string.Join(" ", browserSupperMimeType));
                LoggerWriter.CreateLogFile(sb.ToString(), new AppDirHelper().GetAppDir(AppCategory.WebApp) + "/" + ELogType.DebugData, ELogType.DebugData);
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
