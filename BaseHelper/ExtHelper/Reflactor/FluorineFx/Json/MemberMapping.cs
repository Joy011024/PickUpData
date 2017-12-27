namespace FluorineFx.Json
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct MemberMapping
    {
        private readonly string _mappingName;
        private readonly MemberInfo _member;
        private readonly bool _ignored;
        private readonly bool _readable;
        private readonly bool _writable;
        public MemberMapping(string mappingName, MemberInfo member, bool ignored, bool readable, bool writable)
        {
            this._mappingName = mappingName;
            this._member = member;
            this._ignored = ignored;
            this._readable = readable;
            this._writable = writable;
        }

        public string MappingName
        {
            get
            {
                return this._mappingName;
            }
        }
        public MemberInfo Member
        {
            get
            {
                return this._member;
            }
        }
        public bool Ignored
        {
            get
            {
                return this._ignored;
            }
        }
        public bool Readable
        {
            get
            {
                return this._readable;
            }
        }
        public bool Writable
        {
            get
            {
                return this._writable;
            }
        }
    }
}

