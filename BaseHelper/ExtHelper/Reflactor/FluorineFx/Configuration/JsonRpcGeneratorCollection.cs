namespace FluorineFx.Configuration
{
    using FluorineFx.Collections.Generic;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public sealed class JsonRpcGeneratorCollection : CollectionBase<JsonRpcGenerator>
    {
        private Dictionary<string, JsonRpcGenerator> _generators = new Dictionary<string, JsonRpcGenerator>();

        public override void Add(JsonRpcGenerator value)
        {
            this._generators.Add(value.Name, value);
            base.Add(value);
        }

        public bool Contains(string name)
        {
            return this._generators.ContainsKey(name);
        }

        public override void Insert(int index, JsonRpcGenerator value)
        {
            this._generators.Add(value.Name, value);
            base.Insert(index, value);
        }

        public override bool Remove(JsonRpcGenerator value)
        {
            this._generators.Remove(value.Name);
            return base.Remove(value);
        }

        public JsonRpcGenerator this[string name]
        {
            get
            {
                if (this._generators.ContainsKey(name))
                {
                    return this._generators[name];
                }
                return null;
            }
        }
    }
}

