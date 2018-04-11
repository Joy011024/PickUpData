﻿using System;
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
        string GetQueryModelSampleSql()
        {
            return @"SELECT [ID],[Name],[ParentID],[ParentCode],[Code],[Sort],[IsDelete],[ItemValue],[ItemUsingSize],[CreateTime],[NodeLevel],[ItemDesc]
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
        public static string BuilderValideSql() 
        {
            return @"select count(id) from CategoryItems where code=@code";
        }
    }
}
