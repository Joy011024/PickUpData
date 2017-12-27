namespace FluorineFx.IO.FLV
{
    using FluorineFx;
    using FluorineFx.IO;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;

    internal class FlvReader : ITagReader, IKeyFrameDataAnalyzer
    {
        private long _duration;
        private FileInfo _file;
        private long _firstAudioTag;
        private long _firstVideoTag;
        private bool _generateMetadata;
        private FlvHeader _header;
        private KeyFrameMeta _keyframeMeta;
        private Hashtable _posTagMap;
        private Hashtable _posTimeMap;
        private AMFReader _reader;
        private object _syncLock;
        private int _tagPosition;
        private static readonly ILog log = LogManager.GetLogger(typeof(FlvReader));

        public FlvReader()
        {
            this._syncLock = new object();
            this._firstVideoTag = -1L;
            this._firstAudioTag = -1L;
        }

        public FlvReader(FileInfo file) : this(file, false)
        {
        }

        public FlvReader(FileInfo file, bool generateMetadata)
        {
            this._syncLock = new object();
            this._firstVideoTag = -1L;
            this._firstAudioTag = -1L;
            this._file = file;
            FileStream stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 0x10000);
            this._reader = new AMFReader(stream);
            this._generateMetadata = generateMetadata;
            if (this.GetRemainingBytes() >= 9L)
            {
                this.DecodeHeader();
            }
            this._keyframeMeta = this.AnalyzeKeyFrames();
        }

        public KeyFrameMeta AnalyzeKeyFrames()
        {
            lock (this.SyncRoot)
            {
                if (this._keyframeMeta == null)
                {
                    ArrayList list = new ArrayList();
                    ArrayList list2 = new ArrayList();
                    ArrayList list3 = new ArrayList();
                    ArrayList list4 = new ArrayList();
                    long currentPosition = this.GetCurrentPosition();
                    this.SetCurrentPosition(9L);
                    this._posTagMap = new Hashtable();
                    int num2 = 0;
                    bool flag = true;
                    while (this.HasMoreTags())
                    {
                        long key = this.GetCurrentPosition();
                        this._posTagMap.Add(key, num2++);
                        ITag tag = this.ReadTagHeader();
                        this._duration = tag.Timestamp;
                        if (tag.DataType == IOConstants.TYPE_VIDEO)
                        {
                            if (flag)
                            {
                                flag = false;
                                list3.Clear();
                                list4.Clear();
                            }
                            if (this._firstVideoTag == -1L)
                            {
                                this._firstVideoTag = key;
                            }
                            if (((this._reader.ReadByte() & IOConstants.MASK_VIDEO_FRAMETYPE) >> 4) == IOConstants.FLAG_FRAMETYPE_KEYFRAME)
                            {
                                list.Add(key);
                                list2.Add(tag.Timestamp);
                            }
                        }
                        else if (tag.DataType == IOConstants.TYPE_AUDIO)
                        {
                            if (this._firstAudioTag == -1L)
                            {
                                this._firstAudioTag = key;
                            }
                            if (flag)
                            {
                                list3.Add(key);
                                list4.Add(tag.Timestamp);
                            }
                        }
                        long pos = (key + tag.BodySize) + 15L;
                        if (pos >= this.GetTotalBytes())
                        {
                            log.Info("New position exceeds limit");
                            if (log.get_IsDebugEnabled())
                            {
                                log.Debug("Keyframe analysis");
                                log.Debug(string.Concat(new object[] { " data type=", tag.DataType, " bodysize=", tag.BodySize }));
                                log.Debug(string.Concat(new object[] { " remaining=", this.GetRemainingBytes(), " limit=", this.GetTotalBytes(), " new pos=", pos, " pos=", key }));
                            }
                            break;
                        }
                        this.SetCurrentPosition(pos);
                    }
                    this.SetCurrentPosition(currentPosition);
                    this._keyframeMeta = new KeyFrameMeta();
                    this._keyframeMeta.Duration = this._duration;
                    this._posTimeMap = new Hashtable();
                    if (flag)
                    {
                        list = list3;
                        list2 = list4;
                    }
                    this._keyframeMeta.AudioOnly = flag;
                    this._keyframeMeta.Positions = new long[list.Count];
                    this._keyframeMeta.Timestamps = new int[list2.Count];
                    for (int i = 0; i < this._keyframeMeta.Positions.Length; i++)
                    {
                        this._keyframeMeta.Positions[i] = (long) list[i];
                        this._keyframeMeta.Timestamps[i] = (int) list2[i];
                        this._posTimeMap.Add((long) list[i], (long) ((int) list2[i]));
                    }
                }
                return this._keyframeMeta;
            }
        }

        public void Close()
        {
            this._reader.Close();
        }

        private ITag CreateFileMeta()
        {
            long currentPosition;
            byte num2;
            ByteBuffer stream = ByteBuffer.Allocate(0x400);
            stream.AutoExpand = true;
            AMFWriter writer = new AMFWriter(stream);
            writer.WriteString("onMetaData");
            Hashtable hashtable = new Hashtable();
            hashtable.Add("duration", ((double) this._duration) / 1000.0);
            if (this._firstVideoTag != -1L)
            {
                currentPosition = this.GetCurrentPosition();
                this.SetCurrentPosition(this._firstVideoTag);
                this.ReadTagHeader();
                num2 = this._reader.ReadByte();
                hashtable.Add("videocodecid", num2 & IOConstants.MASK_VIDEO_CODEC);
                this.SetCurrentPosition(currentPosition);
            }
            if (this._firstAudioTag != -1L)
            {
                currentPosition = this.GetCurrentPosition();
                this.SetCurrentPosition(this._firstAudioTag);
                this.ReadTagHeader();
                num2 = this._reader.ReadByte();
                hashtable.Add("audiocodecid", (num2 & IOConstants.MASK_SOUND_FORMAT) >> 4);
                this.SetCurrentPosition(currentPosition);
            }
            hashtable.Add("canSeekToEnd", true);
            writer.WriteAssociativeArray(ObjectEncoding.AMF0, hashtable);
            stream.Flip();
            return new Tag(IOConstants.TYPE_METADATA, 0, stream.Limit, stream.ToArray(), 0);
        }

        public void DecodeHeader()
        {
            this._header = new FlvHeader();
            byte[] buffer = this._reader.ReadBytes(3);
            this._header.Version = this._reader.ReadByte();
            this._header.SetTypeFlags(this._reader.ReadByte());
            this._header.DataOffset = this._reader.ReadInt32();
            if (log.get_IsDebugEnabled())
            {
                log.Debug("Flv header: " + this._header.ToString());
            }
        }

        private long GetCurrentPosition()
        {
            return this._reader.BaseStream.Position;
        }

        public ByteBuffer GetFileData()
        {
            return null;
        }

        private long GetRemainingBytes()
        {
            return (this._reader.BaseStream.Length - this._reader.BaseStream.Position);
        }

        private long GetTotalBytes()
        {
            return this._file.Length;
        }

        public bool HasMoreTags()
        {
            return (this.GetRemainingBytes() > 4L);
        }

        public bool HasVideo()
        {
            KeyFrameMeta meta = this.AnalyzeKeyFrames();
            if (meta == null)
            {
                return false;
            }
            return (!meta.AudioOnly && (meta.Positions.Length > 0));
        }

        public ITag ReadTag()
        {
            lock (this.SyncRoot)
            {
                long currentPosition = this.GetCurrentPosition();
                ITag tag = this.ReadTagHeader();
                if (((this._tagPosition == 0) && (tag.DataType != IOConstants.TYPE_METADATA)) && this._generateMetadata)
                {
                    this.SetCurrentPosition(currentPosition);
                    KeyFrameMeta meta = this.AnalyzeKeyFrames();
                    this._tagPosition++;
                    if (meta != null)
                    {
                        return this.CreateFileMeta();
                    }
                }
                long num2 = this.GetCurrentPosition() + tag.BodySize;
                if (num2 <= this.GetTotalBytes())
                {
                    byte[] buffer = this._reader.ReadBytes(tag.BodySize);
                    tag.Body = buffer;
                    this._tagPosition++;
                }
                return tag;
            }
        }

        private ITag ReadTagHeader()
        {
            int previousTagSize = this._reader.ReadInt32();
            byte dataType = this._reader.ReadByte();
            int bodySize = this._reader.ReadUInt24();
            int timestamp = this._reader.ReadUInt24() | (this._reader.ReadByte() << 0x18);
            int num5 = this._reader.ReadUInt24();
            return new Tag(dataType, timestamp, bodySize, null, previousTagSize);
        }

        private void SetCurrentPosition(long pos)
        {
            if (pos == 0x7fffffffffffffffL)
            {
                pos = this._file.Length;
            }
            this._reader.BaseStream.Position = pos;
        }

        public int AudioCodecId
        {
            get
            {
                if (this.AnalyzeKeyFrames() == null)
                {
                    return -1;
                }
                long currentPosition = this.GetCurrentPosition();
                this.SetCurrentPosition(this._firstAudioTag);
                this.ReadTagHeader();
                byte num2 = this._reader.ReadByte();
                this.SetCurrentPosition(currentPosition);
                return (num2 & IOConstants.MASK_SOUND_FORMAT);
            }
        }

        public long BytesRead
        {
            get
            {
                return this.GetCurrentPosition();
            }
        }

        public long Duration
        {
            get
            {
                return this._duration;
            }
        }

        public IStreamableFile File
        {
            get
            {
                return null;
            }
        }

        public int Offset
        {
            get
            {
                return 0;
            }
        }

        public long Position
        {
            get
            {
                return this.GetCurrentPosition();
            }
            set
            {
                this.SetCurrentPosition(value);
                if (value == 0x7fffffffffffffffL)
                {
                    this._tagPosition = this._posTagMap.Count + 1;
                }
                else
                {
                    this.AnalyzeKeyFrames();
                    if (this._posTagMap.ContainsKey(value))
                    {
                        this._tagPosition = (int) this._posTagMap[value];
                    }
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

        public int VideoCodecId
        {
            get
            {
                if (this.AnalyzeKeyFrames() == null)
                {
                    return -1;
                }
                long currentPosition = this.GetCurrentPosition();
                this.SetCurrentPosition(this._firstVideoTag);
                this.ReadTagHeader();
                byte num2 = this._reader.ReadByte();
                this.SetCurrentPosition(currentPosition);
                return (num2 & IOConstants.MASK_VIDEO_CODEC);
            }
        }
    }
}

