namespace FluorineFx.IO
{
    using System;

    public class AMFHeader
    {
        private object _content;
        private bool _mustUnderstand;
        private string _name;
        public const string ClearedCredentials = "ClearedCredentials";
        public const string CredentialsHeader = "Credentials";
        public const string CredentialsIdHeader = "CredentialsId";
        public const string DebugHeader = "amf_server_debug";
        public const string RequestPersistentHeader = "RequestPersistentHeader";
        public const string ServiceBrowserHeader = "DescribeService";

        public AMFHeader(string name, bool mustUnderstand, object content)
        {
            this._name = name;
            this._mustUnderstand = mustUnderstand;
            this._content = content;
        }

        public object Content
        {
            get
            {
                return this._content;
            }
        }

        public bool IsClearedCredentials
        {
            get
            {
                return ((this._content is string) && (((string) this._content) == "ClearedCredentials"));
            }
        }

        public bool MustUnderstand
        {
            get
            {
                return this._mustUnderstand;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }
    }
}

