namespace FluorineFx.IO
{
    using System;

    public class KeyFrameMeta
    {
        private bool _audioOnly;
        private long _duration;
        private long[] _positions;
        private int[] _timestamps;

        public bool AudioOnly
        {
            get
            {
                return this._audioOnly;
            }
            set
            {
                this._audioOnly = value;
            }
        }

        public long Duration
        {
            get
            {
                return this._duration;
            }
            set
            {
                this._duration = value;
            }
        }

        public long[] Positions
        {
            get
            {
                return this._positions;
            }
            set
            {
                this._positions = value;
            }
        }

        public int[] Timestamps
        {
            get
            {
                return this._timestamps;
            }
            set
            {
                this._timestamps = value;
            }
        }
    }
}

