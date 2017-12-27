namespace FluorineFx.Messaging
{
    using System;
    using System.Collections;

    public interface IFlexFactory
    {
        FactoryInstance CreateFactoryInstance(string id, Hashtable properties);
        object Lookup(FactoryInstance factoryInstance);
    }
}

