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
   
    public class LoggerWriter
    {
        /// <summary>
        /// 将数据写入到日志
        /// </summary>
        /// <param name="text"></param>
        /// <param name="path"></param>
        /// <param name="log"></param>
        /// <param name="fileName"></param>
        /// <param name="existsWrite">存在相同名称的文件进行替换还是追加</param>
        public static void CreateLogFile(string text, string path, ELogType log, string fileName = null, bool existsWrite = false, Encoding encode = null)
        {
            if (string.IsNullOrEmpty(text)) { return; }
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            DateTime now = DateTime.Now;
            string file = path;
            if (!Directory.Exists(file))
                Directory.CreateDirectory(file);
            string filetype = ".txt";
            if (string.IsNullOrEmpty(fileName))
                file += "/" + now.ToString("yyyyMMdd") + filetype;
            else
            {//需要判断是否增加后缀
                if (!fileName.Contains("."))
                {
                    file += "/" + fileName + filetype;
                }
                else
                {
                    file += "/" + fileName;
                }
            }
            FileStream fs;
            if (!existsWrite)
            {
                fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Write);
            }
            else
            {
                if (File.Exists(file))
                {
                    text = "\r\n" + text;
                    fs = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.Write);
                }
                else
                {
                    fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Write);
                }
            }
            if (encode == null)
            {
                encode = Encoding.UTF8;
            }
            StreamWriter sw = new StreamWriter(fs, encode);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(CreateSign());
            sb.AppendLine(log.ToString());
            sb.AppendLine(text);
            sw.Write(sb.ToString());
            sw.Close();
            fs.Close();
        }
        static string CreateSign()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 32; i++)
            {
                sb.Append("-");
            }
            return sb.ToString();
        }
    }
  
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
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="text"></param>
        /// <param name="title"></param>
        /// <param name="insert"></param>
        public static void WriteLog(this string text, ELogType title,bool insert)
        {
            LoggerWriter.CreateLogFile(text, DllDir + GetYearWeekIndex(), title, GetNowDayIndex(), insert);
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
