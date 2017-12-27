namespace FluorineFx.Data
{
    using System;

    internal class ItemWrapper
    {
        private object _instance;

        public ItemWrapper(object instance)
        {
            this._instance = instance;
        }

        public object Instance
        {
            get
            {
                return this._instance;
            }
            set
            {
                this._instance = value;
            }
        }
    }
}

