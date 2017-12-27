namespace FluorineFx.Configuration
{
    using FluorineFx.Collections.Generic;
    using System;
    using System.Collections.Generic;

    public sealed class PathEntryCollection : CollectionBase<PathEntry>
    {
        private Dictionary<string, PathEntry> _excludedPaths = new Dictionary<string, PathEntry>(StringComparer.OrdinalIgnoreCase);

        public override void Add(PathEntry value)
        {
            this._excludedPaths.Add(value.Path, value);
            base.Add(value);
        }

        public bool Contains(string path)
        {
            return this._excludedPaths.ContainsKey(path);
        }

        public override void Insert(int index, PathEntry value)
        {
            this._excludedPaths.Add(value.Path, value);
            base.Insert(index, value);
        }

        public override bool Remove(PathEntry value)
        {
            this._excludedPaths.Remove(value.Path);
            return base.Remove(value);
        }
    }
}

