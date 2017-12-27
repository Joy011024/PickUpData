namespace FluorineFx.Messaging
{
    using FluorineFx.Context;
    using System;
    using System.Collections;

    public class DotNetFactory : IFlexFactory
    {
        public FactoryInstance CreateFactoryInstance(string id, Hashtable properties)
        {
            DotNetFactoryInstance instance = new DotNetFactoryInstance(this, id, properties) {
                Source = properties["source"] as string,
                Scope = properties["scope"] as string
            };
            if (instance.Scope == null)
            {
                instance.Scope = "request";
            }
            instance.AttributeId = properties["attribute-id"] as string;
            return instance;
        }

        public object Lookup(FactoryInstance factoryInstance)
        {
            DotNetFactoryInstance instance = factoryInstance as DotNetFactoryInstance;
            switch (instance.Scope)
            {
                case "application":
                    return instance.ApplicationInstance;

                case "session":
                {
                    if (FluorineContext.Current.Session == null)
                    {
                        return null;
                    }
                    object obj2 = FluorineContext.Current.Session[instance.AttributeId];
                    if (obj2 == null)
                    {
                        obj2 = instance.CreateInstance();
                        FluorineContext.Current.Session[instance.AttributeId] = obj2;
                    }
                    return obj2;
                }
            }
            return instance.CreateInstance();
        }
    }
}

