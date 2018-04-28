using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
using System.ComponentModel;
namespace HRApp.Model
{
    public class CategoryItems : CategoryData
    {
        [Description("配置项被引用次数")]
        public int ItemUsingSize { get; set; }
        [Description("内容项的描述")]
        public string ItemDesc { get; set; }
        [Description("配置的值")]
        public string ItemValue { get; set; }
        [Description("索引拼音")]
        public string IndexSpell { get; set; }
        string GetQueryModelSampleSql()
        {
            return @"SELECT [ID],[Name],[ParentID],[ParentCode],[Code],[Sort],[IsDelete],[ItemValue],[ItemUsingSize],[CreateTime],[NodeLevel],[ItemDesc],[IndexSpell]
  FROM [dbo].[CategoryItems] ";
        }
        public string BuilderSqlParam()
        {
            string sql = GetQueryModelSampleSql() + " where ParentCode=@code";
            return sql;
        }
        [DescriptionSort("查询全部配置")]
        public string QueryAllDataOfSql() 
        {
            return GetQueryModelSampleSql();
        }
        public string GetFirstOneSql() 
        {
            return GetQueryModelSampleSql() + " where ID={Id}";
        }
        public static string BuilderValideSql() 
        {
            return @"select count(id) from CategoryItems where code=@code";
        }
        /// <summary>
        /// 修改的SQL
        /// </summary>
        /// <returns></returns>
        public   string GetUpdateSql() 
        {
            return @"UPDATE [HrApp].[dbo].[CategoryItems]
                       SET [Name] = {Name}
                          ,[Code] = {Code}
                          ,[Sort] = {Sort}
                          ,[IsDelete] = {IsDelete}
                          ,[ItemValue] = {ItemValue}
                          ,[ItemUsingSize] ={ItemUsingSize}
                          ,[CreateTime] = {CreateTime}
                          ,[NodeLevel] = {NodeLevel}
                          ,[ItemDesc] = {ItemDesc}
                          ,[IndexSpell] = {IndexSpell}
	                      where ID={Id}";
        }
        /// <summary>
        /// 修改检索关键字的SQL
        /// </summary>
        /// <returns></returns>
        public string GetChangeSpellWord() 
        {
            return @"UPDATE [HrApp].[dbo].[CategoryItems]  SET [IndexSpell] = {IndexSpell}   where ID={Id}";
        }
        public string GetInsertSql() 
        {
            return @"insert into CategoryItems([Name],[ParentID],[ParentCode],[Code],[Sort],[IsDelete],[ItemUsingSize],[CreateTime],[NodeLevel],[ItemDesc],[ItemValue],[IndexSpell])
values({Name},{ParentId},{ParentCode},{Code},{Sort},{IsDelete},{ItemUsingSize},{CreateTime},{NodeLevel},{ItemDesc},{ItemValue},{IndexSpell})";
        }
        public string GetQueryByIndexSpell() 
        {
            return GetQueryModelSampleSql() + " where IndexSpell like {IndexSpell}";
        }
    }
}
