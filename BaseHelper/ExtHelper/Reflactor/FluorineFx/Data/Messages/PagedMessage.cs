namespace FluorineFx.Data.Messages
{
    using System;

    [CLSCompliant(false)]
    public class PagedMessage : SequencedMessage
    {
        private int _pageCount;
        private int _pageIndex;

        public int pageCount
        {
            get
            {
                return this._pageCount;
            }
            set
            {
                this._pageCount = value;
            }
        }

        public int pageIndex
        {
            get
            {
                return this._pageIndex;
            }
            set
            {
                this._pageIndex = value;
            }
        }
    }
}

