namespace FluorineFx
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public sealed class RemotingServiceAttribute : Attribute
    {
        private string _serviceName;

        public RemotingServiceAttribute()
        {
        }

        public RemotingServiceAttribute(string serviceName)
        {
            this._serviceName = serviceName;
        }
    }
}

