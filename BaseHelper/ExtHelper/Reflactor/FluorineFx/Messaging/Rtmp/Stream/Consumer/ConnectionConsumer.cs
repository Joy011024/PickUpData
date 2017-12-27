namespace FluorineFx.Messaging.Rtmp.Stream.Consumer
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Messaging;
    using FluorineFx.Messaging.Api.Stream;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmp;
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Messaging.Rtmp.Stream;
    using FluorineFx.Messaging.Rtmp.Stream.Messages;
    using log4net;
    using System;

    internal class ConnectionConsumer : IPushableConsumer, IConsumer, IMessageComponent, IPipeConnectionListener
    {
        private RtmpChannel _audio;
        private int _chunkSize = -1;
        private RtmpConnection _connection;
        private RtmpChannel _data;
        private StreamTracker _streamTracker;
        private RtmpChannel _video;
        private static ILog log = LogManager.GetLogger(typeof(ConnectionConsumer));

        public ConnectionConsumer(RtmpConnection connection, int videoChannel, int audioChannel, int dataChannel)
        {
            this._connection = connection;
            this._video = connection.GetChannel(videoChannel);
            this._audio = connection.GetChannel(audioChannel);
            this._data = connection.GetChannel(dataChannel);
            this._streamTracker = new StreamTracker();
        }

        public void OnOOBControlMessage(IMessageComponent source, IPipe pipe, OOBControlMessage oobCtrlMsg)
        {
            if ("ConnectionConsumer".Equals(oobCtrlMsg.Target))
            {
                if ("pendingCount".Equals(oobCtrlMsg.ServiceName))
                {
                    oobCtrlMsg.Result = this._connection.PendingMessages;
                }
                else if ("pendingVideoCount".Equals(oobCtrlMsg.ServiceName))
                {
                    IClientStream streamByChannelId = null;
                    if (this._connection is IStreamCapableConnection)
                    {
                        streamByChannelId = (this._connection as IStreamCapableConnection).GetStreamByChannelId(this._video.ChannelId);
                    }
                    if (streamByChannelId != null)
                    {
                        oobCtrlMsg.Result = this._connection.GetPendingVideoMessages(streamByChannelId.StreamId);
                    }
                    else
                    {
                        oobCtrlMsg.Result = 0L;
                    }
                }
                else if ("writeDelta".Equals(oobCtrlMsg.ServiceName))
                {
                    long num = 0L;
                    IBWControllable parentBWControllable = this._connection as IBWControllable;
                    while ((parentBWControllable != null) && (parentBWControllable.BandwidthConfiguration == null))
                    {
                        parentBWControllable = parentBWControllable.GetParentBWControllable();
                    }
                    if ((parentBWControllable != null) && (parentBWControllable.BandwidthConfiguration != null))
                    {
                        IBandwidthConfigure bandwidthConfiguration = parentBWControllable.BandwidthConfiguration;
                        if (bandwidthConfiguration is IConnectionBWConfig)
                        {
                            num = (bandwidthConfiguration as IConnectionBWConfig).DownstreamBandwidth / 8L;
                        }
                    }
                    if (num <= 0L)
                    {
                        num = 0x1e000L;
                    }
                    oobCtrlMsg.Result = new long[] { this._connection.WrittenBytes - this._connection.ClientBytesRead, num / 2L };
                }
                else if ("chunkSize".Equals(oobCtrlMsg.ServiceName))
                {
                    int num2 = (int) oobCtrlMsg.ServiceParameterMap["chunkSize"];
                    if (num2 != this._chunkSize)
                    {
                        this._chunkSize = num2;
                        ChunkSize message = new ChunkSize(this._chunkSize);
                        this._connection.GetChannel(2).Write(message);
                    }
                }
            }
        }

        public void OnPipeConnectionEvent(PipeConnectionEvent evt)
        {
            if (evt.Type == 2)
            {
                this._connection.CloseChannel(this._video.ChannelId);
                this._connection.CloseChannel(this._audio.ChannelId);
                this._connection.CloseChannel(this._data.ChannelId);
            }
        }

        public void PushMessage(IPipe pipe, IMessage message)
        {
            if (message is ResetMessage)
            {
                this._streamTracker.Reset();
            }
            else if (message is StatusMessage)
            {
                StatusMessage message2 = message as StatusMessage;
                this._data.SendStatus(message2.body as StatusASO);
            }
            else if (message is RtmpMessage)
            {
                RtmpMessage message3 = message as RtmpMessage;
                IRtmpEvent body = message3.body;
                RtmpHeader header = new RtmpHeader();
                int num = this._streamTracker.Add(body);
                if (num < 0)
                {
                    log.Warn("Skipping message with negative timestamp.");
                }
                else
                {
                    header.IsTimerRelative = this._streamTracker.IsRelative;
                    header.Timer = num;
                    switch (body.DataType)
                    {
                        case 3:
                        {
                            BytesRead read = new BytesRead((body as BytesRead).Bytes);
                            header.IsTimerRelative = false;
                            header.Timer = 0;
                            read.Header = header;
                            read.Timestamp = header.Timer;
                            this._connection.GetChannel(2).Write(read);
                            return;
                        }
                        case 4:
                        {
                            Ping ping = new Ping((body as Ping).Value1, (body as Ping).Value2, (body as Ping).Value3, (body as Ping).Value4);
                            header.IsTimerRelative = false;
                            header.Timer = 0;
                            ping.Header = header;
                            ping.Timestamp = header.Timer;
                            this._connection.Ping(ping);
                            return;
                        }
                        case 8:
                        {
                            AudioData data2 = new AudioData((body as AudioData).Data) {
                                Header = header,
                                Timestamp = header.Timer
                            };
                            this._audio.Write(data2);
                            return;
                        }
                        case 9:
                        {
                            VideoData data = new VideoData((body as VideoData).Data) {
                                Header = header,
                                Timestamp = header.Timer
                            };
                            this._video.Write(data);
                            return;
                        }
                        case 15:
                        {
                            FlexStreamSend send = new FlexStreamSend((body as Notify).Data) {
                                Header = header,
                                Timestamp = header.Timer
                            };
                            this._data.Write(send);
                            return;
                        }
                        case 0x12:
                        {
                            Notify notify = new Notify((body as Notify).Data) {
                                Header = header,
                                Timestamp = header.Timer
                            };
                            this._data.Write(notify);
                            return;
                        }
                    }
                    this._data.Write(body);
                }
            }
        }
    }
}

