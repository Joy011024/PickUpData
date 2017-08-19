using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Common.Data;
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
        DebugData=7
    }
    public class LoggerWriter 
    {
        /// <summary>
        /// 将数据写入到日志
        /// </summary>
        /// <param name="text"></param>
        /// <param name="path"></param>
        /// <param name="log"></param>
        public static void  CreateLogFile(string text,string path,ELogType log) 
        {
            if (string.IsNullOrEmpty(text)) { return; }
            if (!string.IsNullOrEmpty(path)&&!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(path);
            }
            DateTime now = DateTime.Now;
            string file = path + "/" + log.ToString() + now.ToString(CommonFormat.DateTimeIntFormat) + ".txt";
            FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(text);
            sw.Close();
            fs.Close();
        }
    }
    public class FileHelper
    {
        public static string ReadFile(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader rs = new StreamReader(file);
            // byte[] bytes = new byte[1024];
            StringBuilder sb = new StringBuilder();
            while (rs.Peek() > 0)
            {
                sb.Append(rs.ReadLine());
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
            while (foreachLevel>0)
            {
                path = di.Parent.FullName;
                di = new DirectoryInfo(originDir);
                foreachLevel--;
            }
            return originDir;
        }
    }
}
