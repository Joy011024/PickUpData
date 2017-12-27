namespace FluorineFx.Json
{
    using System;
    using System.Collections;
    using System.Reflection;

    internal class MemberMappingCollection : CollectionBase
    {
        private Hashtable _dict = new Hashtable();

        public int Add(MemberMapping memberMapping)
        {
            if (this.Contains(memberMapping.MappingName))
            {
                if (memberMapping.Ignored)
                {
                    return -1;
                }
                MemberMapping mapping = this[memberMapping.MappingName];
                if (!mapping.Ignored)
                {
                    throw new JsonSerializationException(string.Format("A member with the name '{0}' already exists on {1}. Use the JsonPropertyAttribute to specify another name.", memberMapping.MappingName, memberMapping.Member.DeclaringType));
                }
                this.Remove(mapping);
            }
            this._dict[memberMapping.MappingName] = memberMapping;
            return base.List.Add(memberMapping);
        }

        public bool Contains(MemberMapping value)
        {
            return base.List.Contains(value);
        }

        public bool Contains(string key)
        {
            return this._dict.Contains(key);
        }

        public int IndexOf(MemberMapping value)
        {
            return base.List.IndexOf(value);
        }

        public void Remove(MemberMapping value)
        {
            this._dict.Remove(value.MappingName);
            base.List.Remove(value);
        }

        public MemberMapping this[int index]
        {
            get
            {
                return (MemberMapping) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        public MemberMapping this[string key]
        {
            get
            {
                return (MemberMapping) this._dict[key];
            }
            set
            {
                this._dict[key] = value;
            }
        }
    }
}

