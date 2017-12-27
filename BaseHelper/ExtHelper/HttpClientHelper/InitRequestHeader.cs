using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpClientHelper
{
    /// <summary>
    /// 初始化化准备请求头
    /// </summary>
    public class RequestHeaderHelper
    {
        public static Dictionary<string,string> InitHeader(string header) 
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
                //这里需要进行一次特殊的判断（是否首位字符为:存在部分请求的项为  :method）
                if (item.StartsWith(":")) 
                {
                  int index=  item.IndexOf(":", 1);
                  string key =item.Substring(0, index);
                  string value =item.Substring(index+1);
                  if (!request.ContainsKey(key))
                  {
                      request.Add(key, value);
                  }
                  else
                  {
                      request[key] = value;
                  }
                }
                int x = item.IndexOf(":");
                if(x>0)
                {
                    string key = item.Substring(0, x).Trim();
                    string value = item.Substring(x + 1);
                    if (!request.ContainsKey(key))
                    {
                        request.Add(key, value);
                    }
                    else 
                    {
                        request[key] = value;
                    }
                }
            }
            return request;
        }
    }
}
