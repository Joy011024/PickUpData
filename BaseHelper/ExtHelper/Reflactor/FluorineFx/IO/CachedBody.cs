namespace FluorineFx.IO
{
    using FluorineFx;
    using FluorineFx.Messaging.Messages;
    using System;

    internal class CachedBody : ResponseBody
    {
        public CachedBody(AMFBody requestBody, object content) : base(requestBody, content)
        {
        }

        public CachedBody(AMFBody requestBody, IMessage message, object content) : base(requestBody, content)
        {
            AcknowledgeMessage message2 = new AcknowledgeMessage {
                body = content,
                correlationId = message.messageId,
                destination = message.destination,
                clientId = message.clientId
            };
            base.Content = message2;
        }

        protected override void WriteBodyData(ObjectEncoding objectEncoding, AMFWriter writer)
        {
            writer.WriteData(objectEncoding, base.Content);
        }
    }
}

