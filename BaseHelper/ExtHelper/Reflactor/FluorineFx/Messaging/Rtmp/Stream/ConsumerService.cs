namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Configuration;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Messaging;
    using FluorineFx.Messaging.Api.Stream;
    using FluorineFx.Messaging.Rtmp;
    using FluorineFx.Messaging.Rtmp.Stream.Consumer;
    using System;

    internal class ConsumerService : IConsumerService, IScopeService, IService
    {
        public IMessageOutput GetConsumerOutput(IClientStream stream)
        {
            IStreamCapableConnection connection = stream.Connection;
            if (!((connection != null) && (connection is RtmpConnection)))
            {
                return null;
            }
            RtmpConnection connection2 = connection as RtmpConnection;
            OutputStream stream2 = connection2.CreateOutputStream(stream.StreamId);
            IPipe pipe = new InMemoryPushPushPipe();
            pipe.Subscribe(new ConnectionConsumer(connection2, stream2.Video.ChannelId, stream2.Audio.ChannelId, stream2.Data.ChannelId), null);
            return pipe;
        }

        public void Shutdown()
        {
        }

        public void Start(ConfigurationSection configuration)
        {
        }
    }
}

