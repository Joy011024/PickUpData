namespace FluorineFx.IO
{
    using FluorineFx;
    using FluorineFx.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    [CLSCompliant(false)]
    public class AMFMessage
    {
        protected List<AMFBody> _bodies;
        protected List<AMFHeader> _headers;
        protected ushort _version;

        public AMFMessage() : this(0)
        {
        }

        public AMFMessage(ushort version)
        {
            this._version = 0;
            this._version = version;
            this._headers = new List<AMFHeader>(1);
            this._bodies = new List<AMFBody>(1);
        }

        public void AddBody(AMFBody body)
        {
            this._bodies.Add(body);
        }

        public void AddHeader(AMFHeader header)
        {
            this._headers.Add(header);
        }

        public AMFBody GetBodyAt(int index)
        {
            return this._bodies[index];
        }

        public AMFHeader GetHeader(string header)
        {
            for (int i = 0; (this._headers != null) && (i < this._headers.Count); i++)
            {
                AMFHeader header2 = this._headers[i];
                if (header2.Name == header)
                {
                    return header2;
                }
            }
            return null;
        }

        public AMFHeader GetHeaderAt(int index)
        {
            return this._headers[index];
        }

        public void RemoveHeader(string header)
        {
            for (int i = 0; (this._headers != null) && (i < this._headers.Count); i++)
            {
                AMFHeader header2 = this._headers[i];
                if (header2.Name == header)
                {
                    this._headers.RemoveAt(i);
                }
            }
        }

        public ReadOnlyCollection<AMFBody> Bodies
        {
            get
            {
                return this._bodies.AsReadOnly();
            }
        }

        public int BodyCount
        {
            get
            {
                return this._bodies.Count;
            }
        }

        public int HeaderCount
        {
            get
            {
                return this._headers.Count;
            }
        }

        public FluorineFx.ObjectEncoding ObjectEncoding
        {
            get
            {
                if ((this._version == 0) || (this._version == 1))
                {
                    return FluorineFx.ObjectEncoding.AMF0;
                }
                if (this._version != 3)
                {
                    throw new UnexpectedAMF();
                }
                return FluorineFx.ObjectEncoding.AMF3;
            }
        }

        public ushort Version
        {
            get
            {
                return this._version;
            }
        }
    }
}

