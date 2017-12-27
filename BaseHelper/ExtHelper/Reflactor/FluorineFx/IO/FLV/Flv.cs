namespace FluorineFx.IO.FLV
{
    using FluorineFx.IO;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;

    internal class Flv : IFlv, IStreamableFile
    {
        private FileInfo _file;
        private bool _generateMetadata;
        private FluorineFx.IO.FLV.MetaData _metaData;
        private FluorineFx.IO.FLV.MetaService _metaService;
        private static readonly ILog log = LogManager.GetLogger(typeof(Flv));

        public Flv(FileInfo file) : this(file, false)
        {
        }

        public Flv(FileInfo file, bool generateMetadata)
        {
            this._file = file;
            this._generateMetadata = generateMetadata;
            int num = 0;
            if (!this._generateMetadata)
            {
                try
                {
                    FlvReader reader = new FlvReader(this._file);
                    ITag tag = null;
                    while (reader.HasMoreTags() && (++num < 5))
                    {
                        tag = reader.ReadTag();
                        if (tag.DataType == IOConstants.TYPE_METADATA)
                        {
                            if (this._metaService == null)
                            {
                                this._metaService = new FluorineFx.IO.FLV.MetaService(this._file);
                            }
                            this._metaData = this._metaService.ReadMetaData(tag.Body);
                        }
                    }
                    reader.Close();
                }
                catch (Exception exception)
                {
                    log.Error("An error occured looking for metadata:", exception);
                }
            }
        }

        public void FlushHeaders()
        {
        }

        public ITagWriter GetAppendWriter()
        {
            if (!this._file.Exists)
            {
                log.Info("File does not exist, calling writer. This will create a new file.");
                return this.GetWriter();
            }
            return new FlvWriter(new FileStream(this._file.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read, 0x10000), true);
        }

        public ITagReader GetReader()
        {
            string name = this._file.Name;
            if (this._file.Exists)
            {
                if (log.get_IsDebugEnabled())
                {
                    log.Debug("File size: " + this._file.Length);
                }
                return new FlvReader(this._file, this._generateMetadata);
            }
            log.Info("Creating new file: " + name);
            using (this._file.Create())
            {
            }
            this._file.Refresh();
            return new FlvReader(this._file, this._generateMetadata);
        }

        public ITagWriter GetWriter()
        {
            if (this._file.Exists)
            {
                this._file.Delete();
            }
            ITagWriter writer = new FlvWriter(this._file.Create(), false);
            writer.WriteHeader();
            return writer;
        }

        public ITagReader ReaderFromNearestKeyFrame(int seekPoint)
        {
            return null;
        }

        public void RefreshHeaders()
        {
        }

        public ITagWriter WriterFromNearestKeyFrame(int seekPoint)
        {
            return null;
        }

        public bool HasKeyFrameData
        {
            get
            {
                return false;
            }
        }

        public bool HasMetaData
        {
            get
            {
                return (this._metaData != null);
            }
        }

        public Hashtable KeyFrameData
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public FluorineFx.IO.FLV.MetaData MetaData
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public FluorineFx.IO.FLV.MetaService MetaService
        {
            get
            {
                return this._metaService;
            }
            set
            {
                this._metaService = value;
            }
        }
    }
}

