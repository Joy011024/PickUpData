using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
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
            Application.Run(new TecentDataFrm());
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

    }
}
