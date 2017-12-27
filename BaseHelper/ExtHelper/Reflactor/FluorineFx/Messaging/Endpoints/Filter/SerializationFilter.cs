namespace FluorineFx.Messaging.Endpoints.Filter
{
    using FluorineFx.IO;
    using FluorineFx.Messaging.Endpoints;
    using System;

    internal class SerializationFilter : AbstractFilter
    {
        private bool _useLegacyCollection = false;

        public override void Invoke(AMFContext context)
        {
            AMFSerializer serializer = new AMFSerializer(context.OutputStream) {
                UseLegacyCollection = this._useLegacyCollection
            };
            serializer.WriteMessage(context.MessageOutput);
            serializer.Flush();
        }

        public bool UseLegacyCollection
        {
            get
            {
                return this._useLegacyCollection;
            }
            set
            {
                this._useLegacyCollection = value;
            }
        }
    }
}

