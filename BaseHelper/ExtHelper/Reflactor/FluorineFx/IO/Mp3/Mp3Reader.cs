namespace FluorineFx.IO.Mp3
{
    using FluorineFx;
    using FluorineFx.IO;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    internal class Mp3Reader : ITagReader, IKeyFrameDataAnalyzer
    {
        private double _currentTime;
        private int _dataRate;
        private long _duration;
        private FileInfo _file;
        private ITag _fileMeta;
        private FileStream _fileStream;
        private bool _firstFrame;
        private KeyFrameMeta _frameMeta;
        private Dictionary<long, double> _posTimeMap;
        private int _prevSize;
        private object _syncLock = new object();
        private ITag _tag;
        private static readonly ILog log = LogManager.GetLogger(typeof(Mp3Reader));

        public Mp3Reader(FileInfo file)
        {
            this._file = file;
            this._fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 0x10000);
            this.AnalyzeKeyFrames();
            this._firstFrame = true;
            this.ProcessID3v2Header();
            this._fileMeta = this.CreateFileMeta();
            if ((this._fileStream.Length - this._fileStream.Position) > 4L)
            {
                this.SearchNextFrame();
                long position = this._fileStream.Position;
                Mp3Header header = this.ReadHeader();
                this._fileStream.Position = position;
                if (header == null)
                {
                    throw new NotSupportedException("No initial header found.");
                }
                this.CheckValidHeader(header);
            }
        }

        public KeyFrameMeta AnalyzeKeyFrames()
        {
            lock (this._syncLock)
            {
                if (this._frameMeta == null)
                {
                    List<long> list = new List<long>();
                    List<double> list2 = new List<double>();
                    this._dataRate = 0;
                    long num = 0L;
                    int num2 = 0;
                    long position = this._fileStream.Position;
                    double item = 0.0;
                    this._fileStream.Position = 0L;
                    this.ProcessID3v2Header();
                    this.SearchNextFrame();
                    while (this.HasMoreTags())
                    {
                        Mp3Header header = this.ReadHeader();
                        if ((header == null) || (header.FrameSize == 0))
                        {
                            break;
                        }
                        long num5 = this._fileStream.Position - 4L;
                        if ((num5 + header.FrameSize) > this._fileStream.Length)
                        {
                            break;
                        }
                        list.Add(num5);
                        list2.Add(item);
                        num += header.BitRate / 0x3e8;
                        item += header.FrameDuration;
                        this._fileStream.Position = num5 + header.FrameSize;
                        num2++;
                    }
                    this._fileStream.Position = position;
                    this._duration = (long) item;
                    this._dataRate = (int) (num / ((long) num2));
                    this._posTimeMap = new Dictionary<long, double>();
                    this._frameMeta = new KeyFrameMeta();
                    this._frameMeta.Duration = this._duration;
                    this._frameMeta.Positions = new long[list.Count];
                    this._frameMeta.Timestamps = new int[list2.Count];
                    this._frameMeta.AudioOnly = true;
                    for (int i = 0; i < this._frameMeta.Positions.Length; i++)
                    {
                        this._frameMeta.Positions[i] = (int) list[i];
                        this._frameMeta.Timestamps[i] = (int) list2[i];
                        this._posTimeMap.Add(list[i], list2[i]);
                    }
                }
                return this._frameMeta;
            }
        }

        private void CheckValidHeader(Mp3Header header)
        {
            switch (header.SampleRate)
            {
                case 0x1589:
                case 0x2b11:
                case 0x5622:
                case 0xac44:
                    return;
            }
            throw new NotSupportedException("Unsupported sample rate: " + header.SampleRate);
        }

        public void Close()
        {
            if (this._posTimeMap != null)
            {
                this._posTimeMap.Clear();
            }
            this._fileStream.Close();
        }

        private ITag CreateFileMeta()
        {
            ByteBuffer stream = ByteBuffer.Allocate(0x400);
            stream.AutoExpand = true;
            AMFWriter writer = new AMFWriter(stream);
            writer.WriteString("onMetaData");
            Hashtable hashtable = new Hashtable();
            hashtable.Add("duration", ((double) this._frameMeta.Timestamps[this._frameMeta.Timestamps.Length - 1]) / 1000.0);
            hashtable.Add("audiocodecid", IOConstants.FLAG_FORMAT_MP3);
            if (this._dataRate > 0)
            {
                hashtable.Add("audiodatarate", this._dataRate);
            }
            hashtable.Add("canSeekToEnd", true);
            writer.WriteAssociativeArray(ObjectEncoding.AMF0, hashtable);
            stream.Flip();
            return new Tag(IOConstants.TYPE_METADATA, 0, stream.Limit, stream.ToArray(), this._prevSize);
        }

        public void DecodeHeader()
        {
        }

        public bool HasMoreTags()
        {
            Mp3Header header = null;
            while ((header == null) && ((this._fileStream.Length - this._fileStream.Position) > 4L))
            {
                try
                {
                    byte[] buffer = new byte[4];
                    this._fileStream.Read(buffer, 0, 4);
                    int data = (((buffer[0] << 0x18) | (buffer[1] << 0x10)) | (buffer[2] << 8)) | buffer[3];
                    header = new Mp3Header(data);
                }
                catch (IOException exception)
                {
                    log.Error("MP3Reader HasMoreTags", exception);
                    break;
                }
                catch (Exception)
                {
                    this.SearchNextFrame();
                }
            }
            if (header == null)
            {
                return false;
            }
            if (header.FrameSize == 0)
            {
                return false;
            }
            if (((this._fileStream.Position + header.FrameSize) - 4L) > this._fileStream.Length)
            {
                this._fileStream.Position = this._fileStream.Length;
                return false;
            }
            this._fileStream.Position -= 4L;
            return true;
        }

        public bool HasVideo()
        {
            return false;
        }

        private void ProcessID3v2Header()
        {
            if ((this._fileStream.Length - this._fileStream.Position) > 10L)
            {
                long position = this._fileStream.Position;
                byte num2 = (byte) this._fileStream.ReadByte();
                byte num3 = (byte) this._fileStream.ReadByte();
                byte num4 = (byte) this._fileStream.ReadByte();
                if (((num2 != 0x49) || (num3 != 0x44)) || (num4 != 0x33))
                {
                    this._fileStream.Position = position;
                }
                else
                {
                    this._fileStream.Seek(3L, SeekOrigin.Current);
                    int num5 = ((((this._fileStream.ReadByte() & 0x7f) << 0x15) | ((this._fileStream.ReadByte() & 0x7f) << 14)) | ((this._fileStream.ReadByte() & 0x7f) << 7)) | (this._fileStream.ReadByte() & 0x7f);
                    this._fileStream.Seek((long) num5, SeekOrigin.Current);
                }
            }
        }

        private Mp3Header ReadHeader()
        {
            Mp3Header header = null;
            while ((header == null) && ((this._fileStream.Length - this._fileStream.Position) > 4L))
            {
                try
                {
                    byte[] buffer = new byte[4];
                    this._fileStream.Read(buffer, 0, 4);
                    int data = (((buffer[0] << 0x18) | (buffer[1] << 0x10)) | (buffer[2] << 8)) | buffer[3];
                    header = new Mp3Header(data);
                }
                catch (IOException exception)
                {
                    log.Error("MP3Reader ReadTag", exception);
                    return header;
                }
                catch (Exception)
                {
                    this.SearchNextFrame();
                }
            }
            return header;
        }

        public ITag ReadTag()
        {
            lock (this._syncLock)
            {
                if (this._firstFrame)
                {
                    this._firstFrame = false;
                    return this._fileMeta;
                }
                Mp3Header header = this.ReadHeader();
                if (header == null)
                {
                    return null;
                }
                int frameSize = header.FrameSize;
                if (frameSize == 0)
                {
                    return null;
                }
                if (((this._fileStream.Position + frameSize) - 4L) > this._fileStream.Length)
                {
                    this._fileStream.Position = this._fileStream.Length;
                    return null;
                }
                this._tag = new Tag(IOConstants.TYPE_AUDIO, (int) this._currentTime, frameSize + 1, null, this._prevSize);
                this._prevSize = frameSize + 1;
                this._currentTime += header.FrameDuration;
                byte[] buffer = new byte[this._tag.BodySize];
                byte num2 = (byte) ((IOConstants.FLAG_FORMAT_MP3 << 4) | (IOConstants.FLAG_SIZE_16_BIT << 1));
                int sampleRate = header.SampleRate;
                if (sampleRate == 0x2b11)
                {
                    num2 = (byte) (num2 | ((byte) (IOConstants.FLAG_RATE_11_KHZ << 2)));
                }
                else if (sampleRate != 0x5622)
                {
                    if (sampleRate != 0xac44)
                    {
                        goto Label_0163;
                    }
                    num2 = (byte) (num2 | ((byte) (IOConstants.FLAG_RATE_44_KHZ << 2)));
                }
                else
                {
                    num2 = (byte) (num2 | ((byte) (IOConstants.FLAG_RATE_22_KHZ << 2)));
                }
                goto Label_0171;
            Label_0163:
                num2 = (byte) (num2 | ((byte) (IOConstants.FLAG_RATE_5_5_KHZ << 2)));
            Label_0171:
                num2 = (byte) (num2 | (header.IsStereo ? IOConstants.FLAG_TYPE_STEREO : IOConstants.FLAG_TYPE_MONO));
                buffer[0] = num2;
                int data = header.Data;
                buffer[1] = (byte) (0xff & (data >> 0x18));
                buffer[2] = (byte) (0xff & (data >> 0x10));
                buffer[3] = (byte) (0xff & (data >> 8));
                buffer[4] = (byte) (0xff & data);
                this._fileStream.Read(buffer, 5, frameSize - 4);
                this._tag.Body = buffer;
                return this._tag;
            }
        }

        public void SearchNextFrame()
        {
            while ((this._fileStream.Length - this._fileStream.Position) > 1L)
            {
                int num = this._fileStream.ReadByte() & 0xff;
                if ((num == 0xff) && ((this._fileStream.ReadByte() & 0xe0) == 0xe0))
                {
                    this._fileStream.Position -= 2L;
                    break;
                }
            }
        }

        public long BytesRead
        {
            get
            {
                return this._fileStream.Position;
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
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                if (value == 0x7fffffffffffffffL)
                {
                    this._fileStream.Position = this._fileStream.Length;
                    this._currentTime = this._duration;
                }
                else
                {
                    this._fileStream.Position = value;
                    this.SearchNextFrame();
                    this.AnalyzeKeyFrames();
                    if (this._posTimeMap.ContainsKey(this._fileStream.Position))
                    {
                        this._currentTime = this._posTimeMap[this._fileStream.Position];
                    }
                    else
                    {
                        this._currentTime = 0.0;
                    }
                }
            }
        }
    }
}

