namespace FluorineFx.Json.Rpc
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Json;
    using FluorineFx.Json.Services;
    using FluorineFx.Messaging;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;
    using System.Security;
    using System.Web;

    internal sealed class JsonRpcProxyGenerator : JsonRpcServiceFeature
    {
        private Hashtable _generators;
        private DateTime _lastModifiedTime;
        private static readonly ILog log = LogManager.GetLogger(typeof(JsonRpcProxyGenerator));

        public JsonRpcProxyGenerator(MessageBroker messageBroker) : base(messageBroker)
        {
            this._lastModifiedTime = DateTime.MinValue;
            this._generators = new Hashtable();
            this._generators.Add("default", new DefaultJsonRpcProxyGenerator());
            try
            {
                JsonRpcGeneratorCollection jsonRpcGenerators = FluorineConfiguration.Instance.FluorineSettings.JSonSettings.JsonRpcGenerators;
                for (int i = 0; i < jsonRpcGenerators.Count; i++)
                {
                    JsonRpcGenerator generator = jsonRpcGenerators[i];
                    IJsonRpcProxyGenerator generator2 = ObjectFactory.CreateInstance(generator.Type) as IJsonRpcProxyGenerator;
                    if (generator2 != null)
                    {
                        this._generators.Add(generator.Name, generator2);
                    }
                    else if (log.get_IsErrorEnabled())
                    {
                        log.Error(string.Format("JsonRpcGenerator {0} was not found", generator.Name));
                    }
                }
            }
            catch (Exception exception)
            {
                log.Fatal("Creating JsonRpcProxyGenerator failed", exception);
            }
        }

        private bool Modified()
        {
            DateTime time;
            if (this._lastModifiedTime == DateTime.MinValue)
            {
                return true;
            }
            string input = StringUtils.MaskNullString(base.Request.Headers["If-Modified-Since"]);
            int index = input.IndexOf(';');
            if (index >= 0)
            {
                input = input.Substring(0, index);
            }
            if (input.Length == 0)
            {
                return true;
            }
            try
            {
                time = DateTimeUtils.ParseInternetDate(input);
            }
            catch (FormatException)
            {
                return true;
            }
            DateTime time2 = this._lastModifiedTime;
            time2 = new DateTime(time2.Year, time2.Month, time2.Day, time2.Hour, time2.Minute, time2.Second);
            return (time2 > time);
        }

        protected override void ProcessRequest()
        {
            Destination destination = null;
            string destinationId;
            string s = base.Request.QueryString["source"];
            if (!StringUtils.IsNullOrEmpty(s))
            {
                destinationId = this.MessageBroker.GetDestinationId(s);
                destination = this.MessageBroker.GetDestination(destinationId);
            }
            else
            {
                destinationId = base.Request.QueryString["destination"];
                if (!StringUtils.IsNullOrEmpty(destinationId))
                {
                    destination = this.MessageBroker.GetDestination(destinationId);
                    s = destination.Source;
                }
            }
            FactoryInstance factoryInstance = destination.GetFactoryInstance();
            factoryInstance.Source = s;
            Type instanceClass = factoryInstance.GetInstanceClass();
            if (instanceClass != null)
            {
                ServiceClass service = JsonRpcServiceReflector.FromType(instanceClass);
                this.UpdateLastModifiedTime(instanceClass);
                if (!this.Modified())
                {
                    base.Response.StatusCode = 0x130;
                }
                else
                {
                    if (this._lastModifiedTime != DateTime.MinValue)
                    {
                        base.Response.Cache.SetCacheability(HttpCacheability.Public);
                        base.Response.Cache.SetLastModified(this._lastModifiedTime);
                    }
                    base.Response.ContentType = "text/javascript";
                    string str3 = service.Name + "Proxy.js";
                    base.Response.AppendHeader("Content-Disposition", "attachment; filename=" + str3);
                    string key = "default";
                    if (!StringUtils.IsNullOrEmpty(base.Request.QueryString["generator"]))
                    {
                        key = base.Request.QueryString["generator"];
                    }
                    if (this._generators.Contains(key))
                    {
                        (this._generators[key] as IJsonRpcProxyGenerator).WriteProxy(service, new IndentedTextWriter(base.Response.Output), base.Request);
                    }
                    else if (log.get_IsErrorEnabled())
                    {
                        log.Error(string.Format("JsonRpcGenerator {0} was not found", key));
                    }
                }
            }
        }

        private DateTime UpdateLastModifiedTime(Type type)
        {
            if (this._lastModifiedTime == DateTime.MinValue)
            {
                this._lastModifiedTime = DateTime.Now;
                try
                {
                    Uri uri = new Uri(type.Assembly.CodeBase);
                    if ((uri != null) && uri.IsFile)
                    {
                        string localPath = uri.LocalPath;
                        if (File.Exists(localPath))
                        {
                            try
                            {
                                this._lastModifiedTime = File.GetLastWriteTime(localPath);
                            }
                            catch (UnauthorizedAccessException)
                            {
                            }
                            catch (IOException)
                            {
                            }
                        }
                    }
                }
                catch (SecurityException)
                {
                }
            }
            return this._lastModifiedTime;
        }
    }
}

