using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace CefSharpWin
{
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
            Dictionary<string, List<System.Net.Cookie>> cis = cookie as Dictionary<string, List<System.Net.Cookie>>;
            if (cis.Count == 0)
            {
                return  ;
            }
            foreach (var item in cis)
            {
                for (int i = 0; i < item.Value.Count; i++)
                { //RAIL_EXPIRATION:1535825068536 
                    CookiePool.Add(item.Value[i]);
                }
            }
            string contacter= HttpHelper.GetResponse(SystemConfig.ContacterUrl, CookiePool);
            contacter.WriteLog(ELogType.HttpResponse, true);
            return  ;
        }
        
    }
}
