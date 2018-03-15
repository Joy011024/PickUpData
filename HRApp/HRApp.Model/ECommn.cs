using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
namespace HRApp.Model
{
    [DescriptionSort("数据来源分类")]
    public enum EDataFrom
    {
        DB=1,
        WebAPI=2
    }
    [DescriptionSort("以下均为定制化配置，父节点对应鸽子默认配置的父节点下")]
    public enum SpecialExt
    {
        /*
         假如要查询UI你表数据，设置默认配置项DefaultQueryLimit 值为100条，但是这个表要查询200条，
         可以在DefaultQueryLimit节点下增加子节点 命名=UinDefaultQueryLimit 
         */
        [DescriptionSort("查询数量限制（table+QueryLimit）")]
        QueryLimit=1,
        [DescriptionSort("数据来源( 数据+DataFrom )")]
        DataFrom=2
    }
    [DescriptionSort("默认配置")]
    public enum DefaultSet
    {
        [DescriptionSort("默认查询的数据量")]
        DefaultQueryLimit=1,
        [DescriptionSort("默认数据来源")]
        DefaultDataFrom=2,
        [DescriptionSort("默认数据库在相同的服务器")]
        DefaultDBIsSameServicePC=3
    }
}
