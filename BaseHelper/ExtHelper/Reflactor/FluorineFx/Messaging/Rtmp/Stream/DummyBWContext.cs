namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Api;
    using System;

    internal class DummyBWContext : IBWControlContext
    {
        private IBWControllable _controllable;

        public DummyBWContext(IBWControllable controllable)
        {
            this._controllable = controllable;
        }

        public IBWControllable GetBWControllable()
        {
            return this._controllable;
        }
    }
}

