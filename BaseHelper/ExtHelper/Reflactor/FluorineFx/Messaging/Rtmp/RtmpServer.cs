namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    internal sealed class RtmpServer : DisposableBase
    {
        private FluorineFx.Util.BufferPool _bufferPool;
        private Hashtable _connections = new Hashtable();
        private IEndpoint _endpoint;
        private FluorineFx.Messaging.Rtmp.RtmpHandler _rtmpHandler;
        private ArrayList _socketListeners = new ArrayList();
        private static readonly ILog log = LogManager.GetLogger(typeof(RtmpServer));

        private event ErrorHandler _onErrorEvent;

        public event ErrorHandler OnError
        {
            add
            {
                this._onErrorEvent += value;
            }
            remove
            {
                this._onErrorEvent -= value;
            }
        }

        public RtmpServer(RtmpEndpoint endpoint)
        {
            this._endpoint = endpoint;
            this._rtmpHandler = new FluorineFx.Messaging.Rtmp.RtmpHandler(endpoint);
        }

        internal void AddConnection(RtmpServerConnection connection)
        {
            if (!base.IsDisposed)
            {
                lock (this._connections)
                {
                    this._connections[connection] = connection;
                }
            }
        }

        public void AddListener(IPEndPoint localEndPoint)
        {
            this.AddListener(localEndPoint, 1);
        }

        public void AddListener(IPEndPoint localEndPoint, int acceptCount)
        {
            if (!base.IsDisposed)
            {
                lock (this._socketListeners)
                {
                    RtmpSocketListener listener = new RtmpSocketListener(this, localEndPoint, acceptCount);
                    this._socketListeners.Add(listener);
                }
            }
        }

        protected override void Free()
        {
            this.Stop();
            if (this._bufferPool != null)
            {
                this._bufferPool.Dispose();
            }
        }

        internal RtmpServerConnection[] GetConnections()
        {
            RtmpServerConnection[] array = null;
            if (!base.IsDisposed)
            {
                lock (this._connections)
                {
                    array = new RtmpServerConnection[this._connections.Count];
                    this._connections.Keys.CopyTo(array, 0);
                }
            }
            return array;
        }

        internal RtmpSocketListener[] GetSocketListeners()
        {
            RtmpSocketListener[] array = null;
            if (!base.IsDisposed)
            {
                lock (this._socketListeners)
                {
                    array = new RtmpSocketListener[this._socketListeners.Count];
                    this._socketListeners.CopyTo(array, 0);
                }
            }
            return array;
        }

        internal void InitializeConnection(Socket socket)
        {
            if (!base.IsDisposed)
            {
                RtmpServerConnection connection = new RtmpServerConnection(this, socket);
                if (log.get_IsDebugEnabled())
                {
                    log.Debug(__Res.GetString("Rtmp_SocketListenerAccept", new object[] { connection.ConnectionId }));
                }
                this.AddConnection(connection);
                this._rtmpHandler.ConnectionOpened(connection);
                connection.BeginReceive(true);
            }
        }

        internal void OnConnectionClose(RtmpServerConnection connection)
        {
            if (!base.IsDisposed)
            {
                this.RemoveConnection(connection);
            }
        }

        internal void RaiseOnError(Exception exception)
        {
            if (this._onErrorEvent != null)
            {
                this._onErrorEvent(this, new ServerErrorEventArgs(exception));
            }
        }

        internal void RemoveConnection(RtmpServerConnection connection)
        {
            if (!base.IsDisposed)
            {
                lock (this._connections)
                {
                    this._connections.Remove(connection);
                }
            }
        }

        public void RemoveListener(RtmpSocketListener socketListener)
        {
            if (!base.IsDisposed)
            {
                lock (this._socketListeners)
                {
                    this._socketListeners.Remove(socketListener);
                }
            }
        }

        public void Start()
        {
            try
            {
                if (log.get_IsInfoEnabled())
                {
                    log.Info(__Res.GetString("SocketServer_Start"));
                }
                this._bufferPool = new FluorineFx.Util.BufferPool(FluorineConfiguration.Instance.FluorineSettings.RtmpServer.RtmpTransportSettings.ReceiveBufferSize);
                if (!base.IsDisposed)
                {
                    foreach (RtmpSocketListener listener in this._socketListeners)
                    {
                        listener.Start();
                    }
                }
                if (log.get_IsInfoEnabled())
                {
                    log.Info(__Res.GetString("SocketServer_Started"));
                }
            }
            catch (Exception exception)
            {
                if (log.get_IsFatalEnabled())
                {
                    log.Fatal("SocketServer failed", exception);
                }
            }
        }

        public void Stop()
        {
            if (!base.IsDisposed)
            {
                try
                {
                    if (log.get_IsInfoEnabled())
                    {
                        log.Info(__Res.GetString("SocketServer_Stopping"));
                    }
                    this.StopListeners();
                    this.StopConnections();
                    if (log.get_IsInfoEnabled())
                    {
                        log.Info(__Res.GetString("SocketServer_Stopped"));
                    }
                }
                catch (Exception exception)
                {
                    if (log.get_IsFatalEnabled())
                    {
                        log.Fatal(__Res.GetString("SocketServer_Failed"), exception);
                    }
                }
            }
        }

        private void StopConnections()
        {
            if (!base.IsDisposed)
            {
                RtmpServerConnection[] connections = this.GetConnections();
                if (connections != null)
                {
                    foreach (RtmpServerConnection connection in connections)
                    {
                        try
                        {
                            connection.Close();
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        private void StopListeners()
        {
            if (!base.IsDisposed)
            {
                RtmpSocketListener[] socketListeners = this.GetSocketListeners();
                if (socketListeners != null)
                {
                    foreach (RtmpSocketListener listener in socketListeners)
                    {
                        try
                        {
                            listener.Stop();
                            this.RemoveListener(listener);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        internal FluorineFx.Util.BufferPool BufferPool
        {
            get
            {
                return this._bufferPool;
            }
        }

        internal IEndpoint Endpoint
        {
            get
            {
                return this._endpoint;
            }
        }

        internal IRtmpHandler RtmpHandler
        {
            get
            {
                return this._rtmpHandler;
            }
        }
    }
}

