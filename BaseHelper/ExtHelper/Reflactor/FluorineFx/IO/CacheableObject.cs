namespace FluorineFx.IO
{
    using System;

    internal class CacheableObject
    {
        private string _cacheKey;
        private object _obj;
        private string _source;

        public CacheableObject(string source, string cacheKey, object obj)
        {
            this._source = source;
            this._cacheKey = cacheKey;
            this._obj = obj;
        }

        public string CacheKey
        {
            get
            {
                return this._cacheKey;
            }
        }

        public object Object
        {
            get
            {
                return this._obj;
            }
        }

        public string Source
        {
            get
            {
                return this._source;
            }
        }
    }
}

