namespace FluorineFx.IO.FLV
{
    using FluorineFx;
    using FluorineFx.IO;
    using System;
    using System.Collections;
    using System.IO;

    [CLSCompliant(false)]
    public class MetaService
    {
        private AMFReader _deserializer;
        private FileInfo _file;
        private Stream _input;
        private Stream _output;
        private AMFWriter _serializer;

        public MetaService()
        {
        }

        public MetaService(FileInfo file)
        {
            this._file = file;
        }

        private int GetTimeInMilliseconds(MetaCue metaCue)
        {
            return (int) (metaCue.Time * 1000.0);
        }

        private ITag InjectMetaCue(MetaCue meta, ITag tag)
        {
            MemoryStream stream = new MemoryStream();
            AMFWriter writer = new AMFWriter(stream);
            writer.WriteData(ObjectEncoding.AMF0, "onCuePoint");
            writer.WriteData(ObjectEncoding.AMF0, meta);
            byte[] body = stream.ToArray();
            return new Tag(IOConstants.TYPE_METADATA, this.GetTimeInMilliseconds(meta), body.Length, body, tag.PreviousTagSize);
        }

        private ITag InjectMetaData(MetaData meta, ITag tag)
        {
            MemoryStream stream = new MemoryStream();
            AMFWriter writer = new AMFWriter(stream);
            writer.WriteData(ObjectEncoding.AMF0, "onMetaData");
            writer.WriteData(ObjectEncoding.AMF0, meta);
            byte[] body = stream.ToArray();
            return new Tag(IOConstants.TYPE_METADATA, 0, body.Length, body, tag.PreviousTagSize);
        }

        public MetaCue[] ReadMetaCue()
        {
            return null;
        }

        public MetaData ReadMetaData(byte[] buffer)
        {
            MetaData data = new MetaData();
            MemoryStream stream = new MemoryStream(buffer);
            AMFReader reader = new AMFReader(stream);
            string str = reader.ReadData() as string;
            IDictionary dictionary = reader.ReadData() as IDictionary;
            data.PutAll(dictionary);
            return data;
        }

        public void Write(MetaData meta)
        {
            MetaCue[] metaCue = meta.MetaCue;
            FlvReader reader = new FlvReader(this._file, false);
            FlvWriter writer = new FlvWriter(this._output, false);
            ITag tag = null;
            if (reader.HasMoreTags())
            {
                tag = reader.ReadTag();
                if ((tag.DataType == IOConstants.TYPE_METADATA) && !reader.HasMoreTags())
                {
                    throw new IOException("File we're writing is metadata only?");
                }
            }
            meta.Duration = ((double) reader.Duration) / 1000.0;
            meta.VideoCodecId = reader.VideoCodecId;
            meta.AudioCodecId = reader.AudioCodecId;
            ITag tag2 = this.InjectMetaData(meta, tag);
            tag2.PreviousTagSize = 0;
            tag.PreviousTagSize = tag2.BodySize;
            writer.WriteHeader();
            writer.WriteTag(tag2);
            writer.WriteTag(tag);
            int timeInMilliseconds = 0;
            int index = 0;
            if (metaCue != null)
            {
                Array.Sort<MetaCue>(metaCue);
                timeInMilliseconds = this.GetTimeInMilliseconds(metaCue[0]);
            }
            while (reader.HasMoreTags())
            {
                tag = reader.ReadTag();
                if (index < metaCue.Length)
                {
                    while (tag.Timestamp > timeInMilliseconds)
                    {
                        tag2 = this.InjectMetaCue(metaCue[index], tag);
                        writer.WriteTag(tag2);
                        tag.PreviousTagSize = tag2.BodySize;
                        index++;
                        if (index > (metaCue.Length - 1))
                        {
                            break;
                        }
                        timeInMilliseconds = this.GetTimeInMilliseconds(metaCue[index]);
                    }
                }
                if (tag.DataType != IOConstants.TYPE_METADATA)
                {
                    writer.WriteTag(tag);
                }
            }
            writer.Close();
        }

        public void WriteMetaData(MetaData metaData)
        {
        }

        public AMFReader Deserializer
        {
            get
            {
                return this._deserializer;
            }
            set
            {
                this._deserializer = value;
            }
        }

        public Stream Input
        {
            get
            {
                return this._input;
            }
            set
            {
                this._input = value;
            }
        }

        public Stream Output
        {
            get
            {
                return this._output;
            }
            set
            {
                this._output = value;
            }
        }

        public AMFWriter Serializer
        {
            get
            {
                return this._serializer;
            }
            set
            {
                this._serializer = value;
            }
        }
    }
}

