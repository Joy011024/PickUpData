using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CefSharp;
namespace CefSharpWin
{
    #region  测试在执行网页完毕之后触发
    public class CookieVisitor : CefSharp.ICookieVisitor
    {
        public static Dictionary<string, List<System.Net.Cookie>> CookieDict = new Dictionary<string, List<System.Net.Cookie>>();
        public event Action<CefSharp.Cookie> SendCookie;
        public bool Visit(CefSharp.Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            //此处调用前需要先过滤URL
            visitor_SendCookie(cookie);
            deleteCookie = false;
            if (SendCookie != null)
            {
                SendCookie(cookie);
            }

            return true;
        }
        /// 回调事件
        public void visitor_SendCookie(CefSharp.Cookie obj)
        {
            string domain = obj.Domain;
            
            if (!CookieDict.ContainsKey(domain))
            {
                CookieDict.Add(domain, new List<System.Net.Cookie>());
            }
            System.Net.Cookie ck = new System.Net.Cookie(obj.Name, obj.Value, obj.Path, obj.Domain);

            CookieDict[domain].Add(ck);
            if (!obj.Domain.Contains(SystemConfig.CookieDomain))
            {
                return;
            }
            string cookie = "Domain:" + obj.Domain.TrimStart('.') + "\r\n" + obj.Name + ":" + obj.Value;
        }
        public void Dispose()
        {

        }
    }
    /// <summary>
    /// 在绑定cookie提取时请设置回调GetCookieResponse事件
    /// </summary>
    public class RequestHandler_new : CefSharp.Handler.DefaultRequestHandler //CefSharp.Example.Handlers
    {
        public string _directory = "/log/DownloadFile/";
        public GetCookieTodo GetCookieResponse { get; set; }

        private Dictionary<UInt64, MemoryStreamResponseFilter> responseDictionary = new Dictionary<UInt64, MemoryStreamResponseFilter>();


        public IRequestHandler _requestHeandler;

        public RequestHandler_new(IRequestHandler rh) : base()
        {
            _requestHeandler = rh;
        }

        public override CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {

            Uri url;
            if (Uri.TryCreate(request.Url, UriKind.Absolute, out url) == false)
            {
                //If we're unable to parse the Uri then cancel the request
                // avoid throwing any exceptions here as we're being called by unmanaged code
                return CefReturnValue.Cancel;
            }
            var extension = url.ToString().ToLower();
            if (request.ResourceType == ResourceType.Image || extension.EndsWith(".jpg") || extension.EndsWith(".png") || extension.EndsWith(".gif") || extension.EndsWith(".jpeg"))
            {
                System.Diagnostics.Debug.WriteLine(url);//打印
            }
            if (_requestHeandler != null)
            {
                return _requestHeandler.OnBeforeResourceLoad(browserControl, browser, frame, request, callback);
                //return base.OnBeforeResourceLoad(browserControl, browser, frame, request, callback);
            }

            return CefReturnValue.Continue;
        }

