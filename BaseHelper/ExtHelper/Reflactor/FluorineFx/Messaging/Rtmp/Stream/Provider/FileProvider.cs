namespace FluorineFx.Messaging.Rtmp.Stream.Provider
{
    using FluorineFx.IO;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Messaging;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Messaging.Rtmp.IO;
    using FluorineFx.Messaging.Rtmp.Stream;
    using FluorineFx.Messaging.Rtmp.Stream.Messages;
    using log4net;
    using System;
    using System.IO;

    internal class FileProvider : IPassive, ISeekableProvider, IPullableProvider, IPipeConnectionListener, IStreamTypeAwareProvider, IProvider, IMessageComponent
    {
        private FileInfo _file;
        private KeyFrameMeta _keyFrameMeta;
        private IPipe _pipe;
        private ITagReader _reader;
        private IScope _scope;
        private int _start;
        private object _syncLock = new object();
        private static ILog log = LogManager.GetLogger(typeof(FileProvider));

        public FileProvider(IScope scope, FileInfo file)
        {
            this._scope = scope;
            this._file = file;
        }

        public bool HasVideo()
        {
            return ((this._reader != null) && this._reader.HasVideo());
        }

        private void Init()
        {
            IStreamableFileService service = ((IStreamableFileFactory) ScopeUtils.GetScopeService(this._scope, typeof(IStreamableFileFactory))).GetService(this._file);
            if (service == null)
            {
                log.Error("No service found for " + this._file.FullName);
            }
            else
            {
                this._reader = service.GetStreamableFile(this._file).GetReader();
                if (this._start > 0)
                {
                    this.Seek(this._start);
                }
            }
        }

        public void OnOOBControlMessage(IMessageComponent source, IPipe pipe, OOBControlMessage oobCtrlMsg)
        {
            if (typeof(IPassive).Name.Equals(oobCtrlMsg.Target))
            {
                if (oobCtrlMsg.ServiceName.Equals("Init"))
                {
                    int num = (int) oobCtrlMsg.ServiceParameterMap["startTS"];
                    this._start = num;
                }
            }
            else if (typeof(ISeekableProvider).Name.Equals(oobCtrlMsg.Target))
            {
                if (oobCtrlMsg.ServiceName.Equals("Seek"))
                {
                    int ts = (int) oobCtrlMsg.ServiceParameterMap["position"];
                    int num3 = this.Seek(ts);
                    oobCtrlMsg.Result = num3;
                }
            }
            else if (typeof(IStreamTypeAwareProvider).Name.Equals(oobCtrlMsg.Target) && oobCtrlMsg.ServiceName.Equals("HasVideo"))
            {
                oobCtrlMsg.Result = this.HasVideo();
            }
        }

        public void OnPipeConnectionEvent(PipeConnectionEvent evt)
        {
            switch (evt.Type)
            {
                case 0:
                    if (this._pipe == null)
                    {
                        this._pipe = evt.Source as IPipe;
                    }
                    break;

                case 2:
                    if (this._pipe == evt.Source)
                    {
                        this._pipe = null;
                        this.Uninit();
                    }
                    break;

                case 5:
                    if (this._pipe == evt.Source)
                    {
                        this.Uninit();
                    }
                    break;
            }
        }

        public IMessage PullMessage(IPipe pipe)
        {
            lock (this._syncLock)
            {
                if (this._pipe != pipe)
                {
                    return null;
                }
                if (this._reader == null)
                {
                    this.Init();
                }
                if (!this._reader.HasMoreTags())
                {
                    return null;
                }
                ITag tag = this._reader.ReadTag();
                IRtmpEvent event2 = null;
                int timestamp = tag.Timestamp;
                switch (tag.DataType)
                {
                    case 8:
                        event2 = new AudioData(tag.Body);
                        break;

                    case 9:
                        event2 = new VideoData(tag.Body);
                        break;

                    case 0x12:
                        event2 = new Notify(tag.Body);
                        break;

                    case 20:
                        event2 = new Invoke(tag.Body);
                        break;

                    default:
                        log.Warn("Unexpected type " + tag.DataType);
                        event2 = new Unknown(tag.DataType, tag.Body);
                        break;
                }
                event2.Timestamp = timestamp;
                return new RtmpMessage { body = event2 };
            }
        }

        public IMessage PullMessage(IPipe pipe, long wait)
        {
            return this.PullMessage(pipe);
        }

        public int Seek(int ts)
        {
            lock (this._syncLock)
            {
                if (this._keyFrameMeta == null)
                {
                    if (!(this._reader is IKeyFrameDataAnalyzer))
                    {
                        return ts;
                    }
                    this._keyFrameMeta = (this._reader as IKeyFrameDataAnalyzer).AnalyzeKeyFrames();
                }
                if (this._keyFrameMeta.Positions.Length == 0)
                {
                    return ts;
                }
                if (ts >= this._keyFrameMeta.Duration)
                {
                    this._reader.Position = 0x7fffffffffffffffL;
                    return (int) this._keyFrameMeta.Duration;
                }
                int index = 0;
                for (int i = 0; i < this._keyFrameMeta.Positions.Length; i++)
                {
                    if (this._keyFrameMeta.Timestamps[i] > ts)
                    {
                        break;
                    }
                    index = i;
                }
                this._reader.Position = this._keyFrameMeta.Positions[index];
                return this._keyFrameMeta.Timestamps[index];
            }
        }

        private void Uninit()
        {
            lock (this._syncLock)
            {
                if (this._reader != null)
                {
                    this._reader.Close();
                    this._reader = null;
                }
            }
        }

        public int Start
        {
            get
            {
                return this._start;
            }
            set
            {
                this._start = value;
            }
        }
    }
}

