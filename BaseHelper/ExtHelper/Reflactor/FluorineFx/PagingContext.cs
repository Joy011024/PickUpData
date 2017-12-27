namespace FluorineFx
{
    using FluorineFx.Context;
    using System;

    public class PagingContext
    {
        private int _limit;
        private int _offset;
        private const string FluorinePagingContextKey = "__@fluorinepagingcontext";

        internal PagingContext(int offset, int limit)
        {
            this._offset = offset;
            this._limit = limit;
        }

        internal static void SetPagingContext(PagingContext current)
        {
            FluorineContext.Current.Items["__@fluorinepagingcontext"] = current;
        }

        public static PagingContext Current
        {
            get
            {
                return (FluorineContext.Current.Items["__@fluorinepagingcontext"] as PagingContext);
            }
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
    }
}

