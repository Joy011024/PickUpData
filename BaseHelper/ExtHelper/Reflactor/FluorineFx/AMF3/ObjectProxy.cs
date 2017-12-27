namespace FluorineFx.AMF3
{
    using FluorineFx;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    [CLSCompliant(false)]
    public class ObjectProxy : Dictionary<string, object>
    {
        public void ReadExternal(IDataInput input)
        {
            object obj2 = input.ReadObject();
            if (obj2 is IDictionary)
            {
                IDictionary dictionary = obj2 as IDictionary;
                foreach (DictionaryEntry entry in dictionary)
                {
                    base.Add(entry.Key as string, entry.Value);
                }
            }
        }

        public void WriteExternal(IDataOutput output)
        {
            ASObject obj2 = new ASObject();
            foreach (KeyValuePair<string, object> pair in this)
            {
                obj2.Add(pair.Key, pair.Value);
            }
            output.WriteObject(obj2);
        }
    }
}

