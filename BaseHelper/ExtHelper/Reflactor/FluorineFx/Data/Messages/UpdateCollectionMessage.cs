namespace FluorineFx.Data.Messages
{
    using FluorineFx.Data;
    using System;

    [CLSCompliant(false)]
    public class UpdateCollectionMessage : DataMessage
    {
        private object[] _collectionId;
        private bool _replace;
        private int _updateMode;
        public const int ClientUpdate = 0;
        public const int ServerOverride = 2;
        public const int ServerUpdate = 1;

        public UpdateCollectionMessage()
        {
            base._operation = 0x11;
        }

        public void AddItemIdentityChange(int updateType, int position, object identity)
        {
            UpdateCollectionRange range = new UpdateCollectionRange {
                position = position,
                updateType = updateType,
                identities = new object[] { identity }
            };
            object[] body = base.body as object[];
            if (body == null)
            {
                body = new object[] { range };
            }
            else
            {
                object[] array = new object[body.Length + 1];
                body.CopyTo(array, 0);
                body[body.Length - 1] = range;
            }
            base.body = body;
        }

        public object[] collectionId
        {
            get
            {
                return this._collectionId;
            }
            set
            {
                this._collectionId = value;
            }
        }

        public bool replace
        {
            get
            {
                return this._replace;
            }
            set
            {
                this._replace = value;
            }
        }

        public int updateMode
        {
            get
            {
                return this._updateMode;
            }
            set
            {
                this._updateMode = value;
            }
        }
    }
}

