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
    public static class Logger
    {
       public static string DllDir;
        static Logger()
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
            LoggerWriter.CreateLogFile(text, Logdir + GetYearWeekIndex(), title, GetNowDayIndex(), insert);
        }
        [System.ComponentModel.Description("每日唯一")]
        public static void WriteLogForEverDay(this string text,ELogType type)
        {
            FileHelper.ReplaceTxt(Logdir + GetYearWeekIndex()+ DateTime.Now.ToString("yyyyMMdd")+"/", type.ToString(), text);
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
        /// <summary>
        /// 获取今天的年以及所属周 如 201736（表示2017年第36周）
        /// </summary>
        /// <returns></returns>
        public static string GetYearWeekIndex()
        {
            GregorianCalendar gc = new GregorianCalendar();
            DateTime now = DateTime.Now;
            int week = gc.GetWeekOfYear(now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);//今天是今年第几周
            return now.Year.ToString() + week.ToString();
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
