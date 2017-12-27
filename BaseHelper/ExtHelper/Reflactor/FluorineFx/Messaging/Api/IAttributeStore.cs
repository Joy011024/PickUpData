namespace FluorineFx.Messaging.Api
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IAttributeStore
    {
        void CopyTo(object[] array, int index);
        object GetAttribute(string name);
        object GetAttribute(string name, object defaultValue);
        ICollection GetAttributeNames();
        bool HasAttribute(string name);
        bool RemoveAttribute(string name);
        void RemoveAttributes();
        bool SetAttribute(string name, object value);
        void SetAttributes(IAttributeStore values);
        void SetAttributes(IDictionary<string, object> values);

        int AttributesCount { get; }

        bool IsEmpty { get; }

        object this[string name] { get; set; }
    }
}

