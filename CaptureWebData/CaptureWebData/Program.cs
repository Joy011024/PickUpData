using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using EmailHelper;
using Domain.CommonData;
using CaptureManage.AppWin;
using Infrastructure.ExtService;
using System.Text;
using Domain.CommonData;
namespace CaptureWebData
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Test();
           // Application.Run(new Main());
            Application.Run(new TecentDataFrm());
        }
        static void Test() 
        {
            DataLink dl = new DataLink();
            StringBuilder tip = new StringBuilder();
            tip.AppendLine("time:\t" + DateTime.Now.ToString(SystemConfig.DateTimeFormat));
            tip.AppendLine("Guid:\t" + Guid.NewGuid());
            dl.SendDataToOtherPlatform(LanguageItem.Tip_PickUpErrorlockAccount, tip.ToString());//需要知道当前在进行采集的账户

            PickUpTianMaoHtml tm = new PickUpTianMaoHtml();
            string dir= new AppDirHelper().GetAppDir(AppCategory.WinApp);
            tm.DoHtmlFileAnalysis(dir + @"\HttpResponse\list.tmall.com\HttpResponse");
        }
        
    }
    public class SystemConfig
    {
        static string redisIp;
        public static string RedisIp 
        {
            get 
            {
                if (string.IsNullOrEmpty(redisIp))
                {
                    redisIp = ConfigurationManager.AppSettings["RedisIp"];
                }
                return redisIp;
            }
        }

        static int redisPort;
        public static int RedisPort
        {
            get
            {
                if (redisPort==0)
                {
                    string port = ConfigurationManager.AppSettings["RedisPort"];
                    int.TryParse(port, out  redisPort);
                    if (redisPort == 0)
                    {
                        redisPort = 6379;
                    }
                }
                return redisPort;
            }
        }

        static string redisPsw;
        public static string RedisPsw
        {
            get
            {
                if (string.IsNullOrEmpty(redisPsw))
                {
                    redisPsw = ConfigurationManager.AppSettings["RedisPsw"];
                   
                }
                return redisPsw;
            }
        }
        static bool useRedis;
        /// <summary>
        /// 启用Redis缓存存储功能
        /// </summary>
        public static bool OpenRedis
        {
            get 
            {
                useRedis = ConfigurationManager.AppSettings["UsingRedis"] == "true";
                return useRedis;
            }
        }
        static string cacheReleativeDir;
        /// <summary>
        /// redis缓存项的文件的相对路径
        /// </summary>
        public static string RedisCacheFromFileReleative
        {
            get 
            {
                if(string.IsNullOrEmpty(cacheReleativeDir))
                    cacheReleativeDir = ConfigurationManager.AppSettings["RedisCacheReleativeFile"];
                return cacheReleativeDir;
            }
        }
        static bool? openAutoQuertyDBTotal;
        /// <summary>
        /// 开启自动查询数据库中采集的数据量
        /// </summary>
        public static bool OpenAutoQuertyDBTotal
        {
            get 
            {
                if (openAutoQuertyDBTotal.HasValue)
                {
                    string cfg = ConfigurationManager.AppSettings["OpenAutoQuertyDBTotal"];
                    openAutoQuertyDBTotal = cfg == "true";
                }
                return openAutoQuertyDBTotal.HasValue;
            }
        }
        static string iisCode501;
        public static string IIS501 
        {
            get 
            {
                if (string.IsNullOrEmpty(iisCode501))
                    iisCode501 = ConfigurationManager.AppSettings["IIS501"];
                return iisCode501;
            }
        }

        #region smtp邮件服务
        public static string EmailId { get { return ConfigurationManager.AppSettings["EmailId"]; } }
        public static string EmailKey { get { return ConfigurationManager.AppSettings["EmailKey"]; } }
        public static string EmailClient { get { return ConfigurationManager.AppSettings["SMTPClient"]; } }
        public static bool EmailEnableSsl { get { return ConfigurationManager.AppSettings["enableSsl"]=="true"; } }
        public static int? EmailClientPort 
        {
            get 
            {
                int? port=null;
                string ps = ConfigurationManager.AppSettings["emailClientPort"];
                if (!string.IsNullOrEmpty(ps))
                { 
                    int p=0;
                    int.TryParse(ps, out p);
                    if (p > 0) port = p;
                }
                return port;
            }
        }
        public static string DefaultEmailTo 
        {
            get 
            {
                string to = ConfigurationManager.AppSettings["EmailToUser"];
                return to;
            }
        }
        #endregion
        /// <summary>
        /// 日期格式 精确到毫秒
        /// </summary>
        public static string DateTimeFormat 
        {
            get 
            {
                return ConfigurationManager.AppSettings["DateTimeFormat"];
            }
        }
        static string captureWebDataWinAssembly;
        /// <summary>
        /// 采集网络数据窗体的程序集
        /// </summary>
        public static string CaptureWebDataWinAssembly
        {
            get 
            {
                if (string.IsNullOrEmpty(captureWebDataWinAssembly))
                    captureWebDataWinAssembly = ConfigurationManager.AppSettings["CaptureWebDataWinAssembly"];
                return captureWebDataWinAssembly;
            }
        }
        
        static string usingWebConfigDir;
        /// <summary>
        /// 日志路径采用配置文件中路径
        /// </summary>
        public static bool LogDirIsFromWebConfig
        {
            get 
            {
                if (string.IsNullOrEmpty(usingWebConfigDir))
                {
                    usingWebConfigDir = ConfigurationManager.AppSettings["LogDirIsFromWebConfig"];
                }
                usingWebConfigDir = string.IsNullOrEmpty(usingWebConfigDir) ? "true" : usingWebConfigDir;
                return usingWebConfigDir == "true";
            }
        }
        /// <summary>
        /// 配置文件中设置的日志路径
        /// </summary>
        static string LogDir
        {
            get
            {
                return ConfigurationManager.AppSettings["LogDir"];
            }
        }
        static string exeDir;
        /// <summary>
        /// 当前程序的路径
        /// </summary>
        public static string ExeDir
        {
            get
            {
                if (LogDirIsFromWebConfig)
                {
                    exeDir = LogDir;
                }
                if (string.IsNullOrEmpty(exeDir))
                {

                    exeDir = new Infrastructure.ExtService.AppDirHelper().GetAppDir(Infrastructure.ExtService.AppCategory.WinApp);
                }
                return exeDir;
            }
        }
        static string redisValueIsJsonFormat;
        public static bool RedisValueIsJsonFormat
        {
            get 
            {
                if (string.IsNullOrEmpty(redisValueIsJsonFormat))
                {
                    redisValueIsJsonFormat = ConfigurationManager.AppSettings["RedisValueIsJsonFormat"];
                }
                return redisValueIsJsonFormat == "true";
            }
        }
        static string cfgFileExistsIsDoReplace;
        public static bool CfgFileExistsIsDoReplace 
        {
            get 
            {
                if (string.IsNullOrEmpty(cfgFileExistsIsDoReplace))
                {
                    cfgFileExistsIsDoReplace = ConfigurationManager.AppSettings["CfgFileExistsIsDoReplace"];
                }
                return cfgFileExistsIsDoReplace=="true";
            }
        }
        static string dateTimeIntFormat;
        /// <summary>
        /// 日期戳形式
        /// </summary>
        public static string DateTimeIntFormat
        {
            get 
            {
                if (string.IsNullOrEmpty(dateTimeIntFormat))
                {
                    dateTimeIntFormat = ConfigurationManager.AppSettings["DateTimeIntFormat"];
                }
                return dateTimeIntFormat;
            }
        }
    }
    public class DataLink
    {
        /// <summary>
        /// 将数据发送给其他的平台
        /// </summary>
        /// <param name="title">数据标题</param>
        /// <param name="document">数据</param>
        public void SendDataToOtherPlatform(string title,string document) 
        {
            SendEmail(title, document);
        }  
        void SendEmail(string subject, string body)
        {
            string dir = SystemConfig.ExeDir + "\\" + typeof(ELogType).Name;
            string file = DateTime.Now.ToString(Common.Data.CommonFormat.DateIntFormat) + ".log";
            string msg = DateTime.Now.ToString(Common.Data.CommonFormat.DateTimeFormat) + "\r\n";
            try
            {
                string from = SystemConfig.EmailId;
                EmailService email = new EmailService(SystemConfig.EmailClient, from, SystemConfig.EmailKey,
                    SystemConfig.EmailClientPort, SystemConfig.EmailEnableSsl);
                email.LogPath = dir;
                //此处需要验证 发信人和发件人的区别
                email.SendEmail(subject, body, from, from, SystemConfig.DefaultEmailTo, null, false, System.Net.Mail.MailPriority.High, null);
                msg +="EmailSend Success";
                LoggerWriter.CreateLogFile(msg, dir, ELogType.EmailLog, file, true);
            }
            catch (Exception ex)
            {
                 msg += ex.Message;
                LoggerWriter.CreateLogFile(msg, dir, ELogType.EmailLog, file, true);
            }
        }
    }
}
