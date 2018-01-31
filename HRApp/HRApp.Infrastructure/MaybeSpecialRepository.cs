using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
namespace HRApp.Infrastructure
{
    public class MaybeSpecialRepository : IMaybeSpecialRepository
    {
        public string SqlConnString
        {
            get;
            set;
        }

        public bool SaveMaybeSpecialWord(char word, string spellName,out object responseCode)
        {//存储疑似生僻字
            string cmd = @"EXEC	@return_value = [dbo].[SP_VerifyAndAddMaybeSpellName]
		@Name = @Name,
		@Code =@Code,
		@result = @result OUTPUT";
            MaybeSpellName maybe = new MaybeSpellName() 
            {
                 Name=word.ToString(),
                 Code=spellName
            };
            return CommonRepository.RunProcedureNoQuery(cmd, SqlConnString, maybe, "@result", SqlDbType.Int, out responseCode) > 0;
        }

        public void BulkSave( List<MaybeSpellName> rows)
        {
             CommonRepository.BulkSave<MaybeSpellName>(typeof(MaybeSpellName).Name, SqlConnString, rows);
        }
    }
}
