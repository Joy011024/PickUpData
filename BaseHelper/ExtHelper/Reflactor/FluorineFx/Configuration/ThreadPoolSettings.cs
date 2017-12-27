namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class ThreadPoolSettings
    {
        private int _idleTimeout = 0xea60;
        private int _maxWorkerThreads = 0x3e8;
        private int _minWorkerThreads = 0;

        [XmlAttribute(DataType="int", AttributeName="idleTimeout")]
        public int IdleTimeout
        {
            get
            {
                return this._idleTimeout;
            }
            set
            {
                this._idleTimeout = value;
            }
        }

        [XmlAttribute(DataType="int", AttributeName="maxWorkerThreads")]
        public int MaxWorkerThreads
        {
            get
            {
                return this._maxWorkerThreads;
            }
            set
            {
                this._maxWorkerThreads = value;
            }
        }

        [XmlAttribute(DataType="int", AttributeName="minWorkerThreads")]
        public int MinWorkerThreads
        {
            get
            {
                return this._minWorkerThreads;
            }
            set
            {
                this._minWorkerThreads = value;
            }
        }
    }
}

