namespace FluorineFx.Messaging.Rtmp.Stream.Consumer
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
    using System.Collections;
    using System.IO;

    internal class FileConsumer : IPushableConsumer, IConsumer, IMessageComponent, IPipeConnectionListener
    {
        private FileInfo _file;
        private int _lastTimestamp;
        private string _mode;
        private int _offset;
        private IScope _scope;
        private int _startTimestamp;
        private object _syncLock = new object();
        private ITagWriter _writer;
        private static ILog log = LogManager.GetLogger(typeof(FileConsumer));

        public FileConsumer(IScope scope, FileInfo file)
        {
            this._scope = scope;
            this._file = file;
            this._offset = 0;
            this._lastTimestamp = 0;
            this._startTimestamp = -1;
        }

        private void Init()
        {
            IStreamableFileFactory scopeService = ScopeUtils.GetScopeService(this._scope, typeof(IStreamableFileFactory)) as IStreamableFileFactory;
            string directoryName = Path.GetDirectoryName(this._file.FullName);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            if (!this._file.Exists)
            {
                using (this._file.Create())
                {
                }
            }
            IStreamableFile streamableFile = scopeService.GetService(this._file).GetStreamableFile(this._file);
            if ((this._mode == null) || this._mode.Equals("record"))
            {
                this._writer = streamableFile.GetWriter();
            }
            else
            {
                if (!this._mode.Equals("append"))
                {
                    throw new NotSupportedException("Illegal mode type: " + this._mode);
                }
                this._writer = streamableFile.GetAppendWriter();
            }
        }

        public void OnOOBControlMessage(IMessageComponent source, IPipe pipe, OOBControlMessage oobCtrlMsg)
        {
        }

        public void OnPipeConnectionEvent(PipeConnectionEvent evt)
        {
            switch (evt.Type)
            {
                case 2:
                    this.Uninit();
                    break;

                case 4:
                    if (evt.Consumer == this)
                    {
                        IDictionary parameterMap = evt.ParameterMap;
                        if (parameterMap != null)
                        {
                            this._mode = parameterMap["mode"] as string;
                        }
                        break;
                    }
                    break;

                case 5:
                    if (evt.Consumer != this)
                    {
                    }
                    break;
            }
        }

        public void PushMessage(IPipe pipe, IMessage message)
        {
            lock (this.SyncRoot)
            {
                if (message is ResetMessage)
                {
                    this._startTimestamp = -1;
                    this._offset += this._lastTimestamp;
                }
                else if (!(message is StatusMessage) && (message is RtmpMessage))
                {
                    if (this._writer == null)
                    {
                        this.Init();
                    }
                    RtmpMessage message2 = message as RtmpMessage;
                    IRtmpEvent body = message2.body;
                    if (this._startTimestamp == -1)
                    {
                        this._startTimestamp = body.Timestamp;
                    }
                    int num = body.Timestamp - this._startTimestamp;
                    if (num < 0)
                    {
                        log.Warn("Skipping message with negative timestamp.");
                    }
                    else
                    {
                        this._lastTimestamp = num;
                        ITag tag = new Tag {
                            DataType = body.DataType,
                            Timestamp = num + this._offset
                        };
                        if (body is IStreamData)
                        {
                            tag.Body = (body as IStreamData).Data.ToArray();
                        }
                        try
                        {
                            this._writer.WriteTag(tag);
                        }
                        catch (IOException exception)
                        {
                            log.Error("Error writing tag", exception);
                        }
                    }
                }
            }
        }

        private void Uninit()
        {
            lock (this.SyncRoot)
            {
                if (this._writer != null)
                {
                    this._writer.Close();
                    this._writer = null;
                }
            }
        }

        public object SyncRoot
        {
            get
            {
                return this._syncLock;
            }
        }
    }
}

