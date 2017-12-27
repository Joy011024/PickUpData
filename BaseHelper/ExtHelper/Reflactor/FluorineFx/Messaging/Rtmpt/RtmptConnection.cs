namespace FluorineFx.Messaging.Rtmpt
{
    using FluorineFx;
    using FluorineFx.Collections;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmp;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.Net;
    using System.Web;

    internal class RtmptConnection : RtmpConnection
    {
        protected ByteBuffer _buffer;
        protected long _noPendingMessages;
        protected LinkedList _notifyMessages;
        protected LinkedList _pendingMessages;
        protected byte _pollingDelay;
        protected long _readBytes;
        private IPEndPoint _remoteEndPoint;
        private RtmptServer _rtmptServer;
        protected long _writtenBytes;
        private static object[] EmptyList = new object[0];
        protected static long INCREASE_POLLING_DELAY_COUNT = 10L;
        protected static byte INITIAL_POLLING_DELAY = 0;
        private static readonly ILog log = LogManager.GetLogger(typeof(RtmptConnection));
        protected static byte MAX_POLLING_DELAY = 0x20;

        public RtmptConnection(RtmptServer rtmptServer, string path, Hashtable parameters) : base(path, parameters)
        {
            this._pollingDelay = INITIAL_POLLING_DELAY;
            this._pendingMessages = new LinkedList();
            this._notifyMessages = new LinkedList();
            this._rtmptServer = rtmptServer;
            IPAddress address = IPAddress.Parse(HttpContext.Current.Request.UserHostAddress);
            this._remoteEndPoint = new IPEndPoint(address, 80);
            this._buffer = ByteBuffer.Allocate(0x800);
            this._readBytes = 0L;
            this._writtenBytes = 0L;
        }

        public RtmptConnection(IPEndPoint ipEndPoint, RtmptServer rtmptServer, string path, Hashtable parameters) : base(path, parameters)
        {
            this._pollingDelay = INITIAL_POLLING_DELAY;
            this._pendingMessages = new LinkedList();
            this._notifyMessages = new LinkedList();
            this._rtmptServer = rtmptServer;
            this._remoteEndPoint = ipEndPoint;
            this._buffer = ByteBuffer.Allocate(0x800);
            this._readBytes = 0L;
            this._writtenBytes = 0L;
        }

        public override void Close()
        {
            lock (base.SyncRoot)
            {
                base.SetIsClosing(true);
            }
        }

        public IList Decode(ByteBuffer data)
        {
            if (base.State == RtmpState.Disconnected)
            {
                return EmptyList;
            }
            this._readBytes += data.Limit;
            this._buffer.Put(data);
            this._buffer.Flip();
            return RtmpProtocolDecoder.DecodeBuffer(base.Context, this._buffer);
        }

        public void DeferredClose()
        {
            lock (base.SyncRoot)
            {
                this._notifyMessages.Clear();
                this._pendingMessages.Clear();
                base.Close();
                this._rtmptServer.OnConnectionClose(this);
            }
        }

        public ByteBuffer GetPendingMessages(int targetSize)
        {
            if (this._pendingMessages.Count == 0)
            {
                this._noPendingMessages += 1L;
                if (this._noPendingMessages > INCREASE_POLLING_DELAY_COUNT)
                {
                    if (this._pollingDelay == 0)
                    {
                        this._pollingDelay = 1;
                    }
                    this._pollingDelay = (byte) (this._pollingDelay * 2);
                    if (this._pollingDelay > MAX_POLLING_DELAY)
                    {
                        this._pollingDelay = MAX_POLLING_DELAY;
                    }
                }
                return null;
            }
            ByteBuffer buffer = ByteBuffer.Allocate(0x800);
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("Rtmpt_ReturningMessages", new object[] { this._pendingMessages.Count }));
            }
            this._noPendingMessages = 0L;
            this._pollingDelay = INITIAL_POLLING_DELAY;
            while (buffer.Limit < targetSize)
            {
                object obj3;
                if (this._pendingMessages.Count == 0)
                {
                    break;
                }
                lock ((obj3 = this._pendingMessages.SyncRoot))
                {
                    foreach (ByteBuffer buffer2 in this._pendingMessages)
                    {
                        buffer.Put(buffer2);
                    }
                    this._pendingMessages.Clear();
                }
                LinkedList list = new LinkedList();
                lock ((obj3 = this._notifyMessages.SyncRoot))
                {
                    list.AddAll(this._notifyMessages);
                    this._notifyMessages.Clear();
                }
                foreach (object obj2 in list)
                {
                    try
                    {
                        this._rtmptServer.RtmpHandler.MessageSent(this, obj2);
                    }
                    catch (Exception exception)
                    {
                        log.Error(__Res.GetString("Rtmpt_NotifyError"), exception);
                    }
                }
            }
            buffer.Flip();
            this._writtenBytes += buffer.Limit;
            return buffer;
        }

        protected override void OnInactive()
        {
            this.Close();
            this.DeferredClose();
        }

        public override void Push(IMessage message, IMessageClient messageClient)
        {
            if (base.State != RtmpState.Disconnected)
            {
                BaseRtmpHandler.Push(this, message, messageClient);
            }
        }

        public void RawWrite(ByteBuffer packet)
        {
            lock (this._pendingMessages.SyncRoot)
            {
                this._pendingMessages.Add(packet);
            }
        }

        public override string ToString()
        {
            return ("RtmptConnection " + base._connectionId);
        }

        public override void Write(RtmpPacket packet)
        {
            if (base.State != RtmpState.Disconnected)
            {
                lock (base.SyncRoot)
                {
                    ByteBuffer buffer;
                    try
                    {
                        buffer = RtmpProtocolEncoder.Encode(base.Context, packet);
                    }
                    catch (Exception exception)
                    {
                        log.Error("Could not encode message " + packet, exception);
                        return;
                    }
                    this.WritingMessage(packet);
                    this.RawWrite(buffer);
                    lock (this._notifyMessages.SyncRoot)
                    {
                        this._notifyMessages.Add(packet);
                    }
                }
            }
        }

        public override int ClientLeaseTime
        {
            get
            {
                return Math.Max(this.Endpoint.GetMessageBroker().FlexClientSettings.TimeoutMinutes, 1);
            }
        }

        public IEndpoint Endpoint
        {
            get
            {
                return this._rtmptServer.Endpoint;
            }
        }

        public byte PollingDelay
        {
            get
            {
                if (base.State == RtmpState.Disconnected)
                {
                    return 0;
                }
                return (byte) (this._pollingDelay + 1);
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
                return this._remoteEndPoint;
            }
        }

        public override long WrittenBytes
        {
            get
            {
                return this._writtenBytes;
            }
        }
    }
}

