using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using HttpClientHelper;
using Domain.CommonData;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading;
namespace GatherIdCard
{
    class Program
    {
        static void Main(string[] args)
        {
            //Thread[] th = new Thread[2];
            //th[0] = new Thread(QueryUserRemarkLossId);
            //th[0].Start();
            GetEduList();
           // RegexSpilder();
            //GatherMoMoHoster();
            Console.ReadLine();
            //QueryUserRemarkLossId();
        }
        static void GatherUserInfo()
        {
            HtmlElementAnalysis.IdgxskyHtmlAnalysic(AppConfig.FileSaveDir);
        }
        void GatherIdCardNews() 
        {
            int initPageIndex = 1;
            int pages = 300;
            //采集用户身份证【挂失】
            while (initPageIndex < pages)
            {
                string url = string.Format(AppConfig.GatherPersonInfor, initPageIndex);
                QueryIdCard(url);
                initPageIndex++;
            }
        }

        static void GatherUserIdCard()
        {// 南宁市 | 柳州市 | 桂林市 | 梧州市 | 北海市 | 钦州市 | 贵港市 | 玉林市 | 百色市 | 防城港市|贺州市 | 河池市 | 来宾市 | 崇左市 | 外省地区
            string[] city = new string[] { 
                "%C4%CF%C4%FE%CA%D0", "%C1%F8%D6%DD%CA%D0", "%B9%F0%C1%D6%CA%D0", "%CE%E0%D6%DD%CA%D0", "%B1%B1%BA%A3%CA%D0","%C7%D5%D6%DD%CA%D0"
                ,"%B9%F3%B8%DB%CA%D0","%D3%F1%C1%D6%CA%D0","%B0%D9%C9%AB%CA%D0","%B7%C0%B3%C7%B8%DB%CA%D0","%BA%D8%D6%DD%CA%D0","%BA%D3%B3%D8%CA%D0"
                ,"%C0%B4%B1%F6%CA%D0","%B3%E7%D7%F3%CA%D0","%CD%E2%CA%A1%B5%D8%C7%F8"
            };
            int cityIndex = 0;
            int initPageIndex = 1;
            int pages = 300;
            //采集用户身份证【挂失】
            while (initPageIndex < pages)
            {
                string url = string.Format(AppConfig.GatherIdCardUrl, initPageIndex, city[cityIndex]);
                QueryIdCard(url);
                initPageIndex++;
                if (cityIndex < city.Length && initPageIndex == pages)
                {
                    cityIndex++;
                    initPageIndex = 0;
                }
            }
        }
        static void QueryIdCard(string url)
        {
            string requestParam = @"Accept:text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8
Accept-Encoding:gzip, deflate
Accept-Language:zh-CN,zh;q=0.8
Cache-Control:max-age=0
Connection:keep-alive
Cookie:ASPSESSIONIDSQABCDRA=AJCBIMLAFHDDPOKGJDPBGIHB; gxskydzx31_f5ee_saltkey=V8j84g4P; gxskydzx31_f5ee_lastvisit=1503227237; gxskydzx31_f5ee_sid=wL5R7g; gxskydzx31_f5ee_lastact=1503230837%09nav_js.php%09; UM_distinctid=15dff8a57c50-0ad597d1229d24-474a0521-15f900-15dff8a57c61; Card3=Url=; ASPSESSIONIDQQCACCRB=PBHHKPLACDPLIPHOAFPIJOBI; CNZZDATA1259785388=339484251-1503230374-%7C1503230374
Host:idcard.gxsky.com
Referer:{url}
Upgrade-Insecure-Requests:1
User-Agent:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36";
            requestParam = requestParam.Replace("{url}", url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            //request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:19.0) Gecko/20100101 Firefox/19.0";
            request.KeepAlive = false;
            request.AllowWriteStreamBuffering = true;//对响应数据进行缓冲处理
            try
            {
                HttpWebResponse webResponse = null;
                Stream str = null;
                webResponse = (HttpWebResponse)request.GetResponse();//此处有坑【如果http响应超时此处会抛出异常】
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {//http请求失败

                }
                str = webResponse.GetResponseStream();
                GZipStream gzip = new GZipStream(str, CompressionMode.Decompress);//解压缩
                Encoding enc = Encoding.GetEncoding("gb2312");
                StreamReader sr = new StreamReader(gzip, enc);
                string text = sr.ReadToEnd();
                str.Dispose();
                gzip.Close();
                sr.Close();
               // HttpClientExtend.GetHttpResponseData(url);
                AssemblyDataExt ass = new AssemblyDataExt();
                string dir = ass.ForeachDir(ass.GetAssemblyDir(), 3);
                LoggerWriter.CreateLogFile(text, dir + "/" + ELogType.DataLog, ELogType.DataLog);
            }
            catch (Exception ex)
            {
                return;
            }
            
        }
        static void QueryUserRemarkLossId() 
        {
            int cur = 1;

            string text = GetRequestData(string.Format(AppConfig.UserRemarkLoseIdCard, cur));
            
            string startRegex = "<form method=post";
            if (!text.Contains(startRegex))
            {
                return;
            }
            int position = text.IndexOf(startRegex);
            string page = text.Substring(position + startRegex.Length);
            string endRegex = "<input";
            string releate= page.Substring(0, page.IndexOf(endRegex));
            AssemblyDataExt ass = new AssemblyDataExt();
            string dir = ass.ForeachDir(ass.GetAssemblyDir(), 3);
            LoggerWriter.CreateLogFile(text, dir + "/" + ELogType.DataLog, ELogType.DataLog);
            string sign = "#ff0000'>";
            releate = releate.Substring(releate.LastIndexOf(sign) + sign.Length);
            releate = releate.Substring(0, releate.LastIndexOf("</b>"));
            Regex reg = new Regex("([0-9]+)");
            MatchCollection list= reg.Matches(releate);
            //--->"3500</font></b>条记录 <b>35"
            Match total = list[0];// total
            Match limit = list[1];
            int sum = int.Parse(total.ToString());
            int size = int.Parse(limit.ToString());
            int pages = (sum / size) + (sum % size > 0 ? 1 : 0);
            cur++;
            while (cur <= pages)
            {
                text = GetRequestData(string.Format(AppConfig.UserRemarkLoseIdCard, cur));
                cur++;
                if (string.IsNullOrEmpty(text))
                {
                    continue;
                }
                LoggerWriter.CreateLogFile(text, dir + "/" + ELogType.DataLog, ELogType.DataLog);
            }
        }
        static string GetRequestData(string url) 
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            //request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:19.0) Gecko/20100101 Firefox/19.0";
            request.KeepAlive = false;
            request.AllowWriteStreamBuffering = true;//对响应数据进行缓冲处理
            try
            {
                HttpWebResponse webResponse = null;
                Stream str = null;
                webResponse = (HttpWebResponse)request.GetResponse();//此处有坑【如果http响应超时此处会抛出异常】
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {//http请求失败

                }
                str = webResponse.GetResponseStream();
                GZipStream gzip = new GZipStream(str, CompressionMode.Decompress);//解压缩
                Encoding enc = Encoding.GetEncoding("gb2312");
                StreamReader sr = new StreamReader(gzip, enc);
                string text = sr.ReadToEnd();
                str.Dispose();
                gzip.Close();
                sr.Close();
                // HttpClientExtend.GetHttpResponseData(url);
                return text;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        static void GetEduList() 
        {
            AppConfig app = new AppConfig();
            HttpHelper http = new HttpHelper();
            string response = http.GetHttpResponse(AppConfig.EduListUrl + app.FormatEduComUrlParam("普通本科", 1, 30), string.Empty);
            LoggerWriter.CreateLogFile(response, AppDir.GetParentDir(3)+ "/" + ELogType.SpliderDataLog, ELogType.DataLog);
        } 
        static void GatherMoMoHoster() 
        {
            //1  获取 cookie 
            string cookieUrl = "https://web.immomo.com/fonts/fontawesome-webfont.woff2?v=4.4.0";
            string cookie = GetRequestData(cookieUrl);
            string requestHead = @"Accept:*/*
Accept-Encoding:gzip, deflate, br
Accept-Language:zh-CN,zh;q=0.8
Connection:keep-alive
Cookie:MMID=fc1dce98c1610ba745e394d25531d844; MMSSID=a6efe97f91e8449bc71eab3b266bc391; Hm_lvt_96a25bfd79bc4377847ba1e9d5dfbe8a=1504194645; Hm_lpvt_96a25bfd79bc4377847ba1e9d5dfbe8a=1504194645; __v3_c_sesslist_10052=et6xh8sbra_dfl; __v3_c_pv_10052=1; __v3_c_session_10052=1504194642159238; __v3_c_today_10052=1; __v3_c_review_10052=0; __v3_c_last_10052=1504194642159; __v3_c_visitor=1504194642159238; __v3_c_session_at_10052=1504194646409; s_id=5d189f720ca0f72e22f624746c7be202; cId=19464624661178; L_V_T=d91ade7b-edc7-403b-ad79-fb6bcfb2c5cc; L_V_T.sig=ZhuK-3ov4Zua3cE3yFu3-RV0Kho; webmomo.sig=4TdIPyaRNmlicDRBBcVgoWQIgn8; web-imi-bew=s%3A254863613.RZ6mavZhWIGwrD1%2FMftKB6s7URkLQ3hWgdLWAafNrO8; web-imi-bew.sig=uEejlS3DD0RtEWvpYs5MUQzZs8c; io=z7C9eol9rTu_LwcyBZ4P; Hm_lvt_c391e69b0f7798b6e990aecbd611a3d4=1504194650,1504194739,1504195035; Hm_lpvt_c391e69b0f7798b6e990aecbd611a3d4=1504195127
Host:web.immomo.com
Referer:https://web.immomo.com/
User-Agent:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36
X-Requested-With:XMLHttpRequest";
            string text= GetRequestData(AppConfig.MoMoHoster);
        }
        static void RegexSpilder()
        {
            string path = @"F:\SecurityData\IDCard\DebugIdCard\DataLog";
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fis = di.GetFiles("*.txt");
            Console.WriteLine("start->" + DateTime.Now.ToString(AppConfig.DateTimeFormat));
            DateTime create = fis.Min(f => f.CreationTime);
            FileInfo file = fis.Where(f => f.CreationTime == create).FirstOrDefault();
            Console.WriteLine("query target file name->"+DateTime.Now.ToString(AppConfig.DateTimeFormat));
            //提取html
            string html= FileHelper.ReadFile(file.FullName);
            Regex reg = new Regex("<tbody.*(?=>)(.|\n)*?</tbody>$");//"<tbody>([^</tbody>]*)"
            //((?<Nested><\k<HtmlTag>[^>]*>)|</\k<HtmlTag>>(?<-Nested>)|.*?)*</\k<HtmlTag>>
            // ((?<Nested><\k<HtmlTag>[^>]*>)|</\k<HtmlTag>>(?<-Nested>)|.*?)*</\k<HtmlTag>>
            MatchCollection mc= reg.Matches(html);
            Console.WriteLine("Analysic one file end->" + DateTime.Now.ToString(AppConfig.DateTimeFormat));
            foreach (Match item in mc)
            {
                int total = item.Groups.Count;
                foreach (Group ele in item.Groups)
                {
                    Console.WriteLine(ele.Value);
                }
            }
        }
    } 
    public class AppConfig
    {
        static string idCardUrl;
        public static string GatherIdCardUrl
        {
            get
            {
                if (string.IsNullOrEmpty(idCardUrl))
                {
                    idCardUrl = ConfigurationManager.AppSettings["GatherIdCardUrl"];
                }
                return idCardUrl;
            }
        }

        static string userInfo;
        public static string GatherPersonInfor
        {
            get
            {
                if (string.IsNullOrEmpty(userInfo))
                {
                    userInfo = ConfigurationManager.AppSettings["GatherPersonInfor"];
                }
                return userInfo;
            }
        }
        static string fileSaveDir;
        public static string FileSaveDir
        {
            get
            {
                if (string.IsNullOrEmpty(fileSaveDir))
                {
                    fileSaveDir = ConfigurationManager.AppSettings["FileSaveDir"];
                }
                if (string.IsNullOrEmpty(fileSaveDir))
                {
                    AssemblyDataExt ass = new AssemblyDataExt();
                    fileSaveDir = ass.ForeachDir(ass.GetAssemblyDir(), 3);
                }
                return fileSaveDir;
            }
        }

        static string userRedmarkLoseId;
        public static string UserRemarkLoseIdCard
        {
            get
            {
                if (string.IsNullOrEmpty(userRedmarkLoseId))
                {
                    userRedmarkLoseId = ConfigurationManager.AppSettings["UserRemarkLoseIdCard"];
                }
                return userRedmarkLoseId;
            }
        }
        static string momoHoster;
        public static string MoMoHoster
        {
            get 
            {
                if (string.IsNullOrEmpty(momoHoster))
                {
                    momoHoster = ConfigurationManager.AppSettings["MoMoHoster"];
                }
                return momoHoster;
            }
        }
        static string dateTimeformat;
        public static string DateTimeFormat
        {
            get 
            {
                if (string.IsNullOrEmpty(dateTimeformat))
                    dateTimeformat = ConfigurationManager.AppSettings["DateTimeFormat"];
                return dateTimeformat;
            }
        }
        static string eduComList;
        /// <summary>
        /// 大学数据URL
        /// </summary>
        public static string EduListUrl 
        {
            get 
            {
                if (string.IsNullOrEmpty(eduComList))
                    eduComList = ConfigurationManager.AppSettings["EduListUrl"];
                return eduComList;
            }
        }
        /// <summary>
        /// 学校数据采集URL参数格式化
        /// </summary>
        string EduComListParam 
        {
            get
            {
                string param = @"province:
schooltype:{0}
page:{1}
size:{2}
callback:jQuery18301668075825546702_1505119180990
keyWord1:
schoolprop:
schoolflag:
schoolsort:
schoolid:";//{type}普通本科
                string[] ps = param.Split(new string[] { "\r\n" },StringSplitOptions.None);
                return string.Join("\r\n", ps);
            }
        }
        /// <summary>
        /// 组装学校数据采集URL参数
        /// </summary>
        /// <param name="schoolType">学校类型</param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public string FormatEduComUrlParam(string schoolType,int page,int limit) 
        {
            if (string.IsNullOrEmpty(schoolType))
                schoolType = "普通本科";
            if (page < 1)
                page = 1;
            if (limit < 1)
                limit = 30;
           return string.Format(EduComListParam, schoolType, page, limit);
        }
    }
}
