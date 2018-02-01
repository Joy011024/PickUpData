using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
namespace HRApp.Model
{
    [Description("生僻字转拼音")]
    public class SpecialSpellName:SpellChineseWord
    {
        public int Id { get; set; }
        public int IsDeleted { get; set; }
        [Description("拼音错误")]
        public bool IsErrorSpell { get; set; }
    }
    [Description("汉字可能的拼音")]
    public class MaybeSpellName:SpellChineseWord
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateDayInt { get; set; }
        [Description("是否生僻字")]
        public bool IsSpecialChinese { get; set; }
        //
        string cmd = @"INSERT INTO  [dbo].[MaybeSpellName]   ([Id],[Name],[Code],[CreateTime],[CreateDayInt],IsSpecialChinese)
     VALUES  ({Id},{Name},{Code},{CreateTime},{CreateDayInt},{IsSpecialChinese})";
        public void SaveMaybeSpecialChinese(string name,string code) 
        {
            string cmd = @"EXEC	@return_value = [dbo].[SP_VerifyAndAddMaybeSpellName]
		@Name = N'篆',
		@Code = N'Zhuan',
		@result = @result OUTPUT";
        }
        public void InitParam() 
        {
            Id = Guid.NewGuid();
            CreateTime = DateTime.Now;
            CreateDayInt = int.Parse(CreateTime.ToString("yyyyMMdd"));
        }
    }
}
