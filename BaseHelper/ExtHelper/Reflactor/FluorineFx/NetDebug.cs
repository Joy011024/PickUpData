namespace FluorineFx
{
    using System;
    using System.Collections;

    public class NetDebug
    {
        private static ArrayList _traceStack = ArrayList.Synchronized(new ArrayList());

        private NetDebug()
        {
        }

        public static void Clear()
        {
            _traceStack.Clear();
        }

        internal static ArrayList GetTraceStack()
        {
            return _traceStack;
        }

        public static void Trace(object obj)
        {
            if (obj != null)
            {
                _traceStack.Add(obj);
            }
        }

        public static void Trace(string message)
        {
            if (message != null)
            {
                _traceStack.Add(message);
            }
        }
    }
}

