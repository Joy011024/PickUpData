namespace FluorineFx.IO.Writers
{
    using FluorineFx;
    using FluorineFx.IO;
    using System;
    using System.Collections.Specialized;
    using System.Reflection;

    internal class AMF0NameObjectCollectionWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            NameObjectCollectionBase base2 = data as NameObjectCollectionBase;
            object[] customAttributes = base2.GetType().GetCustomAttributes(typeof(DefaultMemberAttribute), false);
            if ((customAttributes != null) && (customAttributes.Length > 0))
            {
                DefaultMemberAttribute attribute = customAttributes[0] as DefaultMemberAttribute;
                PropertyInfo property = base2.GetType().GetProperty(attribute.MemberName, new Type[] { typeof(string) });
                if (property != null)
                {
                    ASObject asObject = new ASObject();
                    for (int i = 0; i < base2.Keys.Count; i++)
                    {
                        string key = base2.Keys[i];
                        object obj3 = property.GetValue(base2, new object[] { key });
                        asObject.Add(key, obj3);
                    }
                    writer.WriteASO(ObjectEncoding.AMF0, asObject);
                    return;
                }
            }
            writer.WriteObject(ObjectEncoding.AMF0, data);
        }

        public bool IsPrimitive
        {
            get
            {
                return false;
            }
        }
    }
}

