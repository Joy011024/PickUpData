using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using System.ComponentModel;

namespace HttpClientHelper
{
    public class HttpClientExtend
    {
        public static CookieContainer HttpResponseAfterCookie{get; private  set;}
        public CookieContainer HttpResponseCookie
        {
            get { return HttpResponseAfterCookie; }
        }
        static void ClearCookie() 
        {
            HttpResponseAfterCookie = new CookieContainer();
        }
        public class ResponseData
        {
            public long ContentLength { get; set; }
            public string ContentType { get; set; }
            public string ContentExtension { get; set; }
            public string ContentString { get; set; }
            public Dictionary<string, string> ResponseHeader { get; set; }
        }
        /// <summary>
        /// 【对于传递的数据为json时post请求参数传递无效】调用post请求【cs客户端以及bs端都可以调用】
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestContent"></param>
        /// <param name="timeOutSpan">手动设置的请求超时间隔【如果不设置则使用默认值1分钟】</param>
        /// <returns></returns>
        public static string HttpClientPost(string url, string requestContent, TimeSpan? timeOutSpan)
        {
            string result = string.Empty;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            if (!timeOutSpan.HasValue||timeOutSpan.Value==new TimeSpan())
            {
                client.Timeout = new TimeSpan(0, 1, 0);//设置请求超时拒绝操作
            }
            else
            {
                client.Timeout = timeOutSpan.Value;
            }
            string param = string.Empty;
            if (!string.IsNullOrEmpty(requestContent))
            {
                param = requestContent;
            }
            HttpContent content = new StringContent(param, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response == null)
            {
                response.Dispose();
                return result;
            }
            result = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            return result;
        }
        public static string HttpClientGet(string url)
        {
            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 1, 0);
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.StatusCode != HttpStatusCode.OK)
            {//http请求出现异常 写日志
                return string.Empty;
            }
            return response.Content.ReadAsStringAsync().Result;
        }


        /// <summary>
        /// 发送http的post请求并接收请求的cookie
        /// </summary>
        /// <param name="url">http请求的URL</param>
        /// <param name="json">http请求的参数</param>
        /// <param name="timeOutSpan">http请求的超时时间【单位：秒】（默认值为60秒）</param>
        /// <returns>含有cookie数据的返回结果</returns>
        public static WebResponse HttpWebRequestPost(string url, string json, int timeOutSpan)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            int length = bytes.Length;
            request.ContentLength = length;
            if (timeOutSpan>0)
            {
                request.Timeout = timeOutSpan * 1000;
            }
            else
            {
                request.Timeout = 1000 * 60;
            }
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, length);
            }
            request.CookieContainer = new CookieContainer();//请求不提供cookie
            WebResponse response = request.GetResponse();//获取http请求相应结果
            request.KeepAlive = false;
            // HttpWebResponse httpResponse=(HttpWebResponse)response;
            return response;
        }
        /// <summary>
        /// 提供cookie的 Post的http请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="cookies">cookie的键值对</param>
        /// <returns></returns>
        public static WebResponse HttpWebRequestPost(string url, string json, Dictionary<string, string> cookies)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            StringBuilder cookie = new StringBuilder();
            foreach (var item in cookies)
            {
                cookie.Append(item.Key + "=" + item.Value + ";");
            }
            request.Headers.Add("Cookie", cookie.ToString());
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            int length = bytes.Length;
            request.ContentLength = length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, length);
            }
            WebResponse response = request.GetResponse();//获取http请求相应结果
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="json">请求参数的json形式</param>
        /// <param name="cookies">请求附带的cookie</param>
        /// <returns></returns>
        public static string HttpWebRequestPost(string url, string json, string cookies)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            if (!string.IsNullOrEmpty(cookies))
            {
                request.Headers.Add("Cookie", cookies);
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            int length = bytes.Length;
            request.ContentLength = length;
            request.ServicePoint.ConnectionLimit = int.MaxValue;
            try
            {
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, length);
                }
                // request.CookieContainer = new CookieContainer();//请求不提供cookie
                WebResponse response = request.GetResponse();//获取http请求相应结果
                string result = GetResponseString(response);
                response.Close();
                return result;
            }
            catch (WebException ex) 
            {
                HttpWebResponse hwpr = (HttpWebResponse)ex.Response;
                return GetResponseString(hwpr);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static WebResponse HttpWebRequestGet(string url)
        {
            string unicode = url;//这里需要进行URL处理
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            return request.GetResponse();
        }
        public static Dictionary<string, string> GetResponseCookies(WebResponse response)
        {
            Dictionary<string, string> cookies = new Dictionary<string, string>();
            HttpWebResponse httpResponse = (HttpWebResponse)response;
            CookieCollection contain = httpResponse.Cookies;
            foreach (Cookie item in contain)
            {
                cookies.Add(item.Name, item.Value);
            }
            response.Close();
            return cookies;
        }
        public static string GetResponseString(WebResponse response)
        {
            string responseString = string.Empty;
            Stream rs = response.GetResponseStream();
            StreamReader sr = new StreamReader(rs, Encoding.UTF8);
            responseString = sr.ReadToEnd();
            return responseString;
        }
        /// <summary>
        /// 提供web response的ContentType获取文件扩展名
        /// </summary>
        /// <param name="webResponseContentType"></param>
        /// <returns>文件扩展名</returns>
        public static string GetFileExtension(string webResponseContentType)
        {//video/mp4
            string ext = string.Empty;
            if (string.IsNullOrEmpty(webResponseContentType))
            {
                return ext;
            }
            string[] splits = webResponseContentType.Split('/');
            if (splits.Length > 1)
            {
                ext = "." + splits[1];
            }
            return ext;
        }
        public static ResponseData GetHttpResponseData(string url)
        {
            System.GC.Collect();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:19.0) Gecko/20100101 Firefox/19.0";
            request.KeepAlive = false;
            request.AllowWriteStreamBuffering = true;//对响应数据进行缓冲处理
            HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();//此处有坑【如果http响应超时此处会抛出异常】
            if (webResponse.StatusCode != HttpStatusCode.OK)
            {//http请求失败

            }
            ResponseData data = new ResponseData();
            data.ContentLength = webResponse.ContentLength;
            data.ContentType = webResponse.ContentType;
            string ext = string.Empty;
            if (!string.IsNullOrEmpty(data.ContentType))
            {
                string[] splits = data.ContentType.Split('/');//此处有坑【返回的类型多样性】
                if (splits.Length > 1)
                {
                    data.ContentExtension = "." + splits[1];
                }
            }
            string file = webResponse.Headers["Content-Disposition"];
            WebHeaderCollection headers = webResponse.Headers;
            webResponse.Close();
            request.Abort();
            request = null;
            return data;
        }
        public static string GetHttpContentType(string url)
        {//video/mp4
            string ext = string.Empty;
            if (string.IsNullOrEmpty(url))
            {
                return ext;
            }
            HttpWebRequest reques = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = reques.GetResponse();
            if (response == null)
            {
                return ext;
            }
            string[] splits = response.ContentType.Split('/');
            if (splits.Length > 1)
            {
                ext = "." + splits[1];
            }
            response.Close();
            return ext;
        }
        public static void ContinueDownLoad(string url, string fileFullName, int readWriteFileTimeOut, int timeOutWait, int onceReadByteLength)
        {
            HttpWebRequest request = null;
            FileStream fs;
            long nowStartPosition = 0;
            bool isAppend = false;
            if (!File.Exists(fileFullName))
            { //首先判断提供的文件全路径是否存在
                string dir = Path.GetDirectoryName(fileFullName);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                fs = new FileStream(fileFullName + ".log", FileMode.Create, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(fileFullName, FileMode.Append, FileAccess.Write);
                //fs = File.OpenWrite(fileFullName);
                //定位继续下载操作的位置
                nowStartPosition = fs.Length;
                fs.Seek(nowStartPosition, SeekOrigin.Current);
                isAppend = true;
            }
            System.GC.Collect();
            request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            if (readWriteFileTimeOut <= 0)
            {
                readWriteFileTimeOut = 1000;
            }
            if (timeOutWait <= 0)
            {
                timeOutWait = 1000;
            }
            request.ReadWriteTimeout = readWriteFileTimeOut;
            request.Timeout = timeOutWait;
            if (nowStartPosition > 0)
            {
                request.AddRange((int)nowStartPosition);
            }
            string conn = request.Connection;
            //如何判断是否响应成功
            WebResponse response = request.GetResponse();
            //如果是指定位置开始下载 则返回值为PartialContent 对应http状态码为206，【等待下载】
            HttpWebResponse webResponse = (HttpWebResponse)response;
            WebHeaderCollection heads = response.Headers;
            if (webResponse.StatusCode != HttpStatusCode.OK && webResponse.StatusCode != HttpStatusCode.PartialContent)
            {//请求失败
                fs.Close();
                return;
            }
            if (!isAppend)
            {
                fs = new FileStream(fileFullName + GetFileExtension(response.ContentType), FileMode.CreateNew, FileAccess.Write);
            }
            long fileLength = response.ContentLength + nowStartPosition;//文件总大小
            if (onceReadByteLength <= 0)
            {//每次读取多少字节的文件流
                onceReadByteLength = 1024;
            }
            Stream stream = response.GetResponseStream();
            int size = onceReadByteLength;
            //此处需要判断响应返回的内容是否为流文件数据
            byte[] bytes = new byte[onceReadByteLength];
            while (size > 0)
            {
                try
                {
                    size = stream.Read(bytes, 0, onceReadByteLength);
                    fs.Write(bytes, 0, onceReadByteLength);
                    bytes = new byte[onceReadByteLength];
                    // fs.Write(bytes, 0, onceReadByteLength);
                    fs.Flush();
                    //stream.Close();
                    //fs.Close();
                }
                catch (Exception ex)
                {//请求超时
                    fs.Close();
                    stream.Close();
                    request.Abort();
                    ContinueDownLoad(url, fileFullName, readWriteFileTimeOut, timeOutWait, onceReadByteLength);
                }
            }
            fs.Close();
            stream.Close();
            request.Abort();
        }
        public static void HttpPost(string url, string json)
        {
            HttpClient hc = new HttpClient();
            HttpResponseMessage msg = hc.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).Result;
            System.Threading.Tasks.Task<string> ts = msg.Content.ReadAsStringAsync();
            string result = ts.Result;
        }
        public static void SetRequestParam( string jsonString, HttpWebRequest request) 
        {
            byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
            int length = bytes.Length;
            request.ContentLength = length;
            request.ServicePoint.ConnectionLimit = int.MaxValue;
            try
            {
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, length);
                }
            }
            catch (Exception ex) { }
        }
        public static void DownloadSmallFile(string url,string dir,string saveFileName) 
        {
            string unicode = url;//这里需要进行URL处理
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            WebResponse response= request.GetResponse();//获取小文件的文件流
            if (!Directory.Exists(dir))
            {//如果目录不存在，则进行创建
                Directory.CreateDirectory(dir);
            }
            string fullName = dir + "/" + saveFileName;
            Stream stream = response.GetResponseStream();
            FileStream fs = new FileStream(fullName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            int len = 1024;
            byte[] buffer=new byte[len];
            len = stream.Read(buffer, 0, len);
            while (len>0) 
            {
                fs.Write(buffer, 0, len);
                len = stream.Read(buffer, 0, buffer.Length);
            }
            fs.Close();
            response.Close();
        }
        static void FillHttpWebRequestHead(HttpWebRequest request, string head)
        {
            if (string.IsNullOrEmpty(head))
            {
                return;
            }
            string[] arr = head.Replace("\r", "\n").Replace("\n\n", "\n").Split('\n');
            foreach (string s in arr)
            {
                if (string.IsNullOrEmpty(s)) break;
                if (s.StartsWith("GET", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                if (s.StartsWith("POST", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }


                if (s.StartsWith("Cookie", StringComparison.OrdinalIgnoreCase)) continue;
                int x = s.IndexOf(":");
                if (x > 0)
                {
                    try
                    {
                        var key = s.Substring(0, x);
                        var value = s.Substring(x);
                        request.Headers.Add(key, value);
                    }
                    catch (Exception ex)
                    { }
                }
            }
        }
        public static string RunPosterContainerHeaderHavaParam(string url, string head, string json, CookieContainer cookie = null)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            if (cookie != null)
                request.CookieContainer = cookie;
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            byte[] jsonStream = Encoding.UTF8.GetBytes(json);
            request.ContentLength = jsonStream.Length;
            using (Stream requestSt = request.GetRequestStream())
            { //进行请求时的流信息
                requestSt.Write(jsonStream, 0, jsonStream.Length);
                requestSt.Close();
            }
            FillHttpWebRequestHead(request, head);
            WebResponse response = request.GetResponse();
            Stream st = response.GetResponseStream();
            StreamReader sr = new StreamReader(st, Encoding.UTF8);
            string text = sr.ReadToEnd();
            sr.Close();
            ClearCookie();
            //提取cookie
            response.Close();
            //此处的cookie如何进行提取？？
            return text;
        }
  
        #region 存在问题
        public static void ASyncDownload(string url, string fileFullName, bool isAsync, DownloadProgressChangedEventHandler Client_DownloadProgressChanged, AsyncCompletedEventHandler Client_DownloadFileCompleted)
        {
            if (Client_DownloadFileCompleted == null)
            {
                Client_DownloadFileCompleted = new AsyncCompletedEventHandler(DownloadFileCompleted);
            }
            if (Client_DownloadProgressChanged == null)
            {
                Client_DownloadProgressChanged = new DownloadProgressChangedEventHandler(DownloadProgressChanged);
            }
            WebClient client = new WebClient();
            if (isAsync)
            {
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(Client_DownloadFileCompleted);
                client.DownloadFileAsync(new Uri(url), fileFullName);
            }
            else
            {
                client.DownloadFile(new Uri(url), fileFullName);
            }
        }
        static void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {//下载进度

        }
        static void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {//文件下载完成执行的动作

        }

        #endregion
    }
}
