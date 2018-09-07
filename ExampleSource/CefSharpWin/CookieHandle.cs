using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace CefSharpWin
{
    public class HttpRequestFlag
    {
        /// <summary>
        /// 是否获取到请求所需的cookie
        /// </summary>
        public static bool ContainerFullCookie { get; set; }
    }
    public class CookieHandle
    {
        public static CookieContainer CookiePool;
        /// <summary>
        /// 【setup3】 cookie处理
        /// </summary>
        /// <param name="cookie"></param>
        public static void FillCookieContainer(object cookie)
        {
            CookiePool = new CookieContainer();
            Dictionary<string, Dictionary<string, System.Net.Cookie>> cis = cookie as Dictionary<string, Dictionary<string, System.Net.Cookie>>;
            CookieVisitor.OutputCookie(cis).ToString().DebugLog(ELogType.SessionOrCookieLog, true);
            if (cis.Count == 0)
            {
                return  ;
            }

 			bool isTocken = false;
            foreach (var item in cis)
            { //RAIL_EXPIRATION:1535825068536 
                foreach (var ck in item.Value)
                {
                    if (item.Value[ck.Key].Name == "tk")
                    {
                        isTocken = true;
                    }
                    CookiePool.Add(item.Value[ck.Key]);
                }
            }
            if (!isTocken)
            {
                return;
            } 		 //此处需要判断是否获取了全部的cookie
            string url = SystemConfig.ContacterUrl;
            string contacter= HttpHelper.GetResponse(url, CookiePool);
            url.DebugLog(ELogType.HttpResponse, true);
            contacter.DebugLog(ELogType.HttpResponse, true);
            return  ;
        }
        
    }
}
