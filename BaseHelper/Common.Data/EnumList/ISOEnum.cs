using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Data
{
    public enum EISOSex
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知性别")]
        Unknow = 0,
        /// <summary>
        /// 男性
        /// </summary>
        [Description("男")]
        Men = 1,
        [Description("女")]
        Women = 2,
        [Description("女变男")]
        WomenToWen=5,
        [Description ("男变女")]
        WenToWomen=6,
        [Description("未定义的性别")]
        Undefined=9 
    }
    public enum EHttpRequestCategory
    {
        [Description("Get请求")]
        Get=1,
        [Description("Post请求")]
        Post=2
    }
    [Description("数据来源类别")]
    public enum DataFromEnum
    {
        [Description("未知来源")]
        Unknow = 0,
        [Description("缓存")]
        Cache = 1,
        [Description("SqlServer数据库")]
        SqlServer = 2,
        [Description("MySQL数据库")]
        MySql = 3,
        [Description("Oracle数据库")]
        Oracle = 4,
        [Description("模板文件")]
        TemplateFile = 5,
        [Description("网络")]
        Network = 6
    }
}
