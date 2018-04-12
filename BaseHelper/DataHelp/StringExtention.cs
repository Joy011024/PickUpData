using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DataHelp
{
    public static class StringExtention
    {
        /// <summary>
        /// 替换常见的转义字符
        /// </summary>
        /// <param name="oldString"></param>
        /// <param name="replace">涉及进行转义字符替换的键值【形式：key:\\n,Value：\n】</param>
        /// <returns></returns>
        public static string EscapeCharactersReplace(this string oldString, Dictionary<string, string> replace) 
        {
            if (string.IsNullOrEmpty(oldString)) 
            {
                return oldString;
            }
            foreach (KeyValuePair<string,string> item in replace)
            {
                oldString=oldString.Replace(item.Key, item.Value);
            }
            return oldString;
        }
        /// <summary>
        /// 对客户端输入的简单转义字符替换【\t,\r,\n】转换成实际上的转义符
        /// </summary>
        /// <param name="oldString"></param>
        /// <returns></returns>
        public static string SampleEscapeCharactersReplace(this string oldString)
        {
            if (string.IsNullOrEmpty(oldString)) 
            {
                return oldString;
            }
            Dictionary<string, string> esc = new Dictionary<string, string>();
            esc.Add("\\r", "\r");
            esc.Add("\\n", "\n");
            esc.Add("\\t", "\t");
            foreach (KeyValuePair<string, string> item in esc)
            {
                oldString=oldString.Replace(item.Key, item.Value);
            }
            return oldString;
        }
        /// <summary>
        /// 字符串数组转为字符串
        /// </summary>
        /// <param name="data">字符串数组</param>
        /// <param name="linkSign">连接字符</param>
        /// <returns></returns>
        public static string ArrayConvertString(this string[] data,string linkSign) 
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in data)
            {
                result.Append(item + linkSign);
            }
            return result.ToString();
        }
        public static string ArrayConvertStringAppendLine(this string[] data) 
        {
            StringBuilder result = new StringBuilder();
            foreach (string item in data)
            {
                result.AppendLine(item);
            }
            return result.ToString();
        }
        public static bool IsDigital(this string str) 
        {
            Regex reg = new Regex(@"^\d+$");
            return reg.IsMatch(str);
        }
    }
}
