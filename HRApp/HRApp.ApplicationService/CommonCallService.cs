using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ExtService;
using System.Text.RegularExpressions;
namespace HRApp.ApplicationService
{
    public class CommonCallService
    {
        public static string  TextConvertSpellName(string text) 
        {//文本转拼音
            Regex reg = new Regex(@"[A-Za-z0-9\p{P}]+$");//字符【A-Za-z】数字【0-9】已经标点符号【\p{P}】
            foreach (var item in text)
            {
                string word = item.ToString();
                Match m = reg.Match(word);
                //是否为阿拉伯数字或者字母
                GroupCollection gc = m.Groups;
                if (gc.Count > 0 && !string.IsNullOrEmpty(gc[0].Value)) 
                {//
                    continue;
                }
                string spell = item.CharConvertSpellName(true);
                //如果此处原样返回则说明此词不支持解析
                if (spell == word)
                { 
                
                }
                //如果解析的拼音不正常（生僻字解析错误 解析结果可能为 zuo）
                if (spell.ToLower() == word.ToLower())
                {

                }
            }
            return string.Empty;
        }
    }
}
