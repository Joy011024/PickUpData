namespace FluorineFx.Data
{
    using System;

    public class UpdateCollectionRange
    {
        private object[] _identities;
        private int _position;
        private int _updateType;
        public const int DeleteFromCollection = 1;
        public const int InsertIntoCollection = 0;

        public object[] identities
        {
            get
            {
                return this._identities;
            }
            set
            {
                this._identities = value;
            }
        }

        public int position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }

        public int updateType
        {
            get
            {
                return this._updateType;
            }
            set
            {
                this._updateType = value;
            }
        }
    }
}

