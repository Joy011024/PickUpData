namespace FluorineFx.IO.FLV
{
    using FluorineFx;
    using FluorineFx.IO;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;

    internal class FlvWriter : ITagWriter
    {
        private bool _append;
        private int _audioCodecId = -1;
        private long _bytesWritten;
        private int _duration;
        private int _fileMetaSize = 0;
        private long _metaPosition;
        private object _syncLock = new object();
        private int _videoCodecId = -1;
        private AMFWriter _writer;
        private static readonly ILog log = LogManager.GetLogger(typeof(FlvWriter));

        public FlvWriter(Stream stream, bool append)
        {
            this._writer = new AMFWriter(stream);
            this._append = append;
            if (this._append)
            {
                if (stream.Length > 0x18L)
                {
                    try
                    {
                        stream.Position = 9L;
                        byte[] buffer = new byte[15];
                        stream.Read(buffer, 0, 4);
                        byte num = (byte) stream.ReadByte();
                        if (num == IOConstants.TYPE_METADATA)
                        {
                            this._metaPosition = stream.Position - 1L;
                            stream.Read(buffer, 5, 3);
                            int num2 = ((buffer[5] << 0x10) | (buffer[6] << 8)) | buffer[7];
                            stream.Read(buffer, 8, 4);
                            stream.Read(buffer, 12, 3);
                            byte[] buffer2 = new byte[num2];
                            stream.Read(buffer2, 0, buffer2.Length);
                            MemoryStream stream2 = new MemoryStream(buffer2);
                            AMFReader reader = new AMFReader(stream2);
                            string str = reader.ReadData() as string;
                            IDictionary dictionary = reader.ReadData() as IDictionary;
                            if (dictionary.Contains("duration"))
                            {
                                this._duration = Convert.ToInt32(dictionary["duration"]);
                            }
                            else
                            {
                                log.Warn("Could not read Flv duration from metadata");
                            }
                        }
                        else
                        {
                            log.Warn("Could not read Flv duration");
                        }
                    }
                    catch (Exception exception)
                    {
                        log.Warn("Error reading Flv duration", exception);
                    }
                }
                stream.Seek(0L, SeekOrigin.End);
            }
        }

        public void Close()
        {
            try
            {
                if (this._metaPosition > 0L)
                {
                    long position = this._writer.BaseStream.Position;
                    try
                    {
                        this._writer.BaseStream.Position = this._metaPosition;
                        this.WriteMetadataTag(this._duration * 0.001, this._videoCodecId, this._audioCodecId);
                    }
                    finally
                    {
                        this._writer.BaseStream.Position = position;
                    }
                }
                this._writer.Close();
            }
            catch (IOException exception)
            {
                log.Error("FlvWriter close", exception);
            }
        }

        public void WriteHeader()
        {
            this._writer.WriteByte(70);
            this._writer.WriteByte(0x4c);
            this._writer.WriteByte(0x56);
            this._writer.WriteByte(1);
            this._writer.WriteByte(5);
            this._writer.WriteInt32(9);
            this._writer.WriteInt32(0);
        }

        private void WriteMetadataTag(double duration, object videoCodecId, object audioCodecId)
        {
            this._metaPosition = this._writer.BaseStream.Position;
            MemoryStream stream = new MemoryStream();
            AMFWriter writer = new AMFWriter(stream);
            writer.WriteString("onMetaData");
            Hashtable hashtable = new Hashtable();
            hashtable.Add("duration", this._duration);
            if (videoCodecId != null)
            {
                hashtable.Add("videocodecid", videoCodecId);
            }
            if (audioCodecId != null)
            {
                hashtable.Add("audiocodecid", audioCodecId);
            }
            hashtable.Add("canSeekToEnd", true);
            writer.WriteAssociativeArray(ObjectEncoding.AMF0, hashtable);
            byte[] body = stream.ToArray();
            if (this._fileMetaSize == 0)
            {
                this._fileMetaSize = body.Length;
            }
            ITag tag = new Tag(IOConstants.TYPE_METADATA, 0, body.Length, body, 0);
            this.WriteTag(tag);
        }

        public bool WriteStream(byte[] buffer)
        {
            return false;
        }

        public bool WriteTag(ITag tag)
        {
            long position = this._writer.BaseStream.Position;
            if ((!this._append && (this._bytesWritten == 0L)) && (tag.DataType != IOConstants.TYPE_METADATA))
            {
                this.WriteMetadataTag(0.0, -1, -1);
            }
            this._writer.WriteByte(tag.DataType);
            this._writer.WriteUInt24(tag.BodySize);
            this._writer.WriteUInt24(tag.Timestamp);
            byte num2 = (byte) ((tag.Timestamp & 0xff000000L) >> 0x18);
            this._writer.WriteByte(num2);
            this._writer.WriteUInt24(0);
            if (tag.BodySize != 0)
            {
                byte num3;
                byte[] body = tag.Body;
                this._writer.WriteBytes(body);
                if ((this._audioCodecId == -1) && (tag.DataType == IOConstants.TYPE_AUDIO))
                {
                    num3 = body[0];
                    this._audioCodecId = (num3 & IOConstants.MASK_SOUND_FORMAT) >> 4;
                }
                else if ((this._videoCodecId == -1) && (tag.DataType == IOConstants.TYPE_VIDEO))
                {
                    num3 = body[0];
                    this._videoCodecId = num3 & IOConstants.MASK_VIDEO_CODEC;
                }
            }
            this._duration = Math.Max(this._duration, tag.Timestamp);
            this._writer.WriteInt32(tag.BodySize + 11);
            this._bytesWritten += this._writer.BaseStream.Position - position;
            return true;
        }

        public bool WriteTag(byte type, ByteBuffer data)
        {
            return false;
        }

        public long BytesWritten
        {
            get
            {
                return this._bytesWritten;
            }
        }

        public IStreamableFile File
        {
            get
            {
                return null;
            }
        }

        public long Position
        {
            get
            {
                return this._writer.BaseStream.Position;
            }
        }
    }
}

