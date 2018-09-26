using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Globalization;
using Domain.CommonData;
namespace CefSharpWin
{ 
    public static class LoggerManage
    {
       public static string DllDir;
        static LoggerManage()
        {
            if (string.IsNullOrEmpty(DllDir))
            {

                DllDir = new AppDir().Dir();
            }
        }
        public static string Logdir
        {
            get
            {
                return DllDir + "Log/";
            }
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="text"></param>
        /// <param name="title"></param>
        /// <param name="insert"></param>
        public static void WriteLog(this string text, ELogType title,bool insert)
        {
            LoggerWriter.CreateLogFile(text, Logdir + Logger. GetYearMonth(), title, GetNowDayIndex(), insert);
        }
        [System.ComponentModel.Description("每日唯一")]
        public static void WriteLogForEverDay(this string text,ELogType type)
        {
            LoggerWriter.CreateLogFile(text,Logdir + Logger. GetYearMonth()+"/"+ DateTime.Now.ToString("yyyyMMdd")+"/", type,type.ToString(),true);
        }
        /// <summary>
        /// 获取当前天所属的周
        /// </summary>
        /// <returns></returns>
        public static string GetWeekIndex()
        {
            GregorianCalendar gc = new GregorianCalendar();
            int week = gc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);//今天是今年第几周
            return week.ToString();
        }
        public static string GetNowDayIndex()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }
        public static string GenerateTimeOfFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }
        private static void DownLoadFile()
        {//进行文件下载

        }
        
    }
    class AppDir
    {
        public string Dir  ()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
 
   
}
