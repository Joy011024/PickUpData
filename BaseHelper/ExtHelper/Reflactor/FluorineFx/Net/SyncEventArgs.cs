namespace FluorineFx.Net
{
    using FluorineFx;
    using System;

    public class SyncEventArgs : EventArgs
    {
        private ASObject[] _changeList;

        internal SyncEventArgs(ASObject[] changeList)
        {
            this._changeList = changeList;
        }

        public ASObject[] ChangeList
        {
            get
            {
                return this._changeList;
            }
        }
    }
}

