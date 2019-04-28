using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Infrastructure.ExtService;
using HRApp.Model;
using IHRApp.Infrastructure;
using HRApp.Infrastructure;
using HRApp.ApplicationService;
using HRApp.IApplicationService;
using Domain.CommonData;
using System.Diagnostics;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using HRApp.Web.Controllers;
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
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalNotifyHandle handle = new GlobalNotifyHandle();
            string format = "yyyy-MM-dd HH:mm:ss fff";
            string time = DateTime.Now.ToString(format);
            string path = LogPrepare.GetLogPath();
            ELogType heart = ELogType.HeartBeatLine;
            string file = LogPrepare.GetLogName(heart);
            LoggerWriter.CreateLogFile(time + "begin init Data ", path, heart,file, true);
            Domain.GlobalModel.AppRunData.InitAppData();
            time = DateTime.Now.ToString(format);
            LoggerWriter.CreateLogFile(time + "begin init Data", path,heart, file, true);
            Domain.GlobalModel.AppRunData.AppName = this.GetType().Name;
            IocMvcFactoryHelper.GetIocDict(true);
            InitAppSetting.AppSettingItemsInDB = RefreshAppSetting.QueryAllAppSetting(IocMvcFactoryHelper.GetInterface<IAppSettingService>());
            RefreshAppSetting.RefreshFileVersion();
            AppProcess.GlobalXmlManage();
            AppProcess.CallTodo();
          
        }
        
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
        public static Dictionary<string, object> GetIocDict(bool updateIoc)
        {
            if (updateIoc || propertyVal.Count == 0)
            {
                propertyVal.Clear();
                OrmIocFactory();
            }
            return propertyVal;
        }
        /// <summary>
        /// 提取接口实例化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInterface<T>() where T:class
        {
            string name = typeof(T).Name;
            return  GetIocDict(false)[name] as T;
        }
        static void OrmIocFactory()
        {
            if (propertyVal.Count > 0)
            {//存储字典非空出来 

            }
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
            propertyVal.Add(typeof(IEnumDataRepository).Name+".SqlConnString", InitAppSetting.AccountDBConnString);//账号库
            propertyVal.Add(typeof(IEnumDataService).Name + ".SqlConnString", InitAppSetting.AccountDBConnString);//账号库
            propertyVal.Add(typeof(IAppAccountRepository).Name + ".SqlConnString", InitAppSetting.AccountDBConnString);//账号库
            propertyVal.Add(typeof(IAppAccountService).Name + ".SqlConnString", InitAppSetting.AccountDBConnString);//账号库
            propertyVal.Add(typeof(IMenuRepository).Name + ".SqlConnString", InitAppSetting.AccountDBConnString);//账号库
            propertyVal.Add(typeof(IDataFromOtherRepository).Name + ".SqlConnString", InitAppSetting.QueryUinDB);//这个是用于系统中查询其他库的数据切换操作
            #region dal层属性
            #region Account DB
            IEnumDataRepository enumDal = ioc.IocConvert<IEnumDataRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(EnumDataRepository).Name);
            ioc.IocFillProperty(enumDal, propertyVal);
            IAppAccountRepository accountDal = ioc.IocConvert<IAppAccountRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(AppAccountRepository).Name);
            ioc.IocFillProperty(accountDal, propertyVal);
            IMenuRepository menuDal = ioc.IocConvert<IMenuRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(MenuRepository).Name);
            ioc.IocFillProperty(menuDal, propertyVal);
            #endregion
            #region log --all
            ILogDataRepository logDal = ioc.IocConvert<ILogDataRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(LogDataRepository).Name);
            ioc.IocFillProperty(logDal, propertyVal);
            #endregion
            IAppRepository appDal = ioc.IocConvert<IHRApp.Infrastructure.IAppRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(AppRepository).Name);
            ioc.IocFillProperty<IAppRepository, IAppRepository>(appDal, propertyVal);
            IAppSettingRepository appSettingDal = ioc.IocConvert<IAppSettingRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(AppSettingRepository).Name);
            ioc.IocFillProperty(appSettingDal, propertyVal);
            
            IOrganizationRepository organzeDal = ioc.IocConvert<IOrganizationRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(OrganizationRepository).Name);
            ioc.IocFillProperty(organzeDal, propertyVal);
            IContactDataRepository contacterDal = ioc.IocConvert<IContactDataRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(ContactDataRepository).Name);
            ioc.IocFillProperty(contacterDal, propertyVal);
            IMaybeSpecialRepository maybeSpecialDal = ioc.IocConvert<IMaybeSpecialRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(MaybeSpecialRepository).Name);
            ioc.IocFillProperty<IMaybeSpecialRepository>(maybeSpecialDal, propertyVal);
            ISpecialSpellNameRepository speicalSpellDal = ioc.IocConvert<ISpecialSpellNameRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(SpecialSpellNameRepository).Name);
            ioc.IocFillProperty<ISpecialSpellNameRepository>(speicalSpellDal, propertyVal);
            IDataFromOtherRepository dataFormOtherDal = ioc.IocConvert<IDataFromOtherRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(DataFromOtherRepository).Name);
            ioc.IocFillProperty<IDataFromOtherRepository>(dataFormOtherDal, propertyVal);
            IReportEnumDataRepository reportDal=ioc.IocConvert<IReportEnumDataRepository>(dllDir,mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(ReportEnumDataRepository).Name);
            ioc.IocFillProperty<IReportEnumDataRepository>(reportDal, propertyVal);
            IRelyTableRepository relyDal = ioc.IocConvert<IRelyTableRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(RelyTableRepository).Name);
            ioc.IocFillProperty<IRelyTableRepository>(relyDal, propertyVal);
            IEmailDataRepository emailDal = ioc.IocConvert<IEmailDataRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(EmailDataRepository).Name);
            ioc.IocFillProperty<IEmailDataRepository>(emailDal, propertyVal);
            #endregion
            #region orm中dal层实例化存储到字典中
            propertyVal.Add(typeof(ILogDataRepository).Name, logDal);
            propertyVal.Add(typeof(IEnumDataRepository).Name, enumDal);
            propertyVal.Add(typeof(IAppAccountRepository).Name, accountDal);
            propertyVal.Add(typeof(IAppRepository).Name, appDal);
            propertyVal.Add(typeof(IAppSettingRepository).Name, appSettingDal);
            propertyVal.Add(typeof(IMenuRepository).Name, menuDal);
            propertyVal.Add(typeof(IOrganizationRepository).Name, organzeDal);
            propertyVal.Add(typeof(IContactDataRepository).Name, contacterDal);
            propertyVal.Add(typeof(IMaybeSpecialRepository).Name, maybeSpecialDal);
            propertyVal.Add(typeof(ISpecialSpellNameRepository).Name, speicalSpellDal);
            propertyVal.Add(typeof(IDataFromOtherRepository).Name, dataFormOtherDal);
            propertyVal.Add(typeof(IReportEnumDataRepository).Name, reportDal);
            propertyVal.Add(typeof(IRelyTableRepository).Name, relyDal);
            propertyVal.Add(typeof(IEmailDataRepository).Name, emailDal);
            #endregion
            #region 业务层
            IEnumDataService  enumBll = ioc.IocConvert<IEnumDataService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(EnumDataService).Name);
            ioc.IocFillProperty<IEnumDataService, EnumDataService>(enumBll, propertyVal);
            propertyVal.Add(typeof(IEnumDataService).Name, enumBll);
            IAppAccountService accountBll = ioc.IocConvert<IAppAccountService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(AppAccountService).Name);
            ioc.IocFillProperty<IAppAccountService, AppAccountService>(accountBll, propertyVal);
            propertyVal.Add(typeof(IAppAccountService).Name, accountBll);
            //构造函数的参数注入  判断构造函数的参数是否需要进行注入
            IAppDataService appService = ioc.IocConvert<IAppDataService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(AppDataService).Name);
            ioc.IocFillProperty<IAppDataService, AppDataService>(appService, propertyVal);
            propertyVal.Add(typeof(IAppDataService).Name, appService);
            IAppSettingService appSetService = ioc.IocConvert<IAppSettingService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(AppSettingService).Name);
            ioc.IocFillProperty<IAppSettingService, AppSettingService>(appSetService, propertyVal); //属性和字段注入 
            propertyVal.Add(typeof(IAppSettingService).Name, appSetService);
            IMenuService menuService = ioc.IocConvert<IMenuService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(MenuService).Name);
            ioc.IocFillProperty<IMenuService, MenuService>(menuService, propertyVal);
            propertyVal.Add(typeof(IMenuService).Name, menuService);
            IOrganizationService organzeService = ioc.IocConvert<IOrganizationService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(OrganizationService).Name);
            ioc.IocFillProperty<IOrganizationService, OrganizationService>(organzeService, propertyVal);
            propertyVal.Add(typeof(IOrganizationService).Name, organzeService);
            IContactDataService contactService = ioc.IocConvert<IContactDataService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(ContactDataService).Name);
            ioc.IocFillProperty<IContactDataService, ContactDataService>(contactService, propertyVal);
            propertyVal.Add(typeof(IContactDataService).Name, contactService);
            IMaybeSpecialService maybeSpeiclaService = ioc.IocConvert<IMaybeSpecialService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(MaybeSpecialService).Name);
            ioc.IocFillProperty<IMaybeSpecialService, MaybeSpecialService>(maybeSpeiclaService, propertyVal);
            propertyVal.Add(typeof(IMaybeSpecialService).Name, maybeSpeiclaService);
            ISpecialSpellNameService specialSpellService = ioc.IocConvert<ISpecialSpellNameService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(SpecialSpellNameService).Name);
            ioc.IocFillProperty<ISpecialSpellNameService, SpecialSpellNameService>(specialSpellService, propertyVal);
            propertyVal.Add(typeof(ISpecialSpellNameService).Name, specialSpellService);
            IDataFromOtherService dataFormService = ioc.IocConvert<IDataFromOtherService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(DataFromOtherService).Name);
            ioc.IocFillProperty<IDataFromOtherService, DataFromOtherService>(dataFormService, propertyVal);
            propertyVal.Add(typeof(IDataFromOtherService).Name, dataFormService);
            IReportEnumDataService reportBll = ioc.IocConvert<IReportEnumDataService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(ReportEnumDataService).Name);
            ioc.IocFillProperty<IReportEnumDataService, ReportEnumDataService>(reportBll, propertyVal);
            propertyVal.Add(typeof(IReportEnumDataService).Name, reportBll);
            IRelyTableService relyBll = ioc.IocConvert<IRelyTableService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(RelyTableService).Name);
            ioc.IocFillProperty<IRelyTableService>(relyBll, propertyVal);
            propertyVal.Add(typeof(IRelyTableService).Name, relyBll);
            IEmailDataService emailBll = ioc.IocConvert<IEmailDataService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(EmailDataService).Name);
            ioc.IocFillProperty<IEmailDataService>(emailBll, propertyVal);
            propertyVal.Add(typeof(IEmailDataService).Name, emailBll);
            ILogDataService logBll = ioc.IocConvert<ILogDataService>(dllDir, mvc[MvcLevel.Bll].AssemblyName, mvc[MvcLevel.Bll].Namespace, typeof(LogDataService).Name);
            ioc.IocFillProperty<ILogDataService>(logBll, propertyVal);
            propertyVal.Add(typeof(ILogDataService).Name, logBll);
            #endregion
        }
        
    }
    public   class RefreshAppSetting
    {
        
        public static Dictionary<string, string> QueryAllAppSetting(IAppSettingService service)
        {
            Dictionary<string, string> app = new Dictionary<string, string>();
            foreach (var item in service.QueryAll())
            {
                app.Add(item.Code, item.ItemValue);
            }
            return app;
        }
        /// <summary>
        /// 刷新文件版本控制串
        /// </summary>
        public static void RefreshFileVersion() 
        {
            string version = Common.Data.CommonFormat.DateToHourIntFormat;
            if (InitAppSetting.AppSettingItemsInDB.ContainsKey(EAppSetting.FileVersionFormat.ToString()))
            {
                string dbVersionFormat = InitAppSetting.AppSettingItemsInDB[EAppSetting.FileVersionFormat.ToString()];
                if (!string.IsNullOrEmpty(dbVersionFormat))
                {
                    version = dbVersionFormat;
                }
            }
            InitAppSetting.Version = DateTime.Now.ToString(version);
            InitAppSetting.CodeVersion = InitAppSetting.CodeVersionFromCfg();
        }
       
        public static EmailSystemSetting GetSystemEmailAccount()
        {
            string type = InitAppSetting.AppSettingItemsInDB[EAppSetting.SMTP.ToString()];
            short tv = short.Parse(type);
            EmailSystemSetting setting = new EmailSystemSetting()
            {
                EmailHost = InitAppSetting.AppSettingItemsInDB[EAppSetting.SMTPClient.ToString()],
                EmailAuthortyCode = InitAppSetting.AppSettingItemsInDB[EAppSetting.SystemEmailSMPTAuthor.ToString()],
                EmailAccount = InitAppSetting.AppSettingItemsInDB[EAppSetting.SystemEmailSendBy.ToString()],
                EmailHostPort = EmailSystemSetting.GetHostPortSmtp(tv),
                Smtp = (EnumSMTP)tv
            };
            return setting;
        }
    }
    public class AppProcess
    {
        [DescriptionSort("后台运行次数")]
        static int BackRunNumber = 0;
        static System.ComponentModel.BackgroundWorker appProcess = new System.ComponentModel.BackgroundWorker();
        public static bool StopBackProcess;
        static bool bindProcess = false;//是否已经绑定后台进程
        static void EveryDayTrigger() 
        {
            if (!bindProcess)
            {
               // appProcess.DoWork += new System.ComponentModel.DoWorkEventHandler(BackProcessDoWork);
            }
           
        }
        static void BackProcessDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ELogType el = ELogType.BackgroundProcess;
            while (true)
            {
                string path = LogPrepare.GetLogPath();
                if (StopBackProcess)
                {
                    LoggerWriter.CreateLogFile("Stop Background process =" + DateTime.Now.ToString(Common.Data.CommonFormat.DateTimeFormat), path, el, LogPrepare.GetLogName(el), true);
                    return;
                }
                BackRunNumber++;
                EveryDayDo();
                System.Threading.Thread.Sleep(1000 * 60*15);//15 分钟触发一次
            }
            
        }
        static void EveryDayDo() 
        {
            string path = LogPrepare.GetLogPath();
            try
            {
                StringBuilder text = new StringBuilder();
                text.AppendFormat("Background process【{0}】,index={1} ,time ={2}", GetStartWebOfProcess(), BackRunNumber, DateTime.Now.ToString(Common.Data.CommonFormat.DateTimeFormat));
                ELogType el = ELogType.BackgroundProcess;
                LoggerWriter.CreateLogFile(text.ToString(), path, el, LogPrepare.GetLogName(el), true);
                DoSomeInitEventFacadeFactory dosome = new DoSomeInitEventFacadeFactory();
                dosome.ActiveEmailSmtp();
                //读取xml配置
                string xmlFile = InitAppSetting.DefaultLogPath + "/XmlConfig/AppConfig.xml";
                //执行成功时间写入到xml中
                UiCfgNode.NodeKeyValue = typeof(AppEmailAccount).Name;
                AppEmailAccount appEmail = xmlFile.GetNodeSpecialeAttribute<AppEmailAccount>(UiCfgNode, nodeCfgFormat);
                appEmail.EmailLastActiveTime = DateTime.Now.ToString(Common.Data.CommonFormat.DateTimeFormat);
                appEmail.EmailAccount = InitAppSetting.AppSettingItemsInDB[EAppSetting.SystemEmailSendBy.ToString()];
                xmlFile.UpdateXmlNode(appEmail, UiCfgNode, nodeCfgFormat);
                LoggerWriter.CreateLogFile(Newtonsoft.Json.JsonConvert.SerializeObject(appEmail), path,el, LogPrepare.GetLogName(el), false);
            }
            catch (Exception ex)
            {
                ELogType el = ELogType.BackgroundProcess;
                LoggerWriter.CreateLogFile("【Error】"+ex.Message.ToString(), path, el, LogPrepare.GetLogName(el), true);
            }
        }
       static XmlNodeDataAttribute UiCfgNode = new XmlNodeDataAttribute()
        {//字典所属上级节点信息
            NodeName = "configuration/appSettings",
            NodeKeyName = "name"
        };
        static XmlNodeDataAttribute nodeCfgFormat = new XmlNodeDataAttribute()
        {//字典中每一项节点配置项
            NodeName = "add",
            NodeKeyName = "key",
            NodeKeyValue = "value"
        };
        public static void CallTodo() 
        {
            if (!bindProcess)
            {
                EveryDayTrigger();
                bindProcess = true;
            }
            appProcess.RunWorkerAsync();
        }
        public static void StopTodo() 
        {
            BackRunNumber = 0;
            StopBackProcess = true;
        }
        public static void NowProcess() 
        {
            //获取当前进程
            string path = LogPrepare.GetLogPath();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            Process cur = Process.GetCurrentProcess();
            string pn = cur.ProcessName;//当前运行进程 w3wp/iisexpress
            sb.AppendLine("ProcessName=" + pn);
            string site =cur.Site!=null? cur.Site.Name:string.Empty;
            sb.AppendLine("Site=" + site);
            string maiche = cur.MachineName;
            sb.AppendLine("MachineName{.=local}=" + maiche);
            string window = cur.MainWindowTitle;
            sb.AppendLine("MainWindowTitle=" + window);
            string poolName = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);//应用程序池名称 HrApp/Clr4IntegratedAppPool
            sb.AppendLine("APP_POOL_ID=" + poolName);
            // string json = Newtonsoft.Json.JsonConvert.SerializeObject(cur);
            ELogType lt = ELogType.BackgroundProcess;
            LoggerWriter.CreateLogFile(sb.ToString(), path, lt, LogPrepare.GetLogName(lt), true);
        }
        public static string GetStartWebOfProcess() 
        {
           return Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);
        }
        public static void GlobalXmlManage() 
        {//全局xml配置
            string globalXml = ConfigurationManager.AppSettings["GlobalXmlName"];
            string xmlDir = HttpContext.Current.Server.MapPath("/XmlConfig/" + globalXml);
            //读取xml配置
            UiCfgNode.NodeKeyValue = typeof(GlobalSetting).Name;
            GlobalSetting global = xmlDir.GetNodeSpecialeAttribute<GlobalSetting>(UiCfgNode, nodeCfgFormat);
            InitAppSetting.Global = global;
        }
        async void AsyncExcuteLog() 
        {/*
          不能直接调用方法，否则异步无效
          * 此异步方法缺少await运算符，将以同步方式运行。请考虑使用await运算符等待非阻止的API调用，或者使用await Task.Run(...)在后台线程上执行占用大量CPU的工作
          
          */
            await Task.Run(() => KeepLog());
        }
        void KeepLog() 
        {
            string excute = DateTime.Now.ToString(" HHmmss");
            LogExt.WriteLogProcess(excute,ELogType.HeartBeatLine);//心跳线检测日志痕迹
        }
    }
    public class AppEmailAccount
    {
        public string EmailAccount { get;set;}
        public string EmailLastActiveTime { get; set; }
        public string EmailLastActiveSuccessTime { get; set; }
        public string EmailTodayActiveIsSuccess { get; set; }
        public string EmailTodayActiveFailureNumber { get; set; }
        public string ExistsNoActiveEmailAccount { get; set; }
    }
    public class TestClass 
    {
        public void TestFun() 
        {
            B b = new B();
            /*
             //如果是使用子类声明
             * 只调用子类的方法
             */
            b.ExcuteA();
            b.ExcuteAFun();
            /*
             使用父类声明
             */
            A call = new B();
            B cb = (B)call;
            cb.ExcuteA();
            cb.ExcuteAFun();
            //子类调用：只触发子类方法
            call.ExcuteA();
            call.ExcuteAFun();
            //父类调用：只调用父类
            //子类调用：new只进入父类，override只调用父类
        }
        static void CalculatePoint(int point)
        {//菲波那切数列
            int result = 0;
            int temp = 0;
            int param = 1;
            for (int i = 1; i <= point; i++)
            {
                temp = result;
                result = result + param;
                param = temp;
            }
        }
    }
    //new override 使用
    public class A
    {
        public A() 
        {
            ContructCall();
        }
        public virtual void ExcuteA() 
        {
            Console.Write("Excute A");
            //进入子函数
        }
        public virtual void ExcuteAFun() 
        {
            Console.Write("Excute A");
        }
        public virtual void ContructCall() 
        {
        
        }
    }
    public class B : A 
    {
        int temp;
        public B() 
        {
        
        }
        public override void ExcuteA() 
        {
            //执行b的事件
            Console.Write("Excute B");
        }
        public new void ExcuteAFun()
        {
            //执行b的事件
            Console.Write("Excute B");
        }
        public virtual void ContructCall() 
        {
            int y = temp;
        }
    }

    public class LogPrepare
    {
        public static string GetLogPath()
        {
            return string.Format("{0}\\{1}", InitAppSetting.LogPath , DateTime.Now.ToString(Common.Data.CommonFormat.YearMonth));
        }
        public static string GetLogName(ELogType log)
        {
            DateTime now = DateTime.Now;
            return string.Format("{0}.log", now.ToString(Common.Data.CommonFormat.DateIntFormat));
        }
    }
     
}