using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataHelp
{
    public static class ConvertIntData
    {
        /// <summary>
        /// 十进制字符串转换为16进制字符串【如果返回为null表示出现异常】
        /// </summary>
        /// <param name="intData">10进制的字符串</param>
        /// <returns></returns>
        public static string Convert16ToString(this string intData) 
        {
            int data = 0;
            if (!int.TryParse(intData, out data)) 
            {
                return null;
            }
            return Convert16ToString(data);
        }
        /// <summary>
        /// 十进制转换为16进制字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Convert16ToString(this int data) 
        {
            return data.ToString("x8");
        }
        /// <summary>
        /// 16进制转换为10进制字符串【如果返回为null表示出现异常】
        /// </summary>
        /// <param name="str16"></param>
        /// <returns></returns>
        public static string ConvertIntFrom16String(this string str16) 
        {
            try
            {
               return Int32.Parse(str16, System.Globalization.NumberStyles.HexNumber).ToString();
            }
            catch (Exception ex) 
            {
                return null;
            }
        }
    }
}
