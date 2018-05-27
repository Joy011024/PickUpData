using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using HttpClientHelper;
using DataHelp;
using System.Threading;
using Domain.CommonData;
using System.ComponentModel;
using Common.Data;
using System.Globalization;
using Infrastructure.MsSqlService.SqlHelper;
using DataHelp;
using CommonHelperEntity;
using Common.Data;
using Infrastructure.ExtService;
namespace CaptureWebData
{
    public enum EGender
    {
        [Description("所有")]
        All=0,
        [Description("男")]
        Men = 1,
        [Description("女")]
        Women = 2
    }
    public class QueryQQParam 
    {
        public int num = 30;//20
        public int page = 0;
        public int sessionid = 0;
        public string keyword { get; set; }
        public int agerg { get; set; }
        public int sex = 2;
        public int firston = 2;
        /// <summary>
        /// 有无摄像头
        /// </summary>
        public int video { get; set; }
        public string country = "1";//中国
        public string province = "11";//北京
        public string city ="8";//8 海淀
        public string district = "0";
        public string hcountry = "1";
        public string hprovince { get; set; }
        public string hcity { get; set; }
        public string hdistrict { get; set; }
        public int online = 1;//是否在线
        public int ldw = 215578015;
        public string UrlParam() 
        {
            string param = "num={num}&page={page}&sessionid={sessionid}&keyword={keyword}"
                + "&agerg={agerg}&sex={sex}&firston={firston}&video={video}&country={country}&province={province}"
                + "&city={city}&district={district}&hcountry={hcountry}&hprovince={hprovince}&hcity={hcity}&hdistrict={hdistrict}&online={online}&ldw={ldw}";
            param = param.Replace("{num}", num.ToString());
            param = param.Replace("{page}", page.ToString());
            param = param.Replace("{sessionid}", sessionid.ToString());
            param = param.Replace("{keyword}", keyword);
            param = param.Replace("{agerg}", agerg.ToString());
            param = param.Replace("{sex}", sex.ToString());
            param = param.Replace("{firston}", firston.ToString());
            param = param.Replace("{video}", video.ToString());
            param = param.Replace("{country}", country);
            param = param.Replace("{province}", province);
            param = param.Replace("{city}", city);
            param = param.Replace("{district}", district);
            param = param.Replace("{hcountry}", hcountry);
            param = param.Replace("{hprovince}", hprovince);
            param = param.Replace("{hcity}", hcity);
            param = param.Replace("{hdistrict}", hdistrict);
            param = param.Replace("{online}", online.ToString());
            param = param.Replace("{ldw}", ldw.ToString());
            return param;
        }
    }
    public class QQDataDA
    {
        string findQQAccountUrl = ConfigurationManager.AppSettings["QueryQQAccountUrl"];
        string findQQAcountRequestHeader = @"
Accept:application/json, text/javascript, */*; q=0.01
Accept-Encoding:gzip, deflate
Accept-Language:zh-CN,zh;q=0.8
Connection:keep-alive
Content-Length:172
Content-Type:application/x-www-form-urlencoded; charset=UTF-8
Cookie:pgv_pvi=2166054912; RK=5Q8eAGcqdH; tvfe_boss_uuid=4326f9f3a442deb4; pgv_si=s5240826880; pgv_info=ssid=s5407698800; pgv_pvid=3887722460; o_cookie=158055983; _qpsvr_localtk=tk3290; pac_uid=1_158055983; ptisp=; ptcz=50381d247d471856795f71bee19f08d2d437f1a1e9ecc4a4d47d90b0744a7ddf; pt2gguin=o0158055983; uin=o0158055983; skey=@tpsvitJnx; ptui_loginuin=158055983; itkn=610722576
Host:cgi.find.qq.com
Origin:http://find.qq.com
Referer:http://find.qq.com/index.html?version=1&im_version=5521&width=910&height=610&search_target=0
User-Agent:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";
        public QQDataDA() { }
        public QueryQQParam QueryParam { get; set; }
        DelegateData.BaseDelegate callback { get; set; }
        public QQDataDA(object param) 
        {
            object[] ps = (object[])param;
            string cookie = ps[0] as string;
            QueryParam = ps[1] as QueryQQParam;
            if (ps.Length > 2) 
            {
                callback = ps[2] as DelegateData.BaseDelegate;
            }
            if (!string.IsNullOrEmpty(cookie)) 
            {
                QueryTecentQQData(cookie);
            }
            
        }
        public PickUpQQDoResponse QueryQQData(string cookie)
        {
            if (string.IsNullOrEmpty(cookie))
            {
                return null;
            }
            return QueryTecentQQData(cookie);
        }
        private PickUpQQDoResponse QueryTecentQQData(string cookie) 
        {
            //从cookie中获取skey
            RequestHeaderHelper request = new RequestHeaderHelper();
            Dictionary<string,string> req= request.PickUpRequestHeader(findQQAcountRequestHeader);
            string key = request.SplitCookie(cookie)["skey"];
            //key = "@66i3M9kYA";//799103928
            int n = 5381;//这一步生成腾讯需要的key
            int cal; 
            for (int r = 0, i = key.Length; r < i; ++r)
            {
                char ch = (char)key.Substring(r, 1)[0];
                int acsii = (int)ch;
                n += (n << 5) + acsii;
            }
            QueryQQParam param = new QueryQQParam();
            if (QueryParam != null) 
            {
                param = QueryParam;
            }
            param.ldw = (n & 2147483647);
            object obj= ForeachFindQQ(param, cookie);
            System.Threading.Thread th = new Thread(new ThreadStart(delegate() {
                if (callback!=null)
                {
                    callback(obj);
                }
            }));
            th.Start();
            return (PickUpQQDoResponse)obj;
        }
        private PickUpQQDoResponse ForeachFindQQ(QueryQQParam param, string cookie)
        {
            string json = param.UrlParam();
            string logPath  = new ConfigurationItems().LogPath + GeneratePathTimeSpan(cookie);
            LoggerWriter.CreateLogFile(json, logPath, ELogType.ParamLog);
            string response = HttpClientExtend.HttpWebRequestPost(findQQAccountUrl, json, cookie);
            LoggerWriter.CreateLogFile(response, logPath, ELogType.DataLog);
            ConfigurationItems c = new ConfigurationItems();
            FindQQDataManage manage = new FindQQDataManage(c.TecentDA);
            JsonData jsondata = new JsonData();
            PickUpQQDoResponse pickup = new PickUpQQDoResponse();
            pickup.responseData= manage.SaveFindQQ(response);
            //此处开启一个线程查询qq群组

            pickup.cookie = cookie;
            pickup.request = json;
            pickup.responseJson = response;
            if (callback != null) 
            {
               
            }
            return pickup;
        }
        /// <summary>
        /// 根据提供的qqcookie获取默认的日志存储路径
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public string GetDefaultLogDir(string cookie) 
        {
            return new ConfigurationItems().LogPath + GeneratePathTimeSpan(cookie);
        }
        public string GetUinFromCookie(string cookie) 
        {
            string tag = "p_uin=o";
            if (!string.IsNullOrEmpty(cookie)&&cookie.Contains(tag))
            {//这是当前登录人的qq号
                string uin = cookie.Substring(cookie.IndexOf(tag) + tag.Length);
                uin = uin.Substring(0, uin.IndexOf(";"));
                return uin;
            }
            return string.Empty;
        }
        /// <summary>
        /// 根据登录qq用户账户以及时间戳生成日志部分路径
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public string GeneratePathTimeSpan(string cookie) 
        {
            string logPath = "\\" + DateTime.Now.ToString(CommonFormat.DateIntFormat);
            string tag = GetUinFromCookie(cookie);
            //当前周次
            GregorianCalendar gc = new GregorianCalendar(GregorianCalendarTypes.TransliteratedEnglish);
            int week = gc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Friday);
            tag +="\\"+ week;
            if (!string.IsNullOrEmpty(tag))
            {//这是当前登录人的qq号
                logPath = "\\" + tag + logPath;
            }
            return logPath;
        }
        public int TodayCountStatic() 
        {
            FindQQDataManage manage = new FindQQDataManage(new ConfigurationItems().TecentDA);
            return manage.CountTodayPickUp();
        }
        public PickUpStatic TodayStatic() 
        {
            FindQQDataManage manage = new FindQQDataManage(new ConfigurationItems().TecentDA);
            return manage.TodayStatic();
        }
        public string QQGroupGather(string  cookie,UinGroupDataRequestParam param)
        {//共同的cookie项： pgv_pvi, pgv_pvid,pgv_si ,RK,uin,o_cookie,ptui_loginuin,ptisp,pt2gguin,uin,skey,itkn
            //缺少项：    
            // 可去除项： 
            string recommandurl = "http://qun.qq.com/cgi-bin/qunapp/recommend2";//这是推荐的qq群
            string url="http://qun.qq.com/cgi-bin/group_search/pc_group_search";
            string requestHeader = @"Accept:application/json, text/javascript, */*; q=0.01
Accept-Encoding:gzip, deflate
Accept-Language:zh-CN,zh;q=0.8
Connection:keep-alive
Content-Length:74
Content-Type:application/x-www-form-urlencoded; charset=UTF-8
Cookie:{Cookie}
Host:qun.qq.com
Origin:http://find.qq.com
Referer:http://find.qq.com/index.html?version=1&im_version=5521&width=910&height=610&search_target=0
User-Agent:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36";
            Dictionary<string, string> head = new Dictionary<string, string>();
            string ck="{Cookie}";
            Dictionary<string,string> hs= new RequestHeaderHelper().PickUpRequestHeader(requestHeader);
            foreach (KeyValuePair<string,string> item in head)
            {
                if (item.Value == ck)
                {
                    head[item.Key] = cookie;
                    break;
                }
            }

