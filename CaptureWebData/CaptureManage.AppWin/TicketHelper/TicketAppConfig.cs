using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
namespace CaptureManage.AppWin
{
    public class TicketAppConfig
    {
        static string ticket12306OpenDB;
        /// <summary>
        ///是否开启数据库功能
        /// </summary>
        public static bool OpenDB 
        {
            get 
            {
                if (string.IsNullOrEmpty(ticket12306OpenDB))
                {
                    ticket12306OpenDB = ConfigurationManager.AppSettings["ticket12306OpenDB"];
                }
                return ticket12306OpenDB == "true";
            }
        }
        static string ticket12306Cfg;
        /// <summary>
        /// 12306 插件配置文件相对路径
        /// </summary>
        public static string Ticket12306CfgReletive
        {
            get 
            {
                if (string.IsNullOrEmpty(ticket12306Cfg))
                {
                    ticket12306Cfg = ConfigurationManager.AppSettings["Ticket12306Cfg"];
                }
                return ticket12306Cfg;
            }
        }
        static string brushTicketCfg;
        /// <summary>
        /// 进行12306刷票时使用到的配置项
        /// </summary>
        public static string BrushTicketCfg
        {
            get 
            {
                if (string.IsNullOrEmpty(brushTicketCfg))
                {
                    brushTicketCfg = ConfigurationManager.AppSettings["Ticket12306PluginCfgDir"];
                }
                return brushTicketCfg;
            }
        }
        static string subwayCfg;

        public static string BeijingSubwayCfgReletive
        {
            get
            {
                if (string.IsNullOrEmpty(ticket12306Cfg))
                {
                    subwayCfg = ConfigurationManager.AppSettings["CitySubwayCfg"];
                }
                return subwayCfg;
            }
        }
        
    }
    public class AppSettingItem
    {
        static string appDataSqlXml;
        public static string AppDataSqlXml
        {
            get
            {
                if (string.IsNullOrEmpty(appDataSqlXml))
                {
                    appDataSqlXml = new AssemblyLoggerDir().LogDir+"/" + ConfigurationManager.AppSettings["AppDataSqlXml"];
                }
                return appDataSqlXml;
            }
        }
    }
    public class AssemblyLoggerDir
    {
        public  string AppDir
        {
            get
            {
                string path = this.GetType().Assembly.Location;
                DirectoryInfo di = new DirectoryInfo(path);
                return di.FullName;
            }
        }
        static string logDir;
        public string LogDir
        {
            get
            {
                if (string.IsNullOrEmpty(logDir))
                {
                    DirectoryInfo di = new DirectoryInfo(AppDir);
                    logDir = di.Parent.FullName;
                }
                return logDir;
            }
        }
       
    }
    public class BaseCfgItem
    {
        static string baseNodeItemFormat;
        /// <summary>
        /// 公用的xml节点项
        /// </summary>
        public static string AppCfgXmlNodeFormat
        {
            get 
            {
                if (string.IsNullOrEmpty(baseNodeItemFormat))
                {
                    baseNodeItemFormat = ConfigurationManager.AppSettings["AppCfgXmlNodeFormat"];
                }
                return baseNodeItemFormat;
            }
        }
    }
}
