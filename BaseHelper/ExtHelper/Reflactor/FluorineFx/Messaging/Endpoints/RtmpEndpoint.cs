namespace FluorineFx.Messaging.Endpoints
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Context;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmp;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;

    internal class RtmpEndpoint : EndpointBase
    {
        private static object _objLock = new object();
        private RtmpServer _rtmpServer;
        private static readonly ILog log = LogManager.GetLogger(typeof(RtmpEndpoint));
        public static string Slash = "/";

        public RtmpEndpoint(MessageBroker messageBroker, ChannelSettings channelSettings) : base(messageBroker, channelSettings)
        {
        }

        private void OnError(object sender, ServerErrorEventArgs e)
        {
            Debug.WriteLine(e.Exception.Message);
            if (log.get_IsErrorEnabled())
            {
                log.Error(__Res.GetString("RtmpEndpoint_Error"), e.Exception);
            }
        }

        public override void Push(IMessage message, MessageClient messageClient)
        {
            IMessageConnection messageConnection = messageClient.MessageConnection;
            Debug.Assert(messageConnection != null);
            if (messageConnection != null)
            {
                messageConnection.Push(message, messageClient);
            }
        }

        public override void Start()
        {
            try
            {
                string str;
                IPEndPoint point;
                if (log.get_IsInfoEnabled())
                {
                    log.Info(__Res.GetString("RtmpEndpoint_Start"));
                }
                IGlobalScope globalScope = base.GetMessageBroker().GlobalScope;
                if (FluorineContext.Current != null)
                {
                    str = Path.Combine(FluorineContext.Current.ApplicationBaseDirectory, "apps");
                }
                else
                {
                    str = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apps");
                }
                if (Directory.Exists(str))
                {
                    foreach (string str2 in Directory.GetDirectories(str))
                    {
                        DirectoryInfo info = new DirectoryInfo(str2);
                        string name = info.Name;
                        ApplicationConfiguration appConfig = ApplicationConfiguration.Load(Path.Combine(str2, "app.config"));
                        WebScope scope2 = new WebScope(this, globalScope, appConfig);
                        ScopeContext context = new ScopeContext("/" + name, globalScope.Context.ClientRegistry, globalScope.Context.ScopeResolver, globalScope.Context.ServiceInvoker, null);
                        scope2.Context = context;
                        IScopeHandler handler = ObjectFactory.CreateInstance(appConfig.ApplicationHandler.Type) as IScopeHandler;
                        if (handler == null)
                        {
                            log.Error(__Res.GetString("Type_InitError", new object[] { appConfig.ApplicationHandler.Type }));
                        }
                        scope2.Handler = handler;
                        scope2.SetContextPath("/" + name);
                        scope2.Register();
                    }
                }
                this._rtmpServer = new RtmpServer(this);
                UriBase uri = base._channelSettings.GetUri();
                int port = 0x78f;
                if ((uri.Port != null) && (uri.Port != string.Empty))
                {
                    port = Convert.ToInt32(uri.Port);
                }
                if (log.get_IsInfoEnabled())
                {
                    log.Info(__Res.GetString("RtmpEndpoint_Starting", new object[] { port.ToString() }));
                }
                if (base._channelSettings.BindAddress != null)
                {
                    point = new IPEndPoint(IPAddress.Parse(base._channelSettings.BindAddress), port);
                }
                else
                {
                    point = new IPEndPoint(IPAddress.Any, port);
                }
                this._rtmpServer.AddListener(point);
                this._rtmpServer.OnError += new ErrorHandler(this.OnError);
                this._rtmpServer.Start();
                if (log.get_IsInfoEnabled())
                {
                    log.Info(__Res.GetString("RtmpEndpoint_Started"));
                }
            }
            catch (Exception exception)
            {
                if (log.get_IsFatalEnabled())
                {
                    log.Fatal("RtmpEndpoint failed", exception);
                }
            }
        }

        public override void Stop()
        {
            try
            {
                if (log.get_IsInfoEnabled())
                {
                    log.Info(__Res.GetString("RtmpEndpoint_Stopping"));
                }
                if (this._rtmpServer != null)
                {
                    this._rtmpServer.Stop();
                    this._rtmpServer.OnError -= new ErrorHandler(this.OnError);
                    this._rtmpServer.Dispose();
                    this._rtmpServer = null;
                }
                if (log.get_IsInfoEnabled())
                {
                    log.Info(__Res.GetString("RtmpEndpoint_Stopped"));
                }
            }
            catch (Exception exception)
            {
                if (log.get_IsFatalEnabled())
                {
                    log.Fatal(__Res.GetString("RtmpEndpoint_Failed"), exception);
                }
            }
        }
    }
}

