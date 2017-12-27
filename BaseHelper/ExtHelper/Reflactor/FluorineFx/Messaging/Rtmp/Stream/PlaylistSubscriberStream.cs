namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx;
    using FluorineFx.Exceptions;
    using FluorineFx.IO;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Messaging;
    using FluorineFx.Messaging.Api.Statistics;
    using FluorineFx.Messaging.Api.Stream;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmp;
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Messaging.Rtmp.Messaging;
    using FluorineFx.Messaging.Rtmp.Stream.Messages;
    using FluorineFx.Scheduling;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;
    using System.Timers;

    internal class PlaylistSubscriberStream : AbstractClientStream, IPlaylistSubscriberStream, ISubscriberStream, IClientStream, IStream, IBWControllable, IPlaylist, IPlaylistSubscriberStreamStatistics, IStreamStatistics, IStatisticsBase
    {
        private int _bufferCheckInterval = 0;
        private IBWControlContext _bwContext;
        private IBWControlService _bwController;
        private long _bytesSent = 0L;
        private IPlaylistController _controller;
        private long _creationTime;
        private int _currentItemIndex;
        private IPlaylistController _defaultController = new SimplePlaylistController();
        private PlayEngine _engine;
        private bool _isRandom;
        private bool _isRepeat;
        private bool _isRewind;
        private IList _items = new ArrayList();
        private bool _receiveAudio = true;
        private bool _receiveVideo = true;
        private int _underrunTrigger = 10;
        private static ILog log = LogManager.GetLogger(typeof(PlaylistSubscriberStream));

        public PlaylistSubscriberStream()
        {
            this._engine = new PlayEngine(this);
            this._currentItemIndex = 0;
            this._creationTime = Environment.TickCount;
        }

        public void AddItem(IPlayItem item)
        {
            lock (base.SyncRoot)
            {
                this._items.Add(item);
            }
        }

        public void AddItem(IPlayItem item, int index)
        {
            lock (base.SyncRoot)
            {
                this._items.Insert(index, item);
            }
        }

        public override void Close()
        {
            this._engine.Close();
            this._bwController.UnregisterBWControllable(this._bwContext);
            this.NotifySubscriberClose();
        }

        public IPlayItem GetItem(int index)
        {
            try
            {
                return (this._items[index] as IPlayItem);
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        private void MoveToNext()
        {
            if (this._controller != null)
            {
                this._currentItemIndex = this._controller.NextItem(this, this._currentItemIndex);
            }
            else
            {
                this._currentItemIndex = this._defaultController.NextItem(this, this._currentItemIndex);
            }
        }

        private void MoveToPrevious()
        {
            if (this._controller != null)
            {
                this._currentItemIndex = this._controller.PreviousItem(this, this._currentItemIndex);
            }
            else
            {
                this._currentItemIndex = this._defaultController.PreviousItem(this, this._currentItemIndex);
            }
        }

        public void NextItem()
        {
            lock (base.SyncRoot)
            {
                this.MoveToNext();
                if (this._currentItemIndex != -1)
                {
                    IPlayItem item = this._items[this._currentItemIndex] as IPlayItem;
                    int count = this._items.Count;
                    while (count-- > 0)
                    {
                        try
                        {
                            this._engine.Play(item, false);
                            break;
                        }
                        catch (IOException exception)
                        {
                            log.Error("Error while starting to play item, moving to next.", exception);
                            this.MoveToNext();
                            if (this._currentItemIndex == -1)
                            {
                                break;
                            }
                            item = this._items[this._currentItemIndex] as IPlayItem;
                        }
                        catch (StreamNotFoundException)
                        {
                            this.MoveToNext();
                            if (this._currentItemIndex == -1)
                            {
                                break;
                            }
                            item = this._items[this._currentItemIndex] as IPlayItem;
                        }
                        catch (NotSupportedException)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void NotifyItemPause(IPlayItem item, int position)
        {
            IStreamAwareScopeHandler streamAwareHandler = base.GetStreamAwareHandler();
            if (streamAwareHandler != null)
            {
                try
                {
                    streamAwareHandler.StreamPlaylistVODItemPause(this, item, position);
                }
                catch (Exception exception)
                {
                    log.Error("Error notify streamPlaylistVODItemPause", exception);
                }
            }
        }

        private void NotifyItemPlay(IPlayItem item, bool isLive)
        {
            IStreamAwareScopeHandler streamAwareHandler = base.GetStreamAwareHandler();
            if (streamAwareHandler != null)
            {
                try
                {
                    streamAwareHandler.StreamPlaylistItemPlay(this, item, isLive);
                }
                catch (Exception exception)
                {
                    log.Error("Error notify streamPlaylistItemPlay", exception);
                }
            }
        }

        private void NotifyItemResume(IPlayItem item, int position)
        {
            IStreamAwareScopeHandler streamAwareHandler = base.GetStreamAwareHandler();
            if (streamAwareHandler != null)
            {
                try
                {
                    streamAwareHandler.StreamPlaylistVODItemResume(this, item, position);
                }
                catch (Exception exception)
                {
                    log.Error("Error notify streamPlaylistVODItemResume", exception);
                }
            }
        }

        private void NotifyItemSeek(IPlayItem item, int position)
        {
            IStreamAwareScopeHandler streamAwareHandler = base.GetStreamAwareHandler();
            if (streamAwareHandler != null)
            {
                try
                {
                    streamAwareHandler.StreamPlaylistVODItemSeek(this, item, position);
                }
                catch (Exception exception)
                {
                    log.Error("Error notify streamPlaylistVODItemSeek", exception);
                }
            }
        }

        private void NotifyItemStop(IPlayItem item)
        {
            IStreamAwareScopeHandler streamAwareHandler = base.GetStreamAwareHandler();
            if (streamAwareHandler != null)
            {
                try
                {
                    streamAwareHandler.StreamPlaylistItemStop(this, item);
                }
                catch (Exception exception)
                {
                    log.Error("Error notify streamPlaylistItemStop", exception);
                }
            }
        }

        private void NotifySubscriberClose()
        {
            IStreamAwareScopeHandler streamAwareHandler = base.GetStreamAwareHandler();
            if (streamAwareHandler != null)
            {
                try
                {
                    streamAwareHandler.StreamSubscriberClose(this);
                }
                catch (Exception exception)
                {
                    log.Error("Error notify streamSubscriberClose", exception);
                }
            }
        }

        private void NotifySubscriberStart()
        {
            IStreamAwareScopeHandler streamAwareHandler = base.GetStreamAwareHandler();
            if (streamAwareHandler != null)
            {
                try
                {
                    streamAwareHandler.StreamSubscriberStart(this);
                }
                catch (Exception exception)
                {
                    log.Error("Error notify streamSubscriberStart", exception);
                }
            }
        }

        private void OnItemEnd()
        {
            this.NextItem();
        }

        public void Pause(int position)
        {
            try
            {
                this._engine.Pause(position);
            }
            catch (NotSupportedException)
            {
                log.Debug("Pause caught an NotSupportedException");
            }
        }

        public void Play()
        {
            lock (base.SyncRoot)
            {
                if (this._items.Count != 0)
                {
                    if (this._currentItemIndex == -1)
                    {
                        this.MoveToNext();
                    }
                    IPlayItem item = this._items[this._currentItemIndex] as IPlayItem;
                    int count = this._items.Count;
                    while (count-- > 0)
                    {
                        try
                        {
                            this._engine.Play(item);
                            break;
                        }
                        catch (StreamNotFoundException)
                        {
                            this.MoveToNext();
                            if (this._currentItemIndex == -1)
                            {
                                break;
                            }
                            item = this._items[this._currentItemIndex] as IPlayItem;
                        }
                        catch (IllegalStateException)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void PreviousItem()
        {
            lock (base.SyncRoot)
            {
                this.Stop();
                this.MoveToPrevious();
                if (this._currentItemIndex != -1)
                {
                    IPlayItem item = this._items[this._currentItemIndex] as IPlayItem;
                    int count = this._items.Count;
                    while (count-- > 0)
                    {
                        try
                        {
                            this._engine.Play(item);
                            break;
                        }
                        catch (IOException exception)
                        {
                            log.Error("Error while starting to play item, moving to next.", exception);
                            this.MoveToPrevious();
                            if (this._currentItemIndex == -1)
                            {
                                break;
                            }
                            item = this._items[this._currentItemIndex] as IPlayItem;
                        }
                        catch (StreamNotFoundException)
                        {
                            this.MoveToPrevious();
                            if (this._currentItemIndex == -1)
                            {
                                break;
                            }
                            item = this._items[this._currentItemIndex] as IPlayItem;
                        }
                        catch (NotSupportedException)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void ReceiveAudio(bool receive)
        {
            if (!(!this._receiveAudio || receive))
            {
                this._engine.SendBlankAudio = true;
            }
            bool flag = !this._receiveAudio && receive;
            this._receiveAudio = receive;
            if (flag)
            {
                this.SeekToCurrentPlayback();
            }
        }

        public void ReceiveVideo(bool receive)
        {
            bool flag = !this._receiveVideo && receive;
            this._receiveVideo = receive;
            if (flag)
            {
                this.SeekToCurrentPlayback();
            }
        }

        public void RemoveAllItems()
        {
            lock (base.SyncRoot)
            {
                this.Stop();
                this._items.Clear();
            }
        }

        public void RemoveItem(int index)
        {
            lock (base.SyncRoot)
            {
                if ((index >= 0) && (index < this._items.Count))
                {
                    int count = this._items.Count;
                    this._items.RemoveAt(index);
                    if ((this._currentItemIndex == index) && (index == (count - 1)))
                    {
                        this._currentItemIndex = index - 1;
                    }
                }
            }
        }

        public void Resume(int position)
        {
            try
            {
                this._engine.Resume(position);
            }
            catch (NotSupportedException)
            {
                log.Debug("Resume caught an NotSupportedException");
            }
        }

        public void Seek(int position)
        {
            try
            {
                this._engine.Seek(position);
            }
            catch (NotSupportedException)
            {
                log.Debug("Seek caught an NotSupportedException");
            }
        }

        private void SeekToCurrentPlayback()
        {
            if (this._engine.IsPullMode)
            {
                try
                {
                    long num = Environment.TickCount - this._engine.PlaybackStart;
                    this._engine.Seek((int) num);
                }
                catch (NotSupportedException)
                {
                }
            }
        }

        public void SetItem(int index)
        {
            lock (base.SyncRoot)
            {
                if ((index >= 0) && (index < this._items.Count))
                {
                    this.Stop();
                    this._currentItemIndex = index;
                    IPlayItem item = this._items[this._currentItemIndex] as IPlayItem;
                    try
                    {
                        this._engine.Play(item);
                    }
                    catch (IOException exception)
                    {
                        log.Error("SetItem caught a IOException", exception);
                    }
                    catch (StreamNotFoundException)
                    {
                        log.Debug("SetItem caught a StreamNotFoundException");
                    }
                    catch (NotSupportedException exception2)
                    {
                        log.Error("Illegal state exception on playlist item setup", exception2);
                    }
                }
            }
        }

        public void SetPlaylistController(IPlaylistController controller)
        {
            this._controller = controller;
        }

        public override void Start()
        {
            this._bwController = base.Scope.GetService(typeof(IBWControlService)) as IBWControlService;
            this._bwContext = this._bwController.RegisterBWControllable(this);
            this._engine.Start();
            this.NotifySubscriberStart();
        }

        public override void Stop()
        {
            try
            {
                this._engine.Stop();
            }
            catch (IllegalStateException)
            {
                log.Debug("Stop caught an IllegalStateException");
            }
        }

        public void Written(object message)
        {
            if (this._engine.IsPullMode)
            {
                try
                {
                    this._engine.PullAndPush();
                }
                catch (Exception exception)
                {
                    log.Error("Error while pulling message.", exception);
                }
            }
        }

        public override IBandwidthConfigure BandwidthConfiguration
        {
            get
            {
                return base.BandwidthConfiguration;
            }
            set
            {
                base.BandwidthConfiguration = value;
                this._engine.UpdateBandwithConfigure();
            }
        }

        public int BufferCheckInterval
        {
            get
            {
                return this._bufferCheckInterval;
            }
            set
            {
                this._bufferCheckInterval = value;
            }
        }

        public long BytesSent
        {
            get
            {
                return this._bytesSent;
            }
        }

        public int Count
        {
            get
            {
                return this._items.Count;
            }
        }

        public long CreationTime
        {
            get
            {
                return this._creationTime;
            }
        }

        public IPlayItem CurrentItem
        {
            get
            {
                return this.GetItem(this.CurrentItemIndex);
            }
        }

        public int CurrentItemIndex
        {
            get
            {
                return this._currentItemIndex;
            }
        }

        public int CurrentTimestamp
        {
            get
            {
                IRtmpEvent lastMessage = this._engine.LastMessage;
                if (lastMessage == null)
                {
                    return 0;
                }
                return lastMessage.Timestamp;
            }
        }

        public double EstimatedBufferFill
        {
            get
            {
                IRtmpEvent lastMessage = this._engine.LastMessage;
                if (lastMessage == null)
                {
                    return 0.0;
                }
                long clientBufferDuration = base.ClientBufferDuration;
                if (clientBufferDuration == 0L)
                {
                    return 100.0;
                }
                long num2 = Environment.TickCount - this._engine.PlaybackStart;
                long num3 = lastMessage.Timestamp - num2;
                return ((num3 * 100.0) / ((double) clientBufferDuration));
            }
        }

        public bool HasMoreItems
        {
            get
            {
                lock (base.SyncRoot)
                {
                    int num = this._currentItemIndex + 1;
                    if (!((num < this._items.Count) || this.IsRepeat))
                    {
                        return false;
                    }
                    return true;
                }
            }
        }

        public bool IsPaused
        {
            get
            {
                return (this._engine.State == PlaylistState.PAUSED);
            }
        }

        public bool IsRandom
        {
            get
            {
                return this._isRandom;
            }
            set
            {
                this._isRandom = value;
            }
        }

        public bool IsRepeat
        {
            get
            {
                return this._isRepeat;
            }
            set
            {
                this._isRepeat = value;
            }
        }

        public bool IsRewind
        {
            get
            {
                return this._isRewind;
            }
            set
            {
                this._isRewind = value;
            }
        }

        public int ItemSize
        {
            get
            {
                return this._items.Count;
            }
        }

        public IPlaylistSubscriberStreamStatistics Statistics
        {
            get
            {
                return this;
            }
        }

        public int UnderrunTrigger
        {
            get
            {
                return this._underrunTrigger;
            }
            set
            {
                this._underrunTrigger = value;
            }
        }

        private class PlayEngine : IFilter, IProvider, IPushableConsumer, IConsumer, IMessageComponent, IPipeConnectionListener, ITokenBucketCallback
        {
            private ITokenBucket _audioBucket;
            private long _bytesSent = 0L;
            private IPlayItem _currentItem;
            private bool _isPullMode;
            private bool _isWaitingForToken = false;
            private IRtmpEvent _lastMessage;
            private IMessageInput _msgIn;
            private IMessageOutput _msgOut;
            private bool _needCheckBandwidth = true;
            private long _nextCheckBufferUnderrun;
            private RtmpMessage _pendingMessage;
            private long _playbackStart;
            private Timer _pullAndPushTimer;
            private ISchedulingService _schedulingService;
            private bool _sendBlankAudio;
            private PlaylistState _state;
            private PlaylistSubscriberStream _stream;
            private int _streamOffset;
            private object _syncLock = new object();
            private int _timestampOffset = 0;
            private ITokenBucket _videoBucket;
            private IFrameDropper _videoFrameDropper = new VideoFrameDropper();
            private int _vodStartTS;
            private string _waitLiveJob;

            public PlayEngine(PlaylistSubscriberStream stream)
            {
                this._stream = stream;
                this._state = PlaylistState.UNINIT;
            }

            public void Available(ITokenBucket bucket, long tokenCount)
            {
                lock (this.SyncRoot)
                {
                    this._isWaitingForToken = false;
                    this._needCheckBandwidth = false;
                    try
                    {
                        this.PullAndPush();
                    }
                    catch (Exception exception)
                    {
                        PlaylistSubscriberStream.log.Error("Error while pulling message.", exception);
                    }
                    this._needCheckBandwidth = true;
                }
            }

            private void ClearWaitJobs()
            {
                lock (this.SyncRoot)
                {
                    if (this._pullAndPushTimer != null)
                    {
                        this._pullAndPushTimer.Enabled = false;
                        this._pullAndPushTimer.Elapsed -= new ElapsedEventHandler(this.PullAndPushTimer_Elapsed);
                        this._pullAndPushTimer = null;
                    }
                    if (this._waitLiveJob != null)
                    {
                        this._schedulingService.RemoveScheduledJob(this._waitLiveJob);
                        this._waitLiveJob = null;
                    }
                }
            }

            public void Close()
            {
                lock (this.SyncRoot)
                {
                    if (this._msgIn != null)
                    {
                        this._msgIn.Unsubscribe(this);
                        this._msgIn = null;
                    }
                    this._state = PlaylistState.CLOSED;
                    this.ClearWaitJobs();
                    this.ReleasePendingMessage();
                    this._lastMessage = null;
                    this.SendClearPing();
                }
            }

            private void DoPushMessage(AsyncMessage message)
            {
                try
                {
                    this._msgOut.PushMessage(message);
                    if (message is RtmpMessage)
                    {
                        IRtmpEvent body = ((RtmpMessage) message).body;
                        if ((body is IStreamData) && (((IStreamData) body).Data != null))
                        {
                            this._bytesSent += ((IStreamData) body).Data.Limit;
                        }
                    }
                }
                catch (IOException exception)
                {
                    PlaylistSubscriberStream.log.Error("Error while pushing message.", exception);
                }
            }

            private void EnsurePullAndPushRunning()
            {
                if (this._isPullMode && (this._pullAndPushTimer == null))
                {
                    lock (this.SyncRoot)
                    {
                        if (this._pullAndPushTimer == null)
                        {
                            this._pullAndPushTimer = new Timer();
                            this._pullAndPushTimer.Elapsed += new ElapsedEventHandler(this.PullAndPushTimer_Elapsed);
                            this._pullAndPushTimer.Interval = 10.0;
                            this._pullAndPushTimer.AutoReset = true;
                            this._pullAndPushTimer.Enabled = true;
                        }
                    }
                }
            }

            private long GetPendingMessagesCount()
            {
                return this._stream.Connection.PendingMessages;
            }

            private long GetPendingVideoMessageCount()
            {
                OOBControlMessage oobCtrlMsg = new OOBControlMessage {
                    Target = "ConnectionConsumer",
                    ServiceName = "pendingVideoCount"
                };
                this._msgOut.SendOOBControlMessage(this, oobCtrlMsg);
                if (oobCtrlMsg.Result != null)
                {
                    return (long) oobCtrlMsg.Result;
                }
                return 0L;
            }

            private long[] GetWriteDelta()
            {
                OOBControlMessage oobCtrlMsg = new OOBControlMessage {
                    Target = "ConnectionConsumer",
                    ServiceName = "writeDelta"
                };
                this._msgOut.SendOOBControlMessage(this, oobCtrlMsg);
                if (oobCtrlMsg.Result != null)
                {
                    return (oobCtrlMsg.Result as long[]);
                }
                return new long[2];
            }

            private bool OkayToSendMessage(IRtmpEvent message)
            {
                if (!(message is IStreamData))
                {
                    throw new ApplicationException("Expected IStreamData but got " + message.GetType().ToString());
                }
                long tickCount = Environment.TickCount;
                if (this._lastMessage != null)
                {
                    long num2 = tickCount - this._playbackStart;
                    long clientBufferDuration = this._stream.ClientBufferDuration;
                    long num4 = this._lastMessage.Timestamp - num2;
                    if (PlaylistSubscriberStream.log.get_IsDebugEnabled())
                    {
                        PlaylistSubscriberStream.log.Debug(string.Concat(new object[] { "OkayToSendMessage: ", this._lastMessage.Timestamp, " ", num2, " ", num4, " ", clientBufferDuration }));
                    }
                    if ((clientBufferDuration > 0L) && (num4 > clientBufferDuration))
                    {
                        return false;
                    }
                }
                long pendingMessagesCount = this.GetPendingMessagesCount();
                if ((this._stream._bufferCheckInterval > 0) && (tickCount >= this._nextCheckBufferUnderrun))
                {
                    if (pendingMessagesCount > this._stream._underrunTrigger)
                    {
                        this.SendInsufficientBandwidthStatus(this._currentItem);
                    }
                    this._nextCheckBufferUnderrun = tickCount + this._stream._bufferCheckInterval;
                }
                if (pendingMessagesCount > this._stream._underrunTrigger)
                {
                    return false;
                }
                if (((IStreamData) message).Data != null)
                {
                    int limit = ((IStreamData) message).Data.Limit;
                    if (message is VideoData)
                    {
                        if (!(!this._needCheckBandwidth || this._videoBucket.AcquireTokenNonblocking((long) limit, this)))
                        {
                            this._isWaitingForToken = true;
                            return false;
                        }
                    }
                    else if ((message is AudioData) && !(!this._needCheckBandwidth || this._audioBucket.AcquireTokenNonblocking((long) limit, this)))
                    {
                        this._isWaitingForToken = true;
                        return false;
                    }
                }
                return true;
            }

            public void OnOOBControlMessage(IMessageComponent source, IPipe pipe, OOBControlMessage oobCtrlMsg)
            {
                if ("ConnectionConsumer".Equals(oobCtrlMsg.Target) && (source is IProvider))
                {
                    this._msgOut.SendOOBControlMessage((IProvider) source, oobCtrlMsg);
                }
            }

            public void OnPipeConnectionEvent(PipeConnectionEvent evt)
            {
                switch (evt.Type)
                {
                    case 1:
                        if (evt.Provider != this)
                        {
                            if (this._waitLiveJob != null)
                            {
                                this._schedulingService.RemoveScheduledJob(this._waitLiveJob);
                                this._waitLiveJob = null;
                            }
                            this.SendPublishedStatus(this._currentItem);
                        }
                        break;

                    case 2:
                        if (!this._isPullMode)
                        {
                            this.SendUnpublishedStatus(this._currentItem);
                            break;
                        }
                        this.SendStopStatus(this._currentItem);
                        break;

                    case 3:
                        if (evt.Consumer == this)
                        {
                            this._isPullMode = true;
                        }
                        break;

                    case 4:
                        if (evt.Consumer == this)
                        {
                            this._isPullMode = false;
                        }
                        break;
                }
            }

            public void Pause(int position)
            {
                lock (this.SyncRoot)
                {
                    if (((this._state != PlaylistState.PLAYING) && (this._state != PlaylistState.STOPPED)) || (this._currentItem == null))
                    {
                        throw new IllegalStateException();
                    }
                    this._state = PlaylistState.PAUSED;
                    this.ReleasePendingMessage();
                    this.ClearWaitJobs();
                    this.SendClearPing();
                    this.SendPauseStatus(this._currentItem);
                    this._stream.NotifyItemPause(this._currentItem, position);
                }
            }

            public void Play(IPlayItem item)
            {
                this.Play(item, true);
            }

            public void Play(IPlayItem item, bool withReset)
            {
                lock (this.SyncRoot)
                {
                    if (this._state != PlaylistState.STOPPED)
                    {
                        throw new NotSupportedException();
                    }
                    if (this._msgIn != null)
                    {
                        this._msgIn.Unsubscribe(this);
                        this._msgIn = null;
                    }
                    int num = (int) (item.Start / 0x3e8L);
                    IScope scope = this._stream.Scope;
                    IScopeContext context = scope.Context;
                    IProviderService scopeService = ScopeUtils.GetScopeService(scope, typeof(IProviderService)) as IProviderService;
                    IMessageInput input = scopeService.GetLiveProviderInput(scope, item.Name, false);
                    IMessageInput vODProviderInput = scopeService.GetVODProviderInput(scope, item.Name);
                    bool flag = input != null;
                    bool flag2 = vODProviderInput != null;
                    bool flag3 = true;
                    int num2 = 3;
                    switch (num)
                    {
                        case -2:
                            if (!flag)
                            {
                                break;
                            }
                            num2 = 0;
                            goto Label_012A;

                        case -1:
                            if (!flag)
                            {
                                goto Label_0111;
                            }
                            num2 = 0;
                            goto Label_012A;

                        default:
                            if (flag2)
                            {
                                num2 = 1;
                            }
                            goto Label_012A;
                    }
                    if (flag2)
                    {
                        num2 = 1;
                    }
                    else
                    {
                        num2 = 2;
                    }
                    goto Label_012A;
                Label_0111:
                    num2 = 2;
                Label_012A:
                    if (num2 == 2)
                    {
                        input = scopeService.GetLiveProviderInput(scope, item.Name, true);
                    }
                    this._currentItem = item;
                    switch (num2)
                    {
                        case 0:
                            this._msgIn = input;
                            this._videoFrameDropper.Reset(FrameDropperState.SEND_KEYFRAMES_CHECK);
                            if (this._msgIn is IBroadcastScope)
                            {
                                IClientBroadcastStream attribute = (this._msgIn as IBroadcastScope).GetAttribute("_transient_publishing_stream") as IClientBroadcastStream;
                                if ((attribute != null) && (attribute.CodecInfo != null))
                                {
                                    IVideoStreamCodec videoCodec = attribute.CodecInfo.VideoCodec;
                                    if (videoCodec != null)
                                    {
                                        ByteBuffer keyframe = videoCodec.GetKeyframe();
                                        if (keyframe != null)
                                        {
                                            VideoData data = new VideoData(keyframe);
                                            try
                                            {
                                                if (withReset)
                                                {
                                                    this.SendReset();
                                                    this.SendResetStatus(item);
                                                    this.SendStartStatus(item);
                                                }
                                                data.Timestamp = 0;
                                                RtmpMessage message = new RtmpMessage {
                                                    body = data
                                                };
                                                this._msgOut.PushMessage(message);
                                                flag3 = false;
                                                this._videoFrameDropper.Reset();
                                            }
                                            finally
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                            this._msgIn.Subscribe(this, null);
                            break;

                        case 1:
                            this._msgIn = vODProviderInput;
                            this._msgIn.Subscribe(this, null);
                            break;

                        case 2:
                            this._msgIn = input;
                            this._msgIn.Subscribe(this, null);
                            if ((num == -1) && (item.Length >= 0L))
                            {
                                PlaylistSubscriberStreamJob job = new PlaylistSubscriberStreamJob(this);
                                this._waitLiveJob = this._schedulingService.AddScheduledOnceJob(item.Length, job);
                            }
                            break;

                        default:
                            this.SendStreamNotFoundStatus(this._currentItem);
                            throw new StreamNotFoundException(item.Name);
                    }
                    this._state = PlaylistState.PLAYING;
                    IMessage message2 = null;
                    this._streamOffset = 0;
                    if (num2 == 1)
                    {
                        if (withReset)
                        {
                            this.ReleasePendingMessage();
                        }
                        this.SendVODInitCM(this._msgIn, item);
                        this._vodStartTS = -1;
                        if (item.Start > 0L)
                        {
                            this._streamOffset = this.SendVODSeekCM(this._msgIn, (int) item.Start);
                            if (this._streamOffset == -1)
                            {
                                this._streamOffset = (int) item.Start;
                            }
                        }
                        message2 = this._msgIn.PullMessage();
                        if (message2 is RtmpMessage)
                        {
                            IRtmpEvent body = ((RtmpMessage) message2).body;
                            if (item.Length == 0L)
                            {
                                body = ((RtmpMessage) message2).body;
                                while ((body != null) && !(body is VideoData))
                                {
                                    message2 = this._msgIn.PullMessage();
                                    if (message2 == null)
                                    {
                                        break;
                                    }
                                    if (message2 is RtmpMessage)
                                    {
                                        body = ((RtmpMessage) message2).body;
                                    }
                                }
                            }
                            if (body != null)
                            {
                                body.Timestamp += this._timestampOffset;
                            }
                        }
                    }
                    if (flag3)
                    {
                        if (withReset)
                        {
                            this.SendReset();
                            this.SendResetStatus(item);
                        }
                        this.SendStartStatus(item);
                        if (!withReset)
                        {
                            this.SendSwitchStatus();
                        }
                    }
                    if (message2 != null)
                    {
                        this.SendMessage((RtmpMessage) message2);
                    }
                    this._stream.NotifyItemPlay(this._currentItem, !this._isPullMode);
                    if (withReset)
                    {
                        this._playbackStart = Environment.TickCount - this._streamOffset;
                        this._nextCheckBufferUnderrun = Environment.TickCount + this._stream._bufferCheckInterval;
                        if (this._currentItem.Length != 0L)
                        {
                            this.EnsurePullAndPushRunning();
                        }
                    }
                }
            }

            internal void PullAndPush()
            {
                lock (this.SyncRoot)
                {
                    IRtmpEvent body;
                    IMessage message;
                    bool flag;
                    if (((this._state == PlaylistState.PLAYING) && this._isPullMode) && !this._isWaitingForToken)
                    {
                        if (this._pendingMessage == null)
                        {
                            goto Label_01C4;
                        }
                        body = this._pendingMessage.body;
                        if (this.OkayToSendMessage(body))
                        {
                            this.SendMessage(this._pendingMessage);
                            this.ReleasePendingMessage();
                        }
                    }
                    goto Label_01DA;
                Label_0082:
                    message = this._msgIn.PullMessage();
                    if (message == null)
                    {
                        this.Stop();
                        goto Label_01DA;
                    }
                    if (message is RtmpMessage)
                    {
                        RtmpMessage message2 = (RtmpMessage) message;
                        body = message2.body;
                        if (!this._stream._receiveAudio && (body is AudioData))
                        {
                            if (!this._sendBlankAudio)
                            {
                                goto Label_01C4;
                            }
                            this._sendBlankAudio = false;
                            body = new AudioData();
                            if (this._lastMessage != null)
                            {
                                body.Timestamp = this._lastMessage.Timestamp - this._timestampOffset;
                            }
                            else
                            {
                                body.Timestamp = -this._timestampOffset;
                            }
                            message2.body = body;
                        }
                        else if (!(this._stream._receiveVideo || !(body is VideoData)))
                        {
                            goto Label_01C4;
                        }
                        body.Timestamp += this._timestampOffset;
                        if (this.OkayToSendMessage(body))
                        {
                            this.SendMessage(message2);
                        }
                        else
                        {
                            this._pendingMessage = message2;
                        }
                        this.EnsurePullAndPushRunning();
                        goto Label_01DA;
                    }
                Label_01C4:
                    flag = true;
                    goto Label_0082;
                Label_01DA:;
                }
            }

            private void PullAndPushTimer_Elapsed(object sender, ElapsedEventArgs e)
            {
                try
                {
                    this.PullAndPush();
                }
                catch (IOException exception)
                {
                    PlaylistSubscriberStream.log.Error("Error while getting message.", exception);
                    this.Stop();
                }
            }

            public void PushMessage(IPipe pipe, IMessage message)
            {
                lock (this.SyncRoot)
                {
                    if (message is ResetMessage)
                    {
                        this.SendReset();
                    }
                    else
                    {
                        if (message is RtmpMessage)
                        {
                            RtmpMessage message2 = (RtmpMessage) message;
                            IRtmpEvent body = message2.body;
                            if (!(body is IStreamData))
                            {
                                throw new ApplicationException("expected IStreamData but got " + body.GetType().FullName);
                            }
                            int limit = ((IStreamData) body).Data.Limit;
                            if (body is VideoData)
                            {
                                IVideoStreamCodec videoCodec = null;
                                if (this._msgIn is IBroadcastScope)
                                {
                                    IClientBroadcastStream attribute = (IClientBroadcastStream) ((IBroadcastScope) this._msgIn).GetAttribute("_transient_publishing_stream");
                                    if ((attribute != null) && (attribute.CodecInfo != null))
                                    {
                                        videoCodec = attribute.CodecInfo.VideoCodec;
                                    }
                                }
                                if ((videoCodec == null) || videoCodec.CanDropFrames)
                                {
                                    if (this._state == PlaylistState.PAUSED)
                                    {
                                        this._videoFrameDropper.DropPacket(message2);
                                        goto Label_0340;
                                    }
                                    long pendingVideoMessageCount = this.GetPendingVideoMessageCount();
                                    if (!this._videoFrameDropper.CanSendPacket(message2, pendingVideoMessageCount))
                                    {
                                        goto Label_0340;
                                    }
                                    bool flag = !this._videoBucket.AcquireToken((long) limit, 0L);
                                    if (!(this._stream._receiveVideo && !flag))
                                    {
                                        this._videoFrameDropper.DropPacket(message2);
                                        goto Label_0340;
                                    }
                                    long[] writeDelta = this.GetWriteDelta();
                                    if (pendingVideoMessageCount > 1L)
                                    {
                                        long tickCount = Environment.TickCount;
                                        if ((this._stream._bufferCheckInterval > 0) && (tickCount >= this._nextCheckBufferUnderrun))
                                        {
                                            this.SendInsufficientBandwidthStatus(this._currentItem);
                                            this._nextCheckBufferUnderrun = tickCount + this._stream._bufferCheckInterval;
                                        }
                                        this._videoFrameDropper.DropPacket(message2);
                                        goto Label_0340;
                                    }
                                    this._videoFrameDropper.SendPacket(message2);
                                }
                            }
                            else if (body is AudioData)
                            {
                                if (!this._stream._receiveAudio && this._sendBlankAudio)
                                {
                                    this._sendBlankAudio = false;
                                    body = new AudioData();
                                    if (this._lastMessage != null)
                                    {
                                        body.Timestamp = this._lastMessage.Timestamp;
                                    }
                                    else
                                    {
                                        body.Timestamp = 0;
                                    }
                                    message2.body = body;
                                }
                                else if (!(((this._state != PlaylistState.PAUSED) && this._stream._receiveAudio) && this._audioBucket.AcquireToken((long) limit, 0L)))
                                {
                                    goto Label_0340;
                                }
                            }
                            if ((body is IStreamData) && (((IStreamData) body).Data != null))
                            {
                                this._bytesSent += ((IStreamData) body).Data.Limit;
                            }
                            this._lastMessage = body;
                        }
                        this._msgOut.PushMessage(message);
                    Label_0340:;
                    }
                }
            }

            private void ReleasePendingMessage()
            {
                lock (this.SyncRoot)
                {
                    if (this._pendingMessage != null)
                    {
                        IRtmpEvent body = this._pendingMessage.body;
                        if ((body is IStreamData) && (((IStreamData) body).Data != null))
                        {
                        }
                        this._pendingMessage.body = null;
                        this._pendingMessage = null;
                    }
                }
            }

            public void Reset(ITokenBucket bucket, long tokenCount)
            {
                this._isWaitingForToken = false;
            }

            public void Resume(int position)
            {
                lock (this.SyncRoot)
                {
                    if (this._state != PlaylistState.PAUSED)
                    {
                        throw new IllegalStateException();
                    }
                    this._state = PlaylistState.PLAYING;
                    this.SendReset();
                    this.SendResumeStatus(this._currentItem);
                    if (this._isPullMode)
                    {
                        this.SendVODSeekCM(this._msgIn, position);
                        this._stream.NotifyItemResume(this._currentItem, position);
                        this._playbackStart = Environment.TickCount - position;
                        if ((this._currentItem.Length >= 0L) && ((position - this._streamOffset) >= this._currentItem.Length))
                        {
                            this.Stop();
                        }
                        else
                        {
                            this.EnsurePullAndPushRunning();
                        }
                    }
                    else
                    {
                        this._stream.NotifyItemResume(this._currentItem, position);
                        this._videoFrameDropper.Reset(FrameDropperState.SEND_KEYFRAMES_CHECK);
                    }
                }
            }

            public void Seek(int position)
            {
                lock (this.SyncRoot)
                {
                    if (((this._state != PlaylistState.PLAYING) && (this._state != PlaylistState.PAUSED)) && (this._state != PlaylistState.STOPPED))
                    {
                        throw new IllegalStateException();
                    }
                    if (!this._isPullMode)
                    {
                        throw new NotSupportedException();
                    }
                    this.ReleasePendingMessage();
                    this.ClearWaitJobs();
                    this._stream._bwController.ResetBuckets(this._stream._bwContext);
                    this._isWaitingForToken = false;
                    this.SendClearPing();
                    this.SendReset();
                    this.SendSeekStatus(this._currentItem, position);
                    this.SendStartStatus(this._currentItem);
                    int num = this.SendVODSeekCM(this._msgIn, position);
                    if (num == -1)
                    {
                        num = position;
                    }
                    this._playbackStart = Environment.TickCount - num;
                    this._stream.NotifyItemSeek(this._currentItem, num);
                    bool flag = false;
                    bool flag2 = false;
                    if (((this._state == PlaylistState.PAUSED) || (this._state == PlaylistState.STOPPED)) && this.SendCheckVideoCM(this._msgIn))
                    {
                        IMessage message;
                        Exception exception;
                        try
                        {
                            message = this._msgIn.PullMessage();
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            PlaylistSubscriberStream.log.Error("Error while pulling message.", exception);
                            message = null;
                        }
                        while (message != null)
                        {
                            if (message is RtmpMessage)
                            {
                                RtmpMessage message2 = (RtmpMessage) message;
                                IRtmpEvent body = message2.body;
                                if ((body is VideoData) && (((VideoData) body).FrameType == FrameType.KEYFRAME))
                                {
                                    body.Timestamp = num;
                                    this.DoPushMessage(message2);
                                    flag = true;
                                    this._lastMessage = body;
                                    break;
                                }
                            }
                            try
                            {
                                message = this._msgIn.PullMessage();
                            }
                            catch (Exception exception2)
                            {
                                exception = exception2;
                                PlaylistSubscriberStream.log.Error("Error while pulling message.", exception);
                                message = null;
                            }
                        }
                    }
                    else
                    {
                        flag2 = true;
                    }
                    if (!flag)
                    {
                        AudioData data = new AudioData {
                            Timestamp = num,
                            Header = new RtmpHeader()
                        };
                        data.Header.Timer = num;
                        data.Header.IsTimerRelative = false;
                        RtmpMessage message3 = new RtmpMessage {
                            body = data
                        };
                        this._lastMessage = data;
                        this.DoPushMessage(message3);
                    }
                    if (flag2)
                    {
                        this.EnsurePullAndPushRunning();
                    }
                    if (((this._state != PlaylistState.STOPPED) && (this._currentItem.Length >= 0L)) && ((position - this._streamOffset) >= this._currentItem.Length))
                    {
                        this.Stop();
                    }
                }
            }

            private bool SendCheckVideoCM(IMessageInput msgIn)
            {
                OOBControlMessage oobCtrlMsg = new OOBControlMessage {
                    Target = typeof(IStreamTypeAwareProvider).Name,
                    ServiceName = "hasVideo"
                };
                msgIn.SendOOBControlMessage(this, oobCtrlMsg);
                return ((oobCtrlMsg.Result is bool) && ((bool) oobCtrlMsg.Result));
            }

            private void SendClearPing()
            {
                Ping ping = new Ping {
                    Value1 = 1,
                    Value2 = this.StreamId
                };
                RtmpMessage message = new RtmpMessage {
                    body = ping
                };
                this.DoPushMessage(message);
            }

            private void SendCompleteStatus()
            {
                int duration = 1;
                this.SendOnPlayStatus("NetStream.Play.Complete", duration, this._bytesSent);
            }

            private void SendInsufficientBandwidthStatus(IPlayItem item)
            {
                StatusASO saso = new StatusASO("NetStream.Play.InsufficientBW") {
                    clientid = this.StreamId,
                    level = "warning",
                    details = item.Name,
                    description = "Data is playing behind the normal speed."
                };
                StatusMessage message = new StatusMessage {
                    body = saso
                };
                this.DoPushMessage(message);
            }

            private void SendMessage(RtmpMessage message)
            {
                if (this._vodStartTS == -1)
                {
                    this._vodStartTS = message.body.Timestamp;
                }
                else if (this._currentItem.Length >= 0L)
                {
                    int num = message.body.Timestamp - this._vodStartTS;
                    if ((num - this._streamOffset) >= this._currentItem.Length)
                    {
                        this.Stop();
                        return;
                    }
                }
                this._lastMessage = message.body;
                if (this._lastMessage is IStreamData)
                {
                    this._bytesSent += ((IStreamData) this._lastMessage).Data.Limit;
                }
                this.DoPushMessage(message);
            }

            private void SendOnPlayStatus(string code, int duration, long bytes)
            {
                MemoryStream stream = new MemoryStream();
                AMFWriter writer = new AMFWriter(stream);
                writer.WriteString("onPlayStatus");
                Hashtable hashtable = new Hashtable();
                hashtable.Add("code", code);
                hashtable.Add("level", "status");
                hashtable.Add("duration", duration);
                hashtable.Add("bytes", bytes);
                writer.WriteAssociativeArray(ObjectEncoding.AMF0, hashtable);
                ByteBuffer data = new ByteBuffer(stream);
                IRtmpEvent event2 = new Notify(data);
                if (this._lastMessage != null)
                {
                    int timestamp = this._lastMessage.Timestamp;
                    event2.Timestamp = timestamp;
                }
                else
                {
                    event2.Timestamp = 0;
                }
                RtmpMessage message = new RtmpMessage {
                    body = event2
                };
                this.DoPushMessage(message);
            }

            private void SendPauseStatus(IPlayItem item)
            {
                StatusASO saso = new StatusASO("NetStream.Pause.Notify") {
                    clientid = this.StreamId,
                    details = item.Name
                };
                StatusMessage message = new StatusMessage {
                    body = saso
                };
                this.DoPushMessage(message);
            }

            private void SendPublishedStatus(IPlayItem item)
            {
                StatusASO saso = new StatusASO("NetStream.Play.PublishNotify") {
                    clientid = this.StreamId,
                    details = item.Name
                };
                StatusMessage message = new StatusMessage {
                    body = saso
                };
                this.DoPushMessage(message);
            }

            private void SendReset()
            {
                if (this._isPullMode)
                {
                    Ping ping = new Ping {
                        Value1 = 4,
                        Value2 = this.StreamId
                    };
                    RtmpMessage message = new RtmpMessage {
                        body = ping
                    };
                    this.DoPushMessage(message);
                }
                Ping ping2 = new Ping {
                    Value1 = 0,
                    Value2 = this.StreamId
                };
                RtmpMessage message2 = new RtmpMessage {
                    body = ping2
                };
                this.DoPushMessage(message2);
                ResetMessage message3 = new ResetMessage();
                this.DoPushMessage(message3);
            }

            private void SendResetStatus(IPlayItem item)
            {
                StatusASO saso = new StatusASO("NetStream.Play.Reset") {
                    clientid = this.StreamId,
                    details = item.Name,
                    description = "Playing and resetting " + item.Name + '.'
                };
                StatusMessage message = new StatusMessage {
                    body = saso
                };
                this.DoPushMessage(message);
            }

            private void SendResumeStatus(IPlayItem item)
            {
                StatusASO saso = new StatusASO("NetStream.Unpause.Notify") {
                    clientid = this.StreamId,
                    details = item.Name
                };
                StatusMessage message = new StatusMessage {
                    body = saso
                };
                this.DoPushMessage(message);
            }

            private void SendSeekStatus(IPlayItem item, int position)
            {
                StatusASO saso = new StatusASO("NetStream.Seek.Notify") {
                    clientid = this.StreamId,
                    details = item.Name,
                    description = string.Concat(new object[] { "Seeking ", position, " (stream ID: ", this.StreamId, ")." })
                };
                StatusMessage message = new StatusMessage {
                    body = saso
                };
                this.DoPushMessage(message);
            }

            private void SendStartStatus(IPlayItem item)
            {
                StatusASO saso = new StatusASO("NetStream.Play.Start") {
                    clientid = this.StreamId,
                    details = item.Name,
                    description = "Started playing " + item.Name + '.'
                };
                StatusMessage message = new StatusMessage {
                    body = saso
                };
                this.DoPushMessage(message);
            }

            private void SendStopStatus(IPlayItem item)
            {
                StatusASO saso = new StatusASO("NetStream.Play.Stop") {
                    clientid = this.StreamId,
                    description = "Stopped playing " + item.Name + ".",
                    details = item.Name
                };
                StatusMessage message = new StatusMessage {
                    body = saso
                };
                this.DoPushMessage(message);
            }

            private void SendStreamNotFoundStatus(IPlayItem item)
            {
                StatusASO saso = new StatusASO("NetStream.Play.StreamNotFound") {
                    clientid = this.StreamId,
                    level = "error",
                    details = item.Name
                };
                StatusMessage message = new StatusMessage {
                    body = saso
                };
                this.DoPushMessage(message);
            }

            private void SendSwitchStatus()
            {
                int duration = 1;
                this.SendOnPlayStatus("NetStream.Play.Switch", duration, this._bytesSent);
            }

            private void SendUnpublishedStatus(IPlayItem item)
            {
                StatusASO saso = new StatusASO("NetStream.Play.UnpublishNotify") {
                    clientid = this.StreamId,
                    details = item.Name
                };
                StatusMessage message = new StatusMessage {
                    body = saso
                };
                this.DoPushMessage(message);
            }

            private void SendVODInitCM(IMessageInput msgIn, IPlayItem item)
            {
                OOBControlMessage oobCtrlMsg = new OOBControlMessage {
                    Target = typeof(IPassive).Name,
                    ServiceName = "init"
                };
                oobCtrlMsg.ServiceParameterMap.Add("startTS", item.Start);
                this._msgIn.SendOOBControlMessage(this, oobCtrlMsg);
            }

            private int SendVODSeekCM(IMessageInput msgIn, int position)
            {
                OOBControlMessage oobCtrlMsg = new OOBControlMessage {
                    Target = typeof(ISeekableProvider).Name,
                    ServiceName = "seek"
                };
                oobCtrlMsg.ServiceParameterMap.Add("position", position);
                msgIn.SendOOBControlMessage(this, oobCtrlMsg);
                if (oobCtrlMsg.Result is int)
                {
                    return (int) oobCtrlMsg.Result;
                }
                return -1;
            }

            public void Start()
            {
                lock (this.SyncRoot)
                {
                    if (this._state != PlaylistState.UNINIT)
                    {
                        throw new NotSupportedException();
                    }
                    this._state = PlaylistState.STOPPED;
                    this._schedulingService = this._stream.Scope.GetService(typeof(ISchedulingService)) as ISchedulingService;
                    this._msgOut = (this._stream.Scope.GetService(typeof(IConsumerService)) as IConsumerService).GetConsumerOutput(this._stream);
                    this._msgOut.Subscribe(this, null);
                    this._audioBucket = this._stream._bwController.GetAudioBucket(this._stream._bwContext);
                    this._videoBucket = this._stream._bwController.GetVideoBucket(this._stream._bwContext);
                }
            }

            public void Stop()
            {
                lock (this.SyncRoot)
                {
                    if ((this._state == PlaylistState.PLAYING) || (this._state == PlaylistState.PAUSED))
                    {
                        this._state = PlaylistState.STOPPED;
                        this._stream.NotifyItemStop(this._currentItem);
                        if (!((this._msgIn == null) || this._isPullMode))
                        {
                            this._msgIn.Unsubscribe(this);
                            this._msgIn = null;
                        }
                        this.ClearWaitJobs();
                        if (!this._stream.HasMoreItems)
                        {
                            this.ReleasePendingMessage();
                            this._stream._bwController.ResetBuckets(this._stream._bwContext);
                            this._isWaitingForToken = false;
                            if (this._stream.ItemSize > 0)
                            {
                                this.SendCompleteStatus();
                            }
                            this._bytesSent = 0L;
                            this.SendClearPing();
                            this.SendStopStatus(this._currentItem);
                        }
                        else
                        {
                            if (this._lastMessage != null)
                            {
                                this._timestampOffset = this._lastMessage.Timestamp;
                            }
                            this._stream.NextItem();
                        }
                    }
                }
            }

            public void UpdateBandwithConfigure()
            {
                this._stream._bwController.UpdateBWConfigure(this._stream._bwContext);
            }

            public bool IsPullMode
            {
                get
                {
                    return this._isPullMode;
                }
            }

            public IRtmpEvent LastMessage
            {
                get
                {
                    return this._lastMessage;
                }
                set
                {
                    this._lastMessage = value;
                }
            }

            public long PlaybackStart
            {
                get
                {
                    return this._playbackStart;
                }
            }

            public bool SendBlankAudio
            {
                get
                {
                    return this._sendBlankAudio;
                }
                set
                {
                    this._sendBlankAudio = value;
                }
            }

            public PlaylistState State
            {
                get
                {
                    return this._state;
                }
            }

            public int StreamId
            {
                get
                {
                    return this._stream.StreamId;
                }
            }

            public object SyncRoot
            {
                get
                {
                    return this._syncLock;
                }
            }

            internal class PlaylistSubscriberStreamJob : ScheduledJobBase
            {
                private PlaylistSubscriberStream.PlayEngine _engine;

                public PlaylistSubscriberStreamJob(PlaylistSubscriberStream.PlayEngine engine)
                {
                    this._engine = engine;
                }

                public override void Execute(ScheduledJobContext context)
                {
                    this._engine._waitLiveJob = null;
                    this._engine._stream.OnItemEnd();
                }
            }
        }
    }
}

