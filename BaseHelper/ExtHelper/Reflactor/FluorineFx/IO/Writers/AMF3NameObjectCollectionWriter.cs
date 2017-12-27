namespace FluorineFx.IO.Writers
{
    using FluorineFx;
    using FluorineFx.IO;
    using System;
    using System.Collections.Specialized;
    using System.Reflection;

    internal class AMF3NameObjectCollectionWriter : IAMFWriter
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
                    ASObject obj2 = new ASObject();
                    for (int i = 0; i < base2.Keys.Count; i++)
                    {
                        string key = base2.Keys[i];
                        object obj3 = property.GetValue(base2, new object[] { key });
                        obj2.Add(key, obj3);
                    }
                    writer.WriteByte(10);
                    writer.WriteAMF3Object(obj2);
                    return;
                }
            }
            writer.WriteByte(10);
            writer.WriteAMF3Object(data);
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

