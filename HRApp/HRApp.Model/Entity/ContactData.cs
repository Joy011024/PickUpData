using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRApp.Model
{
    public class ContactData
    {
        [Domain.CommonData.DescriptionSort("主键")]
        public Guid Id { get; set; }
        [Domain.CommonData.DescriptionSort("联系人类型 外键源自CategoryItems表Id")]
        public int ContactTypeId { get; set; }
        [Domain.CommonData.DescriptionSort("联系方式")]
        public string Value { get; set; }
        [Domain.CommonData.DescriptionSort("联系人")]
        public string ContactName { get; set; }
        [Domain.CommonData.DescriptionSort("联系人归属者（未设置默认为-1）")]
        public string Belonger { get; set; }
        public DateTime CreateTime { get; set; }
        public void InitData() 
        {
            Id = Guid.NewGuid();
            Belonger = "-1";
            CreateTime = DateTime.Now;
        }
        [Domain.CommonData.DescriptionSort("描述：最大长度128")]
        public string Desc { get; set; }
        public string InsertSql() 
        {
            return @"INSERT INTO [HrApp].[dbo].[ContactData] ([Id],[ContactTypeId],[Value],[ContactName],[Belonger],[Createtime],[Desc])
     VALUES ({Id},{ContactTypeId},{Value},{ContactName},{Belonger},{CreateTime},{Desc})";
        }
    }
}
