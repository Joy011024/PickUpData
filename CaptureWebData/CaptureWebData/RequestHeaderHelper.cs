using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaptureWebData
{
    public class RequestHeaderHelper
    {
        /// <summary>
        /// 将从浏览器上直接获取的请求头进行处理成键值形式
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public Dictionary<string, string> PickUpRequestHeader(string header) 
        {
            string[] headerArr = header.Replace("\r\n", "\n").Split('\n');
            Dictionary<string, string> request = new Dictionary<string, string>();
            foreach (string item in headerArr)
            {
                if (string.IsNullOrEmpty(item)) continue;
                if (item.StartsWith("POST", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                if (item.StartsWith("GET", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                if (item.StartsWith("Cookie", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                int x = item.IndexOf(":");
                if (x > 0)
                {
                    string key = item.Substring(0, x).Trim();
                    string value = item.Substring(x + 1);
                    if (request.ContainsKey(key))
                    {
                        request.Add(key, value);
                    }
                }
            }
            return request;
        }
        public Dictionary<string, string> SplitCookie (string cookie)
        {
            Dictionary<string, string> cs = new Dictionary<string, string>();
            string[] items = cookie.Split(';');
            foreach (string s in items)
            {
                string[] item = s.Split('=');
                if (item.Length < 2) { continue; }
                if (!cs.ContainsKey(item[0]))
                {
                    cs.Add(item[0].Trim(), item[1].Trim());
                }
            }
            return cs;
        }
    }
}
