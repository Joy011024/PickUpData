namespace FluorineFx.Configuration
{
    using FluorineFx.Collections.Generic;
    using System;
    using System.Collections.Generic;

    public sealed class MimeTypeEntryCollection : CollectionBase<MimeTypeEntry>
    {
        private Dictionary<string, MimeTypeEntry> _excludedTypes = new Dictionary<string, MimeTypeEntry>(StringComparer.OrdinalIgnoreCase);

        public override void Add(MimeTypeEntry value)
        {
            this._excludedTypes.Add(value.Type, value);
            base.Add(value);
        }

        public bool Contains(string type)
        {
            return this._excludedTypes.ContainsKey(type);
        }

        public override void Insert(int index, MimeTypeEntry value)
        {
            this._excludedTypes.Add(value.Type, value);
            base.Insert(index, value);
        }

        public override bool Remove(MimeTypeEntry value)
        {
            this._excludedTypes.Remove(value.Type);
            return base.Remove(value);
        }
    }
}

