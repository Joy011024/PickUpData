using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Domain.CommonData;
namespace CefSharpWin
{
    public class HttpHelper
    {
        public static string GetResponse(string url, CookieContainer cookies = null)
        {
            string unicode = url;//这里需要进行URL处理
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            if (cookies != null)
            {//提取cookie
                request.CookieContainer = cookies;//本应该是设置cookie，但是在操作中发现这样使用能在获取请求之后将数据返回到cookie【应该是引用类型风表现】                                 
            }
            RequestHeadHelper head = new RequestHeadHelper();
            string con = head.ReadHeadFromText();
            foreach (var item in con.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] h = item.Split(':');
                if (!SystemConfig.IgnoreHeadItem.Contains( h[0] ))
                    request.Headers.Add(h[0], h[1]);
            }
            request.KeepAlive = true;
            request.Host = "kyfw.12306.cn";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            string content = reader.ReadToEnd();
            reader.Close();
            responseStream.Close();
            return content;
        }
    }
    public class RequestHeadHelper
    {
        public string ReadHeadFromText()
        {
            //读取请求头
            string headDir = Logger.DllDir + "HttpExtend/HttpHeader.txt";
            return FileHelper.ReadFile(headDir);
        }
    }
   
}
