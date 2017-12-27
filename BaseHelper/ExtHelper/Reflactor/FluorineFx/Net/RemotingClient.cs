namespace FluorineFx.Net
{
    using FluorineFx;
    using FluorineFx.IO;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Service;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Messaging.Rtmp.Service;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    internal class RemotingClient : INetConnectionClient
    {
        private string _gatewayUrl;
        private NetConnection _netConnection;

        public RemotingClient(NetConnection netConnection)
        {
            this._netConnection = netConnection;
        }

        private void BeginRequestFlashCall(IAsyncResult ar)
        {
            try
            {
                RequestData asyncState = ar.AsyncState as RequestData;
                if (asyncState != null)
                {
                    AMFSerializer serializer = new AMFSerializer(asyncState.Request.EndGetRequestStream(ar));
                    serializer.WriteMessage(asyncState.AmfMessage);
                    serializer.Flush();
                    serializer.Close();
                    asyncState.Request.BeginGetResponse(new AsyncCallback(this.BeginResponseFlashCall), asyncState);
                }
            }
            catch (Exception exception)
            {
                this._netConnection.RaiseNetStatus(exception);
            }
        }

        private void BeginRequestFlexCall(IAsyncResult ar)
        {
            try
            {
                RequestData asyncState = ar.AsyncState as RequestData;
                if (asyncState != null)
                {
                    AMFSerializer serializer = new AMFSerializer(asyncState.Request.EndGetRequestStream(ar));
                    serializer.WriteMessage(asyncState.AmfMessage);
                    serializer.Flush();
                    serializer.Close();
                    asyncState.Request.BeginGetResponse(new AsyncCallback(this.BeginResponseFlexCall), asyncState);
                }
            }
            catch (Exception exception)
            {
                this._netConnection.RaiseNetStatus(exception);
            }
        }

        private void BeginResponseFlashCall(IAsyncResult ar)
        {
            try
            {
                RequestData asyncState = ar.AsyncState as RequestData;
                if (asyncState != null)
                {
                    HttpWebResponse response = (HttpWebResponse) asyncState.Request.EndGetResponse(ar);
                    if (response != null)
                    {
                        Stream responseStream = response.GetResponseStream();
                        if (responseStream != null)
                        {
                            AMFMessage message = new AMFDeserializer(responseStream).ReadAMFMessage();
                            AMFBody bodyAt = message.GetBodyAt(0);
                            for (int i = 0; i < message.HeaderCount; i++)
                            {
                                AMFHeader headerAt = message.GetHeaderAt(i);
                                if (headerAt.Name == "RequestPersistentHeader")
                                {
                                    this._netConnection.AddHeader(headerAt.Name, headerAt.MustUnderstand, headerAt.Content);
                                }
                            }
                            PendingCall call = asyncState.Call;
                            call.Result = bodyAt.Content;
                            if (bodyAt.Target.EndsWith("/onStatus"))
                            {
                                call.Status = 0x13;
                            }
                            else
                            {
                                call.Status = 2;
                            }
                            asyncState.Callback.ResultReceived(call);
                        }
                        else
                        {
                            this._netConnection.RaiseNetStatus("Could not aquire ResponseStream");
                        }
                    }
                    else
                    {
                        this._netConnection.RaiseNetStatus("Could not aquire HttpWebResponse");
                    }
                }
            }
            catch (Exception exception)
            {
                this._netConnection.RaiseNetStatus(exception);
            }
        }

        private void BeginResponseFlexCall(IAsyncResult ar)
        {
            try
            {
                RequestData asyncState = ar.AsyncState as RequestData;
                if (asyncState != null)
                {
                    HttpWebResponse response = (HttpWebResponse) asyncState.Request.EndGetResponse(ar);
                    if (response != null)
                    {
                        Stream responseStream = response.GetResponseStream();
                        if (responseStream != null)
                        {
                            PendingCall call;
                            AMFMessage message = new AMFDeserializer(responseStream).ReadAMFMessage();
                            AMFBody bodyAt = message.GetBodyAt(0);
                            for (int i = 0; i < message.HeaderCount; i++)
                            {
                                AMFHeader headerAt = message.GetHeaderAt(i);
                                if (headerAt.Name == "RequestPersistentHeader")
                                {
                                    this._netConnection.AddHeader(headerAt.Name, headerAt.MustUnderstand, headerAt.Content);
                                }
                            }
                            object content = bodyAt.Content;
                            if (content is ErrorMessage)
                            {
                                call = asyncState.Call;
                                call.Result = content;
                                call.Status = 0x13;
                                asyncState.Callback.ResultReceived(call);
                            }
                            if (content is AcknowledgeMessage)
                            {
                                AcknowledgeMessage message2 = content as AcknowledgeMessage;
                                if ((this._netConnection.ClientId == null) && message2.HeaderExists("DSId"))
                                {
                                    this._netConnection.SetClientId(message2.GetHeader("DSId") as string);
                                }
                                call = asyncState.Call;
                                call.Result = message2.body;
                                call.Status = 2;
                                asyncState.Callback.ResultReceived(call);
                            }
                        }
                        else
                        {
                            this._netConnection.RaiseNetStatus("Could not aquire ResponseStream");
                        }
                    }
                    else
                    {
                        this._netConnection.RaiseNetStatus("Could not aquire HttpWebResponse");
                    }
                }
            }
            catch (Exception exception)
            {
                this._netConnection.RaiseNetStatus(exception);
            }
        }

        public void Call(string command, IPendingServiceCallback callback, params object[] arguments)
        {
            try
            {
                TypeHelper._Init();
                Uri requestUri = new Uri(this._gatewayUrl);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
                request.ContentType = "application/x-amf";
                request.Method = "POST";
                request.CookieContainer = this._netConnection.CookieContainer;
                AMFMessage amfMessage = new AMFMessage((ushort) this._netConnection.ObjectEncoding);
                AMFBody body = new AMFBody(command, callback.GetHashCode().ToString(), arguments);
                amfMessage.AddBody(body);
                foreach (KeyValuePair<string, AMFHeader> pair in this._netConnection.Headers)
                {
                    amfMessage.AddHeader(pair.Value);
                }
                PendingCall call = new PendingCall(command, arguments);
                RequestData state = new RequestData(request, amfMessage, call, callback);
                request.BeginGetRequestStream(new AsyncCallback(this.BeginRequestFlashCall), state);
            }
            catch (Exception exception)
            {
                this._netConnection.RaiseNetStatus(exception);
            }
        }

        public void Call(string endpoint, string destination, string source, string operation, IPendingServiceCallback callback, params object[] arguments)
        {
            if (this._netConnection.ObjectEncoding == ObjectEncoding.AMF0)
            {
                throw new NotSupportedException("AMF0 not supported for Flex RPC");
            }
            try
            {
                TypeHelper._Init();
                Uri requestUri = new Uri(this._gatewayUrl);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
                request.ContentType = "application/x-amf";
                request.Method = "POST";
                request.CookieContainer = this._netConnection.CookieContainer;
                AMFMessage amfMessage = new AMFMessage((ushort) this._netConnection.ObjectEncoding);
                RemotingMessage message2 = new RemotingMessage {
                    clientId = Guid.NewGuid().ToString("D"),
                    destination = destination,
                    messageId = Guid.NewGuid().ToString("D"),
                    timestamp = 0L,
                    timeToLive = 0L
                };
                message2.SetHeader("DSEndpoint", endpoint);
                if (this._netConnection.ClientId == null)
                {
                    message2.SetHeader("DSId", "nil");
                }
                else
                {
                    message2.SetHeader("DSId", this._netConnection.ClientId);
                }
                message2.source = source;
                message2.operation = operation;
                message2.body = arguments;
                foreach (KeyValuePair<string, AMFHeader> pair in this._netConnection.Headers)
                {
                    amfMessage.AddHeader(pair.Value);
                }
                AMFBody body = new AMFBody(null, null, new object[] { message2 });
                amfMessage.AddBody(body);
                PendingCall call = new PendingCall(source, operation, arguments);
                RequestData state = new RequestData(request, amfMessage, call, callback);
                request.BeginGetRequestStream(new AsyncCallback(this.BeginRequestFlexCall), state);
            }
            catch (Exception exception)
            {
                this._netConnection.RaiseNetStatus(exception);
            }
        }

        public void Close()
        {
        }

        public void Connect(string command, params object[] arguments)
        {
            this._gatewayUrl = command;
        }

        public void Write(IRtmpEvent message)
        {
            throw new NotImplementedException();
        }

        public bool Connected
        {
            get
            {
                return true;
            }
        }

        public IConnection Connection
        {
            get
            {
                return null;
            }
        }
    }
}

