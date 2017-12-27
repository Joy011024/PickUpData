namespace FluorineFx.Json
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class JsonConverterCollection : CollectionBase
    {
        public JsonConverterCollection()
        {
        }

        public JsonConverterCollection(JsonConverterCollection collection)
        {
            base.InnerList.AddRange(collection);
        }

        public virtual void Add(JsonConverter converter)
        {
            base.List.Add(converter);
        }

        public bool Contains(JsonConverter converter)
        {
            return base.List.Contains(converter);
        }

        public int IndexOf(JsonConverter converter)
        {
            return base.List.IndexOf(converter);
        }

        protected override void OnValidate(object value)
        {
            base.OnValidate(value);
            if (!(value is JsonConverter))
            {
                throw new ArgumentException("JsonConverterCollection only supports JsonConverter objects.");
            }
        }

        public virtual void Remove(JsonConverter converter)
        {
            base.List.Remove(converter);
        }

        public JsonConverter this[int index]
        {
            get
            {
                return (JsonConverter) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}

