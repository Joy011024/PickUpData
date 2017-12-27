namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Context;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Stream;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Messaging.Rtmp.Stream;
    using FluorineFx.Messaging.Rtmpt;
    using FluorineFx.Scheduling;
    using FluorineFx.Threading;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class RtmpServerConnection : RtmpConnection, IStreamCapableConnection, IConnection, ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener, IBWControllable
    {
        private IConnectionBWConfig _bwConfig;
        private IBWControlContext _bwContext;
        private IEndpoint _endpoint;
        protected string _keepAliveJobName;
        private DateTime _lastAction;
        private Dictionary<int, AtomicInteger> _pendingVideos;
        private ByteBuffer _readBuffer;
        private long _readBytes;
        private RtmpNetworkStream _rtmpNetworkStream;
        private RtmpServer _rtmpServer;
        private RtmptRequest _rtmptRequest;
        private volatile RtmpConnectionState _state;
        protected Dictionary<int, int> _streamBuffers;
        private int _streamCount;
        private Dictionary<int, IClientStream> _streams;
        private long _writtenBytes;
        private static ILog log = LogManager.GetLogger(typeof(RtmpServerConnection));
        private static int StreamId = 0;

        public RtmpServerConnection(RtmpServer rtmpServer, Socket socket) : base(null, null)
        {
            this._streams = new Dictionary<int, IClientStream>();
            this._streamBuffers = new Dictionary<int, int>();
            this._pendingVideos = new Dictionary<int, AtomicInteger>();
            this._endpoint = rtmpServer.Endpoint;
            this._readBuffer = ByteBuffer.Allocate(0x1000);
            this._readBuffer.Flip();
            this._rtmpServer = rtmpServer;
            this._rtmpNetworkStream = new RtmpNetworkStream(socket);
            this._state = 1;
            this.SetIsTunneled(false);
            this.IsTunnelingDetected = false;
        }

        internal void BeginDisconnect()
        {
            if (!base.IsDisposed && !this.IsDisconnecting)
            {
                try
                {
                    this._state = 2;
                    ThreadPoolEx.Global.QueueUserWorkItem(new WaitCallback(this.OnDisconnectCallback), null);
                }
                catch (Exception exception)
                {
                    if (log.get_IsErrorEnabled())
                    {
                        log.Error("BeginDisconnect " + this.ToString(), exception);
                    }
                }
            }
        }

        private void BeginReadCallbackProcessing(IAsyncResult ar)
        {
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("Rtmp_SocketBeginRead", new object[] { base._connectionId }));
            }
            byte[] asyncState = ar.AsyncState as byte[];
            if (!base.IsDisposed && this.IsActive)
            {
                try
                {
                    this._lastAction = DateTime.Now;
                    int length = this._rtmpNetworkStream.EndRead(ar);
                    this._readBytes += length;
                    if (length > 0)
                    {
                        this._readBuffer.Append(asyncState, 0, length);
                        ThreadPoolEx.Global.QueueUserWorkItem(new WaitCallback(this.OnReceivedCallback), null);
                    }
                    else
                    {
                        this.Close();
                    }
                }
                catch (Exception exception)
                {
                    this.HandleError(exception);
                }
                finally
                {
                    SocketBufferPool.Pool.CheckIn(asyncState);
                }
            }
            else
            {
                SocketBufferPool.Pool.CheckIn(asyncState);
            }
        }

        public void BeginReceive(bool IOCPThread)
        {
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("Rtmp_SocketBeginReceive", new object[] { base._connectionId, IOCPThread }));
            }
            if (!IOCPThread)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.BeginReceiveCallbackProcessing), null);
            }
            else
            {
                this.BeginReceiveCallbackProcessing(null);
            }
        }

        public void BeginReceiveCallbackProcessing(object state)
        {
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("Rtmp_SocketReceiveProcessing", new object[] { base._connectionId }));
            }
            if (!base.IsDisposed && this.IsActive)
            {
                byte[] buffer = null;
                try
                {
                    buffer = SocketBufferPool.Pool.CheckOut();
                    this._rtmpNetworkStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(this.BeginReadCallbackProcessing), buffer);
                }
                catch (Exception exception)
                {
                    SocketBufferPool.Pool.CheckIn(buffer);
                    this.HandleError(exception);
                }
            }
        }

        public override void Close()
        {
            lock (base.SyncRoot)
            {
                if (!base.IsDisposed && !this.IsDisconnected)
                {
                    if (!this.IsTunneled)
                    {
                        this.DeferredClose();
                    }
                    else
                    {
                        base.SetIsClosing(true);
                    }
                }
            }
        }

        public override bool Connect(IScope newScope, object[] parameters)
        {
            bool flag = base.Connect(newScope, parameters);
            if (flag && ((base.Scope != null) && (base.Scope.Context != null)))
            {
                this._bwContext = (base.Scope.GetService(typeof(IBWControlService)) as IBWControlService).RegisterBWControllable(this);
            }
            return flag;
        }

        protected string CreateStreamName()
        {
            return Guid.NewGuid().ToString();
        }

        public void DeferredClose()
        {
            lock (base.SyncRoot)
            {
                if (this._keepAliveJobName != null)
                {
                    (base.Scope.GetService(typeof(ISchedulingService)) as ISchedulingService).RemoveScheduledJob(this._keepAliveJobName);
                    this._keepAliveJobName = null;
                }
                if (!base.IsDisposed && !this.IsDisconnected)
                {
                    this._state = 0;
                    IStreamService scopeService = ScopeUtils.GetScopeService(base.Scope, typeof(IStreamService)) as IStreamService;
                    if (scopeService != null)
                    {
                        lock (((ICollection) this._streams).SyncRoot)
                        {
                            IClientStream[] array = new IClientStream[this._streams.Count];
                            this._streams.Values.CopyTo(array, 0);
                            foreach (IClientStream stream in array)
                            {
                                if (stream != null)
                                {
                                    if (log.get_IsDebugEnabled())
                                    {
                                        log.Debug("Closing stream: " + stream.StreamId);
                                    }
                                    scopeService.deleteStream(this, stream.StreamId);
                                    this._streamCount--;
                                }
                            }
                            this._streams.Clear();
                        }
                    }
                    if (((this._bwContext != null) && (base.Scope != null)) && (base.Scope.Context != null))
                    {
                        (base.Scope.GetService(typeof(IBWControlService)) as IBWControlService).UnregisterBWControllable(this._bwContext);
                        this._bwContext = null;
                    }
                    base.Close();
                    this._rtmpServer.OnConnectionClose(this);
                    this._rtmpNetworkStream.Close();
                }
            }
        }

        public void DeleteStreamById(int streamId)
        {
            if (streamId > 0)
            {
                lock (((ICollection) this._streams).SyncRoot)
                {
                    if (this._streams.ContainsKey(streamId - 1))
                    {
                        object obj3;
                        lock ((obj3 = ((ICollection) this._pendingVideos).SyncRoot))
                        {
                            if (this._pendingVideos.ContainsKey(streamId))
                            {
                                this._pendingVideos.Remove(streamId);
                            }
                        }
                        this._streamCount--;
                        if (this._pendingVideos.ContainsKey(streamId - 1))
                        {
                            this._streams.Remove(streamId - 1);
                        }
                        lock ((obj3 = ((ICollection) this._streamBuffers).SyncRoot))
                        {
                            if (this._streamBuffers.ContainsKey(streamId - 1))
                            {
                                this._streamBuffers.Remove(streamId - 1);
                            }
                        }
                    }
                }
            }
        }

        public IBWControllable GetParentBWControllable()
        {
            return null;
        }

        public override long GetPendingVideoMessages(int streamId)
        {
            AtomicInteger integer = null;
            lock (((ICollection) this._pendingVideos).SyncRoot)
            {
                if (this._pendingVideos.ContainsKey(streamId))
                {
                    integer = this._pendingVideos[streamId];
                }
            }
            long num = (integer != null) ? ((long) (integer.Value - this.StreamCount)) : ((long) 0);
            return ((num > 0L) ? num : 0L);
        }

        public IClientStream GetStreamByChannelId(int channelId)
        {
            if (channelId >= 4)
            {
                lock (((ICollection) this._streams).SyncRoot)
                {
                    int streamIdForChannel = base.GetStreamIdForChannel(channelId);
                    if (this._streams.ContainsKey(streamIdForChannel - 1))
                    {
                        return this._streams[streamIdForChannel - 1];
                    }
                }
            }
            return null;
        }

        public IClientStream GetStreamById(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            lock (((ICollection) this._streams).SyncRoot)
            {
                if (this._streams.ContainsKey(id - 1))
                {
                    return this._streams[id - 1];
                }
                return null;
            }
        }

        public ICollection GetStreams()
        {
            lock (((ICollection) this._streams).SyncRoot)
            {
                return this._streams.Values;
            }
        }

        private void HandleError(Exception exception)
        {
            SocketException innerException = exception as SocketException;
            if ((exception.InnerException != null) && (exception.InnerException is SocketException))
            {
                innerException = exception.InnerException as SocketException;
            }
            bool flag = true;
            if ((innerException != null) && (innerException.ErrorCode == 0x2746))
            {
                if (log.get_IsDebugEnabled())
                {
                    log.Debug(__Res.GetString("Rtmp_SocketConnectionReset", new object[] { base._connectionId }));
                }
                flag = false;
            }
            if (flag && log.get_IsErrorEnabled())
            {
                log.Error("Error " + this.ToString(), exception);
            }
            this.BeginDisconnect();
        }

        private void HandleRtmpt()
        {
            if (this._rtmptRequest == null)
            {
                string str5;
                BufferStreamReader reader = new BufferStreamReader(this._readBuffer);
                string[] strArray = reader.ReadLine().Split(new char[] { ' ' });
                string httpMethod = strArray[0];
                string pattern = strArray[1];
                int startIndex = 0;
                while ((startIndex = pattern.IndexOf("%", startIndex)) != -1)
                {
                    pattern = pattern.Substring(0, startIndex) + Uri.HexUnescape(pattern, ref startIndex) + pattern.Substring(startIndex);
                }
                string protocol = strArray[2];
                Hashtable headers = new Hashtable();
                string str6 = null;
                while (((str5 = reader.ReadLine()) != null) && (str5 != string.Empty))
                {
                    if ((str6 != null) && char.IsWhiteSpace(str5[0]))
                    {
                        Hashtable hashtable2;
                        object obj2;
                        (hashtable2 = headers)[obj2 = str6] = hashtable2[obj2] + str5;
                    }
                    else
                    {
                        int index = str5.IndexOf(":");
                        if (index != -1)
                        {
                            str6 = str5.Substring(0, index);
                            string str7 = str5.Substring(index + 1).Trim();
                            headers[str6] = str7;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                this._rtmptRequest = new RtmptRequest(this, pattern, protocol, httpMethod, headers);
            }
            if (this._readBuffer.Remaining == this._rtmptRequest.ContentLength)
            {
                RtmptEndpoint endpoint = this.Endpoint.GetMessageBroker().GetEndpoint("__@fluorinertmpt") as RtmptEndpoint;
                if (endpoint != null)
                {
                    this._readBuffer.Compact();
                    this._rtmptRequest.Data = this._readBuffer;
                    this._readBuffer = ByteBuffer.Allocate(0x1000);
                    this._readBuffer.Flip();
                    endpoint.Service(this._rtmptRequest);
                    this._rtmptRequest = null;
                }
            }
        }

        internal override void MessageSent(RtmpPacket packet)
        {
            if (packet.Message is VideoData)
            {
                int streamId = packet.Header.StreamId;
                lock (((ICollection) this._pendingVideos).SyncRoot)
                {
                    if (this._pendingVideos.ContainsKey(streamId))
                    {
                        this._pendingVideos[streamId].Decrement();
                    }
                }
            }
            base.MessageSent(packet);
        }

        public IClientBroadcastStream NewBroadcastStream(int streamId)
        {
            lock (base.SyncRoot)
            {
                if (streamId < StreamId)
                {
                    return null;
                }
                ClientBroadcastStream stream = new ClientBroadcastStream();
                lock (((ICollection) this._streamBuffers).SyncRoot)
                {
                    if (this._streamBuffers.ContainsKey(streamId - 1))
                    {
                        int bufferTime = this._streamBuffers[streamId - 1];
                        stream.SetClientBufferDuration(bufferTime);
                    }
                }
                stream.StreamId = streamId;
                stream.Connection = this;
                stream.Name = this.CreateStreamName();
                stream.Scope = base.Scope;
                this.RegisterStream(stream);
                this._streamCount++;
                return stream;
            }
        }

        public IPlaylistSubscriberStream NewPlaylistSubscriberStream(int streamId)
        {
            lock (base.SyncRoot)
            {
                if (streamId < StreamId)
                {
                    return null;
                }
                PlaylistSubscriberStream stream = new PlaylistSubscriberStream();
                lock (((ICollection) this._streamBuffers).SyncRoot)
                {
                    if (this._streamBuffers.ContainsKey(streamId - 1))
                    {
                        int bufferTime = this._streamBuffers[streamId - 1];
                        stream.SetClientBufferDuration(bufferTime);
                    }
                }
                stream.Name = this.CreateStreamName();
                stream.Connection = this;
                stream.Scope = base.Scope;
                stream.StreamId = streamId;
                this.RegisterStream(stream);
                this._streamCount++;
                return stream;
            }
        }

        public ISingleItemSubscriberStream NewSingleItemSubscriberStream(int streamId)
        {
            return null;
        }

        private void OnDisconnectCallback(object state)
        {
            object obj2;
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("Rtmp_SocketDisconnectProcessing", new object[] { base._connectionId }));
            }
            Monitor.Enter(obj2 = base.SyncRoot);
            try
            {
                this._rtmpServer.RtmpHandler.ConnectionClosed(this);
            }
            catch (Exception exception)
            {
                if (log.get_IsErrorEnabled())
                {
                    log.Error("OnDisconnectCallback " + this.ToString(), exception);
                }
            }
            finally
            {
                Monitor.Exit(obj2);
            }
        }

        protected override void OnInactive()
        {
            if (!this.IsTunneled)
            {
                this.Timeout();
                this.Close();
                this.DeferredClose();
            }
        }

        private void OnReceivedCallback(object state)
        {
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("Rtmp_SocketReadProcessing", new object[] { base._connectionId }));
            }
            if (log.get_IsDebugEnabled())
            {
                log.Debug("Begin handling packet " + this.ToString());
            }
            if (!this.IsTunnelingDetected)
            {
                this.IsTunnelingDetected = true;
                byte num = this._readBuffer.Get(0);
                this.SetIsTunneled(num != 3);
            }
            try
            {
                if (!this.IsTunneled)
                {
                    List<object> list = RtmpProtocolDecoder.DecodeBuffer(base.Context, this._readBuffer);
                    if ((list != null) && (list.Count > 0))
                    {
                        foreach (object obj2 in list)
                        {
                            if (obj2 is ByteBuffer)
                            {
                                ByteBuffer buf = obj2 as ByteBuffer;
                                this.Send(buf);
                            }
                            else
                            {
                                FluorineRtmpContext.Initialize(this);
                                this._rtmpServer.RtmpHandler.MessageReceived(this, obj2);
                            }
                        }
                    }
                }
                else
                {
                    this.HandleRtmpt();
                }
            }
            catch (Exception exception)
            {
                this.HandleError(exception);
            }
            if (log.get_IsDebugEnabled())
            {
                log.Debug("End handling packet " + this.ToString());
            }
            this.BeginReceive(false);
        }

        public override void Push(IMessage message, IMessageClient messageClient)
        {
            if (this.IsActive)
            {
                BaseRtmpHandler.Push(this, message, messageClient);
            }
        }

        protected void RegisterStream(IClientStream stream)
        {
            lock (((ICollection) this._streams).SyncRoot)
            {
                this._streams[stream.StreamId - 1] = stream;
            }
        }

        internal void RememberStreamBufferDuration(int streamId, int bufferDuration)
        {
            lock (((ICollection) this._streamBuffers).SyncRoot)
            {
                this._streamBuffers.Add(streamId - 1, bufferDuration);
            }
        }

        public int ReserveStreamId()
        {
            return Interlocked.Increment(ref StreamId);
        }

        internal void Send(ByteBuffer buf)
        {
            lock (base.SyncRoot)
            {
                if (!base.IsDisposed && this.IsActive)
                {
                    byte[] buffer = buf.ToArray();
                    try
                    {
                        this._rtmpNetworkStream.Write(buffer, 0, buffer.Length);
                        this._writtenBytes += buffer.Length;
                    }
                    catch (Exception exception)
                    {
                        this.HandleError(exception);
                    }
                    this._lastAction = DateTime.Now;
                }
            }
        }

        internal void SetIsTunneled(bool value)
        {
            base.__fields = value ? ((byte) (base.__fields | 0x20)) : ((byte) (base.__fields & -33));
        }

        internal override void StartRoundTripMeasurement()
        {
            if (FluorineConfiguration.Instance.FluorineSettings.RtmpServer.RtmpConnectionSettings.PingInterval > 0)
            {
                if (this._keepAliveJobName == null)
                {
                    this._keepAliveJobName = (base.Scope.GetService(typeof(ISchedulingService)) as ISchedulingService).AddScheduledJob(FluorineConfiguration.Instance.FluorineSettings.RtmpServer.RtmpConnectionSettings.PingInterval, new KeepAliveJob(this));
                }
                log.Debug("Keep alive job name " + this._keepAliveJobName);
            }
        }

        internal override void StartWaitForHandshake()
        {
            if (FluorineConfiguration.Instance.FluorineSettings.RtmpServer.RtmpConnectionSettings.MaxHandshakeTimeout > 0)
            {
            }
        }

        public void UnreserveStreamId(int streamId)
        {
            this.DeleteStreamById(streamId);
        }

        public override void Write(RtmpPacket packet)
        {
            if (!base.IsDisposed && this.IsActive)
            {
                if (log.get_IsDebugEnabled())
                {
                    log.Debug("Write " + packet.Header);
                }
                if (!this.IsTunneled)
                {
                    this.WritingMessage(packet);
                    ByteBuffer buf = RtmpProtocolEncoder.Encode(base.Context, packet);
                    this.Send(buf);
                    this._rtmpServer.RtmpHandler.MessageSent(this, packet);
                }
                else
                {
                    Debug.Assert(false);
                }
            }
        }

        protected override void WritingMessage(RtmpPacket packet)
        {
            base.WritingMessage(packet);
            if (packet.Message is VideoData)
            {
                int streamId = packet.Header.StreamId;
                AtomicInteger integer = new AtomicInteger();
                AtomicInteger integer2 = null;
                lock (((ICollection) this._pendingVideos).SyncRoot)
                {
                    if (!this._pendingVideos.ContainsKey(streamId))
                    {
                        this._pendingVideos.Add(streamId, integer);
                        integer2 = integer;
                    }
                    else
                    {
                        integer2 = this._pendingVideos[streamId];
                    }
                }
                if (integer2 == null)
                {
                    integer2 = integer;
                }
                integer2.Increment();
            }
        }

        public IBandwidthConfigure BandwidthConfiguration
        {
            get
            {
                return this._bwConfig;
            }
            set
            {
                if (value is IConnectionBWConfig)
                {
                    this._bwConfig = value as IConnectionBWConfig;
                    if (this._bwConfig.DownstreamBandwidth > 0L)
                    {
                        ServerBW message = new ServerBW(((int) this._bwConfig.DownstreamBandwidth) / 8);
                        base.GetChannel(2).Write(message);
                    }
                    if (this._bwConfig.UpstreamBandwidth > 0L)
                    {
                        ClientBW tbw = new ClientBW(((int) this._bwConfig.UpstreamBandwidth) / 8, 0);
                        base.GetChannel(2).Write(tbw);
                        base._bytesReadInterval = ((int) this._bwConfig.UpstreamBandwidth) / 8;
                        base._nextBytesRead = (int) this.WrittenBytes;
                    }
                    if (this._bwContext != null)
                    {
                        (base.Scope.GetService(typeof(IBWControlService)) as IBWControlService).UpdateBWConfigure(this._bwContext);
                    }
                }
            }
        }

        public override int ClientLeaseTime
        {
            get
            {
                return this.Endpoint.GetMessageBroker().FlexClientSettings.TimeoutMinutes;
            }
        }

        public IEndpoint Endpoint
        {
            get
            {
                return this._endpoint;
            }
        }

        public bool IsActive
        {
            get
            {
                return (this._state == 1);
            }
        }

        public bool IsDisconnected
        {
            get
            {
                return (this._state == 0);
            }
        }

        public bool IsDisconnecting
        {
            get
            {
                return (this._state == 2);
            }
        }

        public bool IsTunneled
        {
            get
            {
                return ((base.__fields & 0x20) == 0x20);
            }
        }

        internal bool IsTunnelingDetected
        {
            get
            {
                return ((base.__fields & 0x10) == 0x10);
            }
            set
            {
                base.__fields = value ? ((byte) (base.__fields | 0x10)) : ((byte) (base.__fields & -17));
            }
        }

        public DateTime LastAction
        {
            get
            {
                return this._lastAction;
            }
            set
            {
                this._lastAction = value;
            }
        }

        public override long ReadBytes
        {
            get
            {
                return this._readBytes;
            }
        }

        public override IPEndPoint RemoteEndPoint
        {
            get
            {
                return (this._rtmpNetworkStream.Socket.RemoteEndPoint as IPEndPoint);
            }
        }

        protected int StreamCount
        {
            get
            {
                return this._streamCount;
            }
        }

        public override long WrittenBytes
        {
            get
            {
                return this._writtenBytes;
            }
        }

        private class KeepAliveJob : ScheduledJobBase
        {
            private RtmpServerConnection _connection;

            public KeepAliveJob(RtmpServerConnection connection)
            {
                this._connection = connection;
            }

            public override void Execute(ScheduledJobContext context)
            {
                if (this._connection.IsConnected)
                {
                    long readBytes = this._connection.ReadBytes;
                    if (readBytes > this._connection._lastBytesRead)
                    {
                        this._connection._lastBytesRead = readBytes;
                    }
                    else
                    {
                        FluorineRtmpContext.Initialize(this._connection);
                        if ((this._connection._lastPongReceived > 0) && ((this._connection._lastPingSent - this._connection._lastPongReceived) > FluorineConfiguration.Instance.FluorineSettings.RtmpServer.RtmpConnectionSettings.MaxInactivity))
                        {
                            RtmpServerConnection.log.Debug("Keep alive job name " + this._connection._keepAliveJobName);
                            (this._connection.Scope.GetService(typeof(ISchedulingService)) as ISchedulingService).RemoveScheduledJob(this._connection._keepAliveJobName);
                            this._connection._keepAliveJobName = null;
                            RtmpServerConnection.log.Warn(string.Format("Closing {0} due to too much inactivity ({0}).", this._connection, this._connection._lastPingSent - this._connection._lastPongReceived));
                            this._connection.OnInactive();
                        }
                        else
                        {
                            this._connection.Ping();
                        }
                    }
                }
            }
        }
    }
}

