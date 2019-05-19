using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Common.Data;
using System.Reflection;
using EmailHelper;
using Domain.CommonData;
namespace CaptureWebData
{
    public class ConfigurationItems
    {
        private string _AddUrlDataSp;
        /// <summary>
        /// 添加请求URL使用到的存储过程名称
        /// </summary>
        public string AddUrlDataSp
        {
            get
            {
                if (string.IsNullOrEmpty(_AddUrlDataSp))
                {
                    _AddUrlDataSp = ConfigurationManager.AppSettings["AddUrlDataSp"];
                }
                return _AddUrlDataSp;
            }
        }
        private string _ValidateUrlField;
        /// <summary>
        /// 验证URL必填的字符串字段内容
        /// </summary>
        public string ValidateUrlField
        {
            get
            {
                if (string.IsNullOrEmpty(_ValidateUrlField))
                {
                    _ValidateUrlField = ConfigurationManager.AppSettings["ValidateUrlField"];
                }
                return _ValidateUrlField;
            }
        }
        private string _UrlDataConnString;
        public string UrlDataConnString
        {
            get
            {
                if (string.IsNullOrEmpty(_UrlDataConnString))
                {
                    ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings["UrlDataConnString"];
                    if (setting != null)
                    {
                        _UrlDataConnString = setting.ConnectionString;
                    }
                }
                return _UrlDataConnString;
            }
        }
        /// <summary>
        /// ip数据来源集合
        /// </summary>
        public string IpDataSrc
        {
            get { 
                ConnectionStringSettings sec = ConfigurationManager.ConnectionStrings["IpDataSrc"];
                if (sec == null) { return string.Empty; } 
                return sec.ConnectionString; }
        }
        /// <summary>
        /// IP地址数据保存连接字符串
        /// </summary>
        public string SaveIpAddressConnString 
        {
            get
            {
                ConnectionStringSettings sec = ConfigurationManager.ConnectionStrings["IpAddressData"];
                if (sec == null) { return string.Empty; }
                return sec.ConnectionString;
            }
        }
        public string TecentDA_Read
        {
            get
            {
               
                ConnectionStringSettings sec = ConfigurationManager.ConnectionStrings["TecentDA_Read"];
                if (sec == null) { return string.Empty; }
                return sec.ConnectionString;
            }
        }
        public string TecentDA_Write
        {
            get
            {

                ConnectionStringSettings sec = ConfigurationManager.ConnectionStrings["TecentDA_Write"];
                if (sec == null) { return string.Empty; }
                return sec.ConnectionString;
            }
        }
        /// <summary>
        /// 数据库分类
        /// </summary>
        public string DBType
        {
            get 
            {
                string dbtype = ConfigurationManager.AppSettings["DBType"];
                return dbtype;
            }
        }
        /// <summary>
        /// sqlite数据库连接串
        /// </summary>
        public string SqliteDbConnString
        {
            get 
            {
                ConnectionStringSettings sec = ConfigurationManager.ConnectionStrings["TecentDASQLite"];
                if (sec == null) { return string.Empty; }
                return sec.ConnectionString.Replace("{BaseDir}", AppDomain.CurrentDomain.BaseDirectory);
            }
        }
        public string GetConnString(string connItemName)
        {
            string str = string.Empty;
            ConnectionStringSettings sec = ConfigurationManager.ConnectionStrings[connItemName];
            if (sec != null)
            {
                return sec.ConnectionString;
            }
            return str;
        }
        public string LogPath 
        {
            get
            {
                string sec = ConfigurationManager.AppSettings["LogPath"];
                return sec;
            }
        }
        /// <summary>
        /// 应付Quartz作业池存在相同名称的作业导致 异常
        /// </summary>
        public string AppNameEng 
        {
            get
            {
                string sec = ConfigurationManager.AppSettings["AppNameEng"];
                return sec;
            }
        }
        /// <summary>
        /// 获取当前程序运行的目录
        /// </summary>
        /// <returns></returns>
        public string GetProcessPath() 
        {
           return AppDomain.CurrentDomain.BaseDirectory;
        }
        public string JoinProcessPath() 
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            return string.Join("&", path.Split('\\'));
        }
        public string GetNowAssembly() 
        {
            Assembly now = Assembly.GetEntryAssembly();
            return now.FullName.Split(',')[0];
        }
        public static string GetConfiguration(string cfg) 
        {
            ConnectionStringSettings item = ConfigurationManager.ConnectionStrings[cfg];
            if (item == null)
            {
                return string.Empty;
            }
            return item.ConnectionString;
        }
        /// <summary>
        /// 读取待同步的数据库连接串
        /// </summary>
        public static string GetWaitSyncDBString
        {
            get { return GetConfiguration("WaitSyncTecentDA"); }
        }
        public static bool OupputSql
        {
            get 
            {
                return ConfigurationManager.AppSettings["OupputSql"]=="1";
            }
        }
        private static string _OpenSQLServer;
        /// <summary>
        /// 是否启用sqlserve数据库
        /// </summary>
        public static bool OpenSQLServer
        {
            get
            {
                if (string.IsNullOrEmpty(_OpenSQLServer))
                {
                    _OpenSQLServer = ConfigurationManager.AppSettings["OpenSQLServer"];
                }

                return _OpenSQLServer=="true";
            }
        }
        private static string errorEmailSubject;
        public static string ErrorSubjetFormat
        {
            get
            {
                if (string.IsNullOrEmpty(errorEmailSubject))
                {
                   errorEmailSubject= ConfigurationManager.AppSettings["ErrorEmailSubjetFormat"];
                }
                return errorEmailSubject;
            }
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
                if (redisPort == 0)
                {
                    string port = ConfigurationManager.AppSettings["RedisPort"];
                    int.TryParse(port, out redisPort);
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
                if (string.IsNullOrEmpty(cacheReleativeDir))
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
        public static bool EmailEnableSsl { get { return ConfigurationManager.AppSettings["enableSsl"] == "true"; } }
        public static int? EmailClientPort
        {
            get
            {
                int? port = null;
                string ps = ConfigurationManager.AppSettings["emailClientPort"];
                if (!string.IsNullOrEmpty(ps))
                {
                    int p = 0;
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
                return cfgFileExistsIsDoReplace == "true";
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
        static string divideNum;
        /// <summary>
        /// 消息数据的取余除数
        /// </summary>
        public static int DivideNum
        {
            get
            {
                int num = 0;
                if (string.IsNullOrEmpty(divideNum))
                {
                    divideNum = ConfigurationManager.AppSettings["DivideNum"];
                }
                int.TryParse(divideNum, out num);
                if (num < 1)
                {
                    num = 500;
                }
                return num;
            }
        }
        static string useDBSaveComonData;
        /// <summary>
        /// 是否启用数据库存储基础数据
        /// </summary>
        public static bool UsingDBSaveBaseData
        {
            get
            {
                if (string.IsNullOrEmpty(useDBSaveComonData))
                {
                    useDBSaveComonData = ConfigurationManager.AppSettings["UsingDBSaveBaseData"];
                }
                return useDBSaveComonData == "true";
            }
        }

        static string mainDbType;
        /// <summary>
        /// 业务数据库
        /// </summary>
        public static string MainDBType
        {
            get
            {
                if (string.IsNullOrEmpty(mainDbType))
                {
                    mainDbType = ConfigurationManager.AppSettings["MainDBType"];
                }
                return mainDbType;
            }
        }
        static string logicDBType;
        /// <summary>
        /// 基础数据库
        /// </summary>
        public static string BasicDBType
        {
            get
            {
                if (string.IsNullOrEmpty(logicDBType))
                {
                    logicDBType = ConfigurationManager.AppSettings["BasicDBType"];
                }
                return logicDBType;
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
        public void SendDataToOtherPlatform(string title, string document)
        {
            SendEmail(title, document);
        }
        void SendEmail(string subject, string body)
        {

            string dir = LogPrepare.GetLogPath();
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
                
            }
            catch (Exception ex)
            {
                 msg += ex.Message;
                LoggerWriter.CreateLogFile(msg, dir, ELogType.EmailLog, file, true);
            }
            
        }
    }
    public class LogPrepare
    {
        public static string GetLogPath(string extFolderName=null)
        {
            DateTime now = DateTime.Now;
            string dir = string.Format("{0}\\{1}\\{2}\\{3}", SystemConfig.ExeDir, typeof(ELogType).Name, now.Year,now.ToString(CommonFormat.DateIntFormat));
            if (!string.IsNullOrEmpty(extFolderName))
            {
                dir = string.Format("{0}\\{1}", dir, extFolderName);
            }
            return dir;
        }
        public static string GetLogName(ELogType log)
        {
            return string.Empty;
        }
    }
}
