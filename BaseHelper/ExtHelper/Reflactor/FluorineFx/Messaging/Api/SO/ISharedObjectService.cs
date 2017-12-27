namespace FluorineFx.Messaging.Api.SO
{
    using FluorineFx.Messaging.Api;
    using System;
    using System.Collections;

    public interface ISharedObjectService : IScopeService, IService
    {
        bool ClearSharedObjects(IScope scope, string name);
        bool CreateSharedObject(IScope scope, string name, bool persistent);
        ISharedObject GetSharedObject(IScope scope, string name);
        ISharedObject GetSharedObject(IScope scope, string name, bool persistent);
        ICollection GetSharedObjectNames(IScope scope);
        bool HasSharedObject(IScope scope, string name);
    }
}

