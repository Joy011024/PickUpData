namespace FluorineFx
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class PageSizeAttribute : Attribute
    {
        private int _limit;
        private int _offset;
        private int _pageSize;

        public PageSizeAttribute(int pageSize)
        {
            this._pageSize = pageSize;
            this._offset = 0;
            this._limit = 0x19;
        }

        public PageSizeAttribute(int pageSize, int offset, int limit)
        {
            this._pageSize = pageSize;
            this._offset = offset;
            this._limit = limit;
        }

        public int Limit
        {
            get
            {
                return this._limit;
            }
        }

        public int Offset
        {
            get
            {
                return this._offset;
            }
        }

        public int PageSize
        {
            get
            {
                return this._pageSize;
            }
        }
    }
}

