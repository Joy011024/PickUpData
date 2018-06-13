using Domain.CommonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.GlobalModel;
namespace HRApp.Model
{
    [TableField(DbGeneratedFields = new string[] { "Id" })]
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
        public int ParentId { get; set; }
        public short MenuType { get; set; }
        [DescriptionSort("是否启用")]
        public bool IsEnable { get; set; }
        public string InserSql()
        {
            return @"INSERT INTO [dbo].[Menu] ([Name],[Code],[Url],[Remark],[CreateTime],[ParentId],[MenuType])
     VALUES  ({Name},{Code},{Url},{Remark},{CreateTime},{ParentId},{MenuType}) ";
        }
        public string QueryMenus()
        {
            return " select [Id], [Name],[Code],[Url],[Remark],[CreateTime],[ParentId],[MenuType]  from [dbo].[Menu] ";
        }
        public string ChangeStatueSql() 
        {
            return "Update Menu set IsEnable={IsEnable} where Id={Id}";
        }
        public string ChangeMenuTypeSql()
        {
            return "Update Menu set MenuType={MenuType} where Id={Id}";
        }
        public string ChangeParentMenuSql() 
        {
            return "Update Menu set ParentId={ParentId} where Id={Id} and {ParentId}<>{Id}";
        }
    }
    public class UinMenuObjcet:Menu
    {
        #region ui
        public bool IsChild { get; set; }
        public string ParentCode { get; set; }
        public List<Menu> Childerns { get; set; }
        [DescriptionSort("父节点名")]
        public string ParentName { get; set; }
        public string Tip { get; set; }
        #endregion
    }
}