            string generate = @"Request URL:http://qun.qq.com/cgi-bin/qunapp/recommend2
Request Method:POST
Status Code:200 OK
Remote Address:182.254.104.46:80
Referrer Policy:no-referrer-when-downgrade";
            string form = @"k:交友
n:8
st:1
iso:1
src:1
v:4903
bkn:1053723692
isRecommend:false
city_id:10059
from:1
newSearch:true
keyword:白羊座
sort:0
wantnum:24
page:0
ldw:1053723692";
            //cookie:tvfe_boss_uuid=1e6199e1d2117b2e; pgv_pvi=2689650688; RK=jY8eVEcaan; luin=o0158055983; lskey=0001000072112c965a16959759ae4ea12f3723377b617431ab54af275b17458151ca356e5e7c02ddf05d2898; o_cookie=158055983; pgv_pvid=280615424; pgv_si=s1233967104; ptui_loginuin=1281756329; ptisp=cnc; ptcz=a13b68ec1bc3d52e50539dce656d1c5dddd67990597a5f892944921a0910ae37; pt2gguin=o1281756329; uin=o1281756329; skey=@JcISofHYC
            requestHeader = requestHeader.Replace(ck, cookie);
            param.CalculateUinJsParam(cookie);
            string ps = param.ConvertJson();
            string result = HttpClientExtend.HttpWebRequestPost(url, ps, cookie);
            LoggerWriter.CreateLogFile(result, GetDefaultLogDir(cookie), ELogType.SpliderGroupDataLog);
            return result;
        }
    }
    /// <summary>
    /// 请求完毕返回数据
    /// </summary>
    public class PickUpQQDoResponse
    {
        public string cookie { get; set; }
        public string request { get; set; }
        public string responseJson { get; set; }
        public FindQQResponse responseData { get; set; }

    }
    public class UinDataSyncHelp
    {
        public string SyncToCodeDB(int number,string waitSycnDBName) 
        {
            return @"insert into tecentdatada.dbo.tecentqqdata
(ID,PickUpWhereId,age,city,country,distance,face,gender,nick,province,stat,uin,HeadImageUrl,CreateTime,ImgType)
select top {number} 
ID,PickUpWhereId,age,city,country,distance,face,gender,nick,province,stat,uin,HeadImageUrl,CreateTime,ImgType
 from dbo.tecentqqdata t
where not exists (select id from dbo.SyncFlag where id=t.id)
and not exists (select id from tecentdatada.dbo.tecentqqdata where id=t.id);
--update SyncFlag
insert into SyncFlag  (id,SyncTime)
select top  {number}  id,getdate()
from dbo.tecentqqdata t 
where not exists (select id from dbo.SyncFlag where id=t.id)
and not exists (select id from tecentdatada.dbo.tecentqqdata where id=t.id)"
                .Replace("{waitSycnDBName}", waitSycnDBName).Replace("{number}", number.ToString());
        }
        public void DoIntervalSync(string connDBString) 
        {
            SqlCmdHelper help = new SqlCmdHelper() { SqlConnString = connDBString };
            string[] dbArr = connDBString.Split(';');
            string waitSyncDB = string.Empty;
            foreach (var item in dbArr)
            {
                if (item.Contains("Initial Catalog"))
                {
                    waitSyncDB = item.Split('=')[1].Trim();
                }
            }
            string sql = SyncToCodeDB(200, waitSyncDB);
            string time = DateTime.Now.ToString(CommonFormat.DateTimeFormat);
            LogHelperExt.WriteLog("will Sync uin in  " + waitSyncDB + ".dbo.tecentqqdata  data,time=" + time);
            try
            {
                if (ConfigurationItems.OupputSql)
                {
                    LogHelperExt.WriteLog("will exucte sql= " + sql);
                }
                help.ExcuteNoQuery(sql, null);
                string endTime = DateTime.Now.ToString(CommonFormat.DateTimeFormat);
                LogHelperExt.WriteLog("end Sync  uin data,time=" + endTime);
            }
            catch (Exception ex)
            {
                string endTime = DateTime.Now.ToString(CommonFormat.DateTimeFormat);
                LogHelperExt.WriteLog("Sync uin data error \r\n" + endTime + " \r\n" + ex.Message);
            }
        }
        
    }
    public class LogHelperExt 
    {
        public static void WriteLog(string text)
        {
            LoggerWriter.CreateLogFile(text,
                new AppDirHelper().GetAppDir(AppCategory.WinApp) + "/" + typeof(ELogType).Name,
                ELogType.DebugData, DateTime.Now.ToString(CommonFormat.DateIntFormat) + ".log", true);
        }
    }
}
