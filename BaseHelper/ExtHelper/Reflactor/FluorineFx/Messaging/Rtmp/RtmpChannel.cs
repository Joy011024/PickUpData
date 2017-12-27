namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx.Messaging.Api.Stream;
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Messaging.Rtmp.Service;
    using System;

    [CLSCompliant(false)]
    public class RtmpChannel
    {
        private int _channelId;
        private RtmpConnection _connection;

        internal RtmpChannel(RtmpConnection connection, int channelId)
        {
            this._connection = connection;
            this._channelId = channelId;
        }

        public void Close()
        {
            this._connection.CloseChannel(this._channelId);
        }

        public void SendStatus(StatusASO status)
        {
            Invoke invoke;
            if (!status.code.Equals("NetStream.Data.Start"))
            {
                PendingCall call = new PendingCall(null, "onStatus", new object[] { status });
                invoke = new Invoke {
                    InvokeId = 1,
                    ServiceCall = call
                };
            }
            else
            {
                Call call2 = new Call(null, "onStatus", new object[] { status });
                invoke = (Invoke) new Notify();
                invoke.InvokeId = 1;
                invoke.ServiceCall = call2;
            }
            this.Write(invoke, this._connection.GetStreamIdForChannel(this._channelId));
        }

        public override string ToString()
        {
            return ("RtmpChannel " + this._channelId);
        }

        public void Write(IRtmpEvent message)
        {
            IClientStream streamByChannelId = null;
            if (this._connection is IStreamCapableConnection)
            {
                streamByChannelId = (this._connection as IStreamCapableConnection).GetStreamByChannelId(this._channelId);
            }
            if ((this._channelId <= 3) || (streamByChannelId != null))
            {
                int streamId = (streamByChannelId == null) ? 0 : streamByChannelId.StreamId;
                this.Write(message, streamId);
            }
        }

        private void Write(IRtmpEvent message, int streamId)
        {
            RtmpHeader header = new RtmpHeader();
            RtmpPacket packet = new RtmpPacket(header, message);
            header.ChannelId = this._channelId;
            header.Timer = message.Timestamp;
            header.StreamId = streamId;
            header.DataType = message.DataType;
            if (message.Header != null)
            {
                header.IsTimerRelative = message.Header.IsTimerRelative;
            }
            this._connection.Write(packet);
        }

        public int ChannelId
        {
            get
            {
                return this._channelId;
            }
        }

        public RtmpConnection Connection
        {
            get
            {
                return this._connection;
            }
        }
    }
}

