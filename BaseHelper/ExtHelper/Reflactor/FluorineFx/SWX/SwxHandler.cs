namespace FluorineFx.SWX
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.HttpCompress;
    using FluorineFx.Invocation;
    using FluorineFx.Json;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.Security;
    using System.Web;

    internal class SwxHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SwxHandler));

        public void Handle(HttpApplication httpApplication)
        {
            object data = null;
            string str = null;
            Hashtable hashtable;
            bool debug = false;
            CompressionLevels none = CompressionLevels.None;
            if ((FluorineConfiguration.Instance.HttpCompressSettings.HandleRequest == HandleRequest.Swx) || (FluorineConfiguration.Instance.HttpCompressSettings.HandleRequest == HandleRequest.All))
            {
                none = FluorineConfiguration.Instance.HttpCompressSettings.CompressionLevel;
            }
            bool allowDomain = FluorineConfiguration.Instance.SwxSettings.AllowDomain;
            try
            {
                NameValueCollection queryString;
                string str5;
                if (httpApplication.Request.RequestType == "GET")
                {
                    queryString = httpApplication.Request.QueryString;
                }
                else
                {
                    queryString = httpApplication.Request.Form;
                }
                string typeName = queryString["serviceClass"];
                string methodName = queryString["method"];
                string str4 = queryString["args"];
                str = queryString["url"];
                debug = (queryString["debug"] != null) ? Convert.ToBoolean(queryString["debug"]) : false;
                if (str != null)
                {
                    str = HttpUtility.UrlDecode(str);
                    str = str.Replace("///", "//");
                    try
                    {
                        UriBuilder builder = new UriBuilder(str);
                    }
                    catch (UriFormatException)
                    {
                        if (log.get_IsWarnEnabled())
                        {
                            str5 = __Res.GetString("Swx_InvalidCrossDomainUrl", new object[] { str });
                            log.Warn(str5);
                        }
                        str = null;
                    }
                }
                else if (allowDomain && log.get_IsWarnEnabled())
                {
                    str5 = "No referring URL received from Flash. Cross-domain will not be supported on this call regardless of allowDomain setting";
                    log.Warn(str5);
                }
                switch (str4)
                {
                    case "undefined":
                    case string.Empty:
                        str4 = "[]";
                        break;
                }
                object[] arguments = (JavaScriptConvert.DeserializeObject(str4.Replace(@"\t", "\t").Replace(@"\n", "\n").Replace(@"\'", "'")) as JavaScriptArray).ToArray();
                object obj3 = ObjectFactory.CreateInstance(typeName);
                MethodInfo methodInfo = MethodHandler.GetMethod(obj3.GetType(), methodName, arguments);
                ParameterInfo[] parameters = methodInfo.GetParameters();
                TypeHelper.NarrowValues(arguments, parameters);
                data = new InvocationHandler(methodInfo).Invoke(obj3, arguments);
            }
            catch (TargetInvocationException exception)
            {
                hashtable = new Hashtable();
                hashtable["error"] = true;
                hashtable["code"] = "SERVER.PROCESSING";
                hashtable["message"] = exception.InnerException.Message;
                data = hashtable;
            }
            catch (Exception exception2)
            {
                hashtable = new Hashtable();
                hashtable["error"] = true;
                hashtable["code"] = "SERVER.PROCESSING";
                hashtable["message"] = exception2.Message;
                data = hashtable;
            }
            byte[] buffer = new SwxAssembler().WriteSwf(data, debug, none, str, allowDomain);
            httpApplication.Response.Clear();
            httpApplication.Response.ClearHeaders();
            httpApplication.Response.Buffer = true;
            httpApplication.Response.ContentType = "application/swf";
            int length = buffer.Length;
            httpApplication.Response.AppendHeader("Content-Length", length.ToString());
            httpApplication.Response.AppendHeader("Content-Disposition", "attachment; filename=data.swf");
            if (buffer.Length > 0)
            {
                httpApplication.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            try
            {
                httpApplication.Response.Flush();
            }
            catch (SecurityException)
            {
            }
        }
    }
}

