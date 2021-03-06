﻿using System;
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
        static string _NewCookieItem;
        static Dictionary<string, string> _SampleCookieItem;
        /// <summary>
        /// 自动补充相似cookie项
        /// </summary>
        public static Dictionary<string, string> SampleCookieItem
        {
            get
            {
                if (string.IsNullOrEmpty(_NewCookieItem))
                {
                    _NewCookieItem = GetAppSettingValue("NewCookieItem");
                }
                if (string.IsNullOrEmpty(_NewCookieItem))
                {
                    return new Dictionary<string, string>();
                }
                _SampleCookieItem = new Dictionary<string, string>();
                if (string.IsNullOrEmpty(_NewCookieItem))
                {
                    return _SampleCookieItem;
                }
                foreach (var item in _NewCookieItem.Split('|'))
                {
                    string[] cv = item.Split('=');
                    if (!string.IsNullOrEmpty(cv[0]))
                        _SampleCookieItem.Add(cv[0], cv[1]);
                }
                return _SampleCookieItem;
            }
        }
        static string _IgnoreHeadItem;
        /// <summary>
        /// 忽略的请求头
        /// </summary>
        public static string[] IgnoreHeadItem
        {
            get
            {
                if (string.IsNullOrEmpty(_IgnoreHeadItem))
                {
                    _IgnoreHeadItem = GetAppSettingValue("IgnoreHeadItem");
                }
                return _IgnoreHeadItem.Split('|');
            }
        }
        public static string GetUserTockenUrl
        {
            get
            {
                return GetAppSettingValue("GetUserTocken");
            }
        }
        /// <summary>
        /// http响应被字符集加密
        /// </summary>
        public static bool ResponseIsZip
        {
            get
            {
                return GetAppSettingValue("ResponseIsZip") == "true";
            }
        }
        public static string HttpResponseZip
        {
            get
            {
                return GetAppSettingValue("HttpResponseZip");
            }
        }
        public static string DebugDir
        {
            get
            {
                return new AppDir().Dir();
            }
        }
        public static bool DownloadResource
        {
            get
            {
                return GetAppSettingValue("DownloadResource") == "true";
            }
        }
        /// <summary>
        /// 返回tocken 的请求项
        /// </summary>
        public static string TockenAfterUrl
        {
            get
            {
                return GetAppSettingValue("TockenAfterUrl");
            }
        }
    }
}
