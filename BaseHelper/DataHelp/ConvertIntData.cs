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
		/// <summary>
        /// 输出标准2进制字符串
        /// </summary>
        /// <param name="value10"></param>
        public static string Conver2String(int value10) 
        {
            string value2 = Convert.ToString(value10, 2);
            //不足前置像补0 （4的倍数）
            int len = value2.Length;//当前位数
            int loseLen =4- len % 4;
            return value2.PadLeft(len+loseLen, '0');
        }
        public static string Calculate2And(string first,string second) 
        {
            string min=string.Empty, max=string.Empty;
            #region 避免出现长度不一致的情形
            if (first.Length > second.Length)
            {
                min = second;
                max = first;
            }
            else 
            {
                min = first;
                max = second;
            }
            #endregion
            List<char> outPut = new List<char>();
            for (int i = max.Length-1; i >= 0; i--)
            {
                if (max.Length - i > min.Length)
                {
                    break;
                }
                string fc = max.Substring(i, 1);
                string sc = min.Substring(i, 1);
                if (sc == fc && fc == "1")
                {
                    outPut.Insert(0, '1');
                }
                else {
                    outPut.Insert(0, '0');
                }
            }
            return string.Join("", outPut);
        }
        /// <summary>
        /// 10进制数进行位运算输出运算后10进制结果
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static int Calculate2AndToInt(int first, int second)
        {
            //数字进行位运算，并转化为10进制结果
            string first2 = Conver2String(first);
            string second2 = Conver2String(second);
            string cal = Calculate2And(first2, second2);
            return Convert.ToInt32(cal, 2);
        }
    }
}
