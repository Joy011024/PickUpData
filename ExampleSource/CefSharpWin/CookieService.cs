using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CefSharp;
using Domain.CommonData;
namespace CefSharpWin
{
    #region  测试在执行网页完毕之后触发
    public class CookieVisitor : CefSharp.ICookieVisitor
    {
        public static Dictionary<string, Dictionary<string, System.Net.Cookie>> CookieDict = new Dictionary<string, Dictionary<string, System.Net.Cookie>>();

        public event Action<CefSharp.Cookie> SendCookie;
        public bool Visit(CefSharp.Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            //https://kyfw.12306.cn/passport/web/login  提取cookie，但是该请求的URL是啥
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

            #region  cookie 替换
            System.Net.Cookie ck = new System.Net.Cookie(obj.Name, obj.Value, obj.Path, obj.Domain);

            if (!CookieDict.ContainsKey(domain))
            {
                Dictionary<string, System.Net.Cookie> cookies = new Dictionary<string, System.Net.Cookie>();
                CookieDict.Add(domain, cookies);
            }
            if (!CookieDict[domain].ContainsKey(obj.Name))
            {
                CookieDict[domain].Add(obj.Name, ck);
            }
            CookieDict[domain][obj.Name] = ck;
            Dictionary<string, string> dict = SystemConfig.SampleCookieItem;
            if (dict.ContainsKey(obj.Name))
            {
                string newItem = dict[obj.Name];
                //判断是否增加新项
                System.Net.Cookie generate = new System.Net.Cookie(newItem, obj.Value, obj.Path, obj.Domain);
                if (!CookieDict[domain].ContainsKey(newItem))
                {
                    CookieDict[domain].Add(newItem, generate);
                }
                CookieDict[domain][newItem] = generate;
            }
            if (!obj.Domain.Contains(SystemConfig.CookieDomain))
            {
                return;
            }
            #endregion
            string cookie = "Domain:" + obj.Domain.TrimStart('.') + "\r\n" + obj.Name + ":" + obj.Value;
        }
        public void Dispose()
        {

        }
        /// <summary>
        /// 进行cookie输出
        /// </summary>
        /// <returns></returns>
        public static StringBuilder OutputCookie(Dictionary<string, Dictionary<string, System.Net.Cookie>> CookieDict)
        {
            StringBuilder cookieHead = new StringBuilder();
            foreach (var item in CookieDict)
            {

                cookieHead.AppendLine("Domain:" + item.Key);
                foreach (KeyValuePair<string, System.Net.Cookie> ck in item.Value)
                {
                    cookieHead.AppendLine(ck.Value.Name + ":" + ck.Value.Value);
                }
            }
            return cookieHead;
        }
    }
    /// <summary>
    /// 在绑定cookie提取时请设置回调GetCookieResponse事件
    /// </summary>
    public class RequestHandler_new : CefSharp.Handler.DefaultRequestHandler //CefSharp.Example.Handlers
    {
        public static bool ExistsTocken = false;
        /// <summary>
        /// 已成功访问联系人请求URL
        /// </summary>
        public static bool SuccessVisitedContractUrl = false;
        public static void ClearHistory()
        {
            ExistsTocken = false;
            SuccessVisitedContractUrl = false;
        }
        public string _directory = "/log/DownloadFile/";
        public GetCookieTodo GetCookieResponse { get; set; }

        private Dictionary<UInt64, MemoryStreamResponseFilter> responseDictionary = new Dictionary<UInt64, MemoryStreamResponseFilter>();


        public IRequestHandler _requestHeandler;

