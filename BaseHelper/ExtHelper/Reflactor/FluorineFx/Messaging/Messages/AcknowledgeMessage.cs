namespace FluorineFx.Messaging.Messages
{
    using System;

    [CLSCompliant(false)]
    public class AcknowledgeMessage : AsyncMessage
    {
        public AcknowledgeMessage()
        {
            base._messageId = Guid.NewGuid().ToString("D");
            base._timestamp = Environment.TickCount;
        }
    }
}

