namespace FluorineFx.Messaging.Api.Stream.Support
{
    using FluorineFx.Messaging.Api.Messaging;
    using FluorineFx.Messaging.Api.Stream;
    using System;

    [CLSCompliant(false)]
    public class SimplePlayItem : IPlayItem
    {
        private long _length = -1L;
        private IMessageInput _msgInput;
        private string _name;
        private long _start = -2L;

        public long Length
        {
            get
            {
                return this._length;
            }
            set
            {
                this._length = value;
            }
        }

        public IMessageInput MessageInput
        {
            get
            {
                return this._msgInput;
            }
            set
            {
                this._msgInput = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public long Start
        {
            get
            {
                return this._start;
            }
            set
            {
                this._start = value;
            }
        }
    }
}

