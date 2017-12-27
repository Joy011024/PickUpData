namespace FluorineFx.Messaging.Rtmp.Event
{
    using FluorineFx.Util;
    using System;

    [CLSCompliant(false)]
    public sealed class FlexInvoke : Invoke
    {
        private string _cmd;
        private object _cmdData;
        private object[] _parameters;
        private object _response;

        internal FlexInvoke()
        {
            base._dataType = 0x11;
            this.SetResponseSuccess();
        }

        internal FlexInvoke(ByteBuffer data) : base(data)
        {
            base._dataType = 0x11;
        }

        internal FlexInvoke(string cmd, int invokeId, object cmdData, object[] parameters) : this()
        {
            this._cmd = cmd;
            base.InvokeId = invokeId;
            this._cmdData = cmdData;
            this._parameters = parameters;
        }

        public void SetResponseFailure()
        {
            this._cmd = "_error";
        }

        public void SetResponseSuccess()
        {
            this._cmd = "_result";
        }

        public string Cmd
        {
            get
            {
                return this._cmd;
            }
            set
            {
                this._cmd = value;
            }
        }

        public object CmdData
        {
            get
            {
                return this._cmdData;
            }
        }

        public object[] Parameters
        {
            get
            {
                return this._parameters;
            }
            set
            {
                this._parameters = value;
            }
        }

        public object Response
        {
            get
            {
                return this._response;
            }
            set
            {
                this._response = value;
            }
        }
    }
}

