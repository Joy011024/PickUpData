namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx;
    using FluorineFx.Exceptions;
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Messaging.Rtmp.Service;
    using FluorineFx.Messaging.Rtmp.SO;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal sealed class RtmpProtocolDecoder
    {
        public const int HandshakeSize = 0x600;
        private static readonly ILog log = LogManager.GetLogger(typeof(RtmpProtocolDecoder));

        public static object Decode(RtmpContext context, ByteBuffer stream)
        {
            object obj2;
            long position = stream.Position;
            try
            {
                switch (context.State)
                {
                    case RtmpState.Connect:
                    case RtmpState.Handshake:
                        return DecodeHandshake(context, stream);

                    case RtmpState.Connected:
                        return DecodePacket(context, stream);

                    case RtmpState.Error:
                        return null;
                }
                obj2 = null;
            }
            catch (Exception exception)
            {
                throw new ProtocolException("Error during decoding", exception);
            }
            return obj2;
        }

        private static AudioData DecodeAudioData(ByteBuffer stream)
        {
            return new AudioData(stream);
        }

        public static List<object> DecodeBuffer(RtmpContext context, ByteBuffer stream)
        {
            List<object> list = null;
            try
            {
                long num;
                bool flag;
                goto Label_0099;
            Label_0009:
                num = stream.Remaining;
                if (context.CanStartDecoding(num))
                {
                    context.StartDecoding();
                    if (context.State == RtmpState.Disconnected)
                    {
                        return list;
                    }
                    object item = Decode(context, stream);
                    if (context.HasDecodedObject)
                    {
                        if (list == null)
                        {
                            list = new List<object>();
                        }
                        list.Add(item);
                        goto Label_008A;
                    }
                    if (context.CanContinueDecoding)
                    {
                        goto Label_0099;
                    }
                }
                return list;
            Label_008A:
                if (!stream.HasRemaining)
                {
                    return list;
                }
            Label_0099:
                flag = true;
                goto Label_0009;
            }
            catch
            {
                throw;
            }
            finally
            {
                stream.Compact();
            }
            return list;
        }

        private static BytesRead DecodeBytesRead(ByteBuffer stream)
        {
            return new BytesRead(stream.ReadReverseInt());
        }

        public static byte DecodeChannelId(int headerByte, int byteCount)
        {
            if (byteCount == 1)
            {
                return (byte) (headerByte & 0x3f);
            }
            if (byteCount == 2)
            {
                return (byte) (0x40 + (headerByte & 0xff));
            }
            return (byte) ((0x40 + ((headerByte >> 8) & 0xff)) + ((headerByte & 0xff) << 8));
        }

        private static ChunkSize DecodeChunkSize(ByteBuffer stream)
        {
            return new ChunkSize(stream.GetInt());
        }

        private static ClientBW DecodeClientBW(ByteBuffer stream)
        {
            return new ClientBW(stream.GetInt(), stream.Get());
        }

        private static FlexInvoke DecodeFlexInvoke(ByteBuffer stream)
        {
            int num = stream.ReadByte();
            RtmpReader reader = new RtmpReader(stream);
            string cmd = reader.ReadData() as string;
            int invokeId = Convert.ToInt32(reader.ReadData());
            object cmdData = reader.ReadData();
            List<object> list = new List<object>();
            while (stream.HasRemaining)
            {
                object item = reader.ReadData();
                list.Add(item);
            }
            object[] parameters = list.ToArray();
            FlexInvoke invoke = new FlexInvoke(cmd, invokeId, cmdData, parameters);
            int length = (cmd == null) ? -1 : cmd.LastIndexOf(".");
            string name = (length == -1) ? null : cmd.Substring(0, length);
            string method = (length == -1) ? cmd : cmd.Substring(length + 1, (cmd.Length - length) - 1);
            PendingCall call = new PendingCall(name, method, parameters);
            invoke.ServiceCall = call;
            return invoke;
        }

        private static ISharedObjectMessage DecodeFlexSharedObject(ByteBuffer stream)
        {
            stream.Skip(1);
            RtmpReader reader = new RtmpReader(stream);
            string name = reader.ReadString();
            int version = reader.ReadInt32();
            bool persistent = reader.ReadInt32() == 2;
            reader.ReadInt32();
            SharedObjectMessage so = new FlexSharedObjectMessage(null, name, version, persistent);
            DecodeSharedObject(so, stream, reader);
            return so;
        }

        public static ByteBuffer DecodeHandshake(RtmpContext context, ByteBuffer stream)
        {
            ByteBuffer buffer;
            long remaining = stream.Remaining;
            if (context.Mode == RtmpMode.Server)
            {
                if (context.State == RtmpState.Connect)
                {
                    if (remaining < 0x601L)
                    {
                        if (log.get_IsDebugEnabled())
                        {
                            log.Debug(__Res.GetString("Rtmp_HSInitBuffering", new object[] { remaining }));
                        }
                        context.SetBufferDecoding(0x601L);
                        return null;
                    }
                    buffer = ByteBuffer.Allocate(0xc01);
                    buffer.Put((byte) 3);
                    buffer.PutInt(1);
                    buffer.Fill(0, 0x5fc);
                    stream.Get();
                    ByteBuffer.Put(buffer, stream, 0x600);
                    buffer.Flip();
                    context.State = RtmpState.Handshake;
                    return buffer;
                }
                if (context.State == RtmpState.Handshake)
                {
                    if (remaining < 0x600L)
                    {
                        if (log.get_IsDebugEnabled())
                        {
                            log.Debug(__Res.GetString("Rtmp_HSReplyBuffering", new object[] { remaining }));
                        }
                        context.SetBufferDecoding(0x600L);
                        return null;
                    }
                    stream.Skip(0x600);
                    context.State = RtmpState.Connected;
                    context.ContinueDecoding();
                    return null;
                }
            }
            else if (context.State == RtmpState.Connect)
            {
                int capacity = 0xc01;
                if (remaining < capacity)
                {
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("Rtmp_HSInitBuffering", new object[] { remaining }));
                    }
                    context.SetBufferDecoding((long) capacity);
                    return null;
                }
                buffer = ByteBuffer.Allocate(capacity);
                ByteBuffer.Put(buffer, stream, capacity);
                buffer.Flip();
                context.State = RtmpState.Handshake;
                return buffer;
            }
            return null;
        }

        public static RtmpHeader DecodeHeader(RtmpContext context, RtmpHeader lastHeader, ByteBuffer stream)
        {
            int num2;
            byte num = stream.Get();
            int byteCount = 1;
            if ((num & 0x3f) == 0)
            {
                num2 = ((num & 0xff) << 8) | (stream.Get() & 0xff);
                byteCount = 2;
            }
            else if ((num & 0x3f) == 1)
            {
                num2 = (((num & 0xff) << 0x10) | ((stream.Get() & 0xff) << 8)) | (stream.Get() & 0xff);
                byteCount = 3;
            }
            else
            {
                num2 = num & 0xff;
                byteCount = 1;
            }
            byte num4 = DecodeChannelId(num2, byteCount);
            byte num5 = DecodeHeaderSize(num2, byteCount);
            RtmpHeader header = new RtmpHeader {
                ChannelId = num4,
                IsTimerRelative = num5 != 0
            };
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("Rtmp_DecodeHeader", new object[] { Enum.GetName(typeof(HeaderType), (HeaderType) num5) }));
            }
            switch (((HeaderType) num5))
            {
                case HeaderType.HeaderNew:
                    header.Timer = stream.ReadUInt24();
                    header.Size = stream.ReadUInt24();
                    header.DataType = stream.Get();
                    header.StreamId = stream.ReadReverseInt();
                    break;

                case HeaderType.HeaderSameSource:
                    header.Timer = stream.ReadUInt24();
                    header.Size = stream.ReadUInt24();
                    header.DataType = stream.Get();
                    header.StreamId = lastHeader.StreamId;
                    break;

                case HeaderType.HeaderTimerChange:
                    header.Timer = stream.ReadUInt24();
                    header.Size = lastHeader.Size;
                    header.DataType = lastHeader.DataType;
                    header.StreamId = lastHeader.StreamId;
                    break;

                case HeaderType.HeaderContinue:
                    header.Timer = lastHeader.Timer;
                    header.Size = lastHeader.Size;
                    header.DataType = lastHeader.DataType;
                    header.StreamId = lastHeader.StreamId;
                    header.IsTimerRelative = lastHeader.IsTimerRelative;
                    break;

                default:
                    log.Error("Unexpected header size: " + num5);
                    return null;
            }
            if (header.Timer >= 0xffffff)
            {
                header.Timer = stream.GetInt();
            }
            return header;
        }

        public static byte DecodeHeaderSize(int headerByte, int byteCount)
        {
            if (byteCount == 1)
            {
                return (byte) (headerByte >> 6);
            }
            if (byteCount == 2)
            {
                return (byte) (headerByte >> 14);
            }
            return (byte) (headerByte >> 0x16);
        }

        private static Invoke DecodeInvoke(ByteBuffer stream)
        {
            return (DecodeNotifyOrInvoke(new Invoke(), stream, null) as Invoke);
        }

        public static IRtmpEvent DecodeMessage(RtmpContext context, RtmpHeader header, ByteBuffer stream)
        {
            IRtmpEvent event2 = null;
            switch (header.DataType)
            {
                case 1:
                    event2 = DecodeChunkSize(stream);
                    break;

                case 3:
                    event2 = DecodeBytesRead(stream);
                    break;

                case 4:
                    event2 = DecodePing(stream);
                    break;

                case 5:
                    event2 = DecodeServerBW(stream);
                    break;

                case 6:
                    event2 = DecodeClientBW(stream);
                    break;

                case 8:
                    event2 = DecodeAudioData(stream);
                    break;

                case 9:
                    event2 = DecodeVideoData(stream);
                    break;

                case 0x10:
                    event2 = DecodeFlexSharedObject(stream);
                    break;

                case 0x11:
                    event2 = DecodeFlexInvoke(stream);
                    break;

                case 0x12:
                    if (header.StreamId != 0)
                    {
                        event2 = DecodeStreamMetadata(stream);
                    }
                    else
                    {
                        event2 = DecodeNotify(stream, header);
                    }
                    break;

                case 0x13:
                    event2 = DecodeSharedObject(stream);
                    break;

                case 20:
                    event2 = DecodeInvoke(stream);
                    break;

                default:
                    log.Warn("Unknown object type: " + header.DataType);
                    event2 = DecodeUnknown(stream);
                    break;
            }
            event2.Header = header;
            event2.Timestamp = header.Timer;
            return event2;
        }

        private static Notify DecodeNotify(ByteBuffer stream, RtmpHeader header)
        {
            return DecodeNotifyOrInvoke(new Notify(), stream, header);
        }

        private static Notify DecodeNotifyOrInvoke(Notify notify, ByteBuffer stream, RtmpHeader header)
        {
            long position = stream.Position;
            RtmpReader reader = new RtmpReader(stream);
            string str = reader.ReadData() as string;
            if (!(notify is Invoke))
            {
                stream.Position = position;
                return notify;
            }
            if ((header == null) || (header.StreamId == 0))
            {
                double num2 = (double) reader.ReadData();
                notify.InvokeId = (int) num2;
            }
            object[] args = new object[0];
            if (stream.HasRemaining)
            {
                List<object> list = new List<object>();
                object item = reader.ReadData();
                if (item is IDictionary)
                {
                    notify.ConnectionParameters = item as IDictionary;
                }
                else if (item != null)
                {
                    list.Add(item);
                }
                while (stream.HasRemaining)
                {
                    list.Add(reader.ReadData());
                }
                args = list.ToArray();
            }
            int length = str.LastIndexOf(".");
            string name = (length == -1) ? null : str.Substring(0, length);
            string method = (length == -1) ? str : str.Substring(length + 1, (str.Length - length) - 1);
            if (notify is Invoke)
            {
                PendingCall call = new PendingCall(name, method, args);
                (notify as Invoke).ServiceCall = call;
            }
            else
            {
                Call call2 = new Call(name, method, args);
                notify.ServiceCall = call2;
            }
            return notify;
        }

        public static RtmpPacket DecodePacket(RtmpContext context, ByteBuffer stream)
        {
            int num4;
            int num5;
            int remaining = stream.Remaining;
            if (remaining < 1)
            {
                context.SetBufferDecoding(1L);
                return null;
            }
            int position = (int) stream.Position;
            byte num3 = stream.Get();
            if ((num3 & 0x3f) == 0)
            {
                if (remaining < 2)
                {
                    stream.Position = position;
                    context.SetBufferDecoding(2L);
                    return null;
                }
                num4 = ((num3 & 0xff) << 8) | (stream.Get() & 0xff);
                num5 = 2;
            }
            else if ((num3 & 0x3f) == 1)
            {
                if (remaining < 3)
                {
                    stream.Position = position;
                    context.SetBufferDecoding(3L);
                    return null;
                }
                num4 = (((num3 & 0xff) << 0x10) | ((stream.Get() & 0xff) << 8)) | (stream.Get() & 0xff);
                num5 = 3;
            }
            else
            {
                num4 = num3 & 0xff;
                num5 = 1;
            }
            byte channelId = DecodeChannelId(num4, num5);
            if (channelId < 0)
            {
                throw new ProtocolException("Bad channel id: " + channelId);
            }
            int num8 = GetHeaderLength(DecodeHeaderSize(num4, num5)) + (num5 - 1);
            if (num8 > remaining)
            {
                if (log.get_IsDebugEnabled())
                {
                    log.Debug(__Res.GetString("Rtmp_HeaderBuffering", new object[] { remaining }));
                }
                stream.Position = position;
                context.SetBufferDecoding((long) num8);
                return null;
            }
            stream.Position = position;
            RtmpHeader header = DecodeHeader(context, context.GetLastReadHeader(channelId), stream);
            log.Debug("Decoded header " + header);
            if (header == null)
            {
                throw new ProtocolException("Header is null, check for error");
            }
            context.SetLastReadHeader(channelId, header);
            RtmpPacket lastReadPacket = context.GetLastReadPacket(channelId);
            if (lastReadPacket == null)
            {
                lastReadPacket = new RtmpPacket(header);
                context.SetLastReadPacket(channelId, lastReadPacket);
            }
            ByteBuffer data = lastReadPacket.Data;
            int num9 = 0;
            int num10 = (header.Size + num9) - ((int) data.Position);
            int readChunkSize = context.GetReadChunkSize();
            int numBytesMax = (num10 > readChunkSize) ? readChunkSize : num10;
            if (stream.Remaining < numBytesMax)
            {
                if (log.get_IsDebugEnabled())
                {
                    log.Debug(__Res.GetString("Rtmp_ChunkSmall", new object[] { stream.Remaining, numBytesMax }));
                }
                stream.Position = position;
                context.SetBufferDecoding((long) (num8 + numBytesMax));
                return null;
            }
            ByteBuffer.Put(data, stream, numBytesMax);
            if (data.Position < (header.Size + num9))
            {
                context.ContinueDecoding();
                return null;
            }
            data.Flip();
            IRtmpEvent event2 = DecodeMessage(context, header, data);
            lastReadPacket.Message = event2;
            if (event2 is ChunkSize)
            {
                ChunkSize size = event2 as ChunkSize;
                context.SetReadChunkSize(size.Size);
            }
            context.SetLastReadPacket(channelId, null);
            return lastReadPacket;
        }

        private static Ping DecodePing(ByteBuffer stream)
        {
            Ping ping = new Ping {
                Value1 = stream.GetShort(),
                Value2 = stream.GetInt()
            };
            if (stream.HasRemaining)
            {
                ping.Value3 = stream.GetInt();
            }
            if (stream.HasRemaining)
            {
                ping.Value4 = stream.GetInt();
            }
            return ping;
        }

        private static ServerBW DecodeServerBW(ByteBuffer stream)
        {
            return new ServerBW(stream.GetInt());
        }

        private static ISharedObjectMessage DecodeSharedObject(ByteBuffer stream)
        {
            RtmpReader reader = new RtmpReader(stream);
            string name = reader.ReadString();
            int version = reader.ReadInt32();
            bool persistent = reader.ReadInt32() == 2;
            reader.ReadInt32();
            SharedObjectMessage so = new SharedObjectMessage(null, name, version, persistent);
            DecodeSharedObject(so, stream, reader);
            return so;
        }

        private static void DecodeSharedObject(SharedObjectMessage so, ByteBuffer stream, RtmpReader reader)
        {
            while (stream.HasRemaining)
            {
                SharedObjectEventType type = SharedObjectTypeMapping.ToType(reader.ReadByte());
                string key = null;
                object obj2 = null;
                int @int = stream.GetInt();
                switch (type)
                {
                    case SharedObjectEventType.CLIENT_STATUS:
                        key = reader.ReadString();
                        obj2 = reader.ReadString();
                        break;

                    case SharedObjectEventType.CLIENT_UPDATE_DATA:
                        key = reader.ReadString();
                        obj2 = reader.ReadData();
                        break;

                    default:
                        if ((type != SharedObjectEventType.SERVER_SEND_MESSAGE) && (type != SharedObjectEventType.CLIENT_SEND_MESSAGE))
                        {
                            if (@int > 0)
                            {
                                key = reader.ReadString();
                                if (@int > (key.Length + 2))
                                {
                                    obj2 = reader.ReadData();
                                }
                            }
                        }
                        else
                        {
                            int position = (int) stream.Position;
                            key = reader.ReadData() as string;
                            List<object> list = new List<object>();
                            while ((stream.Position - position) < @int)
                            {
                                object item = reader.ReadData();
                                list.Add(item);
                            }
                            obj2 = list;
                        }
                        break;
                }
                so.AddEvent(type, key, obj2);
            }
        }

        private static Notify DecodeStreamMetadata(ByteBuffer stream)
        {
            return new Notify(stream);
        }

        private static Unknown DecodeUnknown(ByteBuffer stream)
        {
            return new Unknown(stream);
        }

        private static VideoData DecodeVideoData(ByteBuffer stream)
        {
            return new VideoData(stream);
        }

        public static int GetHeaderLength(byte headerSize)
        {
            switch (((HeaderType) headerSize))
            {
                case HeaderType.HeaderNew:
                    return 12;

                case HeaderType.HeaderSameSource:
                    return 8;

                case HeaderType.HeaderTimerChange:
                    return 4;

                case HeaderType.HeaderContinue:
                    return 1;
            }
            return -1;
        }
    }
}

