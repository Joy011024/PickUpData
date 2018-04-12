using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace CommonHelperEntity
{
    /// <summary>
    /// 读写日志操作帮助类
    /// </summary>
    public interface ILogHelper
    {
    }
    public class LogHelper : ILogHelper { }
    /// <summary>
    /// 扩展写日志类
    /// </summary>
    public static class LogHelperExtend
    {
        /// <summary>
        /// 通用相对路径
        /// </summary>
        public static string ComonRelativePath = "Log";
        public static void AppendLog<T>(this T helper, string text, string fileName, string relativePath, int? directoryRelativeDllLevel)
        {
            AppendLog(text, fileName, relativePath, directoryRelativeDllLevel);
        }
        /// <summary>
        ///提供相对路径写入内容到日志文件中
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="fileName">日志文件完整名</param>
        /// <param name="relativePath">相对路径【可以为空】</param>
        /// <param name="directoryRelativeDllLevel">日志所属根目录相对dll的深度【默认值为4,且最多查找到盘符】</param>
        public static void AppendLog(string text, string fileName, string relativePath,int? directoryRelativeDllLevel) 
        {
            int level = directoryRelativeDllLevel.HasValue ? directoryRelativeDllLevel.Value : 4;
            string systemPath = (new InitFileHandler()).GetAssembPath(level);
            //目录将进行选择
            if (!string.IsNullOrEmpty(relativePath) && !Directory.Exists(systemPath + CoreContidionHelper.DirectorySplitSign + relativePath))
            {
                systemPath = (new StringBuilder(systemPath + CoreContidionHelper.DirectorySplitSign + relativePath)).ToString();
            }
            else
            {
                systemPath = (new StringBuilder(systemPath + CoreContidionHelper.DirectorySplitSign + relativePath)).ToString();
            }
            WriteDocument(systemPath, fileName, text);
        }
        public static void AppendLog<T>(this T help,  string text, string fileName, string path) where T:class
        {
            if (string.IsNullOrEmpty(path)) 
            {
                path = (new InitFileHandler()).GetAssembPath(4);
            }
            WriteDocument(path, fileName, text);
        }
        /// <summary>
        /// 在项目目录下写日志
        /// </summary>
        /// <typeparam name="T">集成该操作的实体类类型</typeparam>
        /// <param name="helper">实体</param>
        /// <param name="text">写入的内容</param>
        /// <param name="relativePath">文件相对路径</param>
        /// <param name="fileName">文件名称（包含该文件类型数据）</param>
        /// <param name="directoryRelativeDllLevel">日志所属根目录相对dll的深度【默认值为4,且最多查找到盘符】</param>
        public static void AppendLogUsingProjectPath<T>(this T helper, string text,string relativePath, string fileName,int? directoryRelativeDllLevel) where T : ILogHelper 
        {
            int level = directoryRelativeDllLevel.HasValue ? directoryRelativeDllLevel.Value : 4;
            string systemPath = (new InitFileHandler()).GetAssembPath(level);
            //目录将进行选择
            if (!string.IsNullOrEmpty(relativePath) && !Directory.Exists(systemPath + CoreContidionHelper.DirectorySplitSign + relativePath))
            {
                systemPath = (new StringBuilder(systemPath + CoreContidionHelper.DirectorySplitSign + relativePath)).ToString();
              
            }
            else 
            {
                systemPath = (new StringBuilder(systemPath + CoreContidionHelper.DirectorySplitSign + relativePath)).ToString();
            }
            WriteDocument(systemPath, fileName, text);
        }
        public static bool WriteDocument(string path,string fileName,string text) 
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileFullPath = path + CoreContidionHelper.DirectorySplitSign + fileName;//文件完整名
            FileStream fs;
            if (File.Exists(fileFullPath))
            {
                fs = new FileStream(fileFullPath, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(fileFullPath, FileMode.Create, FileAccess.Write);
            }
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(text);
            sw.Close();
            fs.Close();
            return true;
        }
        public static string GetProjectPath<T>(this T helper, int? directoryRelativeDllLevel) where T:class
        {
            InitFileHandler file = new InitFileHandler();
            return file.GetAssembPath(directoryRelativeDllLevel.HasValue && directoryRelativeDllLevel.Value > 0 ? directoryRelativeDllLevel.Value : 0);
        }
        /// <summary>
        /// 将重要的基础数据写入到文件中【如果文件已存在将替换文件，可以进行读取操作】
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="path">文件存储路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="data">数据</param>
        /// <param name="dataProperties">存储的实体属性信息</param>
        /// <returns>存储是否成功</returns>
        public static bool WriteDataInFile<T>(this T helper, string path,string fileName,List<T> data,string[] dataProperties=null) where T :class 
        {
            if (data == null || data.Count == 0) 
            {
                return false;
            }
            if (dataProperties == null || dataProperties.Length==0) 
            { //首先读取对象的属性
                T entity = default(T);
                dataProperties= entity.GetAllProperties();
            }
            if (dataProperties.Length == 0) 
            {//没有公共属性
                return false;
            }
            StringBuilder sb = new StringBuilder();
            foreach (string item in dataProperties)
            {
                sb.Append(item + "\t");
            }
            List<string> errorproperties = new List<string>();
            foreach (T item in data)
            {
                int runSize = 0;
                Dictionary<string, object> insert = new Dictionary<string, object>();
                foreach (string pi in dataProperties)
                {
                    runSize++;
                    if (errorproperties.Any(s => s == pi)) 
                    {
                        sb.Append(string.Empty + "\t");
                        continue;
                    }
                    bool hasProperty;
                    object property = ReflectionHelp.GetPropertyValue<T>(item, pi, out hasProperty);
                    if (!hasProperty) 
                    {
                        errorproperties.Add(pi);
                        sb.Append(string.Empty + "\t");
                        continue;
                    }
                    string value = (property!=null)?property.ToString():string.Empty;
                    sb.Append(value + "\t");
                    if (runSize == dataProperties.Length-1) 
                    {
                        sb.AppendLine();
                    }
                }
            }
            string fullName = path + "/" + fileName;
            if (!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(path);
            }
            FileStream fs = new FileStream(fullName, FileMode.CreateNew, FileAccess.Write, FileShare.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(sb.ToString());
            sw.Close();
            fs.Close();
            return true;
        }
    }
    public class InitFileHandler 
    {
        /// <summary>
        /// 获取程序集对应的目录
        /// </summary>
        /// <param name="howLevelParent">从程序集往上查找几次</param>
        /// <returns></returns>
        public string GetAssembPath(int howLevelParent) 
        {
            try
            {
                string path = this.GetType().Assembly.CodeBase;//路径格式file:///
                if (!string.IsNullOrEmpty(path)) 
                {
                    path = path.Replace("file:///", string.Empty);
                }
                DirectoryInfo di = new DirectoryInfo(path);
                while (di.Parent != null && howLevelParent > 0) 
                {
                    di = Directory.GetParent(di.FullName);
                    howLevelParent--;
                    path = di.Parent.FullName;
                }
                //查找上上级目录（程序集->bin->项目文件->?）
                return path;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 使用日期获取文件的部分随机名称
        /// </summary>
        /// <returns></returns>
        public static string GetFileGuidNameByDate() 
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }
    }
}
