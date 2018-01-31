using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ExtService;
using System.Text.RegularExpressions;
using Common.Data;
using HRApp.Model;
using IHRApp.Infrastructure;
using HRApp.Infrastructure;
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
        /// <param name="sqlConnString">数据库连接字符串【如果该值不提供则不启用数据库功能】</param>
        /// <returns></returns>
        public static string  TextConvertSpellName(string text,string sqlConnString) 
        {//文本转拼音
            Regex reg = new Regex(@"[A-Za-z0-9\p{P}]+$");//字符【A-Za-z】数字【0-9】已经标点符号【\p{P}】
            StringBuilder sb = new StringBuilder();
            List<MaybeSpellName> maybes = new List<MaybeSpellName>();
            bool openDBFun = !string.IsNullOrEmpty(sqlConnString);
            foreach (var item in text)
            {
                string word = item.ToString();
                if (string.IsNullOrEmpty(word)) 
                {//过滤空格
                    continue;
                }
                Match m = reg.Match(word);
                GroupCollection gc = m.Groups;
                if (gc.Count > 0 && !string.IsNullOrEmpty(gc[0].Value))
                {//阿拉伯数字或者字母以及标点符号
                    sb.Append(word);
                    continue;
                }
                string spell = item.CharConvertSpellName(true);
                //如果此处原样返回则说明此词不支持解析
                if (spell == word && openDBFun)
                {
                    MaybeSpellName ms = new MaybeSpellName() { Name = word, Code = spell };
                    ms.InitParam();
                    maybes.Add(ms);
                }
                //如果解析的拼音不正常（生僻字解析错误 解析结果可能为 zuo）
                else if (spell.ToLower() == word.ToLower() && openDBFun)
                {
                    MaybeSpellName ms = new MaybeSpellName() { Name = word, Code = spell };
                    ms.InitParam();
                    maybes.Add(ms);
                }
                else 
                {
                    sb.Append(spell);
                }
            }
            if (openDBFun)
            {//使用数据库进行疑似生僻字存储 [这里应该修改未异步多线程进行，避免出现死锁以及延时的情形]
                SaveMaybeSpell(maybes, sqlConnString);
                
            }
            return sb.ToString();
        }
        static JsonData SaveMaybeSpell(List<MaybeSpellName> maybes, string sqlConnString)
        {
            JsonData json = new JsonData() { Result=true};
            try
            {
                IMaybeSpecialRepository maybeRepository = new MaybeSpecialRepository() { SqlConnString = sqlConnString };
                maybeRepository.BulkSave(maybes);
                json.Success = true;
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return json;
        }
    }
}
