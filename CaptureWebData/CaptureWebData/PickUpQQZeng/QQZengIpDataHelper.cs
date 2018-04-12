using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.CommonData;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using DataHelp;
using System.Net;
using HttpClientHelper;
namespace CaptureWebData
{
    public static class StringExt 
    {
        public static  string LSubstringExt(this string str, string sign)
        {
            if (string.IsNullOrEmpty(str)) 
            {
                return string.Empty;
            }
            if (str.Contains(sign))
            {
                return str.Substring(0, str.IndexOf(sign));
            }
            return string.Empty;
        }
        public static string RSubstringExt(this string str, string sign)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            if (str.Contains(sign))
            {
                return str.Substring(str.IndexOf(sign) + sign.Length);
            }
            return string.Empty;
        }
    }
    public static  class QQZengIpDataHelper
    {
        public static QQZengResponseData HtmlConvertIpData(this string html) 
        {
            /*
             
<tr class="ip_info_a">
	<td class="ip-td">亚洲</td>
	<td class="ip-td">中国</td>
	<td class="ip-td">北京</td>
	<td class="ip-td">北京</td>
	<td class="ip-td"></td>
	<td class="ip-td">鹏博士</td>
</tr>
<tr class="ip_info_b">
	<td class="ip-td">110100</td>
	<td class="ip-td">China</td>
	<td class="ip-td">CN</td>
	<td class="ip-td">116.405285</td>
	<td class="ip-td">39.904989</td>
	<td class="ip-td">20170607</td>
</tr>
             */
            RegexHtmlConvertIpData(html);
            string[] ele = html.Split(new string[] { "class=\"ip-td\"" }, StringSplitOptions.None).
                Where(s =>
                {//<tr class="ip_info_a"><td  
                    if (!s.Contains("</") || !s.Contains(">")) return false;
                    return true;
                }).
                Select(s =>
                {
                    s = s.LSubstringExt("</");
                    s = s.RSubstringExt(">");
                    return s;
                }).ToArray();
            if (ele.Length < 12) 
            {
                return new QQZengResponseData();
            }
            QQZengResponseData qqzeng = new QQZengResponseData();
            qqzeng.continent = ele[0];
            qqzeng.country = ele[1];
            qqzeng.province = ele[2];
            qqzeng.city = ele[3];
            qqzeng.district = ele[4];
            qqzeng.isp = ele[5];
            qqzeng.areacode = ele[6];
            qqzeng.en = ele[7];
            qqzeng.cc = ele[8];
            qqzeng.lng = ele[9];
            qqzeng.lat = ele[10];
            qqzeng.version = ele[11];
            return qqzeng;
        }
        public static void RegexHtmlConvertIpData(this string html) 
        {
           // Regex reg = new Regex(string.Format("<[/]?({0})([^<>]*)>/g","td"));
            Regex reg = new Regex(">*</");//实际上 要提取的文本内容格式 为  >  </
            GroupCollection gc= reg.Match(html).Groups;

        }

        public static  void QueryIpAddress(string ip,string cookie)
        {
            HttpClient client = new HttpClient();
            QQZengResponseData ipData = new QQZengResponseData() { ip = ip };// "59.66.134.35"

            string url = "https://www.qqzeng.com/ip/";
            HttpResponseMessage response = client.GetAsync(url).Result;
            HttpResponseHeaders heads = response.Headers;
            foreach (var item in heads)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
                string value = string.Join("", item.Value);
            }
            SetClientCookie(cookie, client);

            string json = ipData.ConvertJson();
            HttpContent con = new StringContent(json, System.Text.Encoding.UTF8);
            //  SetClientHeader(client,cookies);
            url = "https://www.qqzeng.com/ip/getIpInfo.php";
            string text = client.PostAsync(url, con).Result.Content.ReadAsStringAsync().Result;
            QQZengResponse qqz = text.ConvertObject<QQZengResponse>();
        }
        private static void SetClientHeader(HttpClient client, string selfHead)
        {
            string header = @"Accept:*/*
                Accept-Encoding:gzip, deflate, br
                Accept-Language:zh-CN,zh;q=0.8
                Connection:keep-alive
                Content-Length:11
                Content-Type:application/x-www-form-urlencoded; charset=UTF-8
                Cookie:pgv_pvi=6298830848; pgv_si=s8949588992; f_ip=qqzeng
                Host:www.qqzeng.com
                Origin:https://www.qqzeng.com
                Referer:https://www.qqzeng.com/ip/
                User-Agent:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36
                X-Requested-With:XMLHttpRequest";
            if (!string.IsNullOrEmpty(selfHead))
            {
                header = selfHead;
            }
            string[] headerArr = header.Replace("\r\n", "\n").Split('\n');
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
                    try
                    {
                        var c = item.Substring(0, x).Trim();
                        client.DefaultRequestHeaders.Remove(c);
                        client.DefaultRequestHeaders.Add(c, item.Substring(x + 1));
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

        }
        public static void SetClientCookie(string cookie,HttpClient client) 
        {
            client.DefaultRequestHeaders.Add("Cookie", cookie);
        }
        public static void ExecuteWebHttp(string ip,string cookieStr) 
        {
            QQZengResponseData ipData = new QQZengResponseData() { ip = ip };
            //CookieContainer cc = new CookieContainer();
            //foreach (string item in cookieStr.Split(';'))
            //{
            //    string[] cookie = item.Split('=');
            //    if (cookie.Length < 2) { return; }
            //    cc.Add(new Cookie(cookie[0], cookie[1]));
            //}
            string ret= HttpClientExtend.HttpWebRequestPost("https://www.qqzeng.com/ip/getIpInfo.php", ipData.ConvertJson(), cookieStr);
            QQZengResponse qqz = ret.ConvertObject<QQZengResponse>();
        }
    }
}
