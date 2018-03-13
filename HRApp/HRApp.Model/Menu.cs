using Domain.CommonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRApp.Model
{
    public class Menu : BaseIntPrimary
    {
        [DescriptionSort("菜单名称")]
        public string Name { get; set; }
        [DescriptionSort("编码【默认为菜单的拼音】")]
        public string Code { get; set; }
        [DescriptionSort("描述")]
        public string Remark { get; set; }
        public string Url { get; set; }
        public DateTime CreateTime { get; set; }
        public string InserSql()
        {
            return @"INSERT INTO [dbo].[Menu] ([Name],[Code],[Url],[Remark],[CreateTime])
     VALUES  ({Name},{Code},{Url},{Remark},{CreateTime}) ";
        }
        public string QueryMenus()
        {
            return " select [Id], [Name],[Code],[Url],[Remark],[CreateTime]  from [dbo].[Menu] ";
        }
    }
}
