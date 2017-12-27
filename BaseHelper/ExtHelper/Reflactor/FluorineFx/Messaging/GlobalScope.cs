namespace FluorineFx.Messaging
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Persistence;
    using FluorineFx.Messaging.Rtmp.IO;
    using FluorineFx.Messaging.Rtmp.Stream;
    using FluorineFx.Scheduling;
    using System;
    using System.Collections;

    internal class GlobalScope : Scope, IGlobalScope, IScope, IBasicScope, ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener, IEventObservable, IPersistable, IEnumerable, IServiceContainer, IServiceProvider
    {
        internal GlobalScope()
        {
        }

        public void Register()
        {
            IStreamableFileFactory factory = ObjectFactory.CreateInstance(FluorineConfiguration.Instance.FluorineSettings.StreamableFileFactory.Type) as IStreamableFileFactory;
            base.AddService(typeof(IStreamableFileFactory), factory, false);
            factory.Start(null);
            SchedulingService service = new SchedulingService();
            base.AddService(typeof(ISchedulingService), service, false);
            service.Start(null);
            IBWControlService service2 = ObjectFactory.CreateInstance(FluorineConfiguration.Instance.FluorineSettings.BWControlService.Type) as IBWControlService;
            base.AddService(typeof(IBWControlService), service2, false);
            service2.Start(null);
            base.Init();
        }

        public IServiceProvider ServiceProvider
        {
            get
            {
                return base._serviceContainer;
            }
        }
    }
}

