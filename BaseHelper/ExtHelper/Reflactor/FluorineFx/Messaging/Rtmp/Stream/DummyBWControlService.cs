namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Configuration;
    using FluorineFx.Messaging.Api;
    using System;
    using System.Collections;

    internal class DummyBWControlService : IBWControlService, IService
    {
        private Hashtable _contextMap = new Hashtable();
        private ITokenBucket _dummyBucket = new DummyTokenBukcet();

        public ITokenBucket GetAudioBucket(IBWControlContext context)
        {
            return this._dummyBucket;
        }

        public ITokenBucket GetDataBucket(IBWControlContext context)
        {
            return this._dummyBucket;
        }

        public ITokenBucket GetVideoBucket(IBWControlContext context)
        {
            return this._dummyBucket;
        }

        public IBWControlContext LookupContext(IBWControllable bc)
        {
            lock (this._contextMap.SyncRoot)
            {
                return (this._contextMap[bc] as IBWControlContext);
            }
        }

        public IBWControlContext RegisterBWControllable(IBWControllable bc)
        {
            lock (this._contextMap.SyncRoot)
            {
                if (!this._contextMap.Contains(bc))
                {
                    DummyBWContext context = new DummyBWContext(bc);
                    this._contextMap.Add(bc, context);
                }
                return (this._contextMap[bc] as IBWControlContext);
            }
        }

        public void ResetBuckets(IBWControlContext context)
        {
        }

        public void Shutdown()
        {
        }

        public void Start(ConfigurationSection configuration)
        {
        }

        public void UnregisterBWControllable(IBWControlContext context)
        {
            lock (this._contextMap.SyncRoot)
            {
                this._contextMap.Remove(context.GetBWControllable());
            }
        }

        public void UpdateBWConfigure(IBWControlContext context)
        {
        }
    }
}

