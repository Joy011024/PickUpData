﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using HRApp.Model;
using Infrastructure.ExtService;
using Domain.CommonData;
namespace HRApp.Web
{
    public class InitAppSetting
    {
        public static string Version { get; set; }
        [Domain.CommonData.DescriptionSort("代码版本号")]
        public static string CodeVersion { get; set; }
        [Domain.CommonData.DescriptionSort("数据库版本号")]
        public static string DBVersion { get; set; }
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
        public static string QueryUinDB
        {
            get {
                return ConfigurationManager.ConnectionStrings["UinDBConnString"].ConnectionString;
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
        public static string CodeVersionFromCfg()
        {
            return ConfigurationManager.AppSettings["CodeVersion"];
        }
        /// <summary>
        /// 数据库中存储的系统配置项集合
        /// </summary>
        public static Dictionary<string, string> AppSettingItemsInDB = new Dictionary<string, string>();
        /// <summary>
        /// 默认系统配置根节点代码
        /// </summary>
        public static string DefaultAppsettingRootCode = "-1";
        public static int DefaultGridRowSize
        {
            get 
            {
                if (AppSettingItemsInDB.Count > 0)
                {
                    string cfg = InitAppSetting.AppSettingItemsInDB[EAppSetting.DefaultQuerySize.ToString()];
                    return int.Parse(cfg);
                }
                return 30;
            }
        }
        static string dir;
        public static string DefaultLogPath 
        {
            get 
            {
                if (string.IsNullOrEmpty(dir))
                {
                    dir = new AppDirHelper().GetAppDir(AppCategory.WebApp);
                }
                return dir;
            }
        }
        public static string LogPath
        {
            get 
            {
                return DefaultLogPath + "/" + typeof(ELogType).Name;
            }
        }
    }
    public class ParamNameTemplate
    {
        public static string ViewDataModelName = "Model";
        public static string AppSettingParentNode = "ParentNode";
    }
}