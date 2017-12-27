namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class RuntimeSettings
    {
        private bool _asyncHandler = false;
        private int _maxWorkerThreads = 0;
        private int _minWorkerThreads = 0;

        [XmlAttribute(DataType="boolean", AttributeName="asyncHandler")]
        public bool AsyncHandler
        {
            get
            {
                return this._asyncHandler;
            }
            set
            {
                this._asyncHandler = value;
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

