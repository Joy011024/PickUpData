namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx;
    using FluorineFx.Context;
    using FluorineFx.Exceptions;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Messaging;
    using FluorineFx.Messaging.Api.Service;
    using FluorineFx.Messaging.Api.SO;
    using FluorineFx.Messaging.Api.Stream;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Messaging.Rtmp.SO;
    using FluorineFx.Messaging.Rtmp.Stream;
    using FluorineFx.Messaging.Services;
    using log4net;
    using System;
    using System.Collections;

    internal class RtmpHandler : BaseRtmpHandler
    {
        private IEndpoint _endpoint;
        private static readonly ILog log = LogManager.GetLogger(typeof(RtmpHandler));

        public RtmpHandler(IEndpoint endpoint)
        {
            this._endpoint = endpoint;
        }

        public override void ConnectionOpened(RtmpConnection connection)
        {
            base.ConnectionOpened(connection);
            if (connection.Context.Mode == RtmpMode.Server)
            {
                FluorineRtmpContext.Initialize(connection);
                connection.StartWaitForHandshake();
            }
        }

        public static void InvokeCall(RtmpConnection connection, IServiceCall serviceCall)
        {
            IScope scope = connection.Scope;
            if (!scope.HasHandler || scope.Handler.ServiceCall(connection, serviceCall))
            {
                scope.Context.ServiceInvoker.Invoke(serviceCall, scope);
            }
        }

        private static bool InvokeCall(RtmpConnection connection, IServiceCall serviceCall, object service)
        {
            IScope scope = connection.Scope;
            IScopeContext context = scope.Context;
            if (log.get_IsDebugEnabled())
            {
                log.Debug("Scope: " + scope);
                log.Debug("Service: " + service);
                log.Debug("Context: " + context);
            }
            return context.ServiceInvoker.Invoke(serviceCall, service);
        }

        public override void MessageSent(RtmpConnection connection, object message)
        {
            base.MessageSent(connection, message);
            RtmpPacket packet = message as RtmpPacket;
            int channelId = packet.Header.ChannelId;
            IClientStream streamByChannelId = null;
            if (connection is IStreamCapableConnection)
            {
                streamByChannelId = (connection as IStreamCapableConnection).GetStreamByChannelId(channelId);
            }
            if ((streamByChannelId != null) && (streamByChannelId is PlaylistSubscriberStream))
            {
                (streamByChannelId as PlaylistSubscriberStream).Written(packet.Message);
            }
        }

        protected override void OnChunkSize(RtmpConnection connection, RtmpChannel channel, RtmpHeader source, ChunkSize chunkSize)
        {
            if (connection is IStreamCapableConnection)
            {
                IStreamCapableConnection connection2 = connection as IStreamCapableConnection;
                foreach (IClientStream stream in connection2.GetStreams())
                {
                    if (stream is IClientBroadcastStream)
                    {
                        IClientBroadcastStream stream2 = stream as IClientBroadcastStream;
                        IBroadcastScope basicScope = stream2.Scope.GetBasicScope("bs", stream2.PublishedName) as IBroadcastScope;
                        if (basicScope != null)
                        {
                            OOBControlMessage oobCtrlMsg = new OOBControlMessage {
                                Target = "ClientBroadcastStream",
                                ServiceName = "chunkSize"
                            };
                            oobCtrlMsg.ServiceParameterMap.Add("chunkSize", chunkSize.Size);
                            basicScope.SendOOBControlMessage(null, oobCtrlMsg);
                            if (log.get_IsDebugEnabled())
                            {
                                log.Debug(string.Concat(new object[] { "Sending chunksize ", chunkSize, " to ", stream2.Provider }));
                            }
                        }
                    }
                }
            }
        }

        protected override void OnClientBW(RtmpConnection connection, RtmpChannel channel, RtmpHeader source, ClientBW clientBW)
        {
        }

        protected override void OnFlexInvoke(RtmpConnection connection, RtmpChannel channel, RtmpHeader header, FlexInvoke invoke)
        {
            IMessage message = null;
            if ((invoke.Parameters != null) && (invoke.Parameters.Length > 0))
            {
                message = invoke.Parameters[0] as IMessage;
            }
            if (message != null)
            {
                MessageBroker messageBroker = this.Endpoint.GetMessageBroker();
                if (message.clientId == null)
                {
                    message.clientId = Guid.NewGuid().ToString("D");
                }
                IMessage message2 = messageBroker.RouteMessage(message, this.Endpoint);
                FlexInvoke invoke2 = new FlexInvoke {
                    InvokeId = invoke.InvokeId
                };
                if (message2 is ErrorMessage)
                {
                    invoke2.SetResponseFailure();
                }
                else
                {
                    invoke2.SetResponseSuccess();
                }
                invoke2.Response = message2;
                channel.Write(invoke2);
            }
            else
            {
                this.OnInvoke(connection, channel, header, invoke);
            }
        }

        protected override void OnInvoke(RtmpConnection connection, RtmpChannel channel, RtmpHeader header, Notify invoke)
        {
            IServiceCall serviceCall = invoke.ServiceCall;
            if (serviceCall.ServiceMethodName.Equals("_result") || serviceCall.ServiceMethodName.Equals("_error"))
            {
                base.HandlePendingCallResult(connection, invoke);
            }
            else
            {
                bool flag = false;
                string serviceMethodName = null;
                if (serviceCall.ServiceName == null)
                {
                    StatusASO statusObject;
                    Exception exception2;
                    serviceMethodName = serviceCall.ServiceMethodName;
                    switch (serviceMethodName)
                    {
                        case "connect":
                        {
                            if (connection.IsConnected)
                            {
                                InvokeCall(connection, serviceCall);
                                break;
                            }
                            IDictionary connectionParameters = invoke.ConnectionParameters;
                            string host = null;
                            if (connectionParameters.Contains("tcUrl"))
                            {
                                host = BaseRtmpHandler.GetHostname(connectionParameters["tcUrl"] as string);
                            }
                            if ((host != null) && (host.IndexOf(":") != -1))
                            {
                                host = host.Substring(0, host.IndexOf(":"));
                            }
                            string str3 = connectionParameters["app"] as string;
                            string path = connectionParameters["app"] as string;
                            if ((path != null) && (path.IndexOf("?") != -1))
                            {
                                int index = path.IndexOf("?");
                                connectionParameters["queryString"] = path.Substring(index);
                                path = path.Substring(0, index);
                            }
                            connectionParameters["path"] = path;
                            connection.Setup(host, path, connectionParameters);
                            try
                            {
                                IGlobalScope globalScope = this.Endpoint.GetMessageBroker().GlobalScope;
                                if (globalScope == null)
                                {
                                    serviceCall.Status = 0x10;
                                    if (serviceCall is IPendingServiceCall)
                                    {
                                        statusObject = StatusASO.GetStatusObject("NetConnection.Connect.InvalidApp", connection.ObjectEncoding);
                                        statusObject.description = "No global scope on this server.";
                                        (serviceCall as IPendingServiceCall).Result = statusObject;
                                    }
                                    log.Info(string.Format("No application scope found for {0} on host {1}. Misspelled or missing application folder?", path, host));
                                    flag = true;
                                }
                                else
                                {
                                    IScopeContext context = globalScope.Context;
                                    IScope scope = null;
                                    try
                                    {
                                        scope = context.ResolveScope(globalScope, path);
                                    }
                                    catch (ScopeNotFoundException)
                                    {
                                        if (log.get_IsErrorEnabled())
                                        {
                                            log.Error(__Res.GetString("Scope_NotFound", new object[] { path }));
                                        }
                                        serviceCall.Status = 0x10;
                                        if (serviceCall is IPendingServiceCall)
                                        {
                                            statusObject = StatusASO.GetStatusObject("NetConnection.Connect.Rejected", connection.ObjectEncoding);
                                            statusObject.description = "No scope \"" + path + "\" on this server.";
                                            (serviceCall as IPendingServiceCall).Result = statusObject;
                                        }
                                        flag = true;
                                    }
                                    catch (ScopeShuttingDownException)
                                    {
                                        serviceCall.Status = 0x15;
                                        if (serviceCall is IPendingServiceCall)
                                        {
                                            statusObject = StatusASO.GetStatusObject("NetConnection.Connect.AppShutdown", connection.ObjectEncoding);
                                            statusObject.description = "Application at \"" + path + "\" is currently shutting down.";
                                            (serviceCall as IPendingServiceCall).Result = statusObject;
                                        }
                                        log.Info(string.Format("Application at {0} currently shutting down on {1}", path, host));
                                        flag = true;
                                    }
                                    if (scope != null)
                                    {
                                        StatusASO saso2;
                                        if (log.get_IsInfoEnabled())
                                        {
                                            log.Info(__Res.GetString("Scope_Connect", new object[] { scope.Name }));
                                        }
                                        try
                                        {
                                            bool flag2;
                                            if (str3 == string.Empty)
                                            {
                                                connection.SetIsFlexClient(true);
                                                flag2 = connection.Connect(scope, serviceCall.Arguments);
                                                if (flag2)
                                                {
                                                    string str5;
                                                    if ((serviceCall.Arguments != null) && (serviceCall.Arguments.Length == 3))
                                                    {
                                                        str5 = serviceCall.Arguments[2] as string;
                                                        AuthenticationService service = this.Endpoint.GetMessageBroker().GetService("authentication-service") as AuthenticationService;
                                                        service.Authenticate(str5);
                                                    }
                                                    if ((serviceCall.Arguments != null) && (serviceCall.Arguments.Length == 1))
                                                    {
                                                        str5 = serviceCall.Arguments[0] as string;
                                                        (this.Endpoint.GetMessageBroker().GetService("authentication-service") as AuthenticationService).Authenticate(str5);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                connection.SetIsFlexClient(false);
                                                flag2 = connection.Connect(scope, serviceCall.Arguments);
                                            }
                                            if (flag2)
                                            {
                                                if (log.get_IsDebugEnabled())
                                                {
                                                    log.Debug("Connected RtmpClient: " + connection.Client.Id);
                                                }
                                                serviceCall.Status = 2;
                                                if (serviceCall is IPendingServiceCall)
                                                {
                                                    saso2 = StatusASO.GetStatusObject("NetConnection.Connect.Success", connection.ObjectEncoding);
                                                    saso2.Add("id", connection.Client.Id);
                                                    (serviceCall as IPendingServiceCall).Result = saso2;
                                                }
                                                connection.GetChannel(2).Write(new Ping(0, 0, -1));
                                                connection.StartRoundTripMeasurement();
                                            }
                                            else
                                            {
                                                if (log.get_IsDebugEnabled())
                                                {
                                                    log.Debug("Connect failed");
                                                }
                                                serviceCall.Status = 0x12;
                                                if (serviceCall is IPendingServiceCall)
                                                {
                                                    (serviceCall as IPendingServiceCall).Result = StatusASO.GetStatusObject("NetConnection.Connect.Rejected", connection.ObjectEncoding);
                                                }
                                                flag = true;
                                            }
                                        }
                                        catch (ClientRejectedException exception)
                                        {
                                            if (log.get_IsDebugEnabled())
                                            {
                                                log.Debug("Connect rejected");
                                            }
                                            serviceCall.Status = 0x12;
                                            if (serviceCall is IPendingServiceCall)
                                            {
                                                saso2 = StatusASO.GetStatusObject("NetConnection.Connect.Rejected", connection.ObjectEncoding);
                                                saso2.Application = exception.Reason;
                                                (serviceCall as IPendingServiceCall).Result = saso2;
                                            }
                                            flag = true;
                                        }
                                    }
                                }
                            }
                            catch (Exception exception5)
                            {
                                exception2 = exception5;
                                if (log.get_IsErrorEnabled())
                                {
                                    log.Error("Error connecting", exception2);
                                }
                                serviceCall.Status = 20;
                                if (serviceCall is IPendingServiceCall)
                                {
                                    (serviceCall as IPendingServiceCall).Result = StatusASO.GetStatusObject("NetConnection.Connect.Failed", connection.ObjectEncoding);
                                }
                                flag = true;
                            }
                            break;
                        }
                        case "disconnect":
                            connection.Close();
                            break;

                        case "createStream":
                        case "deleteStream":
                        case "releaseStream":
                        case "publish":
                        case "play":
                        case "seek":
                        case "pause":
                        case "closeStream":
                        case "receiveVideo":
                        case "receiveAudio":
                        {
                            IStreamService scopeService = ScopeUtils.GetScopeService(connection.Scope, typeof(IStreamService)) as IStreamService;
                            statusObject = null;
                            try
                            {
                                if (!InvokeCall(connection, serviceCall, scopeService))
                                {
                                    statusObject = StatusASO.GetStatusObject("NetStream.InvalidArg", connection.ObjectEncoding);
                                    statusObject.description = string.Concat(new object[] { "Failed to ", serviceMethodName, " (stream ID: ", header.StreamId, ")" });
                                }
                            }
                            catch (Exception exception6)
                            {
                                exception2 = exception6;
                                log.Error("Error while invoking " + serviceMethodName + " on stream service.", exception2);
                                statusObject = StatusASO.GetStatusObject("NetStream.Failed", connection.ObjectEncoding);
                                statusObject.description = string.Concat(new object[] { "Error while invoking ", serviceMethodName, " (stream ID: ", header.StreamId, ")" });
                                statusObject.details = exception2.Message;
                            }
                            if (statusObject != null)
                            {
                                channel.SendStatus(statusObject);
                            }
                            break;
                        }
                        default:
                            if (connection.IsConnected)
                            {
                                InvokeCall(connection, serviceCall);
                            }
                            else
                            {
                                if (log.get_IsWarnEnabled())
                                {
                                    log.Warn("Not connected, closing connection");
                                }
                                connection.Close();
                            }
                            break;
                    }
                }
                if (invoke is FlexInvoke)
                {
                    FlexInvoke message = new FlexInvoke {
                        InvokeId = invoke.InvokeId
                    };
                    message.SetResponseSuccess();
                    if (serviceCall is IPendingServiceCall)
                    {
                        IPendingServiceCall call2 = (IPendingServiceCall) serviceCall;
                        message.Response = call2.Result;
                    }
                    channel.Write(message);
                }
                else if (invoke is Invoke)
                {
                    if ((header.StreamId != 0) && ((serviceCall.Status == 4) || (serviceCall.Status == 3)))
                    {
                        if (log.get_IsDebugEnabled())
                        {
                            log.Debug("Method does not have return value, do not reply");
                        }
                        return;
                    }
                    Invoke invoke3 = new Invoke {
                        ServiceCall = serviceCall,
                        InvokeId = invoke.InvokeId
                    };
                    channel.Write(invoke3);
                }
                if (flag)
                {
                    connection.Close();
                }
                if (serviceMethodName == "connect")
                {
                    connection.Context.ObjectEncoding = connection.ObjectEncoding;
                }
            }
        }

        protected override void OnPing(RtmpConnection connection, RtmpChannel channel, RtmpHeader source, Ping ping)
        {
            switch (ping.Value1)
            {
                case 3:
                {
                    if (ping.Value2 == 0)
                    {
                        log.Warn("Unhandled ping: " + ping);
                        break;
                    }
                    IClientStream streamById = null;
                    if (connection is IStreamCapableConnection)
                    {
                        streamById = (connection as IStreamCapableConnection).GetStreamById(ping.Value2);
                    }
                    int bufferDuration = ping.Value3;
                    if (streamById == null)
                    {
                        if (connection is RtmpServerConnection)
                        {
                            (connection as RtmpServerConnection).RememberStreamBufferDuration(ping.Value2, bufferDuration);
                        }
                        if (log.get_IsInfoEnabled())
                        {
                            log.Info("Remembering client buffer on stream: " + bufferDuration);
                        }
                        break;
                    }
                    streamById.SetClientBufferDuration(bufferDuration);
                    if (log.get_IsInfoEnabled())
                    {
                        log.Info("Setting client buffer on stream: " + bufferDuration);
                    }
                    break;
                }
                case 7:
                    connection.PingReceived(ping);
                    break;

                default:
                    log.Warn("Unhandled ping: " + ping);
                    break;
            }
        }

        protected override void OnServerBW(RtmpConnection connection, RtmpChannel channel, RtmpHeader source, ServerBW serverBW)
        {
        }

        protected override void OnSharedObject(RtmpConnection connection, RtmpChannel channel, RtmpHeader header, SharedObjectMessage message)
        {
            ISharedObject sharedObject = null;
            string name = message.Name;
            IScope scope = connection.Scope;
            bool isPersistent = message.IsPersistent;
            if (scope == null)
            {
                SharedObjectMessage message2;
                if (connection.ObjectEncoding == ObjectEncoding.AMF0)
                {
                    message2 = new SharedObjectMessage(name, 0, isPersistent);
                }
                else
                {
                    message2 = new FlexSharedObjectMessage(name, 0, isPersistent);
                }
                message2.AddEvent(new SharedObjectEvent(SharedObjectEventType.CLIENT_STATUS, "SharedObject.NoObjectFound", "error"));
                connection.GetChannel(3).Write(message2);
            }
            else
            {
                ISharedObjectService scopeService = ScopeUtils.GetScopeService(scope, typeof(ISharedObjectService)) as ISharedObjectService;
                if (!scopeService.HasSharedObject(scope, name))
                {
                    ISharedObjectSecurityService service2 = ScopeUtils.GetScopeService(scope, typeof(ISharedObjectSecurityService)) as ISharedObjectSecurityService;
                    if (service2 != null)
                    {
                        IEnumerator sharedObjectSecurity = service2.GetSharedObjectSecurity();
                        while (sharedObjectSecurity.MoveNext())
                        {
                            ISharedObjectSecurity current = sharedObjectSecurity.Current as ISharedObjectSecurity;
                            if (!current.IsCreationAllowed(scope, name, isPersistent))
                            {
                                SendSOCreationFailed(connection, name, isPersistent);
                                return;
                            }
                        }
                    }
                    if (!scopeService.CreateSharedObject(scope, name, isPersistent))
                    {
                        SendSOCreationFailed(connection, name, isPersistent);
                        return;
                    }
                }
                sharedObject = scopeService.GetSharedObject(scope, name);
                if (sharedObject.IsPersistentObject != isPersistent)
                {
                    log.Debug(string.Format("Shared object '{0}' persistence mismatch", name));
                    SendSOPersistenceMismatch(connection, name, isPersistent);
                }
                else
                {
                    sharedObject.DispatchEvent(message);
                }
            }
        }

        private static void SendSOCreationFailed(RtmpConnection connection, string name, bool persistent)
        {
            SharedObjectMessage message;
            if (connection.ObjectEncoding == ObjectEncoding.AMF0)
            {
                message = new SharedObjectMessage(name, 0, persistent);
            }
            else
            {
                message = new FlexSharedObjectMessage(name, 0, persistent);
            }
            message.AddEvent(new SharedObjectEvent(SharedObjectEventType.CLIENT_STATUS, "SharedObject.ObjectCreationFailed", "error"));
            connection.GetChannel(3).Write(message);
        }

        private static void SendSOPersistenceMismatch(RtmpConnection connection, string name, bool persistent)
        {
            SharedObjectMessage message;
            if (connection.ObjectEncoding == ObjectEncoding.AMF0)
            {
                message = new SharedObjectMessage(name, 0, persistent);
            }
            else
            {
                message = new FlexSharedObjectMessage(name, 0, persistent);
            }
            message.AddEvent(new SharedObjectEvent(SharedObjectEventType.CLIENT_STATUS, "SharedObject.BadPersistence", "error"));
            connection.GetChannel(3).Write(message);
        }

        internal IEndpoint Endpoint
        {
            get
            {
                return this._endpoint;
            }
        }
    }
}

