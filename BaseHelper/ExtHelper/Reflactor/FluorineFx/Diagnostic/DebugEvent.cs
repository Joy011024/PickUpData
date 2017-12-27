namespace FluorineFx.Diagnostic
{
    using System;
    using System.Collections;

    internal class DebugEvent : Hashtable
    {
        public DebugEvent()
        {
            this["Source"] = "Server";
            this["EventType"] = "DebugEvent";
            this["Date"] = DateTime.UtcNow;
            this["Time"] = DateTime.UtcNow;
        }
    }
}

