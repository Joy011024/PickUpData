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
namespace GatherIdCard
{
    class Program
    {
        static void Main(string[] args)
        {// 南宁市 | 柳州市 | 桂林市 | 梧州市 | 北海市 | 钦州市 | 贵港市 | 玉林市 | 百色市 | 防城港市|贺州市 | 河池市 | 来宾市 | 崇左市 | 外省地区
            GatherUserInfo();
        }
        static void GatherUserInfo()
        {
           
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
        {
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
                HttpClientExtend.GetHttpResponseData(url);
                AssemblyDataExt ass = new AssemblyDataExt();
                string dir = ass.ForeachDir(ass.GetAssemblyDir(), 3);
                LoggerWriter.CreateLogFile(text, dir + "/" + ELogType.DataLog, ELogType.DataLog);
            }
            catch (Exception ex)
            {
                return;
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
    }
}
