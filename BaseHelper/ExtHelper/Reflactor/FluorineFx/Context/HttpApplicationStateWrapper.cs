namespace FluorineFx.Context
{
    using System;
    using System.Reflection;
    using System.Web;

    internal class HttpApplicationStateWrapper : IApplicationState
    {
        public void Add(string name, object value)
        {
            HttpContext.Current.Application.Add(name, value);
        }

        public void Remove(string key)
        {
            HttpContext.Current.Application.Remove(key);
        }

        public void RemoveAt(int index)
        {
            HttpContext.Current.Application.RemoveAt(index);
        }

        public object this[int index]
        {
            get
            {
                return HttpContext.Current.Application[index];
            }
        }

        public object this[string name]
        {
            get
            {
                return HttpContext.Current.Application[name];
            }
            set
            {
                HttpContext.Current.Application[name] = value;
            }
        }
    }
}

