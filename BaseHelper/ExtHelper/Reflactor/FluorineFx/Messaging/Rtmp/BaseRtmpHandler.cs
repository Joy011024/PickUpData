namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx.Context;
    using FluorineFx.IO;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Service;
    using FluorineFx.Messaging.Api.Stream;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Messaging.Rtmp.SO;
    using FluorineFx.Util;
    using log4net;
    using System;

    [CLSCompliant(false)]
    public abstract class BaseRtmpHandler : IRtmpHandler
    {
        public const string ACTION_CLOSE_STREAM = "closeStream";
        public const string ACTION_CONNECT = "connect";
        public const string ACTION_CREATE_STREAM = "createStream";
        public const string ACTION_DELETE_STREAM = "deleteStream";
        public const string ACTION_DISCONNECT = "disconnect";
        public const string ACTION_PAUSE = "pause";
        public const string ACTION_PLAY = "play";
        public const string ACTION_PUBLISH = "publish";
        public const string ACTION_RECEIVE_AUDIO = "receiveAudio";
        public const string ACTION_RECEIVE_VIDEO = "receiveVideo";
        public const string ACTION_RELEASE_STREAM = "releaseStream";
        public const string ACTION_SEEK = "seek";
        public const string ACTION_STOP = "disconnect";
        private static readonly ILog log = LogManager.GetLogger(typeof(BaseRtmpHandler));

        public virtual void ConnectionClosed(RtmpConnection connection)
        {
            FluorineRtmpContext.Initialize(connection);
            connection.Close();
        }

        public virtual void ConnectionOpened(RtmpConnection connection)
        {
        }

        internal static string GetHostname(string url)
        {
            string[] strArray = url.Split(new char[] { '/' });
            if (strArray.Length == 2)
            {
                return "";
            }
            return strArray[2];
        }

        protected void HandlePendingCallResult(RtmpConnection connection, Notify invoke)
        {
            IServiceCall serviceCall = invoke.ServiceCall;
            IPendingServiceCall pendingCall = connection.GetPendingCall(invoke.InvokeId);
            if (pendingCall != null)
            {
                pendingCall.Status = serviceCall.Status;
                object[] arguments = serviceCall.Arguments;
                if ((arguments != null) && (arguments.Length > 0))
                {
                    pendingCall.Result = arguments[0];
                }
                IPendingServiceCallback[] callbacks = pendingCall.GetCallbacks();
                if ((callbacks != null) && (callbacks.Length > 0))
                {
                    foreach (IPendingServiceCallback callback in callbacks)
                    {
                        try
                        {
                            callback.ResultReceived(pendingCall);
                        }
                        catch (Exception exception)
                        {
                            log.Error("Error while executing callback " + callback, exception);
                        }
                    }
                }
            }
        }

        public void MessageReceived(RtmpConnection connection, object obj)
        {
            IRtmpEvent evt = null;
            try
            {
                RtmpPacket packet = obj as RtmpPacket;
                evt = packet.Message;
                RtmpHeader source = packet.Header;
                RtmpChannel channel = connection.GetChannel(source.ChannelId);
                IClientStream streamById = null;
                if (connection is IStreamCapableConnection)
                {
                    streamById = (connection as IStreamCapableConnection).GetStreamById(source.StreamId);
                }
                FluorineContext.Current.Connection.SetAttribute("__@fluorinestreamid", source.StreamId);
                connection.MessageReceived();
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    log.Debug("RtmpConnection message received, type = " + source.DataType);
                }
                if (evt != null)
                {
                    evt.Source = connection;
                }
                switch (source.DataType)
                {
                    case 1:
                        this.OnChunkSize(connection, channel, source, evt as ChunkSize);
                        return;

                    case 3:
                        this.OnStreamBytesRead(connection, channel, source, evt as BytesRead);
                        return;

                    case 4:
                        this.OnPing(connection, channel, source, evt as Ping);
                        return;

                    case 5:
                        this.OnServerBW(connection, channel, source, evt as ServerBW);
                        return;

                    case 6:
                        this.OnClientBW(connection, channel, source, evt as ClientBW);
                        return;

                    case 8:
                    case 9:
                        if (streamById != null)
                        {
                            ((IEventDispatcher) streamById).DispatchEvent(evt);
                        }
                        return;

                    case 15:
                        if (streamById != null)
                        {
                            (streamById as IEventDispatcher).DispatchEvent(evt);
                        }
                        return;

                    case 0x10:
                    case 0x13:
                        this.OnSharedObject(connection, channel, source, evt as SharedObjectMessage);
                        return;

                    case 0x11:
                        this.OnFlexInvoke(connection, channel, source, evt as FlexInvoke);
                        if ((((evt.Header.StreamId != 0) && ((evt as Invoke).ServiceCall.ServiceName == null)) && ((evt as Invoke).ServiceCall.ServiceMethodName == "publish")) && (streamById != null))
                        {
                            (streamById as IEventDispatcher).DispatchEvent(evt);
                        }
                        return;

                    case 0x12:
                        if (((evt as Notify).Data == null) || (streamById == null))
                        {
                            break;
                        }
                        (streamById as IEventDispatcher).DispatchEvent(evt);
                        return;

                    case 20:
                        this.OnInvoke(connection, channel, source, evt as Invoke);
                        if ((((evt.Header.StreamId != 0) && ((evt as Invoke).ServiceCall.ServiceName == null)) && ((evt as Invoke).ServiceCall.ServiceMethodName == "publish")) && (streamById != null))
                        {
                            (streamById as IEventDispatcher).DispatchEvent(evt);
                        }
                        return;

                    default:
                        goto Label_0304;
                }
                this.OnInvoke(connection, channel, source, evt as Notify);
                return;
            Label_0304:
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    log.Debug("RtmpService event not handled: " + source.DataType);
                }
            }
            catch (Exception exception)
            {
                log.Error("Runtime error", exception);
            }
        }

        public virtual void MessageSent(RtmpConnection connection, object message)
        {
            if (!(message is ByteBuffer))
            {
                connection.MessageSent(message as RtmpPacket);
            }
        }

        protected abstract void OnChunkSize(RtmpConnection connection, RtmpChannel channel, RtmpHeader source, ChunkSize chunkSize);
        protected abstract void OnClientBW(RtmpConnection connection, RtmpChannel channel, RtmpHeader source, ClientBW clientBW);
        protected abstract void OnFlexInvoke(RtmpConnection connection, RtmpChannel channel, RtmpHeader header, FlexInvoke invoke);
        protected abstract void OnInvoke(RtmpConnection connection, RtmpChannel channel, RtmpHeader header, Notify invoke);
        protected abstract void OnPing(RtmpConnection connection, RtmpChannel channel, RtmpHeader source, Ping ping);
        protected abstract void OnServerBW(RtmpConnection connection, RtmpChannel channel, RtmpHeader source, ServerBW serverBW);
        protected abstract void OnSharedObject(RtmpConnection connection, RtmpChannel channel, RtmpHeader header, SharedObjectMessage message);
        protected void OnStreamBytesRead(RtmpConnection connection, RtmpChannel channel, RtmpHeader source, BytesRead streamBytesRead)
        {
            connection.ReceivedBytesRead(streamBytesRead.Bytes);
        }

        internal static void Push(RtmpConnection connection, IMessage message, IMessageClient messageClient)
        {
            if (connection != null)
            {
                object obj2 = message;
                if (message is BinaryMessage)
                {
                    BinaryMessage message2 = message as BinaryMessage;
                    message2.Update(messageClient);
                    byte[] body = message2.body as byte[];
                    RawBinary binary = new RawBinary(body);
                    obj2 = binary;
                }
                else
                {
                    message.SetHeader("DSDstClientId", messageClient.ClientId);
                    message.clientId = messageClient.ClientId;
                }
                RtmpChannel channel = connection.GetChannel(3);
                FlexInvoke invoke = new FlexInvoke {
                    Cmd = "receive",
                    InvokeId = connection.InvokeId,
                    Response = obj2
                };
                channel.Write(invoke);
            }
        }
    }
}

