namespace FluorineFx.Data
{
    using FluorineFx;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Services;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;

    internal class DataDestination : MessageDestination
    {
        private FluorineFx.Data.SequenceManager _sequenceManager;
        private static readonly ILog log = LogManager.GetLogger(typeof(DataDestination));

        public DataDestination(IService service, DestinationSettings destinationSettings) : base(service, destinationSettings)
        {
            this._sequenceManager = new FluorineFx.Data.SequenceManager(this);
        }

        public bool AutoRefreshFill(IList parameters)
        {
            return ((base.ServiceAdapter is DotNetAdapter) && (base.ServiceAdapter as DotNetAdapter).AutoRefreshFill(parameters));
        }

        internal override void Dump(DumpContext dumpContext)
        {
            base.Dump(dumpContext);
            dumpContext.Indent();
            this._sequenceManager.Dump(dumpContext);
            dumpContext.Unindent();
        }

        public string[] GetIdentityKeys()
        {
            if (base.DestinationSettings.MetadataSettings != null)
            {
                return (base.DestinationSettings.MetadataSettings.Identity.ToArray(typeof(string)) as string[]);
            }
            return new string[0];
        }

        public override MessageClient RemoveSubscriber(string clientId)
        {
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("DataDestination_RemoveSubscriber", new object[] { clientId }));
            }
            MessageClient client = base.RemoveSubscriber(clientId);
            this._sequenceManager.RemoveSubscriber(clientId);
            return client;
        }

        public FluorineFx.Data.SequenceManager SequenceManager
        {
            get
            {
                return this._sequenceManager;
            }
        }
    }
}

