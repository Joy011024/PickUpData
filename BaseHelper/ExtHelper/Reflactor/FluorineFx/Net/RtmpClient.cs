namespace FluorineFx.Net
{
    using FluorineFx;
    using FluorineFx.Exceptions;
    using FluorineFx.Invocation;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Service;
    using FluorineFx.Messaging.Rtmp;
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Messaging.Rtmp.Service;
    using FluorineFx.Messaging.Rtmp.SO;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Reflection;

    internal class RtmpClient : BaseRtmpHandler, INetConnectionClient, IPendingServiceCallback
    {
        private object[] _connectArguments;
        private RtmpClientConnection _connection;
        private Dictionary<string, object> _connectionParameters;
        private NetConnection _netConnection;
        private static readonly ILog log = LogManager.GetLogger(typeof(RtmpClient));

        public RtmpClient(NetConnection netConnection)
        {
            this._netConnection = netConnection;
            this._connectionParameters = new Dictionary<string, object>();
            this._connectionParameters.Add("objectEncoding", (double) this._netConnection.ObjectEncoding);
            this._connectionParameters.Add("capabilities", 15);
            this._connectionParameters.Add("audioCodecs", 1639.0);
            this._connectionParameters.Add("flashVer", this._netConnection.PlayerVersion);
            this._connectionParameters.Add("videoFunction", 1.0);
            this._connectionParameters.Add("fpad", false);
            this._connectionParameters.Add("videoCodecs", 252.0);
        }

        public void Call(string command, IPendingServiceCallback callback, params object[] arguments)
        {
            this._connection.Invoke(command, arguments, callback);
        }

        public void Call(string endpoint, string destination, string source, string operation, IPendingServiceCallback callback, params object[] arguments)
        {
            throw new NotSupportedException();
        }

        public void Close()
        {
            if (this.Connected)
            {
                this._connection.Close();
            }
            this._connection = null;
        }

        public void Connect(string command, params object[] arguments)
        {
            Uri uri = new Uri(command);
            this._connectArguments = arguments;
            this._connectionParameters["tcUrl"] = string.Concat(new object[] { "rtmp://", uri.Host, ':', uri.Port, '/', uri.Query });
            this._connectionParameters["app"] = uri.LocalPath.TrimStart(new char[] { '/' });
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(uri.Host, uri.Port);
            this._connection = new RtmpClientConnection(this, socket);
            this._connection.BeginHandshake();
        }

        public override void ConnectionClosed(RtmpConnection connection)
        {
            base.ConnectionClosed(connection);
            this._netConnection.RaiseDisconnect();
        }

        public override void ConnectionOpened(RtmpConnection connection)
        {
            try
            {
                RtmpChannel channel = connection.GetChannel(3);
                PendingCall serviceCall = new PendingCall("connect", this._connectArguments);
                FluorineFx.Messaging.Rtmp.Event.Invoke message = new FluorineFx.Messaging.Rtmp.Event.Invoke(serviceCall) {
                    ConnectionParameters = this._connectionParameters,
                    InvokeId = connection.InvokeId
                };
                serviceCall.RegisterCallback(this);
                connection.RegisterPendingCall(message.InvokeId, serviceCall);
                channel.Write(message);
            }
            catch (Exception exception)
            {
                this._netConnection.RaiseNetStatus(exception);
            }
        }

        public void Invoke(string method, IPendingServiceCallback callback)
        {
            this._connection.Invoke(method, callback);
        }

        public void Invoke(string method, object[] parameters, IPendingServiceCallback callback)
        {
            this._connection.Invoke(method, parameters, callback);
        }

        protected override void OnChunkSize(RtmpConnection connection, RtmpChannel channel, RtmpHeader source, ChunkSize chunkSize)
        {
        }

        protected override void OnClientBW(RtmpConnection connection, RtmpChannel channel, RtmpHeader source, ClientBW clientBW)
        {
            channel.Write(new ServerBW(clientBW.Bandwidth));
        }

        protected override void OnFlexInvoke(RtmpConnection connection, RtmpChannel channel, RtmpHeader header, FlexInvoke invoke)
        {
            this.OnInvoke(connection, channel, header, invoke);
        }

        protected override void OnInvoke(RtmpConnection connection, RtmpChannel channel, RtmpHeader header, Notify invoke)
        {
            IServiceCall serviceCall = invoke.ServiceCall;
            if ((serviceCall.ServiceMethodName == "_result") || (serviceCall.ServiceMethodName == "_error"))
            {
                if (serviceCall.ServiceMethodName == "_error")
                {
                    serviceCall.Status = 0x13;
                }
                if (serviceCall.ServiceMethodName == "_result")
                {
                    serviceCall.Status = 2;
                }
                base.HandlePendingCallResult(connection, invoke);
            }
            else if (serviceCall is IPendingServiceCall)
            {
                IPendingServiceCall call2 = serviceCall as IPendingServiceCall;
                MethodInfo methodInfo = MethodHandler.GetMethod(this._netConnection.Client.GetType(), serviceCall.ServiceMethodName, serviceCall.Arguments, false, false);
                if (methodInfo != null)
                {
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    object[] array = new object[parameters.Length];
                    serviceCall.Arguments.CopyTo(array, 0);
                    TypeHelper.NarrowValues(array, parameters);
                    try
                    {
                        object obj2 = new InvocationHandler(methodInfo).Invoke(this._netConnection.Client, array);
                        if (methodInfo.ReturnType == typeof(void))
                        {
                            serviceCall.Status = 4;
                        }
                        else
                        {
                            serviceCall.Status = (obj2 == null) ? ((byte) 3) : ((byte) 2);
                            call2.Result = obj2;
                        }
                    }
                    catch (Exception exception)
                    {
                        serviceCall.Exception = exception;
                        serviceCall.Status = 0x13;
                    }
                }
                else
                {
                    string message = __Res.GetString("Invocation_NoSuitableMethod", new object[] { serviceCall.ServiceMethodName });
                    serviceCall.Status = 0x11;
                    serviceCall.Exception = new FluorineException(message);
                }
                if ((serviceCall.Status == 4) || (serviceCall.Status == 3))
                {
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug("Method does not have return value, do not reply");
                    }
                }
                else
                {
                    FluorineFx.Messaging.Rtmp.Event.Invoke invoke2 = new FluorineFx.Messaging.Rtmp.Event.Invoke {
                        ServiceCall = serviceCall,
                        InvokeId = invoke.InvokeId
                    };
                    channel.Write(invoke2);
                }
            }
        }

        protected override void OnPing(RtmpConnection connection, RtmpChannel channel, RtmpHeader source, Ping ping)
        {
            if (ping.Value1 == 6)
            {
                Ping ping2 = new Ping {
                    Value1 = 7
                };
                int num = Environment.TickCount & ((int) 0xffffffffL);
                ping2.Value2 = num;
                ping2.Value3 = -1;
                connection.Ping(ping2);
            }
        }

        protected override void OnServerBW(RtmpConnection connection, RtmpChannel channel, RtmpHeader source, ServerBW serverBW)
        {
        }

        protected override void OnSharedObject(RtmpConnection connection, RtmpChannel channel, RtmpHeader header, SharedObjectMessage message)
        {
            this._netConnection.OnSharedObject(connection, channel, header, message);
        }

        public void ResultReceived(IPendingServiceCall call)
        {
            ASObject result = call.Result as ASObject;
            this._netConnection.RaiseNetStatus(result);
            if (!result.ContainsKey("level") || !(result["level"].ToString() == "error"))
            {
                this._netConnection.RaiseOnConnect();
            }
        }

        public void Write(IRtmpEvent message)
        {
            this._connection.GetChannel(3).Write(message);
        }

        public bool Connected
        {
            get
            {
                return ((this._connection != null) && this._connection.IsConnected);
            }
        }

        public IConnection Connection
        {
            get
            {
                return this._connection;
            }
        }
    }
}

