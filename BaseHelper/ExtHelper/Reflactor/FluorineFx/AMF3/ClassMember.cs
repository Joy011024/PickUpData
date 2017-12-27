namespace FluorineFx.AMF3
{
    using System;
    using System.Reflection;

    public sealed class ClassMember
    {
        private System.Reflection.BindingFlags _bindingFlags;
        private MemberTypes _memberType;
        private string _name;

        internal ClassMember(string name, System.Reflection.BindingFlags bindingFlags, MemberTypes memberType)
        {
            this._name = name;
            this._bindingFlags = bindingFlags;
            this._memberType = memberType;
        }

        public System.Reflection.BindingFlags BindingFlags
        {
            get
            {
                return this._bindingFlags;
            }
        }

        public MemberTypes MemberType
        {
            get
            {
                return this._memberType;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }
    }
}

