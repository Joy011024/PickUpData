namespace FluorineFx.Data.Messages
{
    using FluorineFx.Messaging.Messages;
    using System;

    [CLSCompliant(false)]
    public class SequencedMessage : AcknowledgeMessage
    {
        private DataMessage _dataMessage;
        private int _sequenceId;
        private object[] _sequenceProxies;
        private int _sequenceSize;

        public DataMessage dataMessage
        {
            get
            {
                return this._dataMessage;
            }
            set
            {
                this._dataMessage = value;
            }
        }

        public int sequenceId
        {
            get
            {
                return this._sequenceId;
            }
            set
            {
                this._sequenceId = value;
            }
        }

        public object[] sequenceProxies
        {
            get
            {
                return this._sequenceProxies;
            }
            set
            {
                this._sequenceProxies = value;
            }
        }

        public int sequenceSize
        {
            get
            {
                return this._sequenceSize;
            }
            set
            {
                this._sequenceSize = value;
            }
        }
    }
}

