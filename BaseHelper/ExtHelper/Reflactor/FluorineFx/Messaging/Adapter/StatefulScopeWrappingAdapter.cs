namespace FluorineFx.Messaging.Adapter
{
    using FluorineFx.Messaging.Api;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public class StatefulScopeWrappingAdapter : AbstractScopeAdapter, IScopeAware, IAttributeStore
    {
        private IScope _scope;

        public void CopyTo(object[] array, int index)
        {
            this._scope.CopyTo(array, index);
        }

        public bool CreateChildScope(string name)
        {
            return this._scope.CreateChildScope(name);
        }

        public object GetAttribute(string name)
        {
            return this._scope.GetAttribute(name);
        }

        public object GetAttribute(string name, object defaultValue)
        {
            return this._scope.GetAttribute(name, defaultValue);
        }

        public ICollection GetAttributeNames()
        {
            return this._scope.GetAttributeNames();
        }

        public IScope GetChildScope(string name)
        {
            return this._scope.GetScope(name);
        }

        public ICollection GetChildScopeNames()
        {
            return this._scope.GetScopeNames();
        }

        public ICollection GetClients()
        {
            return this._scope.GetClients();
        }

        public IEnumerator GetConnections()
        {
            return this._scope.GetConnections();
        }

        public IScopeContext GetContext()
        {
            return this._scope.Context;
        }

        public bool HasAttribute(string name)
        {
            return this._scope.HasAttribute(name);
        }

        public bool HasChildScope(string name)
        {
            return this._scope.HasChildScope(name);
        }

        public ICollection LookupConnections(IClient client)
        {
            return this._scope.LookupConnections(client);
        }

        public bool RemoveAttribute(string name)
        {
            return this._scope.RemoveAttribute(name);
        }

        public void RemoveAttributes()
        {
            this._scope.RemoveAttributes();
        }

        public bool SetAttribute(string name, object value)
        {
            return this._scope.SetAttribute(name, value);
        }

        public void SetAttributes(IAttributeStore values)
        {
            this._scope.SetAttributes(values);
        }

        public void SetAttributes(IDictionary<string, object> values)
        {
            this._scope.SetAttributes(values);
        }

        public void SetScope(IScope scope)
        {
            this._scope = scope;
        }

        public int AttributesCount
        {
            get
            {
                return this._scope.AttributesCount;
            }
        }

        public int Depth
        {
            get
            {
                return this._scope.Depth;
            }
        }

        public bool HasParent
        {
            get
            {
                return this._scope.HasParent;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this._scope.IsEmpty;
            }
        }

        public object this[string name]
        {
            get
            {
                return this.GetAttribute(name);
            }
            set
            {
                this.SetAttribute(name, value);
            }
        }

        public string Name
        {
            get
            {
                return this._scope.Name;
            }
        }

        public IScope Parent
        {
            get
            {
                return this._scope.Parent;
            }
        }

        public string Path
        {
            get
            {
                return this._scope.Path;
            }
        }

        public IScope Scope
        {
            get
            {
                return this._scope;
            }
        }
    }
}

