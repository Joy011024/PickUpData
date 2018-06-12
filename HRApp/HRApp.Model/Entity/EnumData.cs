using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
using Domain.GlobalModel;
namespace HRApp.Model
{
    [TableField(TableName = "Enum", DbGeneratedFields = new string[] { "Id" })]
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
        public string QueryEnumMembersSqlFormat(string[] columns,bool isContainerDelete)
        {
            string sql= @"SELECT  {Columns}
              FROM [dbo].[Enum] where ParentId =
              (
	            SELECT id from dbo.enum where Code={Code}
              )".Replace("{Columns}", string.Join(",", columns));
            if (!isContainerDelete)
            {
                sql += " and IsDelete=0 ";
            }
            return sql;
        }
    }
}
