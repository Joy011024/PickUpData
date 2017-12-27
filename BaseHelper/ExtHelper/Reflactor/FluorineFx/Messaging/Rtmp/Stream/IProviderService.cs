namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Messaging;
    using FluorineFx.Messaging.Api.Stream;
    using System;
    using System.Collections;
    using System.IO;

    internal interface IProviderService : IScopeService, IService
    {
        IEnumerator GetBroadcastStreamNames(IScope scope);
        IMessageInput GetLiveProviderInput(IScope scope, string name, bool needCreate);
        IMessageInput GetProviderInput(IScope scope, string name);
        FileInfo GetVODProviderFile(IScope scope, string name);
        IMessageInput GetVODProviderInput(IScope scope, string name);
        bool RegisterBroadcastStream(IScope scope, string name, IBroadcastStream broadcastStream);
        bool UnregisterBroadcastStream(IScope scope, string name);
    }
}

