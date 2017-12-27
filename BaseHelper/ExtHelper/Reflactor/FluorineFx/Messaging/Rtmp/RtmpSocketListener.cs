namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Threading;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Net;
    using System.Net.Sockets;

    internal sealed class RtmpSocketListener : DisposableBase
    {
        private int _acceptCount;
        private IPEndPoint _endPoint;
        private FluorineFx.Messaging.Rtmp.RtmpServer _rtmpServer;
        private System.Net.Sockets.Socket _socket;
        private static readonly ILog log = LogManager.GetLogger(typeof(RtmpSocketListener));

        public RtmpSocketListener(FluorineFx.Messaging.Rtmp.RtmpServer rtmpServer, IPEndPoint endPoint, int acceptCount)
        {
            this._rtmpServer = rtmpServer;
            this._endPoint = endPoint;
            this._acceptCount = acceptCount;
        }

        internal void BeginAcceptCallback(IAsyncResult ar)
        {
            if (!base.IsDisposed)
            {
                RtmpSocketListener asyncState = ar.AsyncState as RtmpSocketListener;
                try
                {
                    System.Net.Sockets.Socket socket = asyncState.Socket.EndAccept(ar);
                    socket.NoDelay = FluorineConfiguration.Instance.FluorineSettings.RtmpServer.RtmpTransportSettings.TcpNoDelay;
                    socket.ReceiveBufferSize = FluorineConfiguration.Instance.FluorineSettings.RtmpServer.RtmpTransportSettings.ReceiveBufferSize;
                    socket.SendBufferSize = FluorineConfiguration.Instance.FluorineSettings.RtmpServer.RtmpTransportSettings.SendBufferSize;
                    asyncState.RtmpServer.InitializeConnection(socket);
                    asyncState.Socket.BeginAccept(new AsyncCallback(this.BeginAcceptCallback), asyncState);
                }
                catch (Exception exception)
                {
                    if (this.HandleError(exception))
                    {
                        asyncState.RtmpServer.RaiseOnError(exception);
                    }
                }
            }
        }

        protected override void Free()
        {
            try
            {
                this._socket.Close();
            }
            catch (Exception exception)
            {
                log.Error(exception.Message, exception);
            }
        }

        private bool HandleError(Exception exception)
        {
            SocketException innerException = exception as SocketException;
            if ((exception.InnerException != null) && (exception.InnerException is SocketException))
            {
                innerException = exception.InnerException as SocketException;
            }
            bool flag = true;
            if ((innerException != null) && (innerException.ErrorCode == 0x3e3))
            {
                flag = false;
            }
            if (flag && log.get_IsErrorEnabled())
            {
                log.Error(__Res.GetString("SocketServer_ListenerFail"), exception);
            }
            return flag;
        }

        public void Start()
        {
            try
            {
                this._socket = new System.Net.Sockets.Socket(this._endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this._socket.Bind(this._endPoint);
                this._socket.Listen(0x7fffffff);
                int loopIndex = 0;
                for (int i = 0; i < this._acceptCount; i++)
                {
                    this._socket.BeginAccept(new AsyncCallback(this.BeginAcceptCallback), this);
                    ThreadPoolEx.LoopSleep(ref loopIndex);
                }
            }
            catch (Exception exception)
            {
                this.HandleError(exception);
            }
        }

        public void Stop()
        {
        }

        internal FluorineFx.Messaging.Rtmp.RtmpServer RtmpServer
        {
            get
            {
                return this._rtmpServer;
            }
        }

        internal System.Net.Sockets.Socket Socket
        {
            get
            {
                return this._socket;
            }
        }
    }
}

