using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace CommonHelperEntity
{
    public  class DirectoryInfoHelper 
    {
        /// <summary>
        /// 读取目录下全部文件路径【不包含子目录】
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public List<string> GetAllFilePath(string directoryPath) 
        {
            if (string.IsNullOrEmpty(directoryPath)) 
            {
                return new List<string>();
            }
            if (!Directory.Exists(directoryPath)) 
            {
                return new List<string>();
            }
            string[] paths= Directory.GetFiles(directoryPath);
            if (paths.Count() == 0)
            {
                return new List<string>();
            }
            return  paths.ToList();
        }
        /// <summary>
        /// 查找子目录下存在文件的目录列表
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public List<string> GetHaveFilePathContainChildren(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                return new List<string>();
            }
            if (!Directory.Exists(directoryPath))
            {
                return new List<string>();
            }
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            string[] paths = Directory.GetFiles(directoryPath);
            //使用循环遍历，查找所有的子目录
            
            return paths.ToList();
        }
        /// <summary>
        /// 使用递归查找指定目录下的全部子目录
        /// </summary>
        /// <param name="path">要获取子目录信息对应的目录</param>
        /// <param name="needCheckPath">是否需要检测路径是否存在【当路径是提供查询的目录是建议赋值true,否则不检测路径问题】</param>
        /// <returns></returns>
        public List<string> RecurrenceDirection(string path, bool needCheckPath)
        {
            List<string> paths = new List<string>();
            if (string.IsNullOrEmpty(path))
            {
                return paths;
            }
            if (needCheckPath && !Directory.Exists(path))
            {
                return paths;
            }
            string[] childrens = Directory.GetDirectories(path);
            if (childrens.Length == 0)
            {
                return paths;
            }
            paths.AddRange(childrens);
            foreach (var item in childrens)
            {
                List<string> nodes = RecurrenceDirection(item, false);
                if (nodes == null || nodes.Count == 0) 
                {
                    continue;
                }
                paths.AddRange(nodes);
            }
            return paths;
        }
        /// <summary>
        /// 使用遍历获取文件信息【及其消耗内存，不推荐调用】
        /// </summary>
        /// <param name="path">查找所有文件信息的路径</param>
        /// <param name="needCheckPath">是否检测路径存在信息</param>
        /// <returns></returns>
        public List<FileInfo> RecurrenceFileInDirection(string path, bool needCheckPath) 
        {
            List<FileInfo> fileList = new List<FileInfo>();
            if (string.IsNullOrEmpty(path)) 
            {
                return new List<FileInfo>();
            }
            if (needCheckPath && !Directory.Exists(path)) 
            {
                return new List<FileInfo>();
            }
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files = di.GetFiles();
            string[] children = Directory.GetDirectories(path);
            if (files.Count() == 0 && children.Length == 0) 
            {
                return new List<FileInfo>();
            }
            else if (files.Count() == 0 && children.Length > 0) 
            {//当前目录没有文件但是存在子目录
                foreach (var item in children)
                {
                   List<FileInfo> data= RecurrenceFileInDirection(item, false);
                   if (data.Count > 0) { fileList.AddRange(data); }
                }
            }
            else if (files.Count() > 0)
            {
                fileList.AddRange(files);
                foreach (var file in children)
                {
                    //获取当前目录下的子目录
                    List<FileInfo> data = RecurrenceFileInDirection(file, false);
                    if (data.Count > 0) { fileList.AddRange(data); }
                }
            }
            return fileList;
        }
        /// <summary>
        /// 读取目录下文件的全部路径信息
        /// </summary>
        /// <param name="path">目录</param>
        /// <param name="needCheckPath">目录是否进行存在检测</param>
        /// <returns>目录下全部的文件全路径信息</returns>
        public List<string> RecurrenceFileNameInDirection(string path, bool needCheckPath)
        {
            List<string> fileList = new List<string>();
            if (string.IsNullOrEmpty(path))
            {
                return new List<string>();
            }
            if (needCheckPath && !Directory.Exists(path))
            {
                return new List<string>();
            }
            DirectoryInfo di = new DirectoryInfo(path);
            List<string> files = di.GetFiles().Select(f => f.FullName).ToList();
            string[] children = Directory.GetDirectories(path);
            if (files.Count() == 0 && children.Length == 0)
            {
                return new List<string>();
            }
            else if (files.Count() == 0 && children.Length > 0)
            {//当前目录没有文件但是存在子目录
                foreach (var item in children)
                {
                    List<string> data = RecurrenceFileNameInDirection(item, false);
                    if (data.Count > 0) { fileList.AddRange(data); }
                }
            }
            else if (files.Count() > 0)
            {
                fileList.AddRange(files);
                foreach (var file in children)
                {
                    //获取当前目录下的子目录
                    List<string> data = RecurrenceFileNameInDirection(file, false);
                    if (data.Count > 0) { fileList.AddRange(data); }
                }
            }
            return fileList;
        }
    }
    public static class DirectoryExtend
    {
        public static List<string> GetSubdirectories<T>(this T helper, string path) where T : IDirectoryHelp
        {
            helper.Success = true;
            if (string.IsNullOrEmpty(path)) 
            {
                helper.HavaError = true;
                return new List<string>();
            }
            if (!Directory.Exists(path)) 
            {
                helper.HavaError = true;
                return new List<string>();
            }
            DirectoryInfo di = new DirectoryInfo(path);
            List<string> list = di.GetDirectories().Select(s=>s.FullName).ToList();
            return list;
        }
    }
    public interface IDirectoryHelp
    {
        bool HavaError { get; set; }
        bool Success { get; set; }
        /// <summary>
        /// 读取目录下全部的子目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
      //  List<string> GetAllSubdirectories(string path);
        /// <summary>
        /// 获取非空的子目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        //List<string> GetAllHavaFileSubdirectories(string path);
        /// <summary>
        /// 获取全部为空的子目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        //List<string> GetAllNonFileSubdirectories(string path);
        /// <summary>
        /// 读取路径下全部目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
      //  List<string> GetAllDirectories(string path);
        /// <summary>
        /// 获取全部不为空的目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        //List<string> GetAllHavaFileDirectories(string path);
        /// <summary>
        /// 获取全部为空的目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
       // List<string> GetAllNonFileDirectories(string path);
        /// <summary>
        /// 读取路径下全部的文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        //List<string> GetAllFile(string path);
        /// <summary>
        /// 获取路径下的文件【不包含子目录内的文件】
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
      //  List<string> GetAllChilrenFile(string path);
       
    }
    public class DirectoryHelper : IDirectoryHelp
    {

        public bool HavaError
        {
            get;
            set;
        }
        public bool Success
        {
            get;
            set;
        }
    }
}