namespace FluorineFx.Messaging.Rtmp.Service
{
    using FluorineFx.Messaging.Api.Service;
    using System;

    [CLSCompliant(false)]
    public class Call : IServiceCall
    {
        protected object[] _arguments;
        protected System.Exception _exception;
        protected string _serviceMethodName;
        protected string _serviceName;
        protected byte _status;
        public const byte STATUS_ACCESS_DENIED = 0x12;
        public const byte STATUS_APP_SHUTTING_DOWN = 0x15;
        public const byte STATUS_GENERAL_EXCEPTION = 20;
        public const byte STATUS_INVOCATION_EXCEPTION = 0x13;
        public const byte STATUS_METHOD_NOT_FOUND = 0x11;
        public const byte STATUS_PENDING = 1;
        public const byte STATUS_SERVICE_NOT_FOUND = 0x10;
        public const byte STATUS_SUCCESS_NULL = 3;
        public const byte STATUS_SUCCESS_RESULT = 2;
        public const byte STATUS_SUCCESS_VOID = 4;

        public Call(string method)
        {
            this._status = 1;
            this._serviceMethodName = method;
        }

        public Call(string method, object[] args)
        {
            this._status = 1;
            this._serviceMethodName = method;
            this._arguments = args;
        }

        public Call(string name, string method, object[] args)
        {
            this._status = 1;
            this._serviceName = name;
            this._serviceMethodName = method;
            this._arguments = args;
        }

        public object[] Arguments
        {
            get
            {
                return this._arguments;
            }
        }

        public System.Exception Exception
        {
            get
            {
                return this._exception;
            }
            set
            {
                this._exception = value;
            }
        }

        public bool IsSuccess
        {
            get
            {
                return (((this._status == 2) || (this._status == 3)) || (this._status == 4));
            }
        }

        public string ServiceMethodName
        {
            get
            {
                return this._serviceMethodName;
            }
        }

        public string ServiceName
        {
            get
            {
                return this._serviceName;
            }
        }

        public byte Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
            }
        }
    }
}

