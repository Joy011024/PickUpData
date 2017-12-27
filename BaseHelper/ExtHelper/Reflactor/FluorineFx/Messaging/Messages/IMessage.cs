namespace FluorineFx.Messaging.Messages
{
    using System;
    using System.Collections.Generic;

    public interface IMessage : ICloneable
    {
        object GetHeader(string name);
        bool HeaderExists(string name);
        void SetHeader(string name, object value);

        object body { get; set; }

        object clientId { get; set; }

        string destination { get; set; }

        Dictionary<string, object> headers { get; set; }

        string messageId { get; set; }

        long timestamp { get; set; }

        long timeToLive { get; set; }
    }
}

