using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace GatherIdCard
{
    public class HttpHelper
    {
        public string GetHttpResponse(string url, string cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:19.0) Gecko/20100101 Firefox/19.0";
            request.KeepAlive = false;
            if (!string.IsNullOrEmpty(cookie))
                request.Headers.Add(cookie);
            try
            {
                HttpWebResponse webResponse = null;
                Stream str = null;
                webResponse = (HttpWebResponse)request.GetResponse();//此处有坑【如果http响应超时此处会抛出异常】
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {//http请求失败

                }
                str = webResponse.GetResponseStream();
                StreamReader sr = new StreamReader(str);
                string text = sr.ReadToEnd();
                sr.Close();
                return text;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
