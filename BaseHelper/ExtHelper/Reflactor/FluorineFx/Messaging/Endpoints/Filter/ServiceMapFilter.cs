namespace FluorineFx.Messaging.Endpoints.Filter
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.IO;
    using FluorineFx.Messaging.Endpoints;
    using log4net;
    using System;

    internal class ServiceMapFilter : AbstractFilter
    {
        private EndpointBase _endpoint;
        private static readonly ILog log = LogManager.GetLogger(typeof(ServiceMapFilter));

        public ServiceMapFilter(EndpointBase endpoint)
        {
            this._endpoint = endpoint;
        }

        public override void Invoke(AMFContext context)
        {
            for (int i = 0; i < context.AMFMessage.BodyCount; i++)
            {
                AMFBody bodyAt = context.AMFMessage.GetBodyAt(i);
                if (!bodyAt.IsEmptyTarget && (FluorineConfiguration.Instance.ServiceMap != null))
                {
                    string typeName = bodyAt.TypeName;
                    string method = bodyAt.Method;
                    if ((typeName != null) && FluorineConfiguration.Instance.ServiceMap.Contains(typeName))
                    {
                        string serviceLocation = FluorineConfiguration.Instance.ServiceMap.GetServiceLocation(typeName);
                        method = FluorineConfiguration.Instance.ServiceMap.GetMethod(typeName, method);
                        string str4 = serviceLocation + "." + method;
                        if ((log != null) && log.get_IsDebugEnabled())
                        {
                            log.Debug(__Res.GetString("Service_Mapping", new object[] { bodyAt.Target, str4 }));
                        }
                        bodyAt.Target = str4;
                    }
                }
            }
        }
    }
}

