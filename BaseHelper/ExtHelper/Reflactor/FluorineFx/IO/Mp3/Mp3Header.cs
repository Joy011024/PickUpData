namespace FluorineFx.IO.Mp3
{
    using System;

    internal class Mp3Header
    {
        private byte _audioVersionId;
        private byte _bitRateIndex;
        private byte _channelMode;
        private int _data;
        private byte _layerDescription;
        private bool _paddingBit;
        private bool _protectionBit;
        private byte _samplingRateIndex;
        private static int[,] BITRATES = new int[,] { { 0, 0x20, 0x40, 0x60, 0x80, 160, 0xc0, 0xe0, 0x100, 0x120, 320, 0x160, 0x180, 0x1a0, 0x1c0, -1 }, { 0, 0x20, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 160, 0xc0, 0xe0, 0x100, 320, 0x180, -1 }, { 0, 0x20, 40, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 160, 0xc0, 0xe0, 0x100, 320, -1 }, { 0, 0x20, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 0x90, 160, 0xb0, 0xc0, 0xe0, 0x100, -1 }, { 0, 8, 0x10, 0x18, 0x20, 40, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 0x90, 160, -1 } };
        private static int[,] SAMPLERATES = new int[,] { { 0x2b11, 0x2ee0, 0x1f40, -1 }, { -1, -1, -1, -1 }, { 0x5622, 0x5dc0, 0x3e80, -1 }, { 0xac44, 0xbb80, 0x7d00, -1 } };

        public Mp3Header(int data)
        {
            if ((data & 0xffe00000L) != 0xffe00000L)
            {
                throw new Exception("Invalid frame sync word");
            }
            this._data = data;
            data &= 0x1fffff;
            this._audioVersionId = (byte) ((data >> 0x13) & 3);
            this._layerDescription = (byte) ((data >> 0x11) & 3);
            this._protectionBit = ((data >> 0x10) & 1) == 0;
            this._bitRateIndex = (byte) ((data >> 12) & 15);
            this._samplingRateIndex = (byte) ((data >> 10) & 3);
            this._paddingBit = ((data >> 9) & 1) != 0;
            this._channelMode = (byte) ((data >> 6) & 3);
        }

        public int BitRate
        {
            get
            {
                int num;
                switch (this._audioVersionId)
                {
                    case 0:
                    case 2:
                        if (this._layerDescription != 3)
                        {
                            if ((this._layerDescription == 2) || (this._layerDescription == 1))
                            {
                                num = BITRATES[4, this._bitRateIndex];
                                break;
                            }
                            return -1;
                        }
                        num = BITRATES[3, this._bitRateIndex];
                        break;

                    case 1:
                        return -1;

                    case 3:
                        if (this._layerDescription != 3)
                        {
                            if (this._layerDescription == 2)
                            {
                                num = BITRATES[1, this._bitRateIndex];
                            }
                            else if (this._layerDescription == 1)
                            {
                                num = BITRATES[2, this._bitRateIndex];
                            }
                            else
                            {
                                return -1;
                            }
                            break;
                        }
                        num = BITRATES[0, this._bitRateIndex];
                        break;

                    default:
                        return -1;
                }
                return (num * 0x3e8);
            }
        }

        public int Data
        {
            get
            {
                return this._data;
            }
        }

        public double FrameDuration
        {
            get
            {
                switch (this._layerDescription)
                {
                    case 1:
                    case 2:
                        if (this._audioVersionId != 3)
                        {
                            return (576.0 / (this.SampleRate * 0.001));
                        }
                        return (1152.0 / (this.SampleRate * 0.001));

                    case 3:
                        return (384.0 / (this.SampleRate * 0.001));
                }
                return -1.0;
            }
        }

        public int FrameSize
        {
            get
            {
                switch (this._layerDescription)
                {
                    case 1:
                    case 2:
                        if (this._audioVersionId != 3)
                        {
                            return (((0x48 * this.BitRate) / this.SampleRate) + (this._paddingBit ? 1 : 0));
                        }
                        return (((0x90 * this.BitRate) / this.SampleRate) + (this._paddingBit ? 1 : 0));

                    case 3:
                        return ((((12 * this.BitRate) / this.SampleRate) + (this._paddingBit ? 1 : 0)) * 4);
                }
                return -1;
            }
        }

        public bool IsProtected
        {
            get
            {
                return this._protectionBit;
            }
        }

        public bool IsStereo
        {
            get
            {
                return (this._channelMode != 3);
            }
        }

        public int SampleRate
        {
            get
            {
                return SAMPLERATES[this._audioVersionId, this._samplingRateIndex];
            }
        }
    }
}

