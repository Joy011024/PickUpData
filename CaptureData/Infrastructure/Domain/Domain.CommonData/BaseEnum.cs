using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Common.Data;
using System.Globalization;
namespace Domain.CommonData
{
    public enum RequestMethod
    {
        [Description("Get请求")]
        GET=1,
        [Description("Post请求")]
        POST=2
    }
    /// <summary>
    /// 日志分类
    /// </summary>
    public enum ELogType 
    {
        [Description("错误日志")]
        ErrorLog=1,
        [Description("数据日志")]
        DataLog=2,
        [Description("运行逻辑日志")]
        LogicLog=3,
        [Description("参数日志")]
        ParamLog=4,
        [Description("缓存数据日志")]
        SessionOrCookieLog=5,
        [Description("数据存入数据库日志")]
        DataInDBLog=6,
        [Description("Debug调试日志数据")]
        DebugData=7,
        [Description("性能日志")]
        PerformanceLog=8,
        [Description("爬虫数据日志")]
        SpliderDataLog=9,
        [Description("爬取的组数据")]
        SpliderGroupDataLog=10,
        [Description("Http请求响应")]
        HttpResponse=11,
        [Description("压缩日志")]
        ZipLog=12,
        [Description("邮件日志")]
        EmailLog=13,
        [Description("邮件正文")]
        EmailBody=14,
        [Description("后台进程")]
        BackgroundProcess=15,
        Account=16,
        [Description("心跳线检测")]
        HeartBeatLine=17
    }
    /// <summary>
    /// 程序类型
    /// </summary>
    public enum EAppCategoty 
    {
        [Description("客户端程序")]
        CleintApp=1,
        [Description("服务端程序")]
        ServiceApp=2
    }
    [Description("腾讯头像分类")]
    public enum EUinImageType 
    {
       [Description ("系统照片")]
        System=1,
        [Description("头像")]
        User = 2,
        [Description("其他类型")]
        Other=-1,
    }
    public enum EBrowserMachineType
    {
        [Description("Linux操作系统")]
        Linux=1,
        [Description("Win7操作系统")]
        WinNT=2,
        [Description("未知的操作系统")]
        Unknown=-1
    }
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
        public static void  CreateLogFile(string text,string path,ELogType log,string fileName=null,bool existsWrite=false,Encoding encode=null) 
        {
            if (string.IsNullOrEmpty(text)) { return; }
            if (!string.IsNullOrEmpty(path)&&!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(path);
            }
            DateTime now = DateTime.Now;
            string file = path + "/" + log.ToString();
            if (!Directory.Exists(file))
                Directory.CreateDirectory(file);
            string filetype = ".txt";
            if (string.IsNullOrEmpty(fileName))
                file += "/" + log + now.ToString(CommonFormat.DateTimeIntFormat) + filetype;
            else
            {//需要判断是否增加后缀
                if (!fileName.Contains("."))
                {
                    file += "/" + fileName + filetype;
                }
                else {
                    file += "/" + fileName;
                }
            }
            FileStream fs ;
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
            sw.Write(text);
            sw.Close();
            fs.Close();
        }
    }
    public class FileHelper
    {
        /// <summary>
        /// 读取文件 ，如果文件不存在则返回null
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader rs = new StreamReader(file);
            // byte[] bytes = new byte[1024];
            StringBuilder sb = new StringBuilder();
            while (rs.Peek() > 0)
            {
                sb.AppendLine(rs.ReadLine());
            }
            file.Close();
            rs.Close();
            return sb.ToString();
        }
        public static void WrtiteFile(string path, string fileName, string text)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FileStream file = new FileStream(path + "\\" + fileName, FileMode.OpenOrCreate, FileAccess.Write);
            //待写入的内容过大是否能分段写入
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            int len = bytes.Length;
            file.Write(bytes, 0, bytes.Length);
            file.Flush();
            file.Close();
        }
        public static void WriteLog(string path, string fileName, string text)
        {
            DirExists(path);
            FileStream stream = new FileStream(path + "\\" + fileName, FileMode.Append, FileAccess.Write);
            byte[] txt = Encoding.UTF8.GetBytes(text);
            stream.Write(txt, 0, txt.Length);
            stream.Flush();
            stream.Close();
        }
        static void DirExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
    public static class Logger 
    {
        static string DllDir;
        static Logger()
        {
            if (string.IsNullOrEmpty(DllDir))
            {
                AssemblyDataExt ass = new AssemblyDataExt();
                DllDir = ass.GetAssemblyDir();
            }
        }
        public static void CreateNewAppData(this string document,string fileName,string dir=null) 
        {
            if (string.IsNullOrEmpty(dir))
            {
                dir = (new AssemblyDataExt()).GetAssemblyDir();
            }    
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string fullname = dir + "/" + fileName;
            FileStream fs = new FileStream(fullname,FileMode.Create,FileAccess.Write,FileShare.ReadWrite);
            byte[] txt = Encoding.UTF8.GetBytes(document);
            fs.Write(txt,0,txt.Length);
            fs.Close();
        }
        /// <summary>
        /// 获取当前天所属的周
        /// </summary>
        /// <returns></returns>
        public static string GetWeekIndex() 
        {
            GregorianCalendar gc = new GregorianCalendar();
            int week= gc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);//今天是今年第几周
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
            return now.ToString("yyyy")+ week.ToString();
        }
        public static string GenerateTimeOfFileName() 
        {
          return  DateTime.Now.ToString(CommonFormat.DateTimeIntFormat);
        }
    }
    public  class AssemblyDataExt 
    {
        public  string GetAssemblyDir() 
        {//获取dll文件被引用后所在的程序集
            System.Reflection.Assembly ass= this.GetType().Assembly;
            string path= ass.Location;
            DirectoryInfo di = new DirectoryInfo(path);
            return di.Parent.FullName;
            //D:\Dream\ExcuteHttpCmd\CaptureWeb\CaptureWebData\CaptureWebData\bin\Debug\Domain.CommonData.dll
        }
        public string ForeachDir(string originDir, int foreachLevel)
        {
            DirectoryInfo di = new DirectoryInfo(originDir);
            string path = originDir;
            string root = di.Root.FullName;
            while (foreachLevel>0)
            {
                path = di.Parent.FullName;
                if (path == root)
                {
                    return root;
                }
                di = new DirectoryInfo(path);
                foreachLevel--;
            }
            return path;
        }
    }
    /// <summary>
    /// 获取APP所在文件
    /// </summary>
    public class AppDir 
    {
        public static string GetParentDir(int level)
        {
            AssemblyDataExt ass = new AssemblyDataExt();
            string app = ass.GetAssemblyDir();
            while (level > 0)
            {
                DirectoryInfo di = new DirectoryInfo(app);
                if (di.Root.FullName == app)
                {
                    return app;
                }
                app = di.Parent.FullName;
                level--;
            }
            return app;
        }
    }
}
