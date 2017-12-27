namespace FluorineFx.IO
{
    using FluorineFx;
    using FluorineFx.Messaging.Messages;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    [CLSCompliant(false)]
    public class AMFBody
    {
        protected object _content;
        protected bool _ignoreResults;
        protected bool _isAuthenticationAction;
        protected bool _isDebug;
        protected bool _isDescribeService;
        protected string _response;
        protected string _target;
        public const string OnDebugEvents = "/onDebugEvents";
        public const string OnResult = "/onResult";
        public const string OnStatus = "/onStatus";
        public const string Recordset = "rs://";

        public AMFBody()
        {
        }

        public AMFBody(string target, string response, object content)
        {
            this._target = target;
            this._response = response;
            this._content = content;
        }

        public virtual IList GetParameterList()
        {
            IList body = null;
            if (!this.IsEmptyTarget)
            {
                if (this._content is IList)
                {
                    body = this._content as IList;
                }
                else
                {
                    body = new List<object>();
                    body.Add(this._content);
                }
            }
            else
            {
                object content = this.Content;
                if (content is IList)
                {
                    content = (content as IList)[0];
                }
                IMessage message = content as IMessage;
                if ((message != null) && (message is RemotingMessage))
                {
                    body = message.body as IList;
                }
            }
            if (body == null)
            {
                body = new List<object>();
            }
            return body;
        }

        public string GetRecordsetArgs()
        {
            if ((this._target != null) && this.IsRecordsetDelivery)
            {
                string str = this._target.Substring("rs://".Length);
                return str.Substring(0, str.IndexOf("/"));
            }
            return null;
        }

        public string GetSignature()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Target);
            IList parameterList = this.GetParameterList();
            for (int i = 0; i < parameterList.Count; i++)
            {
                object obj2 = parameterList[i];
                builder.Append(obj2.GetType().FullName);
            }
            return builder.ToString();
        }

        internal void WriteBody(ObjectEncoding objectEncoding, AMFWriter writer)
        {
            writer.Reset();
            if (this.Target == null)
            {
                writer.WriteUTF("null");
            }
            else
            {
                writer.WriteUTF(this.Target);
            }
            if (this.Response == null)
            {
                writer.WriteUTF("null");
            }
            else
            {
                writer.WriteUTF(this.Response);
            }
            writer.WriteInt32(-1);
            this.WriteBodyData(objectEncoding, writer);
        }

        protected virtual void WriteBodyData(ObjectEncoding objectEncoding, AMFWriter writer)
        {
            object content = this.Content;
            writer.WriteData(objectEncoding, content);
        }

        public string Call
        {
            get
            {
                return (this.TypeName + "." + this.Method);
            }
        }

        public object Content
        {
            get
            {
                return this._content;
            }
            set
            {
                this._content = value;
            }
        }

        public bool IgnoreResults
        {
            get
            {
                return this._ignoreResults;
            }
            set
            {
                this._ignoreResults = value;
            }
        }

        public bool IsAuthenticationAction
        {
            get
            {
                return this._isAuthenticationAction;
            }
            set
            {
                this._isAuthenticationAction = value;
            }
        }

        public bool IsDebug
        {
            get
            {
                return this._isDebug;
            }
            set
            {
                this._isDebug = value;
            }
        }

        public bool IsDescribeService
        {
            get
            {
                return this._isDescribeService;
            }
            set
            {
                this._isDescribeService = value;
            }
        }

        public bool IsEmptyTarget
        {
            get
            {
                return (((this._target == null) || (this._target == string.Empty)) || (this._target == "null"));
            }
        }

        public bool IsRecordsetDelivery
        {
            get
            {
                return this._target.StartsWith("rs://");
            }
        }

        public bool IsWebService
        {
            get
            {
                return ((this.TypeName != null) && this.TypeName.ToLower().EndsWith(".asmx"));
            }
        }

        public string Method
        {
            get
            {
                if ((((this._target != "null") && (this._target != null)) && (this._target != string.Empty)) && ((this._target != null) && (this._target.LastIndexOf('.') != -1)))
                {
                    string str = this._target;
                    if (this.IsRecordsetDelivery)
                    {
                        str = str.Substring("rs://".Length);
                        str = str.Substring(str.IndexOf("/") + 1);
                    }
                    if (this.IsRecordsetDelivery)
                    {
                        str = str.Substring(0, str.LastIndexOf('.'));
                    }
                    return str.Substring(str.LastIndexOf('.') + 1);
                }
                return null;
            }
        }

        public string Response
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

        public string TypeName
        {
            get
            {
                if ((((this._target != "null") && (this._target != null)) && (this._target != string.Empty)) && (this._target.LastIndexOf('.') != -1))
                {
                    string str = this._target.Substring(0, this._target.LastIndexOf('.'));
                    if (this.IsRecordsetDelivery)
                    {
                        str = str.Substring("rs://".Length);
                        str = str.Substring(str.IndexOf("/") + 1);
                        str = str.Substring(0, str.LastIndexOf('.'));
                    }
                    return str;
                }
                return null;
            }
        }
    }
}

