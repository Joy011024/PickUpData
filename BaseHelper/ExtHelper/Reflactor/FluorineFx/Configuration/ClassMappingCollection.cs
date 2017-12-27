namespace FluorineFx.Configuration
{
    using FluorineFx.Collections.Generic;
    using System;
    using System.Collections.Generic;

    public sealed class ClassMappingCollection : CollectionBase<ClassMapping>
    {
        private Dictionary<string, string> _customClassToType = new Dictionary<string, string>();
        private Dictionary<string, string> _typeToCustomClass = new Dictionary<string, string>();

        public ClassMappingCollection()
        {
            this.Add("FluorineFx.AMF3.ArrayCollection", "flex.messaging.io.ArrayCollection");
            this.Add("FluorineFx.AMF3.ByteArray", "flex.messaging.io.ByteArray");
            this.Add("FluorineFx.AMF3.ObjectProxy", "flex.messaging.io.ObjectProxy");
            this.Add("FluorineFx.Messaging.Messages.CommandMessage", "flex.messaging.messages.CommandMessage");
            this.Add("FluorineFx.Messaging.Messages.RemotingMessage", "flex.messaging.messages.RemotingMessage");
            this.Add("FluorineFx.Messaging.Messages.AsyncMessage", "flex.messaging.messages.AsyncMessage");
            this.Add("FluorineFx.Messaging.Messages.AcknowledgeMessage", "flex.messaging.messages.AcknowledgeMessage");
            this.Add("FluorineFx.Data.Messages.DataMessage", "flex.data.messages.DataMessage");
            this.Add("FluorineFx.Data.Messages.PagedMessage", "flex.data.messages.PagedMessage");
            this.Add("FluorineFx.Data.Messages.UpdateCollectionMessage", "flex.data.messages.UpdateCollectionMessage");
            this.Add("FluorineFx.Data.Messages.SequencedMessage", "flex.data.messages.SequencedMessage");
            this.Add("FluorineFx.Data.Messages.DataErrorMessage", "flex.data.messages.DataErrorMessage");
            this.Add("FluorineFx.Messaging.Messages.ErrorMessage", "flex.messaging.messages.ErrorMessage");
            this.Add("FluorineFx.Messaging.Messages.RemotingMessage", "flex.messaging.messages.RemotingMessage");
            this.Add("FluorineFx.Messaging.Messages.RPCMessage", "flex.messaging.messages.RPCMessage");
            this.Add("FluorineFx.Data.UpdateCollectionRange", "flex.data.UpdateCollectionRange");
            this.Add("FluorineFx.Messaging.Services.RemotingService", "flex.messaging.services.RemotingService");
            this.Add("FluorineFx.Messaging.Services.MessageService", "flex.messaging.services.MessageService");
            this.Add("FluorineFx.Data.DataService", "flex.data.DataService");
            this.Add("FluorineFx.Messaging.Endpoints.RtmpEndpoint", "flex.messaging.endpoints.RTMPEndpoint");
            this.Add("FluorineFx.Messaging.Endpoints.AMFEndpoint", "flex.messaging.endpoints.AMFEndpoint");
            this.Add("FluorineFx.Messaging.Services.Remoting.DotNetAdapter", "flex.messaging.services.remoting.adapters.JavaAdapter");
        }

        public override void Add(ClassMapping value)
        {
            this._typeToCustomClass[value.Type] = value.CustomClass;
            this._customClassToType[value.CustomClass] = value.Type;
            base.Add(value);
        }

        public void Add(string type, string customClass)
        {
            ClassMapping item = new ClassMapping {
                Type = type,
                CustomClass = customClass
            };
            this.Add(item);
        }

        public string GetCustomClass(string type)
        {
            if (this._typeToCustomClass.ContainsKey(type))
            {
                return this._typeToCustomClass[type];
            }
            return type;
        }

        public string GetType(string customClass)
        {
            if (customClass == null)
            {
                return null;
            }
            if (this._customClassToType.ContainsKey(customClass))
            {
                return this._customClassToType[customClass];
            }
            return customClass;
        }

        public override void Insert(int index, ClassMapping value)
        {
            this._typeToCustomClass[value.Type] = value.CustomClass;
            this._customClassToType[value.CustomClass] = value.Type;
            base.Insert(index, value);
        }

        public override bool Remove(ClassMapping value)
        {
            this._typeToCustomClass.Remove(value.Type);
            this._customClassToType.Remove(value.CustomClass);
            return base.Remove(value);
        }
    }
}

