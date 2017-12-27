namespace FluorineFx.Data.Messages
{
    using FluorineFx.Messaging.Messages;
    using System;
    using System.Collections;

    [CLSCompliant(false)]
    public class DataMessage : AsyncMessage
    {
        private Hashtable _identity;
        protected int _operation;
        public const int AssociationAddOperation = 13;
        public const int AssociationRemoveOperation = 14;
        public const int BatchedOperation = 5;
        public const int CountOperation = 10;
        public const int CreateAndSequenceOperation = 11;
        public const int CreateOperation = 0;
        public const int DeleteOperation = 4;
        public const int FillOperation = 1;
        public const int GetOperation = 2;
        public const int GetOrCreateOperation = 9;
        public const int GetSequenceIdOperation = 12;
        public const int MultiBatchOperation = 6;
        public const string PageIndexHeader = "pageIndex";
        public const int PageItemsOperation = 20;
        public const int PageOperation = 8;
        public const string PageSizeHeader = "pageSize";
        public const int RefreshFillOperation = 0x10;
        public const int ReleaseCollectionOperation = 0x12;
        public const int ReleaseItemOperation = 0x13;
        public const string SequenceIdHeader = "sequenceId";
        public const int TransactedOperation = 7;
        public const int UpdateBodyChanges = 0;
        public const int UpdateBodyNew = 2;
        public const int UpdateBodyPrev = 1;
        public const int UpdateCollectionOperation = 0x11;
        public const int UpdateOperation = 3;

        public DataMessage()
        {
            base._timestamp = Environment.TickCount;
        }

        public Hashtable identity
        {
            get
            {
                return this._identity;
            }
            set
            {
                this._identity = value;
            }
        }

        public int operation
        {
            get
            {
                return this._operation;
            }
            set
            {
                this._operation = value;
            }
        }
    }
}

