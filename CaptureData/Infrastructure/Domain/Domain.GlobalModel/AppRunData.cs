using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.GlobalModel
{
    /// <summary>
    /// 程序运行时的信息
    /// </summary>
    public class AppRunData
    {
        public static string AppName { get; set; }
        public static DateTime RunTime { get; set; }
        public static bool? CanLinkNetwork { get; set; }
        /// <summary>
        /// 公网IP
        /// </summary>
        public static string NetworkIP { get; set; }
        /// <summary>
        /// 局域网IP
        /// </summary>
        public static string LocalAreaId { get; set; }
        /// <summary>
        /// 日志默认存储目录
        /// </summary>
        public static string LogDir { get; set; }
        public static void InitAppData() 
        {
            RunTime = DateTime.Now;
        }
    }
}
