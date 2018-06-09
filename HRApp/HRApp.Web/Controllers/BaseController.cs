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
using HRApp.Model;
using HRApp.Infrastructure;
using IHRApp.Infrastructure;
using HRApp.IApplicationService;
using HRApp.ApplicationService;
using System.ComponentModel;
namespace HRApp.Web.Controllers
{
    [MvcActionResultHelper]
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        [DescriptionSort("根据父节点id查询节点列表")]
        public List<CategoryItems> QueryAppSettingList(string parentCode)
        {
            IAppSettingService appSetService = IocMvcFactoryHelper.GetInterface<IAppSettingService>();
            List<CategoryItems> items = appSetService.SelectNodeItemByParentCode(parentCode);
            return items;
        }
        [DescriptionSort("查询全部配置")]
        public List<CategoryItems> QueryAllAppSetting() 
        {
            IAppSettingService appSetService = IocMvcFactoryHelper.GetInterface<IAppSettingService>();
            List<CategoryItems> items = appSetService.QueryAll();
            return items;
        }
        public BaseController() 
        {

        }
        public List<UinMenuObjcet> QueryAllMenuData() 
        {
            IMenuService ms = IocMvcFactoryHelper.GetInterface<IMenuService>();
            List<UinMenuObjcet> menus = ms.QueryMenusByAuthor(string.Empty)
                .Select(s => s.MapObject<Menu, UinMenuObjcet>()).ToList();//这个参数后期修改为当前操作用户
            Menu uin = menus.Where(s => s.Code == "UinImageGroupService")
                .Select(s => s.MapObject<UinMenuObjcet, Menu>())
                .FirstOrDefault();
            //对于查询出的菜单列表进行用户查询限定组装
            if (uin != null)
            { //是否查询举报列表
                UinMenuObjcet ui = menus.Where(s => s.Code == "UinImageGroupService").FirstOrDefault();
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
                        copy.ParentId = copy.Id;
                        ui.Childerns.Add(copy);
                    }
                }
            }
            return menus;
        }
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
                    /*
                     “System.InvalidCastException”类型的异常在 HRApp.Web.dll 中发生，但未在用户代码中进行处理

其他信息: 无法将类型为“System.String”的对象强制转换为类型“System.Web.HttpCookie”
                     不能直接使用foreach
                     foreach (HttpCookie item in cookies)
                    {
                        sb.AppendLine(item.ConvertJson()+"\r\n");
                    }
                     * https://www.cnblogs.com/answercard/archive/2009/02/02/1382404.html
                     */
                    for (int i = 0; i < cookies.Count; i++)
                    {
                        HttpCookie hc = cookies[i];
                        sb.AppendLine(hc.ConvertJson() + "\r\n");
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
                LoggerWriter.CreateLogFile(sb.ToString(), InitAppSetting.LogPath, ELogType.DebugData);
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
