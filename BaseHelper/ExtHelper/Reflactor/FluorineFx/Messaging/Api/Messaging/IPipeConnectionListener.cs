namespace FluorineFx.Messaging.Api.Messaging
{
    using System;

    [CLSCompliant(false)]
    public interface IPipeConnectionListener
    {
        void OnPipeConnectionEvent(PipeConnectionEvent evt);
    }
}

