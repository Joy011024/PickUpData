namespace FluorineFx
{
    using FluorineFx.Configuration;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal class ObjectFactory
    {
        private static string[] _lacLocations = TypeHelper.GetLacLocations();
        private static Dictionary<string, Type> _typeCache = new Dictionary<string, Type>();
        private static readonly ILog log = LogManager.GetLogger(typeof(ObjectFactory));

        internal static void AddTypeToCache(Type type)
        {
            if (type != null)
            {
                lock (typeof(Type))
                {
                    _typeCache[type.FullName] = type;
                }
            }
        }

        public static bool ContainsType(string typeName)
        {
            if (typeName != null)
            {
                lock (typeof(Type))
                {
                    return _typeCache.ContainsKey(typeName);
                }
            }
            return false;
        }

        public static object CreateInstance(string typeName)
        {
            return CreateInstance(typeName, null);
        }

        public static object CreateInstance(Type type)
        {
            return CreateInstance(type, null);
        }

        public static object CreateInstance(string typeName, object[] args)
        {
            return CreateInstance(Locate(typeName), args);
        }

        public static object CreateInstance(Type type, object[] args)
        {
            if (type != null)
            {
                lock (typeof(Type))
                {
                    if (type.IsAbstract && type.IsSealed)
                    {
                        return type;
                    }
                    if (args == null)
                    {
                        return Activator.CreateInstance(type, BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, null, new object[0], null);
                    }
                    return Activator.CreateInstance(type, BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, null, args, null);
                }
            }
            return null;
        }

        public static Type Locate(string typeName)
        {
            if ((typeName == null) || (typeName == string.Empty))
            {
                return null;
            }
            string key = typeName;
            key = FluorineConfiguration.Instance.GetMappedTypeName(typeName);
            lock (typeof(Type))
            {
                Type type = null;
                if (_typeCache.ContainsKey(key))
                {
                    type = _typeCache[key];
                }
                if (type == null)
                {
                    type = TypeHelper.Locate(key);
                    if (type != null)
                    {
                        _typeCache[key] = type;
                        return type;
                    }
                    type = LocateInLac(key);
                }
                return type;
            }
        }

        public static Type LocateInLac(string typeName)
        {
            if ((typeName == null) || (typeName == string.Empty))
            {
                return null;
            }
            string key = typeName;
            key = FluorineConfiguration.Instance.GetMappedTypeName(typeName);
            lock (typeof(Type))
            {
                Type type = null;
                if (_typeCache.ContainsKey(key))
                {
                    type = _typeCache[key];
                }
                if (type == null)
                {
                    for (int i = 0; i < _lacLocations.Length; i++)
                    {
                        type = TypeHelper.LocateInLac(key, _lacLocations[i]);
                        if (type != null)
                        {
                            _typeCache[key] = type;
                            return type;
                        }
                    }
                }
                return type;
            }
        }
    }
}

