using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonHelperEntity
{
    public class CoreContidionHelper
    {
        /// <summary>
        /// 文件目录分隔符
        /// </summary>
        public const string DirectorySplitSign = "\\";
        /// <summary>
        /// 日志文件相对路径
        /// </summary>
        public const string LogFileRelativePath = "Log";
        /// <summary>
        /// 查用文件名前缀
        /// </summary>
        public const string BaseFileNamePrefix = "Infor_";
        /// <summary>
        /// 记录异常操作的日志前缀名
        /// </summary>
        public const string ExceptionLogFilePrefix = "Error_";
        /// <summary>
        /// 日志文静扩展名
        /// </summary>
        public const string LoginFileExtension = ".Log";
        /// <summary>
        /// 字符分割符
        /// </summary>
        public const char StringSplitSign = '|';
        public static string AppendLogHeadRow() 
        {
          return "TimeSpan ="+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss f");
        }
    }
}
