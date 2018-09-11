using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using Domain.CommonData;
using PureMVC.Interfaces;
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
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            request.KeepAlive = true;
            request.Host = "kyfw.12306.cn";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
            request.Referer = "https://kyfw.12306.cn/otn/passport?redirect=/otn/";
            request.ContentType = "text/plain;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string content = string.Empty;
            
            if (SystemConfig.ResponseIsZip)
            {
                //返回响应中进行编码集压缩处理
                GZipStream gzip = new GZipStream(responseStream, CompressionMode.Decompress);//解压缩
                Encoding enc = Encoding.GetEncoding(SystemConfig.HttpResponseZip);// .GetEncoding("gb2312");
                StreamReader sr = new StreamReader(gzip, Encoding.UTF8);
                content = sr.ReadToEnd();//System.IO.InvalidDataException:“GZip 头中的幻数不正确。请确保正在传入 GZip 流。”
                sr.Close();
                gzip.Close();
            }
            else
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                content = reader.ReadToEnd();
                reader.Close();
            }
            responseStream.Close();

            //解析联系人数据 
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
