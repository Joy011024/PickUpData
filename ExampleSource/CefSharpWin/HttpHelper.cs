using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
namespace CefSharpWin
{
    public class HttpHelper
    {
        public static string GetResponse(string url,   CookieContainer cookies = null)
        {
            string unicode = url;//这里需要进行URL处理
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            if (cookies!=null)
            {//提取cookie
                request.CookieContainer = cookies;//本应该是设置cookie，但是在操作中发现这样使用能在获取请求之后将数据返回到cookie【应该是引用类型风表现】                                 
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            string content = reader.ReadToEnd();
            reader.Close();
            responseStream.Close();
            return content;
        }
    }
}
