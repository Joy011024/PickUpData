namespace FluorineFx.Messaging.Api.Messaging
{
    using System;

    [CLSCompliant(false)]
    public interface IPipe : IMessageInput, IMessageOutput
    {
        void AddPipeConnectionListener(IPipeConnectionListener listener);
        void RemovePipeConnectionListener(IPipeConnectionListener listener);
    }
}

