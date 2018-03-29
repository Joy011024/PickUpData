using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Infrastructure.ExtService;
namespace HRApp.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Test();
            AreaRegistration.RegisterAllAreas();
            InitAppSetting.Version = DateTime.Now.ToString(Common.Data.CommonFormat.DateToHourIntFormat); 
                //DateTime.Now.ToString(Common.Data.CommonFormat.DateIntFormat);
            InitAppSetting.CodeVersion = InitAppSetting.CodeVersionFromCfg();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        void Test() 
        {
          
        }
        void ServiceLevelFactory() 
        {
        
        }
        void OrmLevelFactory() 
        {
            Dictionary<string, object> dals = new Dictionary<string, object>();
            string connString = InitAppSetting.LogicDBConnString;
            InterfaceIocHelper ioc = new InterfaceIocHelper();
            string dir = NowAppDirHelper.GetNowAppDir(AppCategory.WebApp);
            // 获取到的目录 E:\Code\DayDayStudy\PickUpData\HRApp\HRApp.Web\
            IHRApp.Infrastructure.IAppRepository appDal = ioc.IocConvert<IHRApp.Infrastructure.IAppRepository>(dir + "bin\\", "HRApp.Infrastructure.dll", "HRApp.Infrastructure", "AppRepository");
            //构造函数的参数注入  判断构造函数的参数是否需要进行注入
            //属性注入
            appDal.SqlConnString = connString;
            Dictionary<string, object> propertyVal = new Dictionary<string, object>();
            propertyVal.Add("SqlConnString", connString);

        }
    }
}