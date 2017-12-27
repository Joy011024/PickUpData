namespace FluorineFx.Messaging.Endpoints.Filter
{
    using FluorineFx.IO;
    using FluorineFx.Messaging.Endpoints;
    using System;

    internal class DeserializationFilter : AbstractFilter
    {
        private bool _useLegacyCollection = false;

        public override void Invoke(AMFContext context)
        {
            AMFDeserializer deserializer = new AMFDeserializer(context.InputStream) {
                UseLegacyCollection = this._useLegacyCollection
            };
            AMFMessage message = deserializer.ReadAMFMessage();
            context.AMFMessage = message;
            context.MessageOutput = new MessageOutput(message.Version);
            if (deserializer.FailedAMFBodies.Length > 0)
            {
                AMFBody[] failedAMFBodies = deserializer.FailedAMFBodies;
                for (int i = 0; i < failedAMFBodies.Length; i++)
                {
                    context.MessageOutput.AddBody(failedAMFBodies[i]);
                }
            }
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

