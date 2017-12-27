namespace FluorineFx.Messaging.Api.Messaging
{
    using FluorineFx.Messaging.Messages;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    [CLSCompliant(false)]
    public interface IMessageOutput
    {
        ICollection GetProviders();
        void PushMessage(IMessage message);
        void SendOOBControlMessage(IProvider provider, OOBControlMessage oobCtrlMsg);
        bool Subscribe(IProvider provider, Dictionary<string, object> parameterMap);
        bool Unsubscribe(IProvider provider);
    }
}

