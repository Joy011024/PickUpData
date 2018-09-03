using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Configuration;
namespace CefSharpWin
{
   public  class SystemConfig
    {
        static string GetAppSettingValue(string key)
        {
           return ConfigurationManager.AppSettings[key];
        }
        /// <summary>
        /// 主URL
        /// </summary>
        public static string MainUrl
        {
            get
            {
                return GetAppSettingValue("MainUrl");
            }
        }
        /// <summary>
        /// 提取cookie时
        /// </summary>
        public static string InitCookeKey
        {
            get
            {
                return GetAppSettingValue("InitCookeKey");
            }
        }
        /// <summary>
        /// 无条件提取cookie
        /// </summary>
        public static bool AnywhereGetCookie
        {
            get
            {
                return GetAppSettingValue("AnywhereGetCookie") == "true";
            }
        }

        public static string ContacterUrl
        {
            get
            {
                return GetAppSettingValue("ContacterUrl");
            }
        }
        /// <summary>
        /// cookie 解析的域
        /// </summary>
        public static string CookieDomain
        {
            get { return GetAppSettingValue("CookieDomain"); }
        }
        public static string[] CookieFrom
        {
            get
            {
                string from = GetAppSettingValue("CookieFrom");
                return from.Split('|');
            }
        }
    }
}
