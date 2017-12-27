namespace FluorineFx.Data
{
    using FluorineFx.Data.Messages;
    using System;
    using System.Collections;

    internal class MessageBatch
    {
        private DataMessage _incomingMessage;
        private IList _messages;

        public MessageBatch(DataMessage incomingMessage, IList messages)
        {
            this._incomingMessage = incomingMessage;
            this._messages = messages;
        }

        public DataMessage IncomingMessage
        {
            get
            {
                return this._incomingMessage;
            }
        }

        public IList Messages
        {
            get
            {
                return this._messages;
            }
        }
    }
}

