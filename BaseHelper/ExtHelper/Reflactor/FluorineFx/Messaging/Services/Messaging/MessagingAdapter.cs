namespace FluorineFx.Messaging.Services.Messaging
{
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Services;
    using System;

    [CLSCompliant(false)]
    public class MessagingAdapter : ServiceAdapter
    {
        public bool AllowSend(Subtopic subtopic)
        {
            return true;
        }

        public bool AllowSubscribe(Subtopic subtopic)
        {
            return true;
        }

        public override object Invoke(IMessage message)
        {
            (base.Destination.Service as MessageService).PushMessageToClients(message);
            return null;
        }
    }
}

