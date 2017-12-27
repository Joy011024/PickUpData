namespace FluorineFx.Messaging.Api.Messaging
{
    using System;
    using System.Collections.Generic;

    public class OOBControlMessage
    {
        private object _result;
        private string _serviceName;
        private Dictionary<string, object> _serviceParameterMap;
        private string _target;

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

        public string ServiceName
        {
            get
            {
                return this._serviceName;
            }
            set
            {
                this._serviceName = value;
            }
        }

        public Dictionary<string, object> ServiceParameterMap
        {
            get
            {
                if (this._serviceParameterMap == null)
                {
                    this._serviceParameterMap = new Dictionary<string, object>();
                }
                return this._serviceParameterMap;
            }
            set
            {
                this._serviceParameterMap = value;
            }
        }

        public string Target
        {
            get
            {
                return this._target;
            }
            set
            {
                this._target = value;
            }
        }
    }
}

