namespace FluorineFx.Data
{
    using FluorineFx;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Text;

    internal class Sequence : CollectionBase
    {
        private int _id;
        private object[] _parameters;
        private Hashtable _subcribers = new Hashtable();
        private static readonly ILog log = LogManager.GetLogger(typeof(Sequence));

        public int Add(Identity identity)
        {
            return base.InnerList.Add(identity);
        }

        public void AddSubscriber(string clientId)
        {
            lock (this._subcribers)
            {
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    log.Debug(__Res.GetString("Sequence_AddSubscriber", new object[] { clientId, this._id }));
                }
                if (!clientId.StartsWith("srv:"))
                {
                    this._subcribers[clientId] = clientId;
                }
            }
        }

        public bool Contains(Identity identity)
        {
            return base.InnerList.Contains(identity);
        }

        internal void Dump(DumpContext dumpContext)
        {
            dumpContext.AppendLine("Sequence Id = " + this._id.ToString() + " Count = " + base.Count.ToString() + " Subscribers = " + this.SubscriberCount.ToString());
            dumpContext.Indent();
            int num = Math.Min(base.Count, 20);
            StringBuilder builder = new StringBuilder();
            builder.Append("[ ");
            for (int i = 0; i < num; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }
                Identity identity = this[i];
                builder.Append("[");
                foreach (DictionaryEntry entry in identity)
                {
                    builder.Append(identity[entry.Key].ToString());
                }
                builder.Append("]");
            }
            builder.Append(" ]");
            dumpContext.AppendLine(builder.ToString());
            dumpContext.Unindent();
        }

        public int IndexOf(Identity identity)
        {
            return base.InnerList.IndexOf(identity);
        }

        public void Insert(int index, Identity identity)
        {
            base.InnerList.Insert(index, identity);
        }

        public void Remove(Identity identity)
        {
            base.InnerList.Remove(identity);
        }

        public void RemoveSubscriber(string clientId)
        {
            lock (this._subcribers)
            {
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    log.Debug(__Res.GetString("Sequence_RemoveSubscriber", new object[] { clientId, this._id }));
                }
                if (this._subcribers.Contains(clientId))
                {
                    this._subcribers.Remove(clientId);
                }
            }
        }

        public int Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public Identity this[int index]
        {
            get
            {
                return (base.InnerList[index] as Identity);
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

        public int Size
        {
            get
            {
                return base.Count;
            }
        }

        public int SubscriberCount
        {
            get
            {
                lock (this._subcribers)
                {
                    return this._subcribers.Count;
                }
            }
        }
    }
}

