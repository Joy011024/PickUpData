using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Common.Data;
using System.Reflection;
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
        public string TecentDA 
        {
            get
            {
                ConnectionStringSettings sec = ConfigurationManager.ConnectionStrings["TecentDA"];
                if (sec == null) { return string.Empty; }
                return sec.ConnectionString;
            }
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
    }
}
