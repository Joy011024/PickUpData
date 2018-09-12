using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Domain.CommonData;
using PureMVC.Patterns;
namespace CefSharpWin
{
    public class HttpRequestFlag
    {
        /// <summary>
        /// 是否获取到请求所需的cookie
        /// </summary>
        public static bool ContainerFullCookie { get; set; }
        public static bool LogicFrmIsOpen { get; set; }
        public static CookieContainer CookiePool;
        public static void Preparelogin()
        {
            LogicFrmIsOpen = false;
            ContainerFullCookie = false;
            CookiePool = new CookieContainer();
        }
    }
    public class CookieHandle 
    {
       
        /// <summary>
        /// 【setup3】 cookie处理
        /// </summary>
        /// <param name="cookie"></param>
        public static void FillCookieContainer(object cookie)
        {
            CookieContainer CookiePool = new CookieContainer();
            Dictionary<string, Dictionary<string, System.Net.Cookie>> cis = cookie as Dictionary<string, Dictionary<string, System.Net.Cookie>>;
            CookieVisitor.OutputCookie(cis).ToString().WriteLog(ELogType.SessionOrCookieLog, true);
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

            //是否需要再次查询联系人列表
            if (HttpRequestFlag.LogicFrmIsOpen)
            {
                return;
            }
            string contacter= HttpHelper.GetZipResponse(url, CookiePool);
            url.WriteLog(ELogType.HttpResponse, true);
            contacter.WriteLog(ELogType.HttpResponse, true);
            //判断是否需要进行发送
            if (!HttpRequestFlag.LogicFrmIsOpen)
            {//是否已经加载登陆信息
                HttpRequestFlag.CookiePool = CookiePool;
                HttpRequestFlag.LogicFrmIsOpen = true;
                Ticket12306Resonse t123306 = contacter.ConvertData<Ticket12306Resonse>();
                //使用线程打开
                new System.Threading.Thread(() =>
                 {
                     new CommandService().SendNotify(NotifyList.Notify_Refresh_Contacter, t123306, string.Empty);
                 }).Start();
            }
            return  ;
        }
    }
}
