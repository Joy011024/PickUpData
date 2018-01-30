using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHRApp.Infrastructure
{
    public interface IMaybeSpecialRepository
    {
        string SqlConnString { get; set; }
        /// <summary>
        /// 保存疑似生僻字
        /// </summary>
        /// <param name="word">疑似字</param>
        /// <param name="spellName">疑似字使用转换的拼音【该拼音不一定是正确的拼音】</param>
        /// <param name="responseCode">调用存储过程回调参数值【检测之前是否已经录入该疑似词】</param>
        /// <returns></returns>
        bool SaveMaybeSpecialWord(char word,string spellName,out object responseCode);
    }
}
