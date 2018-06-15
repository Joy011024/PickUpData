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
        [DescriptionSort("数据来源：数据库")]
        DB=1,
       [DescriptionSort("数据来源：API接口")]
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
    [Domain.GlobalModel.TableField(CanIgnoreAsTable=true)]
    public class GlobalSetting
    {//这里面的字段均来自于EAppSetting 枚举成员
        public bool ForceRefreshServiceAppSetting { get; set; }
        public bool UsingRedisCache { get; set; }
        public int DefaultQueryLimit { get; set; }
        public int DefaultQuerySize { get; set; }
        public bool UsingEmail { get; set; }
        [Domain.GlobalModel.PropertyIgnoreField]
        public short GridMaxLimitNum { get; set; }//调用接口时候进行校验
        [Domain.GlobalModel.PropertyIgnoreField]
        public short GridDefaultLimitNum { get; set; }//返回UI时的参数
        public short GridLimitNum { get; set; }//实际情况
        [Domain.GlobalModel.PropertyIgnoreField]
        public short NonGridLimitMaxNum { get; set; }
        [Domain.GlobalModel.PropertyIgnoreField]
        public short NonGridLimitDefaultNum { get; set; }
        public short NoGridLimitNum { get; set; }//实际情况
        [Domain.GlobalModel.PropertyIgnoreField]
        public string ReceiverInEmailActive { get; set; }//进行邮件系统激活时邮件接收人
        public string MailInEmailActive { get; set; } //激活邮件时抄送人
    }
    public enum EAppSetting 
    {
        [DescriptionSort("强制刷新服务端配置")]
        ForceRefreshServiceAppSetting=100,
        [DescriptionSort("UI配置")]
        UiAppSetting = 200,
        [DescriptionSort("文件日期戳格式")]
        FileVersionFormat=210,
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
        [DescriptionSort("默认Grid查询数目")]
        DefaultQuerySize=7300,
        [DescriptionSort("举报分类")]
        ReportEnum=8000,
        [DescriptionSort("系统邮件账户配置")]
        SystemEmailSetting = 9000,
        [DescriptionSort("系统邮件账户")]
        SystemEmailSendBy=9100,
        [DescriptionSort("邮件授权码")]
        SystemEmailSMPTAuthor=9110,
        [DescriptionSort("SMTP服务类型")]
        SMTP=9111,
        [DescriptionSort("邮箱客户端")]
        SMTPClient=9200,
        [DescriptionSort("启用邮箱功能")]
        UsingEmail=400

    }
    [DescriptionSort("任职状态")]
    public enum EmployerServeStatue
    { 
        [DescriptionSort("任职")]
        Serve=1,
        [DescriptionSort("离职")]
        Departure= 2,
        [DescriptionSort("晋升")]
        Promotion=3,
        [DescriptionSort("平调")]
        ChangeOrganze=4,
        [DescriptionSort("卸任")]
        Resign=5,
        [DescriptionSort("退休")]
        Retirement=6
    }
    public enum EnumSMTP
    { 
        [DescriptionSort("腾讯SMTP服务")]
        QQ=1,
        [DescriptionSort("网易SMTP服务")]
        NETS163= 2
    }
    public enum EnumEmailBodyType
    { 
        [DescriptionSort("邮件内容")]
        Body=1,
        [DescriptionSort("邮件日志文本")]
        TextPath=2
    }
    public enum EMenuType
    { 
        [DescriptionSort("页面元素")]
        PageElement=1,
        [DescriptionSort("菜单父节点")]
        MenuRoot=2,
        [DescriptionSort("菜单")]
        MenuNode=3
    }
}
