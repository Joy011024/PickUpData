using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ExtService;
using System.Text.RegularExpressions;
namespace HRApp.ApplicationService
{
    /// <summary>
    /// 汉字转拼音后回调事件
    /// </summary>
    /// <param name="word">汉字</param>
    /// <param name="spellName">转换后的拼音</param>
    public delegate void CharConvertSpellCallBack(char word,string spellName);
    public class CommonCallService
    {
        /// <summary>
        /// 将汉字进行拼音转换（对于生僻字进行过滤）
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maybeSpecialWordCallBack"></param>
        /// <param name="recognizeFailureCallBack"></param>
        /// <returns></returns>
        public static string  TextConvertSpellName(string text,CharConvertSpellCallBack maybeSpecialWordCallBack,
            CharConvertSpellCallBack recognizeFailureCallBack) 
        {//文本转拼音
            Regex reg = new Regex(@"[A-Za-z0-9\p{P}]+$");//字符【A-Za-z】数字【0-9】已经标点符号【\p{P}】
            StringBuilder sb = new StringBuilder();
            foreach (var item in text)
            {
                string word = item.ToString();
                Match m = reg.Match(word);
                //是否为阿拉伯数字或者字母
                GroupCollection gc = m.Groups;
                if (gc.Count > 0 && !string.IsNullOrEmpty(gc[0].Value)) 
                {//
                    sb.Append(word);
                    continue;
                }
                string spell = item.CharConvertSpellName(true);
                //如果此处原样返回则说明此词不支持解析
                if (spell == word)
                {
                    maybeSpecialWordCallBack(item, spell);
                }
                //如果解析的拼音不正常（生僻字解析错误 解析结果可能为 zuo）
                else if (spell.ToLower() == word.ToLower())
                {
                    recognizeFailureCallBack(item, spell);
                }
                else 
                {
                    sb.Append(spell);
                }
            }
            return sb.ToString();
        }
    }
}
