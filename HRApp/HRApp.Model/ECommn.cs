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
    public enum AppSetting 
    {
        [DescriptionSort("强制刷新服务端配置")]
        ForceRefreshServiceAppSetting=100,
        [DescriptionSort("UI配置")]
        UiAppSetting = 200,
        [DescriptionSort("动作分类")]
        OptionType=300,
        [DescriptionSort("点赞")]
        SupportClick=310,
        [DescriptionSort("厌恶")]
        HeatClick=320,
        [DescriptionSort("启用redis缓存")]
        UsingRedisCache=1000,
        [DescriptionSort("redis缓存丢失查询数据源")]
        RedisLoseDataUsingDataSource=2000,
        [DescriptionSort("12306刷票频率")]
        RefreshHZIn12306=2100,
        [DescriptionSort("12306停止刷票时间间隔")]
        StopRefreshTimeSpanIn12306=2200,
        [DescriptionSort("12306开始启动刷票时间间隔")]
        StartRefreshTimeSpanIn12306=2300,
        [DescriptionSort("联系人类型管理")]
        LianXiRenLeiXingGuanLi=3000,
        [DescriptionSort("QQ群")]
        QQGroup=3010,
        [DescriptionSort("微信群")]
        WebChatGroup=3020,
        [DescriptionSort("系统轮询调度的时间间隔")]
        XiTongLunXunDiaoDuDeShiJianJianGe=4000,
        [DescriptionSort("腾讯爬虫配置")]
        TecentSpilderSet=4010,
        [DescriptionSort("腾讯爬虫间隔")]
        TecentPickUpTicket=4020,
        [DescriptionSort("账户举报分类")]
        AccountComplaintsType=5000,
        [DescriptionSort("跨库配置说明")]
        CallOtherSqlDBSet=6000,
        [DescriptionSort("数据来源")]
        DataFrom=7000,
        [DescriptionSort("默认查询数据来源")]
        DefaultDataFrom = 7100,
        [DescriptionSort("默认单次查询数据量")]
        DefaultQueryLimit=7200,
        [DescriptionSort("默认查询数目")]
        DefaultQuerySize=7300
    }
}
