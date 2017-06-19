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
        public void QueryQQData(string cookie) 
        {
            if(string.IsNullOrEmpty(cookie))
            {
                return;
            }
            QueryTecentQQData(cookie);
        }
        private void QueryTecentQQData(string cookie) 
        {
            //从cookie中获取skey
            RequestHeaderHelper request = new RequestHeaderHelper();
            Dictionary<string,string> req= request.PickUpRequestHeader(findQQAcountRequestHeader);
            string key = request.SplitCookie(cookie)["skey"];
            //key = "@66i3M9kYA";//799103928
            int n = 5381;
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
            if (callback != null) 
            {
                pickup.cookie = cookie;
                pickup.request = json;
                pickup.responseJson = response;
            }
            return pickup;
        }
        public string GetUinFromCookie(string cookie) 
        {
            string tag = "uin=o";
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
}
