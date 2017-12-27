namespace FluorineFx.Diagnostic
{
    using System;
    using System.Collections;

    internal class TraceHeader : DebugEvent
    {
        public TraceHeader(ArrayList traceStack)
        {
            this["EventType"] = "trace";
            this["messages"] = traceStack;
        }
    }
}

