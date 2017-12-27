namespace FluorineFx.Messaging.Api.Messaging
{
    using FluorineFx.Messaging.Messages;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    [CLSCompliant(false)]
    public interface IMessageInput
    {
        ICollection GetConsumers();
        IMessage PullMessage();
        IMessage PullMessage(long wait);
        void SendOOBControlMessage(IConsumer consumer, OOBControlMessage oobCtrlMsg);
        bool Subscribe(IConsumer consumer, Dictionary<string, object> parameterMap);
        bool Unsubscribe(IConsumer consumer);
    }
}

