using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Infrastructure.ExtService;
using IHRApp.Infrastructure;
using HRApp.Infrastructure;
using HRApp.ApplicationService;
namespace HRApp.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        static Dictionary<string, object> dals = new Dictionary<string, object>();
        protected void Application_Start()
        {
            OrmLevelFactory();
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
        enum MvcLevel
        {
            DAL=1,
            Bll=2
        }
        class AssemblyData
        {
            public string AssemblyName { get; set; }
            public string Namespace { get; set; }
        }
        static void OrmLevelFactory() 
        {
            string connString = InitAppSetting.LogicDBConnString;
            InterfaceIocHelper ioc = new InterfaceIocHelper();
            string dir = NowAppDirHelper.GetNowAppDir(AppCategory.WebApp);
            // 获取到的目录 E:\Code\DayDayStudy\PickUpData\HRApp\HRApp.Web\
            string dllDir=dir + "bin\\";
            Dictionary<MvcLevel, AssemblyData> mvc = new Dictionary<MvcLevel, AssemblyData>();
            mvc.Add(MvcLevel.DAL,new AssemblyData(){ AssemblyName= "HRApp.Infrastructure.dll",Namespace="HRApp.Infrastructure"});
            mvc.Add(MvcLevel.Bll, new AssemblyData() { AssemblyName = "HRApp.ApplicationService.dll", Namespace = "HRApp.ApplicationService" });
            IAppRepository appDal = ioc.IocConvert<IHRApp.Infrastructure.IAppRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(AppRepository).Name);
            //构造函数的参数注入  判断构造函数的参数是否需要进行注入
            //属性注入
            //appDal.SqlConnString = connString;
            Dictionary<string, object> propertyVal = new Dictionary<string, object>();
            propertyVal.Add("SqlConnString", connString);
            ioc.IocFillProperty(appDal, propertyVal);
            propertyVal.Add(typeof(IAppRepository).Name, appDal);
            IApplicationService.IAppSettingService appSetService = ioc.IocConvert<IApplicationService.IAppSettingService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(AppSettingService).Name);

        }
    }
}