        public RequestHandler_new(IRequestHandler rh) : base()
        {
            _requestHeandler = rh;
        }
        #region overide
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
        #endregion
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
            if (SystemSetting.SystemSettingDict["OpenDebug"] == "true")
            {//开启debug模式
                extension.WriteLog(ELogType.Account, true);
                //输入写入到请求日志
                IPostData data = request.PostData;
                if (data != null)
                {
                    //请求内容被转换为了文件流
                    //输出有请求参数的URL
                    StringBuilder requestHead = new StringBuilder();
                    requestHead.AppendLine(string.Format("url hava request param-> {0} ", extension));
                    foreach (var item in request.Headers.AllKeys)
                    {
                        requestHead.Append(string.Format("{0} : {1};", item, request.Headers[item]));
                    }
                    requestHead.ToString().WriteLog(ELogType.HttpRequest, true);
                }
            }
            //if (!extension.Contains(SystemConfig.CookieDomain))
            //{
            //    return;
            //}
            if ((!SystemConfig.AnywhereGetCookie && extension != SystemConfig.InitCookeKey))
            {
                return;
            }
            //这是请求响应头
            ICookieManager cookie = Cef.GetGlobalCookieManager();
            if (SystemConfig.AnywhereGetCookie)
            {//无限制提取cookie
                cookie.VisitAllCookies(new CookieVisitor());
            }
            else
            {
                foreach (var urlFrom in SystemConfig.CookieFrom)
                {
                    cookie.VisitUrlCookies(urlFrom, false, new CookieVisitor());
                }
            }
            if (extension == SystemConfig.TockenAfterUrl)
            {//是否已经获取到了tocken
                ExistsTocken = true;
            }
            if (CookieHandle.GetCoreCookie(CookieVisitor.CookieDict))
            {  //从其他应用中获取到了cookie
                ExistsTocken = true;
            }
            if (ExistsTocken)
            {//提取到了完整的cookie
                StringBuilder cook = new StringBuilder();
                cook.AppendLine(CookieVisitor.OutputCookie(CookieVisitor.CookieDict).ToString());
                cook.ToString().WriteLog(ELogType.SessionOrCookieLog, true); 
                //CookieContainer
                GetCookieResponse(CookieVisitor.CookieDict);
            }
            if (_requestHeandler != null)
            {
                _requestHeandler.OnResourceLoadComplete(browserControl, browser, frame, request, response, status, receivedContentLength);
            }

            if (SystemConfig.DownloadResource)
                DownloadService.DownloadResource(extension, responseDictionary, request, response);
            return;
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

    public class DownloadService
    {
        public static void DownloadResource(string requestUrl, Dictionary<UInt64, MemoryStreamResponseFilter> responseDictionary,IRequest request, IResponse response)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(requestUrl);
            string[] keys = response.ResponseHeaders.AllKeys;
            string key = SystemConfig.CookieDomain;
            sb.AppendLine(key + ":" + response.ResponseHeaders[key]);
            //这是请求头
            string[] headKey = request.Headers.AllKeys;
            StringBuilder head = new StringBuilder();
            foreach (var item in headKey)
            {
                head.AppendLine(item + ":" + request.Headers[item]);
            }
            "Response End".WriteLog(ELogType.HttpResponse, true);

            if (request.ResourceType == ResourceType.Image || requestUrl.EndsWith(".jpg") || requestUrl.EndsWith(".png") || requestUrl.EndsWith(".gif") || requestUrl.EndsWith(".jpeg"))
            {
                string dir = SystemConfig.DebugDir;
                MemoryStreamResponseFilter filter;
                if (responseDictionary.TryGetValue(request.Identifier, out filter))
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    System.Diagnostics.Debug.WriteLine("responseDictionary.Count:" + responseDictionary.Count);

                    //TODO: Do something with the data here
                    var data = filter.Data;
                    var dataLength = filter.Data.Length;
                    //NOTE: You may need to use a different encoding depending on the request
                    //var dataAsUtf8String = Encoding.UTF8.GetString(data);
                   
                    Random ran = new Random(Guid.NewGuid().GetHashCode());
                    if (dataLength > 0)
                    {
                        string fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff-") + ran.Next(99999, 999999) + ".png";
                        string path = dir + fileName;
                        try
                        {
                            fileName = Path.GetFileName(requestUrl);

                            File.WriteAllBytes(path, data);
                            return;
                        }
                        catch (Exception e)
                        {
                            //throw;
                        }

                        fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff-") + ran.Next(99999, 999999) + ".png";
                        path = dir + fileName;
                        File.WriteAllBytes(path, data);//保存数据
                    }
                }
                return;
            }
            
        }
    }
    #endregion

}
