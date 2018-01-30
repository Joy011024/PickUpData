using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IHRApp.Infrastructure;
using HRApp.Infrastructure;
namespace HRApp.Infrastructure
{
    public class CommonCallRepository
    {
        /// <summary>
        /// 供业务层直接调度的录入疑似生僻字函数
        /// </summary>
        /// <param name="dbConnString">数据库连接字符串</param>
        /// <param name="word"></param>
        /// <param name="spellCode"></param>
        /// <param name="ProcedureOutValue">调用存储过程后的输出参数</param>
        /// <returns></returns>
        public static bool SaveMaybeSpecialSpell(string dbConnString, char word, string spellCode, out object ProcedureOutValue) 
        {
            IMaybeSpecialRepository spellRepository = new MaybeSpecialRepository() { SqlConnString = dbConnString };
            bool statueCode = spellRepository.SaveMaybeSpecialWord(word, spellCode, out ProcedureOutValue);
            return statueCode;
        }
    }
}
