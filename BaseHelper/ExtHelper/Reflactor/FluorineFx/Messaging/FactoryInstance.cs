namespace FluorineFx.Messaging
{
    using System;
    using System.Collections;

    public class FactoryInstance
    {
        private string _attributeId;
        private IFlexFactory _factory;
        private string _id;
        private Hashtable _properties;
        private string _scope;
        private string _source;

        public FactoryInstance(IFlexFactory factory, string id, Hashtable properties)
        {
            this._factory = factory;
            this._id = id;
            this._properties = properties;
        }

        public virtual Type GetInstanceClass()
        {
            return null;
        }

        public virtual object Lookup()
        {
            return this._factory.Lookup(this);
        }

        public virtual void OnOperationComplete(object instance)
        {
        }

        public string AttributeId
        {
            get
            {
                return this._attributeId;
            }
            set
            {
                this._attributeId = value;
            }
        }

        public string Id
        {
            get
            {
                return this._id;
            }
        }

        public Hashtable Properties
        {
            get
            {
                return this._properties;
            }
        }

        public virtual string Scope
        {
            get
            {
                return this._scope;
            }
            set
            {
                this._scope = value;
            }
        }

        public virtual string Source
        {
            get
            {
                return this._source;
            }
            set
            {
                this._source = value;
            }
        }
    }
}

