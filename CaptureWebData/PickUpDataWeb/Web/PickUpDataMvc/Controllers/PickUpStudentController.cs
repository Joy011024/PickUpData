using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Data;
using HttpClientHelper;
using System.Net;
using System.Net.Http;
using DataHelp;
using System.Net.Http.Headers;
namespace PickUpDataMvc.Controllers
{
    public class PickUpStudentController : Controller
    {
        //
        // GET: /PickUpStudent/

        public ActionResult JxauEduData()
        {
            Test();
            return View();
        }
        public ActionResult ThirdPage()
        {
            return View();
        }
        public JsonResult CrossDomainAjax(AjaxRequestParam param)
        {
            JsonData json = new JsonData();
            json.Data = HttpClientExtend.HttpWebRequestPost(param.Url, param.JsonString,string.Empty);
            json.Result = true;
            return Json(json);
        }
        void Test() 
        {
            HttpClient client = new HttpClient();
            IpData ip=new IpData(){ ip="59.66.134.35"};

            string url = "https://www.qqzeng.com/ip/";
            HttpResponseMessage response= client.GetAsync(url).Result;
            HttpResponseHeaders heads = response.Headers;
            Dictionary<string, string> cookie = new Dictionary<string, string>();
            List<string> cs = new List<string>();
            foreach (var item in heads)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
                string value=string.Join("", item.Value);
                cs.Add(item.Key +":"+ value);
                cookie.Add(item.Key,value );
            }
            string cookies = string.Join("\r\n", cs);
            
            string json=ip.ConvertJson();
            HttpContent con = new StringContent(json, System.Text.Encoding.UTF8);
          //  SetClientHeader(client,cookies);
            url="https://www.qqzeng.com/ip/getIpInfo.php";
            string text= client.PostAsync(url, con).Result.Content.ReadAsStringAsync().Result;
            QQZengResponse qqz = text.ConvertObject<QQZengResponse>();
        }
        public void SetClientHeader(HttpClient client,string selfHead) 
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
            foreach (string  item in headerArr)
            {
                if (string.IsNullOrEmpty(item)) continue;
                if(item.StartsWith("POST",StringComparison.OrdinalIgnoreCase))
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
                        client.DefaultRequestHeaders.Add(c,item.Substring(x+1));
                    }
                    catch (Exception ex) 
                    {
                       
                    }
                }
            }

        }
    }
}
