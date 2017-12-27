namespace FluorineFx.IO.FLV
{
    using System;

    internal class FlvHeader
    {
        private int _dataOffset = 0;
        private bool _flagAudio;
        private byte _flagReserved01 = 0;
        private byte _flagReserved02 = 0;
        private bool _flagVideo;
        private byte _version = 0;

        internal void SetTypeFlags(byte typeFlags)
        {
            this._flagVideo = ((byte) (((typeFlags << 7) >> 7) & 1)) > 0;
            this._flagAudio = ((byte) (((typeFlags << 5) >> 7) & 1)) > 0;
        }

        public override string ToString()
        {
            string str = "";
            object obj2 = str;
            obj2 = string.Concat(new object[] { obj2, "version: ", this.Version, " \n" });
            obj2 = string.Concat(new object[] { obj2, "type flags video: ", this.FlagVideo, " \n" });
            obj2 = string.Concat(new object[] { obj2, "type flags audio: ", this.FlagAudio, " \n" });
            return string.Concat(new object[] { obj2, "data offset: ", this.DataOffset, "\n" });
        }

        public int DataOffset
        {
            get
            {
                return this._dataOffset;
            }
            set
            {
                this._dataOffset = value;
            }
        }

        public bool FlagAudio
        {
            get
            {
                return this._flagAudio;
            }
            set
            {
                this._flagAudio = value;
            }
        }

        public byte FlagReserved01
        {
            get
            {
                return this._flagReserved01;
            }
            set
            {
                this._flagReserved01 = value;
            }
        }

        public byte FlagReserved02
        {
            get
            {
                return this._flagReserved02;
            }
            set
            {
                this._flagReserved02 = value;
            }
        }

        public bool FlagVideo
        {
            get
            {
                return this._flagVideo;
            }
            set
            {
                this._flagVideo = value;
            }
        }

        public byte Version
        {
            get
            {
                return this._version;
            }
            set
            {
                this._version = value;
            }
        }
    }
}

