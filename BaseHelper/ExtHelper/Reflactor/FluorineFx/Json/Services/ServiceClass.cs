namespace FluorineFx.Json.Services
{
    using FluorineFx.Json;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;

    public sealed class ServiceClass
    {
        private readonly string _description;
        private readonly Hashtable _methods;
        private readonly string _serviceName;

        internal ServiceClass(Type type)
        {
            this._serviceName = type.Name;
            this._description = null;
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            this._methods = new Hashtable();
            foreach (MethodInfo info in methods)
            {
                if (ShouldBuild(info))
                {
                    Method method = Method.FromMethodInfo(info);
                    if (this._methods.Contains(method.Name))
                    {
                        throw new DuplicateMethodException(string.Format("The method '{0}' cannot be exported as '{1}' because this name has already been used by another method on the '{2}' service.", method.Name, method.Name, this._serviceName));
                    }
                    this._methods.Add(method.Name, method);
                }
            }
        }

        public Method FindMethodByName(string name)
        {
            return (this._methods[name] as Method);
        }

        public Method GetMethodByName(string name)
        {
            Method method = this.FindMethodByName(name);
            if (method == null)
            {
                throw new MissingMethodException(this.Name, name);
            }
            return method;
        }

        public ICollection GetMethods()
        {
            return this._methods.Values;
        }

        private static bool ShouldBuild(MethodInfo method)
        {
            Debug.Assert(method != null);
            return (!method.IsAbstract && Attribute.IsDefined(method, typeof(JsonRpcMethodAttribute)));
        }

        public string Description
        {
            get
            {
                return this._description;
            }
        }

        public string Name
        {
            get
            {
                return this._serviceName;
            }
        }
    }
}

