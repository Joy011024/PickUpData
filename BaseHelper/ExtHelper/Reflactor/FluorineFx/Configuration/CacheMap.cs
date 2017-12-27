namespace FluorineFx.Configuration
{
    using FluorineFx.Data;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Caching;

    internal sealed class CacheMap
    {
        private Dictionary<string, CacheDescriptor> _cacheMap = new Dictionary<string, CacheDescriptor>();
        private static readonly ILog log = LogManager.GetLogger(typeof(CacheMap));

        public object Add(string source, string cacheKey, object value)
        {
            string str;
            lock (((ICollection) this._cacheMap).SyncRoot)
            {
                if (this._cacheMap.ContainsKey(source))
                {
                    if ((log != null) && log.get_IsDebugEnabled())
                    {
                        str = "Add to ASP.NET cache name: " + source + " key: " + cacheKey;
                        log.Debug(str);
                    }
                    CacheDescriptor descriptor = this._cacheMap[source];
                    if (!descriptor.SlidingExpiration)
                    {
                        return HttpRuntime.Cache.Add(cacheKey, value, null, DateTime.Now.AddMinutes((double) descriptor.Timeout), TimeSpan.Zero, CacheItemPriority.Normal, null);
                    }
                    return HttpRuntime.Cache.Add(cacheKey, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes((double) descriptor.Timeout), CacheItemPriority.Normal, null);
                }
            }
            if ((log != null) && log.get_IsDebugEnabled())
            {
                str = "Cannot add to ASP.NET cache the name: " + source + " key: " + cacheKey + ". Check your web.config file.";
                log.Debug(str);
            }
            return null;
        }

        public void AddCacheDescriptor(string name, int timeout, bool slidingExpiration)
        {
            CacheDescriptor descriptor = new CacheDescriptor(name, timeout, slidingExpiration);
            lock (((ICollection) this._cacheMap).SyncRoot)
            {
                this._cacheMap[name] = descriptor;
            }
            if ((log != null) && log.get_IsDebugEnabled())
            {
                string str = string.Concat(new object[] { "Loading key: ", name, " to cache map, timeout: ", timeout, " sliding expiration: ", slidingExpiration });
                log.Debug(str);
            }
        }

        public bool ContainsCacheDescriptor(string source)
        {
            if (source != null)
            {
                lock (((ICollection) this._cacheMap).SyncRoot)
                {
                    return this._cacheMap.ContainsKey(source);
                }
            }
            return false;
        }

        public bool ContainsValue(string cacheKey)
        {
            return (HttpRuntime.Cache.Get(cacheKey) != null);
        }

        internal static string GenerateCacheKey(string source, IList arguments)
        {
            int num = ListHashCodeProvider.GenerateHashCode(arguments);
            return (source + "_" + num.ToString());
        }

        public object Get(string cacheKey)
        {
            string str;
            object obj2 = HttpRuntime.Cache.Get(cacheKey);
            if (obj2 != null)
            {
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    str = "Cache hit, name: " + cacheKey;
                    log.Debug(str);
                }
                return obj2;
            }
            if ((log != null) && log.get_IsDebugEnabled())
            {
                str = "Cache miss, name: " + cacheKey;
                log.Debug(str);
            }
            return obj2;
        }

        public int Count
        {
            get
            {
                lock (((ICollection) this._cacheMap).SyncRoot)
                {
                    return this._cacheMap.Count;
                }
            }
        }

        private class CacheDescriptor
        {
            private string _name;
            private bool _slidingExpiration;
            private int _timeout;

            public CacheDescriptor(string name, int timeout, bool slidingExpiration)
            {
                this._name = name;
                this._timeout = timeout;
                this._slidingExpiration = slidingExpiration;
            }

            public string Name
            {
                get
                {
                    return this._name;
                }
            }

            public bool SlidingExpiration
            {
                get
                {
                    return this._slidingExpiration;
                }
            }

            public int Timeout
            {
                get
                {
                    return this._timeout;
                }
            }
        }
    }
}

