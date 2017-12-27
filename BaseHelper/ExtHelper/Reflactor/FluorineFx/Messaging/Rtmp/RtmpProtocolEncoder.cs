namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx.Messaging.Api.Service;
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Messaging.Rtmp.SO;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;

    internal sealed class RtmpProtocolEncoder
    {
        private static ILog _log = LogManager.GetLogger(typeof(RtmpProtocolEncoder));

        private static int CalculateHeaderSize(RtmpHeader header, RtmpHeader lastHeader)
        {
            int num;
            HeaderType headerType = GetHeaderType(header, lastHeader);
            if (header.ChannelId > 320)
            {
                num = 2;
            }
            else if (header.ChannelId > 0x3f)
            {
                num = 1;
            }
            else
            {
                num = 0;
            }
            return (RtmpHeader.GetHeaderLength(headerType) + num);
        }

        public static ByteBuffer Encode(RtmpContext context, object message)
        {
            try
            {
                if (message is ByteBuffer)
                {
                    return (ByteBuffer) message;
                }
                return EncodePacket(context, message as RtmpPacket);
            }
            catch (Exception exception)
            {
                if (_log != null)
                {
                    _log.Fatal("Error encoding object. ", exception);
                }
            }
            return null;
        }

        private static ByteBuffer EncodeAudioData(RtmpContext context, AudioData audioData)
        {
            return audioData.Data;
        }

        private static ByteBuffer EncodeBytesRead(RtmpContext context, BytesRead bytesRead)
        {
            ByteBuffer buffer = ByteBuffer.Allocate(4);
            buffer.PutInt(bytesRead.Bytes);
            return buffer;
        }

        private static ByteBuffer EncodeChunkSize(RtmpContext context, ChunkSize chunkSize)
        {
            ByteBuffer buffer = ByteBuffer.Allocate(4);
            buffer.PutInt(chunkSize.Size);
            return buffer;
        }

        private static ByteBuffer EncodeClientBW(RtmpContext context, ClientBW clientBW)
        {
            ByteBuffer buffer = ByteBuffer.Allocate(5);
            buffer.PutInt(clientBW.Bandwidth);
            buffer.Put(clientBW.Value2);
            return buffer;
        }

        private static ByteBuffer EncodeFlexInvoke(RtmpContext context, FlexInvoke invoke)
        {
            ByteBuffer stream = ByteBuffer.Allocate(0x400);
            stream.AutoExpand = true;
            RtmpWriter writer = new RtmpWriter(stream);
            writer.WriteByte(0);
            writer.WriteData(context.ObjectEncoding, invoke.Cmd);
            writer.WriteData(context.ObjectEncoding, invoke.InvokeId);
            writer.WriteData(context.ObjectEncoding, invoke.CmdData);
            object response = invoke.Response;
            writer.WriteData(context.ObjectEncoding, response);
            return stream;
        }

        private static ByteBuffer EncodeFlexSharedObject(RtmpContext context, ISharedObjectMessage so)
        {
            ByteBuffer output = ByteBuffer.Allocate(0x80);
            output.AutoExpand = true;
            output.Put((byte) 0);
            EncodeSharedObject(context, so, output);
            return output;
        }

        private static ByteBuffer EncodeFlexStreamSend(RtmpContext context, FlexStreamSend msg)
        {
            return msg.Data;
        }

        public static ByteBuffer EncodeHeader(RtmpHeader header, RtmpHeader lastHeader, ByteBuffer buffer)
        {
            HeaderType headerType = GetHeaderType(header, lastHeader);
            EncodeHeaderByte(buffer, (byte) headerType, header.ChannelId);
            switch (headerType)
            {
                case HeaderType.HeaderNew:
                    if (header.Timer >= 0xffffff)
                    {
                        buffer.WriteMediumInt(0xffffff);
                        break;
                    }
                    buffer.WriteMediumInt(header.Timer);
                    break;

                case HeaderType.HeaderSameSource:
                    if (header.Timer >= 0xffffff)
                    {
                        buffer.WriteMediumInt(0xffffff);
                    }
                    else
                    {
                        buffer.WriteMediumInt(header.Timer);
                    }
                    buffer.WriteMediumInt(header.Size);
                    buffer.Put(header.DataType);
                    goto Label_010A;

                case HeaderType.HeaderTimerChange:
                    if (header.Timer >= 0xffffff)
                    {
                        buffer.WriteMediumInt(0xffffff);
                    }
                    else
                    {
                        buffer.WriteMediumInt(header.Timer);
                    }
                    goto Label_010A;

                default:
                    goto Label_010A;
            }
            buffer.WriteMediumInt(header.Size);
            buffer.Put(header.DataType);
            buffer.WriteReverseInt(header.StreamId);
        Label_010A:
            if (header.Timer >= 0xffffff)
            {
                buffer.PutInt(header.Timer);
            }
            return buffer;
        }

        public static void EncodeHeaderByte(ByteBuffer output, byte headerSize, int channelId)
        {
            if (channelId <= 0x3f)
            {
                output.Put((byte) ((headerSize << 6) + channelId));
            }
            else if (channelId <= 320)
            {
                output.Put((byte) (headerSize << 6));
                output.Put((byte) (channelId - 0x40));
            }
            else
            {
                output.Put((byte) ((headerSize << 6) | 1));
                channelId -= 0x40;
                output.Put((byte) (channelId & 0xff));
                output.Put((byte) (channelId >> 8));
            }
        }

        private static ByteBuffer EncodeInvoke(RtmpContext context, Invoke invoke)
        {
            return EncodeNotifyOrInvoke(context, invoke);
        }

        public static ByteBuffer EncodeMessage(RtmpContext context, RtmpHeader header, IRtmpEvent message)
        {
            switch (header.DataType)
            {
                case 1:
                    return EncodeChunkSize(context, message as ChunkSize);

                case 3:
                    return EncodeBytesRead(context, message as BytesRead);

                case 4:
                    return EncodePing(context, message as Ping);

                case 5:
                    return EncodeServerBW(context, message as ServerBW);

                case 6:
                    return EncodeClientBW(context, message as ClientBW);

                case 8:
                    return EncodeAudioData(context, message as AudioData);

                case 9:
                    return EncodeVideoData(context, message as VideoData);

                case 15:
                    return EncodeFlexStreamSend(context, message as FlexStreamSend);

                case 0x10:
                    return EncodeFlexSharedObject(context, message as ISharedObjectMessage);

                case 0x11:
                    return EncodeFlexInvoke(context, message as FlexInvoke);

                case 0x12:
                    if ((message as Notify).ServiceCall != null)
                    {
                        return EncodeNotify(context, message as Notify);
                    }
                    return EncodeStreamMetadata(context, message as Notify);

                case 0x13:
                    return EncodeSharedObject(context, message as ISharedObjectMessage);

                case 20:
                    return EncodeInvoke(context, message as Invoke);
            }
            if (_log.get_IsErrorEnabled())
            {
                _log.Error("Unknown object type: " + header.DataType);
            }
            return null;
        }

        private static ByteBuffer EncodeNotify(RtmpContext context, Notify notify)
        {
            return EncodeNotifyOrInvoke(context, notify);
        }

        private static ByteBuffer EncodeNotifyOrInvoke(RtmpContext context, Notify invoke)
        {
            ByteBuffer stream = ByteBuffer.Allocate(0x400);
            stream.AutoExpand = true;
            RtmpWriter writer = new RtmpWriter(stream);
            IServiceCall serviceCall = invoke.ServiceCall;
            bool flag = serviceCall.Status == 1;
            if (!flag)
            {
                writer.WriteData(context.ObjectEncoding, "_result");
            }
            else
            {
                string data = (serviceCall.ServiceName == null) ? serviceCall.ServiceMethodName : (serviceCall.ServiceName + "." + serviceCall.ServiceMethodName);
                writer.WriteData(context.ObjectEncoding, data);
            }
            if (invoke is Invoke)
            {
                writer.WriteData(context.ObjectEncoding, invoke.InvokeId);
                writer.WriteData(context.ObjectEncoding, invoke.ConnectionParameters);
            }
            if (!flag && (invoke is Invoke))
            {
                IPendingServiceCall call2 = (IPendingServiceCall) serviceCall;
                if (!serviceCall.IsSuccess)
                {
                    StatusASO saso = GenerateErrorResult("NetConnection.Call.Failed", serviceCall.Exception);
                    call2.Result = saso;
                }
                writer.WriteData(context.ObjectEncoding, call2.Result);
                return stream;
            }
            object[] arguments = invoke.ServiceCall.Arguments;
            if (arguments != null)
            {
                foreach (object obj2 in arguments)
                {
                    writer.WriteData(context.ObjectEncoding, obj2);
                }
            }
            return stream;
        }

        public static ByteBuffer EncodePacket(RtmpContext context, RtmpPacket packet)
        {
            RtmpHeader header = packet.Header;
            int channelId = header.ChannelId;
            IRtmpEvent message = packet.Message;
            if (message is ChunkSize)
            {
                ChunkSize size = (ChunkSize) message;
                context.SetWriteChunkSize(size.Size);
            }
            ByteBuffer input = EncodeMessage(context, header, message);
            if (input.Position != 0L)
            {
                input.Flip();
            }
            else
            {
                input.Rewind();
            }
            header.Size = input.Limit;
            RtmpHeader lastWriteHeader = context.GetLastWriteHeader(channelId);
            int num2 = CalculateHeaderSize(header, lastWriteHeader);
            context.SetLastWriteHeader(channelId, header);
            context.SetLastWritePacket(channelId, packet);
            int writeChunkSize = context.GetWriteChunkSize();
            int num4 = 1;
            if (header.ChannelId > 320)
            {
                num4 = 3;
            }
            else if (header.ChannelId > 0x3f)
            {
                num4 = 2;
            }
            int num5 = (int) Math.Ceiling((double) (((float) header.Size) / ((float) writeChunkSize)));
            int capacity = (header.Size + num2) + ((num5 > 0) ? ((num5 - 1) * num4) : 0);
            ByteBuffer buffer = ByteBuffer.Allocate(capacity);
            EncodeHeader(header, lastWriteHeader, buffer);
            if (num5 == 1)
            {
                ByteBuffer.Put(buffer, input, buffer.Remaining);
            }
            else
            {
                for (int i = 0; i < (num5 - 1); i++)
                {
                    ByteBuffer.Put(buffer, input, writeChunkSize);
                    EncodeHeaderByte(buffer, 3, header.ChannelId);
                }
                ByteBuffer.Put(buffer, input, buffer.Remaining);
            }
            buffer.Flip();
            return buffer;
        }

        private static ByteBuffer EncodePing(RtmpContext context, Ping ping)
        {
            int capacity = 6;
            if (ping.Value3 != -1)
            {
                capacity += 4;
            }
            if (ping.Value4 != -1)
            {
                capacity += 4;
            }
            ByteBuffer buffer = ByteBuffer.Allocate(capacity);
            buffer.PutShort(ping.Value1);
            buffer.PutInt(ping.Value2);
            if (ping.Value3 != -1)
            {
                buffer.PutInt(ping.Value3);
            }
            if (ping.Value4 != -1)
            {
                buffer.PutInt(ping.Value4);
            }
            return buffer;
        }

        private static ByteBuffer EncodeServerBW(RtmpContext context, ServerBW serverBW)
        {
            ByteBuffer buffer = ByteBuffer.Allocate(4);
            buffer.PutInt(serverBW.Bandwidth);
            return buffer;
        }

        private static ByteBuffer EncodeSharedObject(RtmpContext context, ISharedObjectMessage so)
        {
            ByteBuffer output = ByteBuffer.Allocate(0x80);
            output.AutoExpand = true;
            EncodeSharedObject(context, so, output);
            return output;
        }

        private static void EncodeSharedObject(RtmpContext context, ISharedObjectMessage so, ByteBuffer output)
        {
            RtmpWriter writer = new RtmpWriter(output);
            writer.WriteUTF(so.Name);
            writer.WriteInt32(so.Version);
            writer.WriteInt32(so.IsPersistent ? 2 : 0);
            writer.WriteInt32(0);
            int num2 = 0;
            foreach (ISharedObjectEvent event2 in so.Events)
            {
                int position;
                byte num3 = SharedObjectTypeMapping.ToByte(event2.Type);
                switch (event2.Type)
                {
                    case SharedObjectEventType.SERVER_CONNECT:
                    case SharedObjectEventType.CLIENT_CLEAR_DATA:
                    case SharedObjectEventType.CLIENT_INITIAL_DATA:
                    {
                        writer.WriteByte(num3);
                        writer.WriteInt32(0);
                        continue;
                    }
                    case SharedObjectEventType.SERVER_DISCONNECT:
                    {
                        writer.WriteByte(num3);
                        output.PutInt((int) output.Position, 0);
                        continue;
                    }
                    case SharedObjectEventType.SERVER_SET_ATTRIBUTE:
                    case SharedObjectEventType.CLIENT_UPDATE_DATA:
                    {
                        if (event2.Key != null)
                        {
                            break;
                        }
                        IDictionary dictionary = event2.Value as IDictionary;
                        foreach (DictionaryEntry entry in dictionary)
                        {
                            writer.WriteByte(num3);
                            position = (int) output.Position;
                            output.Skip(4);
                            string key = entry.Key as string;
                            object data = entry.Value;
                            writer.WriteUTF(key);
                            writer.WriteData(context.ObjectEncoding, data);
                            num2 = (((int) output.Position) - position) - 4;
                            output.PutInt(position, num2);
                        }
                        continue;
                    }
                    case SharedObjectEventType.SERVER_DELETE_ATTRIBUTE:
                    case SharedObjectEventType.CLIENT_DELETE_DATA:
                    case SharedObjectEventType.CLIENT_UPDATE_ATTRIBUTE:
                    {
                        writer.WriteByte(num3);
                        position = (int) output.Position;
                        output.Skip(4);
                        writer.WriteUTF(event2.Key);
                        num2 = (((int) output.Position) - position) - 4;
                        output.PutInt(position, num2);
                        continue;
                    }
                    case SharedObjectEventType.SERVER_SEND_MESSAGE:
                    case SharedObjectEventType.CLIENT_SEND_MESSAGE:
                    {
                        writer.WriteByte(num3);
                        position = (int) output.Position;
                        output.Skip(4);
                        writer.WriteData(context.ObjectEncoding, event2.Key);
                        foreach (object obj3 in event2.Value as IList)
                        {
                            writer.WriteData(context.ObjectEncoding, obj3);
                        }
                        num2 = (((int) output.Position) - position) - 4;
                        output.PutInt(position, num2);
                        continue;
                    }
                    case SharedObjectEventType.CLIENT_STATUS:
                    {
                        writer.WriteByte(num3);
                        position = (int) output.Position;
                        output.Skip(4);
                        writer.WriteUTF(event2.Key);
                        writer.WriteUTF(event2.Value as string);
                        num2 = (((int) output.Position) - position) - 4;
                        output.PutInt(position, num2);
                        continue;
                    }
                    default:
                        goto Label_033A;
                }
                writer.WriteByte(num3);
                position = (int) output.Position;
                output.Skip(4);
                writer.WriteUTF(event2.Key);
                writer.WriteData(context.ObjectEncoding, event2.Value);
                num2 = (((int) output.Position) - position) - 4;
                output.PutInt(position, num2);
                continue;
            Label_033A:
                _log.Error("Unknown event " + event2.Type.ToString());
                writer.WriteByte(num3);
                position = (int) output.Position;
                output.Skip(4);
                if (event2.Key != null)
                {
                    writer.WriteUTF(event2.Key);
                    writer.WriteData(context.ObjectEncoding, event2.Value);
                }
                num2 = (((int) output.Position) - position) - 4;
                output.PutInt(position, num2);
            }
        }

        private static ByteBuffer EncodeStreamMetadata(RtmpContext context, Notify metaData)
        {
            return metaData.Data;
        }

        private static ByteBuffer EncodeVideoData(RtmpContext context, VideoData videoData)
        {
            return videoData.Data;
        }

        private static StatusASO GenerateErrorResult(string code, Exception exception)
        {
            string description = "";
            if ((exception != null) && (exception.Message != null))
            {
                description = exception.Message;
            }
            return new StatusASO(code, "error", description);
        }

        private static HeaderType GetHeaderType(RtmpHeader header, RtmpHeader lastHeader)
        {
            if (!(((lastHeader != null) && (header.StreamId == lastHeader.StreamId)) && header.IsTimerRelative))
            {
                return HeaderType.HeaderNew;
            }
            if ((header.Size != lastHeader.Size) || (header.DataType != lastHeader.DataType))
            {
                return HeaderType.HeaderSameSource;
            }
            if (header.Timer != lastHeader.Timer)
            {
                return HeaderType.HeaderTimerChange;
            }
            return HeaderType.HeaderContinue;
        }
    }
}

