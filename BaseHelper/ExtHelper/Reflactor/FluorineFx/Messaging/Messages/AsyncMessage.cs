namespace FluorineFx.Messaging.Messages
{
    using System;

    [CLSCompliant(false)]
    public class AsyncMessage : MessageBase
    {
        protected string _correlationId;
        public const string SubtopicHeader = "DSSubtopic";

        public string correlationId
        {
            get
            {
                return this._correlationId;
            }
            set
            {
                this._correlationId = value;
            }
        }
    }
}

