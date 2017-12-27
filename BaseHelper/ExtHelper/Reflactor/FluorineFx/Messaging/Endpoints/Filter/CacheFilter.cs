namespace FluorineFx.Messaging.Endpoints.Filter
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.IO;
    using FluorineFx.Messaging.Endpoints;
    using log4net;
    using System;
    using System.Collections;

    internal class CacheFilter : AbstractFilter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CacheFilter));

        public override void Invoke(AMFContext context)
        {
            MessageOutput messageOutput = context.MessageOutput;
            if ((FluorineConfiguration.Instance.CacheMap != null) && (FluorineConfiguration.Instance.CacheMap.Count > 0))
            {
                for (int i = 0; i < context.AMFMessage.BodyCount; i++)
                {
                    AMFBody bodyAt = context.AMFMessage.GetBodyAt(i);
                    if ((messageOutput.GetResponse(bodyAt) == null) && !bodyAt.IsEmptyTarget)
                    {
                        string target = bodyAt.Target;
                        IList parameterList = bodyAt.GetParameterList();
                        string cacheKey = CacheMap.GenerateCacheKey(target, parameterList);
                        if (FluorineConfiguration.Instance.CacheMap.ContainsValue(cacheKey))
                        {
                            object content = FluorineConfiguration.Instance.CacheMap.Get(cacheKey);
                            if ((log != null) && log.get_IsDebugEnabled())
                            {
                                log.Debug(__Res.GetString("Cache_HitKey", new object[] { bodyAt.Target, cacheKey }));
                            }
                            CachedBody body = new CachedBody(bodyAt, content);
                            messageOutput.AddBody(body);
                        }
                    }
                }
            }
        }
    }
}

