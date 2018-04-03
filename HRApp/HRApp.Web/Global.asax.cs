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
using HRApp.IApplicationService;
namespace HRApp.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        static Dictionary<string, object> dals = new Dictionary<string, object>();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            InitAppSetting.Version = DateTime.Now.ToString(Common.Data.CommonFormat.DateToHourIntFormat); 
                //DateTime.Now.ToString(Common.Data.CommonFormat.DateIntFormat);
            InitAppSetting.CodeVersion = InitAppSetting.CodeVersionFromCfg();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            IocMvcFactoryHelper mvc = new IocMvcFactoryHelper();
            mvc.GetIocDict(true);
        }
       
        public class IocMvcFactoryHelper
        {
            enum MvcLevel
            {
                DAL = 1,
                Bll = 2
            }
            class AssemblyData
            {
                public string AssemblyName { get; set; }
                public string Namespace { get; set; }
            }
            static Dictionary<string, object> propertyVal = new Dictionary<string, object>();
            /// <summary>
            /// 获取ioc注入实例
            /// </summary>
            /// <param name="updateIoc">是否更新ioc注入</param>
            /// <returns></returns>
            public Dictionary<string, object> GetIocDict(bool updateIoc) 
            {
                if (updateIoc||propertyVal.Count == 0)
                {
                    propertyVal.Clear();
                    OrmIocFactory();
                }
                return propertyVal;
            }
            static void OrmIocFactory()
            {
                string connString = InitAppSetting.LogicDBConnString;
                InterfaceIocHelper ioc = new InterfaceIocHelper();
                string dir = NowAppDirHelper.GetNowAppDir(AppCategory.WebApp);
                // 获取到的目录 E:\Code\DayDayStudy\PickUpData\HRApp\HRApp.Web\
                string dllDir = dir + "bin\\";
                #region dll路径配置
                Dictionary<MvcLevel, AssemblyData> mvc = new Dictionary<MvcLevel, AssemblyData>();
                mvc.Add(MvcLevel.DAL, new AssemblyData() { AssemblyName = "HRApp.Infrastructure.dll", Namespace = "HRApp.Infrastructure" });
                mvc.Add(MvcLevel.Bll, new AssemblyData() { AssemblyName = "HRApp.ApplicationService.dll", Namespace = "HRApp.ApplicationService" });
                #endregion
                propertyVal.Add("SqlConnString", connString);
                #region dal层属性
                IAppRepository appDal = ioc.IocConvert<IHRApp.Infrastructure.IAppRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(AppRepository).Name);
                ioc.IocFillProperty<IAppRepository, IAppRepository>(appDal, propertyVal);
                IAppSettingRepository appSettingDal = ioc.IocConvert<IHRApp.Infrastructure.IAppSettingRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(AppSettingRepository).Name);
                ioc.IocFillProperty(appSettingDal, propertyVal);
                #endregion
                #region orm中dal层实例化存储到字典中
                propertyVal.Add(typeof(IAppRepository).Name, appDal);
                propertyVal.Add(typeof(IAppSettingRepository).Name, appSettingDal);
                #endregion
                #region 业务层 
                //构造函数的参数注入  判断构造函数的参数是否需要进行注入
               
                IAppSettingService appSetService = ioc.IocConvert<IAppSettingService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(AppSettingService).Name);
                ioc.IocFillProperty<IAppSettingService, AppSettingService>(appSetService, propertyVal); //属性和字段注入 
                propertyVal.Add(typeof(IAppSettingService).Name, appSetService);
                appSetService.QueryWhere(new Model.CategoryItems());
                #endregion
            }
        }
    }
}