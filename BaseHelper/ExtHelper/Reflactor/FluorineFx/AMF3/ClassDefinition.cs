namespace FluorineFx.AMF3
{
    using System;

    public sealed class ClassDefinition
    {
        private string _className;
        private bool _dynamic;
        private bool _externalizable;
        private ClassMember[] _members;
        internal static ClassMember[] EmptyClassMembers = new ClassMember[0];

        internal ClassDefinition(string className, ClassMember[] members, bool externalizable, bool dynamic)
        {
            this._className = className;
            this._members = members;
            this._externalizable = externalizable;
            this._dynamic = dynamic;
        }

        public string ClassName
        {
            get
            {
                return this._className;
            }
        }

        public bool IsDynamic
        {
            get
            {
                return this._dynamic;
            }
        }

        public bool IsExternalizable
        {
            get
            {
                return this._externalizable;
            }
        }

        public bool IsTypedObject
        {
            get
            {
                return ((this._className != null) && (this._className != string.Empty));
            }
        }

        public int MemberCount
        {
            get
            {
                if (this._members == null)
                {
                    return 0;
                }
                return this._members.Length;
            }
        }

        public ClassMember[] Members
        {
            get
            {
                return this._members;
            }
        }
    }
}

