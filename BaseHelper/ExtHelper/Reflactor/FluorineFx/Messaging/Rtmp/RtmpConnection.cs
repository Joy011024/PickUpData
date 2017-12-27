namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Service;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Messaging.Rtmp.Service;
    using FluorineFx.Messaging.Rtmp.Stream;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    [CLSCompliant(false)]
    public abstract class RtmpConnection : BaseConnection, IServiceCapableConnection, IConnection, ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener, IMessageConnection
    {
        protected int _bytesReadInterval;
        private Dictionary<int, RtmpChannel> _channels;
        private long _clientBytesRead;
        private Dictionary<string, IMessageClient> _clients;
        private RtmpContext _context;
        protected Dictionary<DeferredResult, object> _deferredResults;
        private AtomicInteger _invokeId;
        protected long _lastBytesRead;
        protected int _lastPingSent;
        protected int _lastPingTime;
        protected int _lastPongReceived;
        protected int _nextBytesRead;
        protected Dictionary<int, IServiceCall> _pendingCalls;
        private static ILog log = LogManager.GetLogger(typeof(RtmpConnection));

        internal RtmpConnection(string path, IDictionary parameters) : base(path, parameters)
        {
            this._channels = new Dictionary<int, RtmpChannel>();
            this._clients = new Dictionary<string, IMessageClient>();
            this._pendingCalls = new Dictionary<int, IServiceCall>();
            this._deferredResults = new Dictionary<DeferredResult, object>();
            this._invokeId = new AtomicInteger(1);
            this._lastPingTime = -1;
            this._clientBytesRead = 0L;
            this._bytesReadInterval = 0x1e000;
            this._nextBytesRead = 0x1e000;
            this._lastBytesRead = 0L;
            this._context = new RtmpContext(RtmpMode.Server);
        }

        public override void Close()
        {
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("Rtmp_ConnectionClose", new object[] { base._connectionId }));
            }
            lock (base.SyncRoot)
            {
                if (!base.IsDisposed)
                {
                    lock (((ICollection) this._channels).SyncRoot)
                    {
                        this._channels.Clear();
                    }
                    base.Close();
                    this._context.State = RtmpState.Disconnected;
                }
            }
        }

        public void CloseChannel(int channelId)
        {
            lock (((ICollection) this._channels).SyncRoot)
            {
                this._channels[channelId] = null;
            }
        }

        public override bool Connect(IScope newScope, object[] parameters)
        {
            return base.Connect(newScope, parameters);
        }

        public OutputStream CreateOutputStream(int streamId)
        {
            int num = 4 + ((streamId - 1) * 5);
            RtmpChannel data = this.GetChannel(num++);
            RtmpChannel channel = this.GetChannel(num++);
            return new OutputStream(channel, this.GetChannel(num++), data);
        }

        public RtmpChannel GetChannel(int channelId)
        {
            lock (((ICollection) this._channels).SyncRoot)
            {
                if (!this.IsChannelUsed(channelId))
                {
                    this._channels[channelId] = new RtmpChannel(this, channelId);
                }
                return this._channels[channelId];
            }
        }

        public IPendingServiceCall GetPendingCall(int invokeId)
        {
            IPendingServiceCall call = null;
            lock (((ICollection) this._pendingCalls).SyncRoot)
            {
                if (this._pendingCalls.ContainsKey(invokeId))
                {
                    call = this._pendingCalls[invokeId] as IPendingServiceCall;
                }
                if (call != null)
                {
                    this._pendingCalls.Remove(invokeId);
                    return call;
                }
                log.Debug(string.Format("Could not find PendingServiceCall for InvokeId {0}", invokeId));
            }
            return call;
        }

        public int GetStreamIdForChannel(int channelId)
        {
            if (channelId < 4)
            {
                return 0;
            }
            return (((channelId - 4) / 5) + 1);
        }

        public void Invoke(IServiceCall serviceCall)
        {
            this.Invoke(serviceCall, 3);
        }

        public void Invoke(string method)
        {
            this.Invoke(method, null, null);
        }

        public void Invoke(IServiceCall serviceCall, byte channel)
        {
            FluorineFx.Messaging.Rtmp.Event.Invoke message = new FluorineFx.Messaging.Rtmp.Event.Invoke {
                ServiceCall = serviceCall,
                InvokeId = this.InvokeId
            };
            if (serviceCall is IPendingServiceCall)
            {
                lock (((ICollection) this._pendingCalls).SyncRoot)
                {
                    this._pendingCalls[message.InvokeId] = serviceCall;
                }
            }
            this.GetChannel(channel).Write(message);
        }

        public void Invoke(string method, IPendingServiceCallback callback)
        {
            this.Invoke(method, null, callback);
        }

        public void Invoke(string method, object[] parameters)
        {
            this.Invoke(method, parameters, null);
        }

        public void Invoke(string method, object[] parameters, IPendingServiceCallback callback)
        {
            IPendingServiceCall serviceCall = new PendingCall(method, parameters);
            if (callback != null)
            {
                serviceCall.RegisterCallback(callback);
            }
            this.Invoke(serviceCall);
        }

        public bool IsChannelUsed(int channelId)
        {
            lock (((ICollection) this._channels).SyncRoot)
            {
                return (this._channels.ContainsKey(channelId) && (this._channels[channelId] != null));
            }
        }

        public bool IsClientRegistered(string clientId)
        {
            lock (((ICollection) this._clients).SyncRoot)
            {
                return this._clients.ContainsKey(clientId);
            }
        }

        internal void MessageReceived()
        {
            base._readMessages += 1L;
            this.UpdateBytesRead();
        }

        internal virtual void MessageSent(RtmpPacket packet)
        {
            base._writtenMessages += 1L;
        }

        public void Notify(IServiceCall serviceCall)
        {
            this.Notify(serviceCall, 3);
        }

        public void Notify(string method)
        {
            this.Notify(method, null);
        }

        public void Notify(IServiceCall serviceCall, byte channel)
        {
            FluorineFx.Messaging.Rtmp.Event.Notify message = new FluorineFx.Messaging.Rtmp.Event.Notify {
                ServiceCall = serviceCall
            };
            this.GetChannel(channel).Write(message);
        }

        public void Notify(string method, object[] parameters)
        {
            IServiceCall serviceCall = new Call(method, parameters);
            this.Notify(serviceCall);
        }

        protected abstract void OnInactive();
        public override void Ping()
        {
            int tickCount = Environment.TickCount;
            if (this._lastPingSent == 0)
            {
                this._lastPongReceived = tickCount;
            }
            FluorineFx.Messaging.Rtmp.Event.Ping ping = new FluorineFx.Messaging.Rtmp.Event.Ping {
                Value1 = 6
            };
            this._lastPingSent = tickCount;
            int num2 = this._lastPingSent & ((int) 0xffffffffL);
            ping.Value2 = num2;
            ping.Value3 = -1;
            this.Ping(ping);
        }

        public void Ping(FluorineFx.Messaging.Rtmp.Event.Ping ping)
        {
            this.GetChannel(2).Write(ping);
        }

        internal void PingReceived(FluorineFx.Messaging.Rtmp.Event.Ping pong)
        {
            this._lastPongReceived = Environment.TickCount;
            int num = this._lastPongReceived & ((int) 0xffffffffL);
            this._lastPingTime = num - pong.Value2;
        }

        public abstract void Push(IMessage message, IMessageClient messageClient);
        public void ReceivedBytesRead(int bytes)
        {
            log.Info(string.Concat(new object[] { "Client received ", bytes, " bytes, written ", this.WrittenBytes, " bytes, ", base.PendingMessages, " messages pending" }));
            this._clientBytesRead = bytes;
        }

        internal void RegisterDeferredResult(DeferredResult result)
        {
            lock (((ICollection) this._deferredResults).SyncRoot)
            {
                this._deferredResults.Add(result, null);
            }
        }

        public void RegisterMessageClient(IMessageClient client)
        {
            lock (((ICollection) this._clients).SyncRoot)
            {
                if (!this._clients.ContainsKey(client.ClientId))
                {
                    this._clients.Add(client.ClientId, client);
                }
            }
        }

        internal void RegisterPendingCall(int invokeId, IPendingServiceCall call)
        {
            lock (((ICollection) this._pendingCalls).SyncRoot)
            {
                this._pendingCalls[invokeId] = call;
            }
        }

        public void RemoveMessageClient(string clientId)
        {
            lock (((ICollection) this._clients).SyncRoot)
            {
                if (this._clients.ContainsKey(clientId))
                {
                    this._clients.Remove(clientId);
                }
            }
        }

        public void RemoveMessageClients()
        {
            lock (((ICollection) this._clients).SyncRoot)
            {
                this._clients.Clear();
            }
        }

        internal void Setup(string host, string path, IDictionary parameters)
        {
            base._path = path;
            base._parameters = parameters;
            if (base._parameters.Contains("objectEncoding"))
            {
                int num = Convert.ToInt32(base._parameters["objectEncoding"]);
                base._objectEncoding = (ObjectEncoding) num;
            }
        }

        internal virtual void StartRoundTripMeasurement()
        {
        }

        internal virtual void StartWaitForHandshake()
        {
        }

        public override void Timeout()
        {
            lock (base.SyncRoot)
            {
                if (!base.IsDisposed)
                {
                    StatusASO saso;
                    if (base.IsFlexClient)
                    {
                        FlexInvoke message = new FlexInvoke {
                            Cmd = "onstatus"
                        };
                        saso = new StatusASO("NetConnection.Connect.Closed", "status", "Connection Timed Out", null, base.ObjectEncoding);
                        message.Parameters = new object[] { saso };
                        this.GetChannel(3).Write(message);
                    }
                    else
                    {
                        saso = new StatusASO("NetConnection.Connect.Closed", "error", "Connection Timed Out", null, base.ObjectEncoding);
                        this.GetChannel(3).SendStatus(saso);
                    }
                }
            }
        }

        public override string ToString()
        {
            return ("RtmpConnection " + base._connectionId);
        }

        internal void UnregisterDeferredResult(DeferredResult result)
        {
            lock (((ICollection) this._deferredResults).SyncRoot)
            {
                this._deferredResults.Remove(result);
            }
        }

        protected void UpdateBytesRead()
        {
            long readBytes = this.ReadBytes;
            if (readBytes >= this._nextBytesRead)
            {
                BytesRead message = new BytesRead((int) readBytes);
                this.GetChannel(2).Write(message);
                this._nextBytesRead += this._bytesReadInterval;
            }
        }

        public abstract void Write(RtmpPacket packet);
        protected virtual void WritingMessage(RtmpPacket packet)
        {
        }

        public override long ClientBytesRead
        {
            get
            {
                return this._clientBytesRead;
            }
        }

        public int ClientCount
        {
            get
            {
                lock (((ICollection) this._clients).SyncRoot)
                {
                    return this._clients.Count;
                }
            }
        }

        public RtmpContext Context
        {
            get
            {
                return this._context;
            }
        }

        public int InvokeId
        {
            get
            {
                return this._invokeId.Increment();
            }
        }

        public override int LastPingTime
        {
            get
            {
                return this._lastPingTime;
            }
        }

        public override long ReadBytes
        {
            get
            {
                return 0L;
            }
        }

        public RtmpState State
        {
            get
            {
                return this._context.State;
            }
        }

        public override long WrittenBytes
        {
            get
            {
                return 0L;
            }
        }
    }
}

