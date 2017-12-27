namespace FluorineFx.Data
{
    using FluorineFx;
    using FluorineFx.Exceptions;
    using System;
    using System.Collections;
    using System.Reflection;

    internal class Identity : Hashtable
    {
        private object _item;

        private Identity()
        {
        }

        public Identity(IDictionary map)
        {
            foreach (DictionaryEntry entry in map)
            {
                this.Add(entry.Key, entry.Value);
            }
        }

        public Identity(object item)
        {
            this._item = item;
        }

        public override bool Equals(object obj)
        {
            if (obj is Identity)
            {
                Identity identity = obj as Identity;
                if (this.Count != identity.Count)
                {
                    return false;
                }
                foreach (DictionaryEntry entry in this)
                {
                    if (!identity.ContainsKey(entry.Key))
                    {
                        return false;
                    }
                    object obj2 = identity[entry.Key];
                    if (!((entry.Value != null) ? entry.Value.Equals(obj2) : (obj2 == null)))
                    {
                        return false;
                    }
                }
                return true;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int num = 0;
            foreach (DictionaryEntry entry in this)
            {
                if (entry.Value != null)
                {
                    num ^= entry.Value.GetHashCode();
                }
                else
                {
                    num = num;
                }
            }
            return num;
        }

        public static Identity GetIdentity(object item, DataDestination destination)
        {
            string[] identityKeys = destination.GetIdentityKeys();
            Identity identity = new Identity(item);
            foreach (string str in identityKeys)
            {
                Exception exception;
                PropertyInfo property = item.GetType().GetProperty(str);
                if (property != null)
                {
                    try
                    {
                        identity[str] = property.GetValue(item, new object[0]);
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        throw new FluorineException(__Res.GetString("Identity_Failed", new object[] { str }), exception);
                    }
                }
                else
                {
                    try
                    {
                        FieldInfo field = item.GetType().GetField(str, BindingFlags.Public | BindingFlags.Instance);
                        if (field != null)
                        {
                            identity[str] = field.GetValue(item);
                        }
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                        throw new FluorineException(__Res.GetString("Identity_Failed", new object[] { str }), exception);
                    }
                }
            }
            return identity;
        }

        [Transient]
        public object Item
        {
            get
            {
                return this._item;
            }
        }
    }
}

