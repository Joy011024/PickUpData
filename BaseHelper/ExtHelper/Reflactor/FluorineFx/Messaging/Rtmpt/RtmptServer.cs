namespace FluorineFx.Messaging.Rtmpt
{
    using FluorineFx;
    using FluorineFx.Collections;
    using FluorineFx.Context;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Messaging.Rtmp;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;
    using System.Web;

    internal sealed class RtmptServer
    {
        private SynchronizedHashtable _connections = new SynchronizedHashtable();
        private RtmptEndpoint _endpoint;
        private FluorineFx.Messaging.Rtmp.RtmpHandler _rtmpHandler;
        private static readonly ILog log = LogManager.GetLogger(typeof(RtmptServer));
        private static int RESPONSE_TARGET_SIZE = 0x8000;

        public RtmptServer(RtmptEndpoint endpoint)
        {
            this._endpoint = endpoint;
            this._rtmpHandler = new FluorineFx.Messaging.Rtmp.RtmpHandler(endpoint);
        }

        private string GetClientId(RtmptRequest request)
        {
            string url = request.Url;
            if (url == string.Empty)
            {
                return null;
            }
            while ((url.Length > 1) && (url[0] == '/'))
            {
                url = url.Substring(1);
            }
            int index = url.IndexOf('/');
            int num2 = url.IndexOf('/', index + 1);
            if ((index != -1) && (num2 != -1))
            {
                url = url.Substring(index + 1, (num2 - index) - 1);
            }
            return url;
        }

        private string GetClientId(HttpRequest request)
        {
            string httpRequestPath = this.GetHttpRequestPath(request);
            if (httpRequestPath == string.Empty)
            {
                return null;
            }
            while ((httpRequestPath.Length > 1) && (httpRequestPath[0] == '/'))
            {
                httpRequestPath = httpRequestPath.Substring(1);
            }
            int index = httpRequestPath.IndexOf('/');
            int num2 = httpRequestPath.IndexOf('/', index + 1);
            if ((index != -1) && (num2 != -1))
            {
                httpRequestPath = httpRequestPath.Substring(index + 1, (num2 - index) - 1);
            }
            return httpRequestPath;
        }

        private RtmptConnection GetConnection(RtmptRequest request)
        {
            string clientId = this.GetClientId(request);
            return (this._connections[clientId] as RtmptConnection);
        }

        private RtmptConnection GetConnection(HttpRequest request)
        {
            string clientId = this.GetClientId(request);
            return (this._connections[clientId] as RtmptConnection);
        }

        private string GetHttpRequestPath(HttpRequest request)
        {
            string path = request.Path;
            if (request.Headers["RTMPT-command"] != null)
            {
                path = request.Headers["RTMPT-command"];
            }
            return path;
        }

        private void HandleBadRequest(string message, RtmptRequest request)
        {
            ByteBuffer stream = ByteBuffer.Allocate(100);
            StreamWriter writer = new StreamWriter(stream);
            if (request.HttpVersion == 1)
            {
                writer.Write("HTTP/1.1 400 " + message + "\r\n");
                writer.Write("Cache-Control: no-cache\r\n");
            }
            else
            {
                writer.Write("HTTP/1.0 400 " + message + "\r\n");
                writer.Write("Pragma: no-cache\r\n");
            }
            writer.Write("Content-Type: text/plain\r\n");
            writer.Write("Content-Length: " + message.Length.ToString() + "\r\n");
            writer.Write("Connection: Keep-Alive\r\n");
            writer.Write("\r\n");
            writer.Write(message);
            writer.Flush();
            request.Connection.Send(stream);
        }

        private void HandleBadRequest(string message, HttpResponse response)
        {
            response.StatusCode = 400;
            response.ContentType = "text/plain";
            response.AppendHeader("Content-Length", message.Length.ToString());
            response.Write(message);
            response.Flush();
        }

        private void HandleClose(RtmptRequest request)
        {
            RtmptConnection connection = this.GetConnection(request);
            if (connection == null)
            {
                this.HandleBadRequest(__Res.GetString("Rtmpt_UnknownClient", new object[] { request.Url }), request);
            }
            else
            {
                if (connection.Client != null)
                {
                    connection.Client.Renew();
                }
                connection.DeferredClose();
                this.ReturnMessage((byte) 0, request);
            }
        }

        private void HandleClose(HttpRequest request, HttpResponse response)
        {
            RtmptConnection connection = this.GetConnection(request);
            if (connection == null)
            {
                this.HandleBadRequest(__Res.GetString("Rtmpt_UnknownClient", new object[] { this.GetHttpRequestPath(request) }), response);
            }
            else
            {
                if (connection.Client != null)
                {
                    connection.Client.Renew();
                }
                connection.DeferredClose();
                this.ReturnMessage((byte) 0, response);
            }
        }

        private void HandleIdle(RtmptRequest request)
        {
            RtmptConnection connection = this.GetConnection(request);
            if (connection == null)
            {
                this.HandleBadRequest(__Res.GetString("Rtmpt_UnknownClient", new object[] { request.Url }), request);
            }
            else if (connection.IsClosing)
            {
                this.ReturnMessage((byte) 0, request);
                connection.DeferredClose();
            }
            else
            {
                if (connection.Client != null)
                {
                    connection.Client.Renew();
                }
                this.ReturnPendingMessages(connection, request);
            }
        }

        private void HandleIdle(HttpRequest request, HttpResponse response)
        {
            RtmptConnection connection = this.GetConnection(request);
            if (connection == null)
            {
                this.HandleBadRequest(__Res.GetString("Rtmpt_UnknownClient", new object[] { this.GetHttpRequestPath(request) }), response);
            }
            else if (connection.IsClosing)
            {
                this.ReturnMessage((byte) 0, response);
                connection.DeferredClose();
            }
            else
            {
                if (connection.Client != null)
                {
                    connection.Client.Renew();
                }
                this.ReturnPendingMessages(connection, response);
            }
        }

        private void HandleOpen(RtmptRequest request)
        {
            RtmptConnection connection = new RtmptConnection(request.Connection.RemoteEndPoint, this, null, null);
            this._connections[connection.ConnectionId] = connection;
            this.ReturnMessage(connection.ConnectionId + "\n", request);
        }

        private void HandleOpen(HttpRequest request, HttpResponse response)
        {
            RtmptConnection connection = new RtmptConnection(this, null, null);
            this._connections[connection.ConnectionId] = connection;
            this.ReturnMessage(connection.ConnectionId + "\n", response);
        }

        private void HandleSend(RtmptRequest request)
        {
            RtmptConnection connection = this.GetConnection(request);
            if (connection == null)
            {
                this.HandleBadRequest(__Res.GetString("Rtmpt_UnknownClient", new object[] { request.Url }), request);
            }
            else
            {
                if (connection.Client != null)
                {
                    connection.Client.Renew();
                }
                int contentLength = request.ContentLength;
                ByteBuffer data = request.Data;
                IList list = connection.Decode(data);
                if ((list == null) || (list.Count == 0))
                {
                    this.ReturnMessage(connection.PollingDelay, request);
                }
                else
                {
                    foreach (object obj2 in list)
                    {
                        try
                        {
                            if (obj2 is ByteBuffer)
                            {
                                ByteBuffer packet = obj2 as ByteBuffer;
                                connection.RawWrite(packet);
                            }
                            else
                            {
                                FluorineRtmpContext.Initialize(connection);
                                this._rtmpHandler.MessageReceived(connection, obj2);
                            }
                        }
                        catch (Exception exception)
                        {
                            log.Error(__Res.GetString("Rtmp_CouldNotProcessMessage"), exception);
                        }
                    }
                    this.ReturnPendingMessages(connection, request);
                }
            }
        }

        private void HandleSend(HttpRequest request, HttpResponse response)
        {
            RtmptConnection connection = this.GetConnection(request);
            if (connection == null)
            {
                this.HandleBadRequest(__Res.GetString("Rtmpt_UnknownClient", new object[] { this.GetHttpRequestPath(request) }), response);
            }
            else
            {
                if (connection.Client != null)
                {
                    connection.Client.Renew();
                }
                int contentLength = request.ContentLength;
                byte[] buffer = new byte[request.InputStream.Length];
                request.InputStream.Read(buffer, 0, (int) request.InputStream.Length);
                ByteBuffer data = ByteBuffer.Wrap(buffer);
                IList list = connection.Decode(data);
                if ((list == null) || (list.Count == 0))
                {
                    this.ReturnMessage(connection.PollingDelay, response);
                }
                else
                {
                    foreach (object obj2 in list)
                    {
                        try
                        {
                            if (obj2 is ByteBuffer)
                            {
                                ByteBuffer packet = obj2 as ByteBuffer;
                                connection.RawWrite(packet);
                            }
                            else
                            {
                                (FluorineContext.Current as FluorineWebContext).SetConnection(connection);
                                this._rtmpHandler.MessageReceived(connection, obj2);
                            }
                        }
                        catch (Exception exception)
                        {
                            log.Error(__Res.GetString("Rtmp_CouldNotProcessMessage"), exception);
                        }
                    }
                    this.ReturnPendingMessages(connection, response);
                }
            }
        }

        internal void OnConnectionClose(RtmptConnection connection)
        {
            this._connections.Remove(connection.ConnectionId);
        }

        private void ReturnMessage(byte message, RtmptRequest request)
        {
            ByteBuffer stream = ByteBuffer.Allocate(100);
            StreamWriter writer = new StreamWriter(stream);
            if (request.HttpVersion == 1)
            {
                writer.Write("HTTP/1.1 200 OK\r\n");
                writer.Write("Cache-Control: no-cache\r\n");
            }
            else
            {
                writer.Write("HTTP/1.0 200 OK\r\n");
                writer.Write("Pragma: no-cache\r\n");
            }
            writer.Write("Content-Length: 1\r\n");
            writer.Write("Connection: Keep-Alive\r\n");
            writer.Write(string.Format("Content-Type: {0}\r\n", "application/x-fcs"));
            writer.Write("\r\n");
            writer.Write((char) message);
            writer.Flush();
            request.Connection.Send(stream);
        }

        private void ReturnMessage(byte message, HttpResponse response)
        {
            response.StatusCode = 200;
            response.ClearHeaders();
            response.AppendHeader("Connection", "Keep-Alive");
            response.AppendHeader("Content-Length", "1");
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = "application/x-fcs";
            response.Write((char) message);
            response.Flush();
        }

        private void ReturnMessage(string message, RtmptRequest request)
        {
            ByteBuffer stream = ByteBuffer.Allocate(100);
            StreamWriter writer = new StreamWriter(stream);
            if (request.HttpVersion == 1)
            {
                writer.Write("HTTP/1.1 200 OK\r\n");
                writer.Write("Cache-Control: no-cache\r\n");
            }
            else
            {
                writer.Write("HTTP/1.0 200 OK\r\n");
                writer.Write("Pragma: no-cache\r\n");
            }
            writer.Write("Content-Length: " + message.Length.ToString() + "\r\n");
            writer.Write(string.Format("Content-Type: {0}\r\n", "application/x-fcs"));
            writer.Write("Connection: Keep-Alive\r\n");
            writer.Write("\r\n");
            writer.Write(message);
            writer.Flush();
            request.Connection.Send(stream);
        }

        private void ReturnMessage(string message, HttpResponse response)
        {
            response.StatusCode = 200;
            response.ClearHeaders();
            response.AppendHeader("Connection", "Keep-Alive");
            response.AppendHeader("Content-Length", message.Length.ToString());
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = "application/x-fcs";
            response.Write(message);
            response.Flush();
        }

        private void ReturnMessage(RtmptConnection connection, ByteBuffer data, RtmptRequest request)
        {
            ByteBuffer stream = ByteBuffer.Allocate(((int) data.Length) + 30);
            StreamWriter writer = new StreamWriter(stream);
            int num = data.Limit + 1;
            if (request.HttpVersion == 1)
            {
                writer.Write("HTTP/1.1 200 OK\r\n");
                writer.Write("Cache-Control: no-cache\r\n");
            }
            else
            {
                writer.Write("HTTP/1.0 200 OK\r\n");
                writer.Write("Pragma: no-cache\r\n");
            }
            writer.Write(string.Format("Content-Length: {0}\r\n", num));
            writer.Write("Connection: Keep-Alive\r\n");
            writer.Write(string.Format("Content-Type: {0}\r\n", "application/x-fcs"));
            writer.Write("\r\n");
            writer.Write((char) connection.PollingDelay);
            writer.Flush();
            BinaryWriter writer2 = new BinaryWriter(stream);
            byte[] buffer = data.ToArray();
            writer2.Write(buffer);
            writer2.Flush();
            request.Connection.Send(stream);
        }

        private void ReturnMessage(RtmptConnection connection, ByteBuffer data, HttpResponse response)
        {
            response.StatusCode = 200;
            response.ClearHeaders();
            response.AppendHeader("Connection", "Keep-Alive");
            response.AppendHeader("Content-Length", (data.Limit + 1).ToString());
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = "application/x-fcs";
            response.Write((char) connection.PollingDelay);
            byte[] buffer = data.ToArray();
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.Flush();
        }

        private void ReturnPendingMessages(RtmptConnection connection, RtmptRequest request)
        {
            ByteBuffer pendingMessages = connection.GetPendingMessages(RESPONSE_TARGET_SIZE);
            if (pendingMessages == null)
            {
                if (connection.IsClosing)
                {
                    this.ReturnMessage((byte) 0, request);
                }
                else
                {
                    this.ReturnMessage(connection.PollingDelay, request);
                }
            }
            else
            {
                this.ReturnMessage(connection, pendingMessages, request);
            }
        }

        private void ReturnPendingMessages(RtmptConnection connection, HttpResponse response)
        {
            ByteBuffer pendingMessages = connection.GetPendingMessages(RESPONSE_TARGET_SIZE);
            if (pendingMessages == null)
            {
                if (connection.IsClosing)
                {
                    this.ReturnMessage((byte) 0, response);
                }
                else
                {
                    this.ReturnMessage(connection.PollingDelay, response);
                }
            }
            else
            {
                this.ReturnMessage(connection, pendingMessages, response);
            }
        }

        public void Service(RtmptRequest request)
        {
            if ((request.HttpMethod != "POST") || (request.ContentLength == 0))
            {
                this.HandleBadRequest(__Res.GetString("Rtmpt_CommandBadRequest"), request);
            }
            string url = request.Url;
            switch (url[1])
            {
                case 'o':
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("Rtmpt_CommandOpen", new object[] { url }));
                    }
                    this.HandleOpen(request);
                    break;

                case 's':
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("Rtmpt_CommandSend", new object[] { url }));
                    }
                    this.HandleSend(request);
                    break;

                case 'c':
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("Rtmpt_CommandClose", new object[] { url }));
                    }
                    this.HandleClose(request);
                    break;

                case 'i':
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("Rtmpt_CommandIdle", new object[] { url }));
                    }
                    this.HandleIdle(request);
                    break;

                default:
                    this.HandleBadRequest(__Res.GetString("Rtmpt_CommandNotSupported", new object[] { url }), request);
                    break;
            }
        }

        public void Service(HttpRequest request, HttpResponse response)
        {
            if ((request.HttpMethod != "POST") || (request.ContentLength == 0))
            {
                this.HandleBadRequest(__Res.GetString("Rtmpt_CommandBadRequest"), response);
            }
            string httpRequestPath = this.GetHttpRequestPath(request);
            switch (httpRequestPath[1])
            {
                case 'o':
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("Rtmpt_CommandOpen", new object[] { httpRequestPath }));
                    }
                    this.HandleOpen(request, response);
                    break;

                case 's':
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("Rtmpt_CommandSend", new object[] { httpRequestPath }));
                    }
                    this.HandleSend(request, response);
                    break;

                case 'c':
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("Rtmpt_CommandClose", new object[] { httpRequestPath }));
                    }
                    this.HandleClose(request, response);
                    break;

                case 'i':
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("Rtmpt_CommandIdle", new object[] { httpRequestPath }));
                    }
                    this.HandleIdle(request, response);
                    break;

                default:
                    this.HandleBadRequest(__Res.GetString("Rtmpt_CommandNotSupported", new object[] { httpRequestPath }), response);
                    break;
            }
        }

        public IEndpoint Endpoint
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

