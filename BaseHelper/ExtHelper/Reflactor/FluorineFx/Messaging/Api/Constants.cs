namespace FluorineFx.Messaging.Api
{
    using System;

    internal class Constants
    {
        public const int AudioChannel = 0;
        public const string BroadcastScopeStreamAttribute = "_transient_publishing_stream";
        public const string BroadcastScopeType = "bs";
        public const string ClientStreamModeAppend = "append";
        public const string ClientStreamModeLive = "live";
        public const string ClientStreamModeRead = "read";
        public const string ClientStreamModeRecord = "record";
        public const int DataChannel = 2;
        public const int MaxChannelConfigCount = 4;
        public const int OverallChannel = 3;
        public const string ProviderService = "ProviderService";
        public const string SharedObjectSecurityService = "SharedObjectSecurityService";
        public const string SharedObjectService = "SharedObjectService";
        public const string StreamableFileFactory = "StreamableFileFactory";
        public const string StreamAttribute = "_transient_publishing_stream";
        public const string StreamFilenameGenerator = "StreamFilenameGenerator";
        public const string StreamSecurityService = "StreamSecurityService";
        public const string StreamServiceType = "streamService";
        public const string TransientPrefix = "_transient";
        public const byte TypeAudioData = 8;
        public const byte TypeBytesRead = 3;
        public const byte TypeChunkSize = 1;
        public const byte TypeClientBandwidth = 6;
        public const byte TypeFlexInvoke = 0x11;
        public const byte TypeFlexSharedObject = 0x10;
        public const byte TypeFlexStreamEnd = 15;
        public const byte TypeInvoke = 20;
        public const byte TypeNotify = 0x12;
        public const byte TypePing = 4;
        public const byte TypeServerBandwidth = 5;
        public const byte TypeSharedObject = 0x13;
        public const byte TypeStreamMetadata = 0x12;
        public const byte TypeUnknown = 0;
        public const byte TypeVideoData = 9;
        public const int VideoChannel = 1;
    }
}