        public override IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {

            var url = new Uri(request.Url);
            var extension = url.ToString().ToLower();
            if (request.ResourceType == ResourceType.Image || extension.EndsWith(".jpg") || extension.EndsWith(".png") || extension.EndsWith(".gif") || extension.EndsWith(".jpeg"))
            {//Only called for our customScheme
                var dataFilter = new MemoryStreamResponseFilter();//新建成数据 处理器
                responseDictionary.Add(request.Identifier, dataFilter);
                return dataFilter;
            }
            if (_requestHeandler != null)
            {
                return _requestHeandler.GetResourceResponseFilter(browserControl, browser, frame, request, response);
            }
            return null;
        }
        Random _rand = new Random();
        /// <summary>
        /// 【setup2】提取cookie
        /// </summary>
        /// <param name="browserControl"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="status"></param>
        /// <param name="receivedContentLength"></param>
        public override void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            if (response.StatusCode != 200)
            {
                return;
            }
            var url = new Uri(request.Url);
            var extension = url.ToString().ToLower();
            extension.WriteLog(ELogType.Account, true);
            if (!extension.Contains(SystemConfig.CookieDomain))
            {
                return;
            }
            string[] keys = response.ResponseHeaders.AllKeys;
            string key = "Set-Cookie";
            if ((!SystemConfig.AnywhereGetCookie && extension != SystemConfig.InitCookeKey) || !keys.Contains(key))
            {
                return;
            }
            //这是请求响应头
            ICookieManager cookie = Cef.GetGlobalCookieManager();
            CookieVisitor.CookieDict = new Dictionary<string, List<System.Net.Cookie>>();
            if (SystemConfig.AnywhereGetCookie)
            {//无限制提取cookie
                cookie.VisitAllCookies(new CookieVisitor());
            }
            else
            {
                foreach (var urlFrom in SystemConfig.CookieFrom)
                {
                    cookie.VisitUrlCookies(urlFrom, true, new CookieVisitor());
                }
            }
            foreach (var item in CookieVisitor.CookieDict)
            {
                StringBuilder cookieHead = new StringBuilder();
                cookieHead.AppendLine("Domain:" + item.Key);
                cookieHead.AppendLine(string.Join("\r\n", item.Value));
                cookieHead.ToString().WriteLog(ELogType.SessionOrCookieLog, true);
            }
            if (GetCookieResponse != null)
            {
                //CookieContainer
                GetCookieResponse(CookieVisitor.CookieDict);
            }
            return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(extension);
            sb.AppendLine(key + ":" + response.ResponseHeaders[key]);
            //这是请求头
            string[] headKey = request.Headers.AllKeys;
            StringBuilder head = new StringBuilder();
            foreach (var item in headKey)
            {
                head.AppendLine(item + ":" + request.Headers[item]);
            }
            "Response End".WriteLog(ELogType.HttpResponse, true);

            if (request.ResourceType == ResourceType.Image || extension.EndsWith(".jpg") || extension.EndsWith(".png") || extension.EndsWith(".gif") || extension.EndsWith(".jpeg"))
            {
                MemoryStreamResponseFilter filter;
                if (responseDictionary.TryGetValue(request.Identifier, out filter))
                {
                    if (!Directory.Exists(_directory))
                    {
                        Directory.CreateDirectory(_directory);
                    }

                    System.Diagnostics.Debug.WriteLine("responseDictionary.Count:" + responseDictionary.Count);

                    //TODO: Do something with the data here
                    var data = filter.Data;
                    var dataLength = filter.Data.Length;
                    //NOTE: You may need to use a different encoding depending on the request
                    //var dataAsUtf8String = Encoding.UTF8.GetString(data);

                    if (dataLength > 0)
                    {
                        string fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff-") + _rand.Next(99999, 999999) + ".png";
                        string path = _directory + fileName;
                        try
                        {
                            fileName = Path.GetFileName(url.ToString());

                            File.WriteAllBytes(path, data);
                            return;
                        }
                        catch (Exception e)
                        {
                            //throw;
                        }

                        fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff-") + _rand.Next(99999, 999999) + ".png";
                        path = _directory + fileName;
                        File.WriteAllBytes(path, data);//保存数据
                    }
                }
                return;
            }
            if (_requestHeandler != null)
            {
                _requestHeandler.OnResourceLoadComplete(browserControl, browser, frame, request, response, status, receivedContentLength);
            }
        }

    }

    //数据处理器，仅将 数据保存到内存。
    public class MemoryStreamResponseFilter : IResponseFilter
    {
        private MemoryStream memoryStream;
        bool IResponseFilter.InitFilter()
        {
            //NOTE: We could initialize this earlier, just one possible use of InitFilter
            memoryStream = new MemoryStream();
            return true;
        }

        FilterStatus IResponseFilter.Filter(Stream dataIn, out long dataInRead, Stream dataOut, out long dataOutWritten)
        {
            if (dataIn == null)
            {
                dataInRead = 0;
                dataOutWritten = 0;
                return FilterStatus.Done;
            }
            dataInRead = dataIn.Length;
            dataOutWritten = Math.Min(dataInRead, dataOut.Length);
            //Important we copy dataIn to dataOut
            dataIn.CopyTo(dataOut);
            //Copy data to stream
            dataIn.Position = 0;
            dataIn.CopyTo(memoryStream);
            return FilterStatus.Done;
        }

        void IDisposable.Dispose()
        {
            memoryStream.Dispose();
            memoryStream = null;
        }

        public byte[] Data
        {
            get { return memoryStream.ToArray(); }
        }
    }
    #endregion

}
