namespace FluorineFx.Diagnostic
{
    using FluorineFx.IO;
    using System;
    using System.Collections;

    internal class AMFRequestHeaders : DebugEvent
    {
        public AMFRequestHeaders(AMFMessage amfMessage)
        {
            this["EventType"] = "AmfHeaders";
            Hashtable hashtable = new Hashtable();
            for (int i = 0; i < amfMessage.HeaderCount; i++)
            {
                AMFHeader headerAt = amfMessage.GetHeaderAt(i);
                if ((headerAt != null) && (headerAt.Name != null))
                {
                    hashtable[headerAt.Name] = headerAt.Content;
                }
            }
            this["AmfHeaders"] = hashtable;
        }
    }
}

