namespace FluorineFx.Json
{
    using System;

    public abstract class JsonConverter
    {
        protected JsonConverter()
        {
        }

        public abstract bool CanConvert(Type objectType);
        public virtual object ReadJson(JsonReader reader, Type objectType)
        {
            throw new NotImplementedException(string.Format("{0} has not overriden FromJson method.", base.GetType().Name));
        }

        public virtual void WriteJson(JsonWriter writer, object value)
        {
            new JsonSerializer().Serialize(writer, value);
        }
    }
}

