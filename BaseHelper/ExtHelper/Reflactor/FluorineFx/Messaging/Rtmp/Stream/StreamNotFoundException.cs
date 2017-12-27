namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Exceptions;
    using System;

    public class StreamNotFoundException : FluorineException
    {
        public StreamNotFoundException(string name) : base("Stream " + name + " not found.")
        {
        }
    }
}

