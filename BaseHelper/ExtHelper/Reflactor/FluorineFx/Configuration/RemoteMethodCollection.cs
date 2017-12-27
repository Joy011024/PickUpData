namespace FluorineFx.Configuration
{
    using FluorineFx.Collections.Generic;
    using System;
    using System.Collections.Generic;

    public sealed class RemoteMethodCollection : CollectionBase<RemoteMethod>
    {
        private Dictionary<string, string> _methods = new Dictionary<string, string>(3);
        private Dictionary<string, string> _methodsNames = new Dictionary<string, string>(3);

        public override void Add(RemoteMethod value)
        {
            this._methods[value.Name] = value.Method;
            this._methodsNames[value.Method] = value.Name;
            base.Add(value);
        }

        public string GetMethod(string name)
        {
            if (this._methods.ContainsKey(name))
            {
                return this._methods[name];
            }
            return name;
        }

        public string GetMethodName(string method)
        {
            if (this._methodsNames.ContainsKey(method))
            {
                return this._methodsNames[method];
            }
            return method;
        }

        public override void Insert(int index, RemoteMethod value)
        {
            this._methods[value.Name] = value.Method;
            this._methodsNames[value.Method] = value.Name;
            base.Insert(index, value);
        }
    }
}

