namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Api;
    using System;

    public interface IBWControlService : IService
    {
        ITokenBucket GetAudioBucket(IBWControlContext context);
        ITokenBucket GetDataBucket(IBWControlContext context);
        ITokenBucket GetVideoBucket(IBWControlContext context);
        IBWControlContext LookupContext(IBWControllable bc);
        IBWControlContext RegisterBWControllable(IBWControllable bc);
        void ResetBuckets(IBWControlContext context);
        void UnregisterBWControllable(IBWControlContext context);
        void UpdateBWConfigure(IBWControlContext context);
    }
}

