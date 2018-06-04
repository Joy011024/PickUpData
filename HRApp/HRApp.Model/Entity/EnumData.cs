using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
using Domain.GlobalModel;
namespace HRApp.Model
{
    [TableField(TableName = "Enum")]
    public class EnumData : WithLogicFieldWithInt
    {
        public int ParentId { get; set; }
        public int Value { get; set; }
        public string Remark { get; set; }
        public void Init() 
        {
            ParentId = -1;
            CreateTime = DateTime.Now;
            Value = -1;
            Remark = "System Value";
            IsDelete = false;
        }
        public string GetUpdateRemarkSql() 
        {
            return "update Enum set Remark={Remark} where Id={Id}";
        }
    }
}
