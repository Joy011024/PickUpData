using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
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
    }
}
