namespace FluorineFx
{
    using FluorineFx.Browser;
    using FluorineFx.Configuration;
    using FluorineFx.Context;
    using FluorineFx.HttpCompress;
    using FluorineFx.Json.Rpc;
    using FluorineFx.Messaging;
    using FluorineFx.SWX;
    using log4net;
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.SessionState;

    public class FluorineGateway : IHttpModule, IRequiresSessionState
    {
        private static bool _initialized = false;
        private static object _objLock = new object();
        private static string _sourceName = null;
        private static int _unhandledExceptionCount = 0;
        internal const string FluorineHttpCompressKey = "__@fluorinehttpcompress";
        internal const string FluorineMessageServerKey = "__@fluorinemessageserver";
        private static MessageServer messageServer;
        private static IServiceBrowserRenderer serviceBrowserRenderer;

        private void application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication) sender;
            if (application.Request.ContentType == "application/x-amf")
            {
                application.Context.SkipAuthorization = true;
            }
        }

        private void application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication httpApplication = (HttpApplication) sender;
            HttpRequest httpRequest = httpApplication.Request;
            if ((serviceBrowserRenderer != null) && serviceBrowserRenderer.CanRender(httpRequest))
            {
                this.CompressContent(httpApplication);
                FluorineWebContext.Initialize();
                httpApplication.Response.Clear();
                serviceBrowserRenderer.Render(httpApplication);
                httpApplication.CompleteRequest();
            }
            else if (httpApplication.Request.ContentType == "application/x-amf")
            {
                httpApplication.Context.SkipAuthorization = true;
            }
        }

        private void application_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication) sender;
            if ((application.Response.Filter is CompressingFilter) || (application.Response.Filter is ThresholdFilter))
            {
                CompressingFilter compressingFilter = null;
                if (application.Response.Filter is ThresholdFilter)
                {
                    compressingFilter = (application.Response.Filter as ThresholdFilter).CompressingFilter;
                }
                else
                {
                    compressingFilter = application.Response.Filter as CompressingFilter;
                }
                ILog logger = null;
                try
                {
                    logger = LogManager.GetLogger(typeof(FluorineGateway));
                }
                catch
                {
                }
                if (((compressingFilter != null) && (logger != null)) && logger.get_IsDebugEnabled())
                {
                    float num = 0f;
                    if (compressingFilter.TotalIn != 0L)
                    {
                        num = (compressingFilter.TotalOut * 100L) / compressingFilter.TotalIn;
                    }
                    string fileName = Path.GetFileName(application.Request.Path);
                    if (application.Request.ContentType == "application/x-amf")
                    {
                        fileName = fileName + "(x-amf)";
                    }
                    string str2 = __Res.GetString("Compress_Info", new object[] { fileName, num });
                    logger.Debug(str2);
                }
            }
        }

        private void application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication httpApplication = (HttpApplication) sender;
            this.HandleXAmfEx(httpApplication);
            this.HandleSWX(httpApplication);
            this.HandleJSONRPC(httpApplication);
            this.HandleRtmpt(httpApplication);
        }

        private void application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication httpApplication = (HttpApplication) sender;
            this.CompressContent(httpApplication);
        }

        private void application_ReleaseRequestState(object sender, EventArgs e)
        {
            HttpApplication httpApplication = (HttpApplication) sender;
            this.CompressContent(httpApplication);
        }

        private IAsyncResult BeginPreRequestHandlerExecute(object source, EventArgs e, AsyncCallback cb, object state)
        {
            HttpApplication httpApplication = (HttpApplication) source;
            AsyncHandler handler = new AsyncHandler(cb, this, httpApplication, state);
            handler.Start();
            return handler;
        }

        private void CompressContent(HttpApplication httpApplication)
        {
            if (!httpApplication.Context.Items.Contains("__@fluorinehttpcompress"))
            {
                httpApplication.Context.Items.Add("__@fluorinehttpcompress", 1);
                HttpCompressSettings httpCompressSettings = FluorineConfiguration.Instance.HttpCompressSettings;
                if (((httpCompressSettings.HandleRequest != HandleRequest.None) && ((httpCompressSettings.HandleRequest != HandleRequest.Amf) || !(httpApplication.Request.ContentType != "application/x-amf"))) && (httpCompressSettings.CompressionLevel != CompressionLevels.None))
                {
                    string fileName = Path.GetFileName(httpApplication.Request.Path);
                    if (!httpCompressSettings.IsExcludedPath(fileName) && ((httpApplication.Response.ContentType != null) && !httpCompressSettings.IsExcludedMimeType(httpApplication.Response.ContentType)))
                    {
                        httpApplication.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;
                        string str2 = httpApplication.Request.Headers["Accept-Encoding"];
                        if (str2 != null)
                        {
                            CompressingFilter compressStream = GetFilterForScheme(str2.Split(new char[] { ',' }), httpApplication.Response.Filter, httpCompressSettings);
                            if (compressStream != null)
                            {
                                if (httpApplication.Request.ContentType == "application/x-amf")
                                {
                                    httpApplication.Response.Filter = new ThresholdFilter(compressStream, httpApplication.Response.Filter, httpCompressSettings.Threshold);
                                }
                                else
                                {
                                    httpApplication.Response.Filter = compressStream;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            try
            {
                lock (_objLock)
                {
                    if (messageServer != null)
                    {
                        messageServer.Stop();
                    }
                    messageServer = null;
                }
                LogManager.GetLogger(typeof(FluorineGateway)).Info("Stopped FluorineFx Gateway");
            }
            catch (Exception)
            {
            }
        }

        public void Dispose()
        {
        }

        private void EndPreRequestHandlerExecute(IAsyncResult ar)
        {
            AsyncHandler handler = ar as AsyncHandler;
        }

        internal static CompressingFilter GetFilterForScheme(string[] schemes, Stream output, HttpCompressSettings prefs)
        {
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            float num = 0f;
            float num2 = 0f;
            float num3 = 0f;
            for (int i = 0; i < schemes.Length; i++)
            {
                string acceptEncodingValue = schemes[i].Trim().ToLower();
                if (acceptEncodingValue.StartsWith("deflate"))
                {
                    flag = true;
                    float quality = GetQuality(acceptEncodingValue);
                    if (num < quality)
                    {
                        num = quality;
                    }
                }
                else if (acceptEncodingValue.StartsWith("gzip") || acceptEncodingValue.StartsWith("x-gzip"))
                {
                    flag2 = true;
                    float num6 = GetQuality(acceptEncodingValue);
                    if (num2 < num6)
                    {
                        num2 = num6;
                    }
                }
                else if (acceptEncodingValue.StartsWith("*"))
                {
                    flag3 = true;
                    float num7 = GetQuality(acceptEncodingValue);
                    if (num3 < num7)
                    {
                        num3 = num7;
                    }
                }
            }
            bool flag6 = flag3 && (num3 > 0f);
            bool flag4 = (flag && (num > 0f)) || (!flag && flag6);
            bool flag5 = (flag2 && (num2 > 0f)) || (!flag2 && flag6);
            if (!(!flag4 || flag))
            {
                num = num3;
            }
            if (!(!flag5 || flag2))
            {
                num2 = num3;
            }
            if ((flag4 || flag5) || flag6)
            {
                if (flag4 && (!flag5 || (num > num2)))
                {
                    return new DeflateFilter(output, prefs.CompressionLevel);
                }
                if (flag5 && (!flag4 || (num < num2)))
                {
                    return new GZipFilter(output);
                }
                if (flag4 && ((prefs.PreferredAlgorithm == Algorithms.Deflate) || (prefs.PreferredAlgorithm == Algorithms.Default)))
                {
                    return new DeflateFilter(output, prefs.CompressionLevel);
                }
                if (flag5 && (prefs.PreferredAlgorithm == Algorithms.GZip))
                {
                    return new GZipFilter(output);
                }
                if (flag4 || flag6)
                {
                    return new DeflateFilter(output, prefs.CompressionLevel);
                }
                if (flag5)
                {
                    return new GZipFilter(output);
                }
            }
            return null;
        }

        private static string GetPageName(string requestPath)
        {
            if (requestPath.IndexOf('?') != -1)
            {
                requestPath = requestPath.Substring(0, requestPath.IndexOf('?'));
            }
            return requestPath.Remove(0, requestPath.LastIndexOf("/") + 1);
        }

        private static float GetQuality(string acceptEncodingValue)
        {
            int index = acceptEncodingValue.IndexOf("q=");
            if (index >= 0)
            {
                float num2 = 0f;
                try
                {
                    num2 = float.Parse(acceptEncodingValue.Substring(index + 2, acceptEncodingValue.Length - (index + 2)));
                }
                catch (FormatException)
                {
                }
                return num2;
            }
            return 1f;
        }

        internal void HandleJSONRPC(HttpApplication httpApplication)
        {
            if (GetPageName(httpApplication.Request.RawUrl).ToLower() == "jsongateway.aspx")
            {
                httpApplication.Response.Clear();
                ILog logger = null;
                try
                {
                    logger = LogManager.GetLogger(typeof(FluorineGateway));
                    GlobalContext.get_Properties().set_Item("ClientIP", HttpContext.Current.Request.UserHostAddress);
                }
                catch
                {
                }
                if ((logger != null) && logger.get_IsDebugEnabled())
                {
                    logger.Debug(__Res.GetString("Json_Begin"));
                }
                try
                {
                    FluorineWebContext.Initialize();
                    new JsonRpcHandler(httpApplication.Context).ProcessRequest();
                    if ((logger != null) && logger.get_IsDebugEnabled())
                    {
                        logger.Debug(__Res.GetString("Json_End"));
                    }
                    httpApplication.CompleteRequest();
                }
                catch (Exception exception)
                {
                    if (logger != null)
                    {
                        logger.Fatal(__Res.GetString("Json_Fatal"), exception);
                    }
                    httpApplication.Response.Clear();
                    httpApplication.Response.ClearHeaders();
                    httpApplication.Response.Status = __Res.GetString("Json_Fatal404") + " " + exception.Message;
                    httpApplication.CompleteRequest();
                }
            }
        }

        internal void HandleRtmpt(HttpApplication httpApplication)
        {
            if (httpApplication.Request.ContentType == "application/x-fcs")
            {
                httpApplication.Response.Clear();
                httpApplication.Response.ContentType = "application/x-fcs";
                ILog logger = null;
                try
                {
                    logger = LogManager.GetLogger(typeof(FluorineGateway));
                    GlobalContext.get_Properties().set_Item("ClientIP", HttpContext.Current.Request.UserHostAddress);
                }
                catch
                {
                }
                if ((logger != null) && logger.get_IsDebugEnabled())
                {
                    logger.Debug(__Res.GetString("Rtmpt_Begin"));
                }
                try
                {
                    FluorineWebContext.Initialize();
                    if (httpApplication.Request.Headers["RTMPT-command"] != null)
                    {
                        logger.Debug(string.Format("ISAPI rewrite, original URL {0}", httpApplication.Request.Headers["RTMPT-command"]));
                    }
                    if (messageServer != null)
                    {
                        messageServer.ServiceRtmpt();
                    }
                    else if (logger != null)
                    {
                        logger.Fatal(__Res.GetString("MessageServer_AccessFail"));
                    }
                    if ((logger != null) && logger.get_IsDebugEnabled())
                    {
                        logger.Debug(__Res.GetString("Rtmpt_End"));
                    }
                    httpApplication.CompleteRequest();
                }
                catch (Exception exception)
                {
                    if (logger != null)
                    {
                        logger.Fatal(__Res.GetString("Rtmpt_Fatal"), exception);
                    }
                    httpApplication.Response.Clear();
                    httpApplication.Response.ClearHeaders();
                    httpApplication.Response.Status = __Res.GetString("Rtmpt_Fatal404") + " " + exception.Message;
                    httpApplication.CompleteRequest();
                }
            }
        }

        internal void HandleSWX(HttpApplication httpApplication)
        {
            if (GetPageName(httpApplication.Request.RawUrl).ToLower() == "swxgateway.aspx")
            {
                httpApplication.Response.Clear();
                ILog logger = null;
                try
                {
                    logger = LogManager.GetLogger(typeof(FluorineGateway));
                    GlobalContext.get_Properties().set_Item("ClientIP", HttpContext.Current.Request.UserHostAddress);
                }
                catch
                {
                }
                if ((logger != null) && logger.get_IsDebugEnabled())
                {
                    logger.Debug(__Res.GetString("Swx_Begin"));
                }
                try
                {
                    FluorineWebContext.Initialize();
                    new SwxHandler().Handle(httpApplication);
                    if ((logger != null) && logger.get_IsDebugEnabled())
                    {
                        logger.Debug(__Res.GetString("Swx_End"));
                    }
                    httpApplication.CompleteRequest();
                }
                catch (Exception exception)
                {
                    if (logger != null)
                    {
                        logger.Fatal(__Res.GetString("Swx_Fatal"), exception);
                    }
                    httpApplication.Response.Clear();
                    httpApplication.Response.ClearHeaders();
                    httpApplication.Response.Status = __Res.GetString("Swx_Fatal404") + " " + exception.Message;
                    httpApplication.CompleteRequest();
                }
            }
        }

        internal void HandleXAmfEx(HttpApplication httpApplication)
        {
            if (httpApplication.Request.ContentType == "application/x-amf")
            {
                this.CompressContent(httpApplication);
                httpApplication.Response.Clear();
                httpApplication.Response.ContentType = "application/x-amf";
                ILog logger = null;
                try
                {
                    logger = LogManager.GetLogger(typeof(FluorineGateway));
                    GlobalContext.get_Properties().set_Item("ClientIP", HttpContext.Current.Request.UserHostAddress);
                }
                catch
                {
                }
                if ((logger != null) && logger.get_IsDebugEnabled())
                {
                    logger.Debug(__Res.GetString("Amf_Begin"));
                }
                try
                {
                    FluorineWebContext.Initialize();
                    if (messageServer != null)
                    {
                        messageServer.Service();
                    }
                    else if (logger != null)
                    {
                        logger.Fatal(__Res.GetString("MessageServer_AccessFail"));
                    }
                    if ((logger != null) && logger.get_IsDebugEnabled())
                    {
                        logger.Debug(__Res.GetString("Amf_End"));
                    }
                    httpApplication.CompleteRequest();
                }
                catch (Exception exception)
                {
                    if (logger != null)
                    {
                        logger.Fatal(__Res.GetString("Amf_Fatal"), exception);
                    }
                    httpApplication.Response.Clear();
                    httpApplication.Response.ClearHeaders();
                    httpApplication.Response.Status = __Res.GetString("Amf_Fatal404") + " " + exception.Message;
                    httpApplication.CompleteRequest();
                }
            }
        }

        public void Init(HttpApplication application)
        {
            Exception exception;
            object obj2;
            if (!_initialized)
            {
                lock ((obj2 = _objLock))
                {
                    if (!_initialized)
                    {
                        try
                        {
                            new PermissionSet(PermissionState.Unrestricted).Demand();
                            this.WireAppDomain();
                        }
                        catch (SecurityException)
                        {
                        }
                        _initialized = true;
                    }
                }
            }
            application.BeginRequest += new EventHandler(this.application_BeginRequest);
            if (!FluorineConfiguration.Instance.FluorineSettings.Runtime.AsyncHandler)
            {
                application.PreRequestHandlerExecute += new EventHandler(this.application_PreRequestHandlerExecute);
            }
            else
            {
                application.AddOnPreRequestHandlerExecuteAsync(new BeginEventHandler(this.BeginPreRequestHandlerExecute), new EndEventHandler(this.EndPreRequestHandlerExecute));
            }
            application.AuthenticateRequest += new EventHandler(this.application_AuthenticateRequest);
            application.ReleaseRequestState += new EventHandler(this.application_ReleaseRequestState);
            application.PreSendRequestHeaders += new EventHandler(this.application_PreSendRequestHeaders);
            application.EndRequest += new EventHandler(this.application_EndRequest);
            FluorineWebContext.Initialize();
            if (serviceBrowserRenderer == null)
            {
                lock ((obj2 = _objLock))
                {
                    if (serviceBrowserRenderer == null)
                    {
                        try
                        {
                            LogManager.GetLogger(typeof(FluorineGateway)).Info(__Res.GetString("ServiceBrowser_Aquire"));
                        }
                        catch
                        {
                        }
                        try
                        {
                            Type type = ObjectFactory.Locate("FluorineFx.ServiceBrowser.ServiceBrowserRenderer");
                            if (type != null)
                            {
                                serviceBrowserRenderer = Activator.CreateInstance(type) as IServiceBrowserRenderer;
                                if (serviceBrowserRenderer != null)
                                {
                                    try
                                    {
                                        LogManager.GetLogger(typeof(FluorineGateway)).Info(__Res.GetString("ServiceBrowser_Aquired"));
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                        catch (Exception exception2)
                        {
                            exception = exception2;
                            try
                            {
                                LogManager.GetLogger(typeof(FluorineGateway)).Fatal(__Res.GetString("ServiceBrowser_AquireFail"), exception);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            if (messageServer == null)
            {
                lock ((obj2 = _objLock))
                {
                    if (messageServer == null)
                    {
                        try
                        {
                            ILog logger = LogManager.GetLogger(typeof(FluorineGateway));
                            logger.Info("************************************");
                            logger.Info(__Res.GetString("Fluorine_Start"));
                            logger.Info(__Res.GetString("Fluorine_Version", new object[] { Assembly.GetExecutingAssembly().GetName().Version }));
                            logger.Info("************************************");
                            logger.Info(__Res.GetString("MessageServer_Create"));
                        }
                        catch
                        {
                        }
                        messageServer = new MessageServer();
                        try
                        {
                            string configPath = Path.Combine(Path.Combine(HttpRuntime.AppDomainAppPath, "WEB-INF"), "flex");
                            messageServer.Init(configPath, serviceBrowserRenderer != null);
                            messageServer.Start();
                            try
                            {
                                LogManager.GetLogger(typeof(FluorineGateway)).Info(__Res.GetString("MessageServer_Started"));
                            }
                            catch
                            {
                            }
                            HttpContext.Current.Application["__@fluorinemessageserver"] = messageServer;
                        }
                        catch (Exception exception3)
                        {
                            exception = exception3;
                            try
                            {
                                LogManager.GetLogger(typeof(FluorineGateway)).Fatal(__Res.GetString("MessageServer_StartError"), exception);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }

        private void OnUnhandledException(object o, UnhandledExceptionEventArgs e)
        {
            if (Interlocked.Exchange(ref _unhandledExceptionCount, 1) == 0)
            {
                StringBuilder builder = new StringBuilder("\r\n\r\nUnhandledException logged by UnhandledExceptionModule.dll:\r\n\r\nappId=");
                string data = (string) AppDomain.CurrentDomain.GetData(".appId");
                if (data != null)
                {
                    builder.Append(data);
                }
                Exception exception = null;
                for (exception = (Exception) e.ExceptionObject; exception != null; exception = exception.InnerException)
                {
                    builder.AppendFormat("\r\n\r\ntype={0}\r\n\r\nmessage={1}\r\n\r\nstack=\r\n{2}\r\n\r\n", exception.GetType().FullName, exception.Message, exception.StackTrace);
                }
                new EventLog { Source = _sourceName }.WriteEntry(builder.ToString(), EventLogEntryType.Error);
                LogManager.GetLogger(typeof(FluorineGateway)).Fatal(builder.ToString());
            }
        }

        private void WireAppDomain()
        {
            string path = Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "webengine.dll");
            if (File.Exists(path))
            {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(path);
                _sourceName = string.Format(CultureInfo.InvariantCulture, "ASP.NET {0}.{1}.{2}.0", new object[] { versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart });
                if (EventLog.SourceExists(_sourceName))
                {
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.OnUnhandledException);
                }
            }
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(this.CurrentDomain_DomainUnload);
            LogManager.GetLogger(typeof(FluorineGateway)).Info("Adding handler for the DomainUnload event");
        }
    }
}

