namespace FluorineFx.Messaging.Rtmp.Event
{
    using FluorineFx.Messaging.Api.Service;
    using FluorineFx.Util;
    using System;

    [CLSCompliant(false)]
    public class Invoke : Notify
    {
        internal Invoke()
        {
            base._dataType = 20;
        }

        internal Invoke(byte[] data) : base(data)
        {
            base._dataType = 20;
        }

        internal Invoke(IServiceCall serviceCall) : base(serviceCall)
        {
            base._dataType = 20;
        }

        internal Invoke(ByteBuffer data) : base(data)
        {
            base._dataType = 20;
        }
    }
}

