namespace FluorineFx.Net
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Context;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmp;
    using FluorineFx.Threading;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class RtmpClientConnection : RtmpConnection
    {
        private IRtmpHandler _handler;
        private DateTime _lastAction;
        private ByteBuffer _readBuffer;
        private long _readBytes;
        private RtmpNetworkStream _rtmpNetworkStream;
        private volatile RtmpConnectionState _state;
        private long _writtenBytes;
        private static ILog log = LogManager.GetLogger(typeof(RtmpClientConnection));

        public RtmpClientConnection(IRtmpHandler handler, Socket socket) : base(null, null)
        {
            socket.ReceiveBufferSize = FluorineConfiguration.Instance.FluorineSettings.RtmpServer.RtmpTransportSettings.ReceiveBufferSize;
            socket.SendBufferSize = FluorineConfiguration.Instance.FluorineSettings.RtmpServer.RtmpTransportSettings.SendBufferSize;
            this._handler = handler;
            this._readBuffer = ByteBuffer.Allocate(0x1000);
            this._readBuffer.Flip();
            this._rtmpNetworkStream = new RtmpNetworkStream(socket);
            this._state = 1;
            base.Context.SetMode(RtmpMode.Client);
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

        public void BeginHandshake()
        {
            ByteBuffer buf = ByteBuffer.Allocate(0x601);
            buf.Put((byte) 3);
            int tickCount = Environment.TickCount;
            buf.Put(1, (byte) ((tickCount >> 0x18) & 0xff));
            buf.Put(2, (byte) ((tickCount >> 0x10) & 0xff));
            buf.Put(3, (byte) ((tickCount >> 8) & 0xff));
            buf.Put(4, (byte) (tickCount & 0xff));
            tickCount = tickCount % 0x100;
            for (int i = 8; i < 0x600; i += 2)
            {
                tickCount = ((0xb8cd75 * tickCount) + 1) % 0x100;
                buf.Put(i + 1, (byte) (tickCount & 0xff));
            }
            this.Send(buf);
            this.BeginReceive(false);
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
                if (!(base.IsDisposed || this.IsDisconnected))
                {
                    this._state = 0;
                    base.Close();
                    this._handler.ConnectionClosed(this);
                    this._rtmpNetworkStream.Close();
                }
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
                this.Close();
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
            try
            {
                List<object> list = RtmpProtocolDecoder.DecodeBuffer(base.Context, this._readBuffer);
                if (base.Context.State == RtmpState.Handshake)
                {
                    ByteBuffer src = list[0] as ByteBuffer;
                    src.Skip(1);
                    src.Compact();
                    src.Limit = 0x600;
                    ByteBuffer buf = ByteBuffer.Allocate(0x600);
                    buf.Put(src);
                    this.Send(buf);
                    base.Context.State = RtmpState.Connected;
                    this._handler.ConnectionOpened(this);
                }
                else if ((list != null) && (list.Count > 0))
                {
                    foreach (object obj2 in list)
                    {
                        if (obj2 is ByteBuffer)
                        {
                            ByteBuffer buffer3 = obj2 as ByteBuffer;
                            this.Send(buffer3);
                        }
                        else
                        {
                            FluorineRtmpContext.Initialize(this);
                            this._handler.MessageReceived(this, obj2);
                        }
                    }
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

        public override void Write(RtmpPacket packet)
        {
            if (!base.IsDisposed && this.IsActive)
            {
                if (log.get_IsDebugEnabled())
                {
                    log.Debug("Write " + packet.Header);
                }
                this.WritingMessage(packet);
                ByteBuffer buf = RtmpProtocolEncoder.Encode(base.Context, packet);
                this.Send(buf);
                this._handler.MessageSent(this, packet);
            }
        }

        public bool IsActive
        {
            get
            {
                return (this._state == 1);
            }
        }

        public override bool IsConnected
        {
            get
            {
                return (base.Context.State == RtmpState.Connected);
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

        public override long WrittenBytes
        {
            get
            {
                return this._writtenBytes;
            }
        }
    }
}

