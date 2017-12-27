namespace FluorineFx.Util
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public class UriBase
    {
        private string _host;
        private NameValueCollection _parameters;
        private string _password;
        private string _path;
        private string _port;
        private string _protocol;
        private string _user;

        public UriBase()
        {
            this.Clear();
        }

        public UriBase(UriBase uri)
        {
            this.Clear();
            this.ParseUri(uri.Uri);
        }

        public UriBase(string uri)
        {
            this.Clear();
            this.ParseUri(uri);
        }

        public UriBase(string user, string password, string path, string host, string protocol, string port)
        {
            this._user = user;
            this._password = password;
            this._path = path;
            this._host = host;
            this._protocol = protocol;
            this._port = port;
        }

        public UriBase(string user, string password, string path, string host, string protocol, string port, NameValueCollection parameters)
        {
            this._user = user;
            this._password = password;
            this._path = path;
            this._host = host;
            this._protocol = protocol;
            this._port = port;
            this._parameters = parameters;
        }

        public void Clear()
        {
            this._protocol = "";
            this._host = "";
            this._port = null;
            this._path = "";
            this._user = "";
            this._password = "";
            this._parameters = null;
        }

        public void CopyTo(UriBase uri)
        {
            this.Uri = uri.Uri;
        }

        public bool EqualTo(UriBase uri)
        {
            if (uri == null)
            {
                return false;
            }
            return (this.Uri == uri.Uri);
        }

        private void InternalParseUri(string url)
        {
            try
            {
                string input = url;
                if (input.Length == 0)
                {
                    input = ":///";
                }
                Match match = new Regex(@"^(?<protocol>[\w\%]*)://((?'username'[\w\%]*)(:(?'password'[\w\%]*))?@)?(?'host'[\{\}\w\.\(\)\-\%\\\$]*)(:?(?'port'[\{\}\w\.]+))?(/(?'path'[^?]*)?(\?(?'params'.*))?)?").Match(input);
                if (!match.Success)
                {
                    throw new ApplicationException("This Uri cannot be parsed.");
                }
                string str2 = HttpUtility.UrlDecode(match.Result("${username}"));
                string str3 = HttpUtility.UrlDecode(match.Result("${password}"));
                string str4 = HttpUtility.UrlDecode(match.Result("${path}"));
                string str5 = HttpUtility.UrlDecode(match.Result("${host}"));
                string str6 = HttpUtility.UrlDecode(match.Result("${protocol}"));
                string str7 = null;
                if (match.Result("${port}").Length != 0)
                {
                    str7 = match.Result("${port}");
                }
                string str8 = match.Result("${params}");
                NameValueCollection values = new NameValueCollection();
                if ((str8 != null) && (str8 != string.Empty))
                {
                    char[] separator = new char[] { '&' };
                    IEnumerator enumerator = str8.Split(separator).GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        string current = (string) enumerator.Current;
                        separator = new char[] { '=' };
                        string[] strArray = current.Split(separator, 2);
                        if (strArray.Length != 2)
                        {
                            throw new ApplicationException("This Uri cannot be parsed. Invalid parameter.");
                        }
                        values.Add(HttpUtility.UrlDecode(strArray[0]), HttpUtility.UrlDecode(strArray[1]));
                    }
                }
                this._user = str2;
                this._password = str3;
                this._path = str4;
                this._host = str5;
                this._protocol = str6;
                this._port = str7;
                this._parameters = values;
            }
            catch (Exception exception)
            {
                if (exception is ApplicationException)
                {
                    throw;
                }
                throw new ApplicationException("This Uri cannot be parsed.", exception);
            }
        }

        protected void ParseUri(string uri)
        {
            this.InternalParseUri(uri);
        }

        public string Host
        {
            get
            {
                return this._host;
            }
            set
            {
                if (this._host != value)
                {
                    this._host = value;
                }
            }
        }

        public NameValueCollection Parameters
        {
            get
            {
                return this._parameters;
            }
            set
            {
                if (this._parameters != value)
                {
                    this._parameters = value;
                }
            }
        }

        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                if (this._password != value)
                {
                    this._password = value;
                }
            }
        }

        public string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                if (this._path != value)
                {
                    this._path = value;
                }
            }
        }

        public string Port
        {
            get
            {
                return this._port;
            }
            set
            {
                if (this._port != value)
                {
                    this._port = value;
                }
            }
        }

        public string Protocol
        {
            get
            {
                return this._protocol;
            }
            set
            {
                if (this._protocol != value)
                {
                    this._protocol = value;
                }
            }
        }

        public string Uri
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                if (this.Protocol != null)
                {
                    builder.Append(this.Protocol);
                    builder.Append("://");
                    if ((this.User != null) && (this.User != string.Empty))
                    {
                        builder.Append(string.Format("{0}:{1}@", this.User, this.Password));
                    }
                    if (this.Host != null)
                    {
                        builder.Append(this.Host);
                    }
                    if (this.Port != null)
                    {
                        builder.Append(":");
                        builder.Append(this.Port);
                    }
                    if ((this.Path != null) && (this.Path != string.Empty))
                    {
                        builder.Append(string.Format("/{0}", this.Path));
                    }
                    else
                    {
                        builder.Append("/");
                    }
                }
                if (this._parameters != null)
                {
                    for (int i = 0; i < this._parameters.Count; i++)
                    {
                        string key = this._parameters.GetKey(i);
                        string str2 = this._parameters.Get(i);
                        if (i == 0)
                        {
                            builder.Append("?");
                        }
                        else
                        {
                            builder.Append("&");
                        }
                        builder.Append(key);
                        builder.Append("=");
                        builder.Append(str2);
                    }
                }
                return builder.ToString();
            }
            set
            {
                this.ParseUri(value);
            }
        }

        public string User
        {
            get
            {
                return this._user;
            }
            set
            {
                if (this._user != value)
                {
                    this._user = value;
                }
            }
        }
    }
}

