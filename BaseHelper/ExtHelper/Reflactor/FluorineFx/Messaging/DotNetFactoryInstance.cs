namespace FluorineFx.Messaging
{
    using FluorineFx;
    using FluorineFx.Context;
    using System;
    using System.Collections;
    using System.Reflection;

    public class DotNetFactoryInstance : FactoryInstance
    {
        private object _applicationInstance;
        private Type _cachedType;

        public DotNetFactoryInstance(IFlexFactory flexFactory, string id, Hashtable properties) : base(flexFactory, id, properties)
        {
        }

        public object CreateInstance()
        {
            Type instanceClass = this.GetInstanceClass();
            if (instanceClass == null)
            {
                string message = __Res.GetString("Type_InitError", new object[] { this.Source });
                throw new MessageException(message, new TypeLoadException(message));
            }
            if (instanceClass.IsAbstract && instanceClass.IsSealed)
            {
                return instanceClass;
            }
            return Activator.CreateInstance(instanceClass, BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, null, new object[0], null);
        }

        public override Type GetInstanceClass()
        {
            if (this._cachedType == null)
            {
                this._cachedType = ObjectFactory.LocateInLac(this.Source);
            }
            return this._cachedType;
        }

        public object ApplicationInstance
        {
            get
            {
                if (this._applicationInstance == null)
                {
                    lock (typeof(DotNetFactoryInstance))
                    {
                        if (this._applicationInstance == null)
                        {
                            this._applicationInstance = this.CreateInstance();
                        }
                        FluorineContext.Current.ApplicationState[base.AttributeId] = this._applicationInstance;
                    }
                }
                return this._applicationInstance;
            }
        }

        public override string Source
        {
            get
            {
                return base.Source;
            }
            set
            {
                if (base.Source != value)
                {
                    base.Source = value;
                    this._cachedType = null;
                }
            }
        }
    }
}

