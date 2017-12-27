namespace FluorineFx.Configuration
{
    using FluorineFx.Collections.Generic;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public sealed class NullableTypeCollection : CollectionBase<NullableType>
    {
        private Dictionary<string, NullableType> _nullableDictionary = new Dictionary<string, NullableType>(5);

        public override void Add(NullableType value)
        {
            this._nullableDictionary[value.TypeName] = value;
            base.Add(value);
        }

        public bool ContainsKey(Type type)
        {
            return this._nullableDictionary.ContainsKey(type.FullName);
        }

        public override void Insert(int index, NullableType value)
        {
            this._nullableDictionary[value.TypeName] = value;
            base.Insert(index, value);
        }

        public override bool Remove(NullableType value)
        {
            this._nullableDictionary.Remove(value.TypeName);
            return base.Remove(value);
        }

        public object this[Type type]
        {
            get
            {
                if (this._nullableDictionary.ContainsKey(type.FullName))
                {
                    return this._nullableDictionary[type.FullName].NullValue;
                }
                return null;
            }
        }
    }
}

