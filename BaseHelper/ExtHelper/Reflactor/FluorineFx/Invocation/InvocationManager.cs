namespace FluorineFx.Invocation
{
    using System;
    using System.Collections.Generic;

    internal class InvocationManager : IInvocationManager
    {
        private Stack<object> _context = new Stack<object>();
        private Dictionary<object, object> _properties = new Dictionary<object, object>();
        private object _result;

        public Stack<object> Context
        {
            get
            {
                return this._context;
            }
        }

        public Dictionary<object, object> Properties
        {
            get
            {
                return this._properties;
            }
        }

        public object Result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
            }
        }
    }
}

