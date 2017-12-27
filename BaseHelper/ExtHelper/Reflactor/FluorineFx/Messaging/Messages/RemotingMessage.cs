namespace FluorineFx.Messaging.Messages
{
    using System;

    [CLSCompliant(false)]
    public class RemotingMessage : MessageBase
    {
        private string _operation;
        private string _source;

        public string operation
        {
            get
            {
                return this._operation;
            }
            set
            {
                this._operation = value;
            }
        }

        public string source
        {
            get
            {
                return this._source;
            }
            set
            {
                this._source = value;
            }
        }
    }
}

