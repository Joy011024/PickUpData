using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPinyin;
namespace Infrastructure.ExtService
{
    public static class TextConvertCharHelper
    {
        /// <summary>
        /// 将汉字转换为拼音字符串
        /// </summary>
        /// <param name="document"></param>
        /// <param name="everyCharUpper">每一个汉字手写字母是否转换为大写</param>
        /// <returns></returns>
        public static string TextConvertChar(this string document,bool everyCharUpper=false) 
        {
            Encoding enc = Encoding.UTF8;
            document = document.Trim();
            string firstChars=  Pinyin.GetInitials(document, enc);//转换为大写字母
            string pinying= Pinyin.GetPinyin(document);//这是获取到元素的拼音
            if (!everyCharUpper) 
            {//每一个单词的首字母进行大写转换
                return pinying.Replace(" ",string.Empty);
            }
            StringBuilder sb = new StringBuilder();
            foreach (string item in pinying.Split(' '))
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                //首字母进行处理
                sb.Append(item.Substring(0, 1).ToUpper());
                if (item.Length > 1)
                    sb.Append(item.Substring(1));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 单个汉字转换为拼音
        /// </summary>
        /// <param name="word"></param>
        /// <param name="firstIsUpper">汉字首字母转换为大写</param>
        /// <returns></returns>
        public static string CharConvertSpellName(this char word, bool firstIsUpper)
        {
            Encoding enc = Encoding.UTF8;
            string pinying = Pinyin.GetPinyin(word.ToString());//这是获取到元素的拼音
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(pinying))
            {
                return string.Empty;
            }
            //首字母进行处理
            sb.Append(pinying.Substring(0, 1).ToUpper());
            if (pinying.Length > 1)
                sb.Append(pinying.Substring(1));
            return sb.ToString();
        }
        /// <summary>
        /// 获取首字母
        /// </summary>
        /// <param name="document">汉字列表</param>
        /// <param name="everyCharUpper">首字母是否大写</param>
        /// <returns></returns>
        public static string TextConvertFirstChar(this string document, bool everyCharUpper = false)
        {
            Encoding enc = Encoding.UTF8;
            string pinying = Pinyin.GetInitials(document, enc);//大写的首字母
            //首字母进行处理
            if (!everyCharUpper)
            {
                return pinying.ToLower();
            }
            return pinying;
        }
    }
}
