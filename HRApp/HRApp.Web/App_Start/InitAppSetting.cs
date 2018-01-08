using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace HRApp.Web
{
    public class InitAppSetting
    {
        public static string Version { get; set; }
        static string hrDbConnString;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string LogicDBConnString
        {
            get 
            {
                if (string.IsNullOrEmpty(hrDbConnString))
                {
                    hrDbConnString = ConfigurationManager.ConnectionStrings["LogicDBConnString"].ConnectionString;
                }
                return hrDbConnString;
            }
        }
        static string openDebug;
        /// <summary>
        /// 是否开启debug模式
        /// </summary>
        public static bool OpenDebugType 
        {
            get 
            {
                if (string.IsNullOrEmpty(openDebug))
                {
                    openDebug = ConfigurationManager.AppSettings["OpenDebugType"];
                }
                return openDebug == "true";
            }
        }
    }
}