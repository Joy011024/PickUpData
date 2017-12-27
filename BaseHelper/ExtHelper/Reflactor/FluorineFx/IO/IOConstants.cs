namespace FluorineFx.IO
{
    using System;

    internal class IOConstants
    {
        public static byte FLAG_CODEC_H263 = 2;
        public static byte FLAG_CODEC_SCREEN = 3;
        public static byte FLAG_CODEC_VP6 = 4;
        public static byte FLAG_FORMAT_ADPCM = 1;
        public static byte FLAG_FORMAT_MP3 = 2;
        public static byte FLAG_FORMAT_NELLYMOSER = 6;
        public static byte FLAG_FORMAT_NELLYMOSER_8_KHZ = 5;
        public static byte FLAG_FORMAT_RAW = 0;
        public static byte FLAG_FRAMETYPE_DISPOSABLE = 3;
        public static byte FLAG_FRAMETYPE_INTERFRAME = 2;
        public static byte FLAG_FRAMETYPE_KEYFRAME = 1;
        public static byte FLAG_RATE_11_KHZ = 1;
        public static byte FLAG_RATE_22_KHZ = 2;
        public static byte FLAG_RATE_44_KHZ = 3;
        public static byte FLAG_RATE_5_5_KHZ = 0;
        public static byte FLAG_SIZE_16_BIT = 1;
        public static byte FLAG_SIZE_8_BIT = 0;
        public static byte FLAG_TYPE_MONO = 0;
        public static byte FLAG_TYPE_STEREO = 1;
        public static sbyte MASK_SOUND_FORMAT = -15;
        public static byte MASK_SOUND_RATE = 12;
        public static byte MASK_SOUND_SIZE = 2;
        public static byte MASK_SOUND_TYPE = 1;
        public static byte MASK_VIDEO_CODEC = 15;
        public static sbyte MASK_VIDEO_FRAMETYPE = -15;
        public static byte TYPE_AUDIO = 8;
        public static byte TYPE_METADATA = 0x12;
        public static byte TYPE_VIDEO = 9;
    }
}

