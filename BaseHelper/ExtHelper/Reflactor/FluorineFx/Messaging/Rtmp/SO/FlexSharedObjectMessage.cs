namespace FluorineFx.Messaging.Rtmp.SO
{
    using FluorineFx.Messaging.Api.Event;
    using System;

    internal class FlexSharedObjectMessage : SharedObjectMessage
    {
        public FlexSharedObjectMessage(string name, int version, bool persistent) : this(null, name, version, persistent)
        {
        }

        public FlexSharedObjectMessage(IEventListener source, string name, int version, bool persistent) : base(source, name, version, persistent)
        {
            base._dataType = 0x10;
        }
    }
}

