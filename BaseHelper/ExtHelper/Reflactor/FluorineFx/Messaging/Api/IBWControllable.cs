namespace FluorineFx.Messaging.Api
{
    using System;

    public interface IBWControllable
    {
        IBWControllable GetParentBWControllable();

        IBandwidthConfigure BandwidthConfiguration { get; set; }
    }
}

