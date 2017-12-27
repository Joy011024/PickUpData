namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api.Messaging;
    using System;

    [CLSCompliant(false)]
    public interface IPlayItem
    {
        long Length { get; }

        IMessageInput MessageInput { get; }

        string Name { get; }

        long Start { get; }
    }
}

