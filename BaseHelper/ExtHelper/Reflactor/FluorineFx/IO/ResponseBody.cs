namespace FluorineFx.IO
{
    using System;
    using System.Collections;

    [CLSCompliant(false)]
    public class ResponseBody : AMFBody
    {
        private AMFBody _requestBody;

        internal ResponseBody()
        {
        }

        public ResponseBody(AMFBody requestBody)
        {
            this._requestBody = requestBody;
        }

        public ResponseBody(AMFBody requestBody, object content)
        {
            this._requestBody = requestBody;
            base._target = requestBody.Response + "/onResult";
            base._content = content;
            base._response = "null";
        }

        public override IList GetParameterList()
        {
            if (this._requestBody == null)
            {
                return null;
            }
            return this._requestBody.GetParameterList();
        }

        public AMFBody RequestBody
        {
            get
            {
                return this._requestBody;
            }
            set
            {
                this._requestBody = value;
            }
        }
    }
}

