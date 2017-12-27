namespace FluorineFx.Messaging.Api.Messaging
{
    using System;

    [CLSCompliant(false)]
    public interface IMessageComponent
    {
        void OnOOBControlMessage(IMessageComponent source, IPipe pipe, OOBControlMessage oobCtrlMsg);
    }
}

