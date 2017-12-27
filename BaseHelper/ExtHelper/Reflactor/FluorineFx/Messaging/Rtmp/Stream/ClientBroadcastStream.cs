namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Context;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Messaging;
    using FluorineFx.Messaging.Api.Statistics;
    using FluorineFx.Messaging.Api.Stream;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmp;
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Messaging.Rtmp.Messaging;
    using FluorineFx.Messaging.Rtmp.Stream.Codec;
    using FluorineFx.Messaging.Rtmp.Stream.Consumer;
    using FluorineFx.Messaging.Rtmp.Stream.Messages;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class ClientBroadcastStream : AbstractClientStream, IClientBroadcastStream, IClientStream, IBWControllable, IBroadcastStream, IStream, IFilter, IProvider, IPushableConsumer, IConsumer, IMessageComponent, IPipeConnectionListener, IEventDispatcher, IClientBroadcastStreamStatistics, IStreamStatistics, IStatisticsBase
    {
        private int _audioTime = -1;
        private long _bytesReceived;
        private bool _checkVideoCodec = false;
        private int _chunkSize = 0;
        private bool _closed = false;
        private IMessageOutput _connMsgOut;
        private long _creationTime;
        private int _dataTime = -1;
        private int _firstPacketTime = -1;
        private IPipe _livePipe;
        private string _publishedName;
        private bool _recording = false;
        private FileConsumer _recordingFile;
        private string _recordingFilename;
        private IPipe _recordPipe;
        private bool _sendStartNotification = true;
        private StatisticsCounter _subscriberStats = new StatisticsCounter();
        private VideoCodecFactory _videoCodecFactory = null;
        private int _videoTime = -1;
        private static ILog log = LogManager.GetLogger(typeof(ClientBroadcastStream));

        private void CheckSendNotifications(IEvent evt)
        {
            IEventListener source = evt.Source;
            this.SendStartNotifications(source);
        }

        public override void Close()
        {
            lock (base.SyncRoot)
            {
                if (!this._closed)
                {
                    this._closed = true;
                    if (this._livePipe != null)
                    {
                        this._livePipe.Unsubscribe(this);
                    }
                    if (this._recordPipe != null)
                    {
                        this._recordPipe.Unsubscribe(this);
                    }
                    if (this._recording)
                    {
                        this.SendRecordStopNotify();
                    }
                    this.SendPublishStopNotify();
                    this._connMsgOut.Unsubscribe(this);
                    this.NotifyBroadcastClose();
                }
            }
        }

        public void DispatchEvent(IEvent evt)
        {
            if (((!(evt is IRtmpEvent) && (evt.EventType != EventType.STREAM_CONTROL)) && (evt.EventType != EventType.STREAM_DATA)) || this._closed)
            {
                if (log.get_IsDebugEnabled())
                {
                    log.Debug("DispatchEvent: " + evt.EventType);
                }
            }
            else
            {
                IStreamCodecInfo codecInfo = base.CodecInfo;
                StreamCodecInfo info2 = null;
                if (codecInfo is StreamCodecInfo)
                {
                    info2 = codecInfo as StreamCodecInfo;
                }
                IRtmpEvent event2 = evt as IRtmpEvent;
                int num = -1;
                if (this._firstPacketTime == -1)
                {
                    this._firstPacketTime = event2.Timestamp;
                }
                if (event2 is AudioData)
                {
                    if (info2 != null)
                    {
                        info2.HasAudio = true;
                    }
                    if (event2.Header.IsTimerRelative)
                    {
                        this._audioTime += event2.Timestamp;
                    }
                    else
                    {
                        this._audioTime = event2.Timestamp;
                    }
                    num = this._audioTime;
                }
                else if (event2 is VideoData)
                {
                    IVideoStreamCodec videoCodec = null;
                    if ((this._videoCodecFactory != null) && this._checkVideoCodec)
                    {
                        videoCodec = this._videoCodecFactory.GetVideoCodec((event2 as VideoData).Data);
                        if (codecInfo is StreamCodecInfo)
                        {
                            (codecInfo as StreamCodecInfo).VideoCodec = videoCodec;
                        }
                        this._checkVideoCodec = false;
                    }
                    else if (codecInfo != null)
                    {
                        videoCodec = codecInfo.VideoCodec;
                    }
                    if (videoCodec != null)
                    {
                        videoCodec.AddData((event2 as VideoData).Data);
                    }
                    if (info2 != null)
                    {
                        info2.HasVideo = true;
                    }
                    if (event2.Header.IsTimerRelative)
                    {
                        this._videoTime += event2.Timestamp;
                    }
                    else
                    {
                        this._videoTime = event2.Timestamp;
                    }
                    num = this._videoTime;
                }
                else
                {
                    if (event2 is Invoke)
                    {
                        if (event2.Header.IsTimerRelative)
                        {
                            this._dataTime += event2.Timestamp;
                        }
                        else
                        {
                            this._dataTime = event2.Timestamp;
                        }
                        return;
                    }
                    if (event2 is Notify)
                    {
                        if (event2.Header.IsTimerRelative)
                        {
                            this._dataTime += event2.Timestamp;
                        }
                        else
                        {
                            this._dataTime = event2.Timestamp;
                        }
                        num = this._dataTime;
                    }
                }
                if ((event2 is IStreamData) && ((event2 as IStreamData).Data != null))
                {
                    this._bytesReceived += (event2 as IStreamData).Data.Limit;
                }
                this.CheckSendNotifications(evt);
                RtmpMessage message = new RtmpMessage {
                    body = event2
                };
                message.body.Timestamp = num;
                try
                {
                    if (this._livePipe != null)
                    {
                        this._livePipe.PushMessage(message);
                    }
                    if (this._recordPipe != null)
                    {
                        this._recordPipe.PushMessage(message);
                    }
                }
                catch (IOException exception)
                {
                    this.SendRecordFailedNotify(exception.Message);
                    this.Stop();
                }
            }
        }

        private void NotifyBroadcastClose()
        {
            IStreamAwareScopeHandler streamAwareHandler = base.GetStreamAwareHandler();
            if (streamAwareHandler != null)
            {
                try
                {
                    streamAwareHandler.StreamBroadcastClose(this);
                }
                catch (Exception exception)
                {
                    log.Error("Error notify streamBroadcastStop", exception);
                }
            }
        }

        private void NotifyBroadcastStart()
        {
            IStreamAwareScopeHandler streamAwareHandler = base.GetStreamAwareHandler();
            if (streamAwareHandler != null)
            {
                try
                {
                    streamAwareHandler.StreamBroadcastStart(this);
                }
                catch (Exception exception)
                {
                    log.Error("Error notify streamBroadcastStart", exception);
                }
            }
        }

        private void NotifyChunkSize()
        {
            if ((this._chunkSize > 0) && (this._livePipe != null))
            {
                OOBControlMessage oobCtrlMsg = new OOBControlMessage {
                    Target = "ConnectionConsumer",
                    ServiceName = "chunkSize"
                };
                oobCtrlMsg.ServiceParameterMap["chunkSize"] = this._chunkSize;
                this._livePipe.SendOOBControlMessage(this.Provider, oobCtrlMsg);
            }
        }

        public void OnOOBControlMessage(IMessageComponent source, IPipe pipe, OOBControlMessage oobCtrlMsg)
        {
            if ("ClientBroadcastStream".Equals(oobCtrlMsg.Target) && "chunkSize".Equals(oobCtrlMsg.ServiceName))
            {
                this._chunkSize = (int) oobCtrlMsg.ServiceParameterMap["chunkSize"];
                this.NotifyChunkSize();
            }
        }

        public void OnPipeConnectionEvent(PipeConnectionEvent evt)
        {
            switch (evt.Type)
            {
                case 1:
                    if (((evt.Provider == this) && (evt.Source != this._connMsgOut)) && ((evt.ParameterMap == null) || !evt.ParameterMap.ContainsKey("record")))
                    {
                        this._livePipe = evt.Source as IPipe;
                        foreach (IConsumer consumer in this._livePipe.GetConsumers())
                        {
                            this._subscriberStats.Increment();
                        }
                    }
                    break;

                case 2:
                    if (this._livePipe == evt.Source)
                    {
                        this._livePipe = null;
                    }
                    break;

                case 4:
                    if (this._livePipe == evt.Source)
                    {
                        this.NotifyChunkSize();
                    }
                    this._subscriberStats.Increment();
                    break;

                case 5:
                    this._subscriberStats.Decrement();
                    break;
            }
        }

        public void PushMessage(IPipe pipe, IMessage message)
        {
        }

        public void SaveAs(string name, bool isAppend)
        {
            FileInfo file;
            if (log.get_IsDebugEnabled())
            {
                log.Debug(string.Concat(new object[] { "SaveAs - name: ", name, " append: ", isAppend }));
            }
            IStreamCapableConnection connection = base.Connection;
            if (connection == null)
            {
                throw new IOException("Stream is no longer connected");
            }
            IScope scope = connection.Scope;
            IStreamFilenameGenerator scopeService = ScopeUtils.GetScopeService(scope, typeof(IStreamFilenameGenerator)) as IStreamFilenameGenerator;
            string fileName = scopeService.GenerateFilename(scope, name, ".flv", GenerationType.RECORD);
            if (scopeService.ResolvesToAbsolutePath)
            {
                file = new FileInfo(fileName);
            }
            else
            {
                file = scope.Context.GetResource(fileName).File;
            }
            if (!isAppend)
            {
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            else if (!file.Exists)
            {
                isAppend = false;
            }
            file = new FileInfo(file.FullName);
            if (!file.Exists)
            {
                string directoryName = Path.GetDirectoryName(file.FullName);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
            }
            if (!file.Exists)
            {
                using (file.Create())
                {
                }
            }
            if (log.get_IsDebugEnabled())
            {
                log.Debug("Recording file: " + file.FullName);
            }
            this._recordingFile = new FileConsumer(scope, file);
            Dictionary<string, object> parameterMap = new Dictionary<string, object>();
            if (isAppend)
            {
                parameterMap.Add("mode", "append");
            }
            else
            {
                parameterMap.Add("mode", "record");
            }
            this._recordPipe.Subscribe(this._recordingFile, parameterMap);
            this._recording = true;
            this._recordingFilename = fileName;
        }

        private void SendPublishStartNotify()
        {
            StatusASO saso = new StatusASO("NetStream.Publish.Start") {
                clientid = base.StreamId,
                details = this.PublishedName
            };
            StatusMessage message = new StatusMessage {
                body = saso
            };
            try
            {
                this._connMsgOut.PushMessage(message);
            }
            catch (IOException exception)
            {
                log.Error("Error while pushing message.", exception);
            }
        }

        private void SendPublishStopNotify()
        {
            StatusASO saso = new StatusASO("NetStream.Unpublish.Success") {
                clientid = base.StreamId,
                details = this.PublishedName
            };
            StatusMessage message = new StatusMessage {
                body = saso
            };
            try
            {
                this._connMsgOut.PushMessage(message);
            }
            catch (IOException exception)
            {
                log.Error("Error while pushing message.", exception);
            }
        }

        private void SendRecordFailedNotify(string reason)
        {
            StatusASO saso = new StatusASO("NetStream.Record.Failed") {
                level = "error",
                clientid = base.StreamId,
                details = this.PublishedName,
                description = reason
            };
            StatusMessage message = new StatusMessage {
                body = saso
            };
            try
            {
                this._connMsgOut.PushMessage(message);
            }
            catch (IOException exception)
            {
                log.Error("Error while pushing message.", exception);
            }
        }

        private void SendRecordStartNotify()
        {
            StatusASO saso = new StatusASO("NetStream.Record.Start") {
                clientid = base.StreamId,
                details = this.PublishedName
            };
            StatusMessage message = new StatusMessage {
                body = saso
            };
            try
            {
                this._connMsgOut.PushMessage(message);
            }
            catch (IOException exception)
            {
                log.Error("Error while pushing message.", exception);
            }
        }

        private void SendRecordStopNotify()
        {
            StatusASO saso = new StatusASO("NetStream.Record.Stop") {
                clientid = base.StreamId,
                details = this.PublishedName
            };
            StatusMessage message = new StatusMessage {
                body = saso
            };
            try
            {
                this._connMsgOut.PushMessage(message);
            }
            catch (IOException exception)
            {
                log.Error("Error while pushing message.", exception);
            }
        }

        private void SendStartNotifications(IEventListener source)
        {
            if (this._sendStartNotification)
            {
                this._sendStartNotification = false;
                if (source is IConnection)
                {
                    IScope scope = (source as IConnection).Scope;
                    if (scope.HasHandler)
                    {
                        object handler = scope.Handler;
                        if (handler is IStreamAwareScopeHandler)
                        {
                            if (this._recording)
                            {
                                (handler as IStreamAwareScopeHandler).StreamRecordStart(this);
                            }
                            else
                            {
                                (handler as IStreamAwareScopeHandler).StreamPublishStart(this);
                            }
                        }
                    }
                }
                this.SendPublishStartNotify();
                if (this._recording)
                {
                    this.SendRecordStartNotify();
                }
                this.NotifyBroadcastStart();
            }
        }

        public override void Start()
        {
            lock (base.SyncRoot)
            {
                IConsumerService service = base.Scope.GetService(typeof(IConsumerService)) as IConsumerService;
                try
                {
                    this._videoCodecFactory = new VideoCodecFactory();
                    this._checkVideoCodec = true;
                }
                catch (Exception exception)
                {
                    log.Warn("No video codec factory available.", exception);
                }
                this._firstPacketTime = this._audioTime = this._videoTime = this._dataTime = -1;
                this._connMsgOut = service.GetConsumerOutput(this);
                this._connMsgOut.Subscribe(this, null);
                this._recordPipe = new InMemoryPushPushPipe();
                Dictionary<string, object> parameterMap = new Dictionary<string, object>();
                parameterMap.Add("record", null);
                this._recordPipe.Subscribe(this, parameterMap);
                this._recording = false;
                this._recordingFilename = null;
                base.CodecInfo = new StreamCodecInfo();
                this._closed = false;
                this._bytesReceived = 0L;
                this._creationTime = Environment.TickCount;
            }
        }

        public void StartPublishing()
        {
            this.SendStartNotifications(FluorineContext.Current.Connection);
        }

        public override void Stop()
        {
            lock (base.SyncRoot)
            {
                this.StopRecording();
                this.Close();
            }
        }

        public void StopRecording()
        {
            if (this._recording)
            {
                this._recording = false;
                this._recordingFilename = null;
                this._recordPipe.Unsubscribe(this._recordingFile);
                this.SendRecordStopNotify();
            }
        }

        public int ActiveSubscribers
        {
            get
            {
                return this._subscriberStats.Current;
            }
        }

        public long BytesReceived
        {
            get
            {
                return this._bytesReceived;
            }
        }

        public long CreationTime
        {
            get
            {
                return this._creationTime;
            }
        }

        public int CurrentTimestamp
        {
            get
            {
                return Math.Max(Math.Max(this._videoTime, this._audioTime), this._dataTime);
            }
        }

        public int MaxSubscribers
        {
            get
            {
                return this._subscriberStats.Max;
            }
        }

        public IProvider Provider
        {
            get
            {
                return this;
            }
        }

        public string PublishedName
        {
            get
            {
                return this._publishedName;
            }
            set
            {
                if (log.get_IsDebugEnabled())
                {
                    log.Debug("setPublishedName: " + value);
                }
                this._publishedName = value;
            }
        }

        public string SaveFilename
        {
            get
            {
                return this._recordingFilename;
            }
        }

        public IClientBroadcastStreamStatistics Statistics
        {
            get
            {
                return this;
            }
        }

        public int TotalSubscribers
        {
            get
            {
                return this._subscriberStats.Total;
            }
        }
    }
}

