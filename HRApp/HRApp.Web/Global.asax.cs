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
            IocMvcFactoryHelper.GetIocDict(true);
            InitAppSetting.AppSettingItemsInDB = RefreshAppSetting.QueryAllAppSetting(IocMvcFactoryHelper.GetInterface<IAppSettingService>());
            RefreshAppSetting.RefreshFileVersion();
            AppProcess.CallTodo();
            //AppProcess.NowProcess();
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
            propertyVal.Add(typeof(IDataFromOtherRepository).Name + ".SqlConnString", InitAppSetting.QueryUinDB);//这个是用于系统中查询其他库的数据切换操作
            #region dal层属性
            IAppRepository appDal = ioc.IocConvert<IHRApp.Infrastructure.IAppRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(AppRepository).Name);
            ioc.IocFillProperty<IAppRepository, IAppRepository>(appDal, propertyVal);
            IAppSettingRepository appSettingDal = ioc.IocConvert<IAppSettingRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(AppSettingRepository).Name);
            ioc.IocFillProperty(appSettingDal, propertyVal);
            IMenuRepository menuDal = ioc.IocConvert<IMenuRepository>(dllDir, mvc[MvcLevel.DAL].AssemblyName, mvc[MvcLevel.DAL].Namespace, typeof(MenuRepository).Name);
            ioc.IocFillProperty(menuDal, propertyVal);
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
            propertyVal.Add(typeof(IAppRepository).Name, appDal);
            propertyVal.Add(typeof(IAppSettingRepository).Name, appSettingDal);
            propertyVal.Add(typeof(IMenuRepository).Name, menuDal);
            propertyVal.Add(typeof(IContactDataRepository).Name, contacterDal);
            propertyVal.Add(typeof(IMaybeSpecialRepository).Name, maybeSpecialDal);
            propertyVal.Add(typeof(ISpecialSpellNameRepository).Name, speicalSpellDal);
            propertyVal.Add(typeof(IDataFromOtherRepository).Name, dataFormOtherDal);
            propertyVal.Add(typeof(IReportEnumDataRepository).Name, reportDal);
            propertyVal.Add(typeof(IRelyTableRepository).Name, relyDal);
            propertyVal.Add(typeof(IEmailDataRepository).Name, emailDal);
            #endregion
            #region 业务层
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
        public static void EverydayActiveEmailAccount(IEmailDataService emailService)
        {//每日激活邮件账户
            //查询邮件账户列表
            List<EmailAccount> accs = emailService.QueryEmailAccountInDB();
            string dir = InitAppSetting.LogPath;
            string file = InitAppSetting.TodayLogFileName;
            foreach (var item in accs)
            {
                string title = "[每日激活]";
                string time = DateTime.Now.ToString(Common.Data.CommonFormat.DateTimeFormat);
                try
                {
                    //使用邮件账户进行邮件发送
                    short smtp = item.Smtp;
                    //拼接发送的邮件内容
                    EmailSystemSetting ess = new EmailSystemSetting()
                    {
                        EmailAccount = item.Account,
                        EmailAuthortyCode = item.AuthortyCode,
                        EmailHost = item.SmtpHost,
                        EmailHostPort = EmailSystemSetting.GetHostPortSmtp(smtp)
                    };
                    ess.Smtp = (EnumSMTP)smtp;
                    string text = title;
                    title += " " + item.Account;
                    text += "<br/>邮件创建时间 ：" + time;
                    for (int i = 0; i < 10; i++)
                    {
                        text += string.Format("<br/> Guid{0}={1}", (i + 1), Guid.NewGuid().ToString().ToUpper());
                    }
                    AppEmailData emailData = new AppEmailData()
                    {
                        EmailCreateTime = DateTime.Now,
                        To = "158055983@qq.com",
                        Subject = "激活Email_主题163邮件",
                        From = item.Account,
                        Body = text
                    };
                    emailService.SendEmail(ess, emailData, ess.Smtp);
                    LoggerWriter.CreateLogFile(title + "[Success]" + time, dir, ELogType.EmailLog, file, true);
                }
                catch (Exception ex)
                {
                    title += ex.Message;
                    LoggerWriter.CreateLogFile(title + "[Error]" + time, dir, ELogType.EmailLog, file, true);
                }
            }
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
                appProcess.DoWork += new System.ComponentModel.DoWorkEventHandler(BackProcessDoWork);
            }
           
        }
        static void BackProcessDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                if (StopBackProcess)
                {
                    LoggerWriter.CreateLogFile("Stop Background process =" + DateTime.Now.ToString(Common.Data.CommonFormat.DateTimeFormat), InitAppSetting.LogPath, ELogType.BackgroundProcess, InitAppSetting.TodayLogFileName, true);
                    return;
                }
                BackRunNumber++;
                EveryDayDo();
                System.Threading.Thread.Sleep(1000 * 60*15);//15 分钟触发一次
            }
            
        }
        static void EveryDayDo() 
        {
            StringBuilder text = new StringBuilder();
            try
            {
                text.AppendFormat("Background process【{0}】,index={1} ,time ={2}", GetStartWebOfProcess(), BackRunNumber, DateTime.Now.ToString(Common.Data.CommonFormat.DateTimeFormat));
                LoggerWriter.CreateLogFile(text.ToString(), InitAppSetting.LogPath, ELogType.BackgroundProcess, InitAppSetting.TodayLogFileName, true);
                RefreshAppSetting.EverydayActiveEmailAccount(IocMvcFactoryHelper.GetInterface<IEmailDataService>());
                //读取xml配置
                string xmlFile = InitAppSetting.DefaultLogPath + "/XmlConfig/AppConfig.xml";
                //执行成功时间写入到xml中
                AppEmailAccount appEmail = xmlFile.GetNodeSpecialeAttribute<AppEmailAccount>(UiCfgNode, nodeCfgFormat);
                appEmail.EmailLastActiveTime = DateTime.Now.ToString(Common.Data.CommonFormat.DateTimeFormat);
                appEmail.EmailAccount = InitAppSetting.AppSettingItemsInDB[EAppSetting.SystemEmailSendBy.ToString()];
                xmlFile.UpdateXmlNode(appEmail, UiCfgNode, nodeCfgFormat);
                LoggerWriter.CreateLogFile(Newtonsoft.Json.JsonConvert.SerializeObject(appEmail), InitAppSetting.LogPath, ELogType.DebugData, InitAppSetting.TodayLogFileName, false);
            }
            catch (Exception ex)
            {
                LoggerWriter.CreateLogFile("【Error】"+ex.Message.ToString(), InitAppSetting.LogPath, ELogType.BackgroundProcess, InitAppSetting.TodayLogFileName, true);
            }
        }
       static XmlNodeDataAttribute UiCfgNode = new XmlNodeDataAttribute()
        {//字典所属上级节点信息
            NodeName = "configuration/appSettings",
            NodeKeyName = "name",
            NodeKeyValue = "AppEmailAccount"
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
            LoggerWriter.CreateLogFile(sb.ToString(),
                InitAppSetting.LogPath, ELogType.BackgroundProcess, InitAppSetting.TodayLogFileName, true);
        }
        public static string GetStartWebOfProcess() 
        {
           return Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);
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
}