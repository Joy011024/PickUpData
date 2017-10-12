using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using EmailHelper;
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
            Application.Run(new Main());
           // Application.Run(new TecentDataFrm());
        }
        static void Test() 
        {
            //CityDataManage cm = new CityDataManage();
            //cm.ImportDB(@"D:\Dream\ExcuteHttpCmd\CaptureWeb\CaptureWebData\CaptureWebData\Wait\国家城市区域名称.txt");
           // PrepareParam.PrepareHeader();
           
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
        public static bool UseRedis
        {
            get 
            {
                useRedis = ConfigurationManager.AppSettings["UseRedis"] == "true";
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
            string from=SystemConfig.EmailId;
            EmailService email = new EmailService(SystemConfig.EmailClient,from, SystemConfig.EmailKey, 
                SystemConfig.EmailClientPort, SystemConfig.EmailEnableSsl);
            //此处需要验证 发信人和发件人的区别
            email.SendEmail(subject, body, from, from, SystemConfig.DefaultEmailTo, null, false, System.Net.Mail.MailPriority.High, null);
        }
    }
}
