namespace FluorineFx
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    public class ASObject : Dictionary<string, object>
    {
        private string _typeName;

        public ASObject()
        {
        }

        public ASObject(IDictionary<string, object> dictionary) : base(dictionary)
        {
        }

        public ASObject(string typeName)
        {
            this._typeName = typeName;
        }

        public ASObject(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public bool IsTypedObject
        {
            get
            {
                return ((this._typeName != null) && (this._typeName != string.Empty));
            }
        }

        public string TypeName
        {
            get
            {
                return this._typeName;
            }
            set
            {
                this._typeName = value;
            }
        }
    }
}

