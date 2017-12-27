namespace FluorineFx.Data.Assemblers
{
    using System;
    using System.Collections;

    public class Assembler : IAssembler
    {
        public const int AppendToFill = 2;
        public const int DoNotExecuteFill = 0;
        public const int ExecuteFill = 1;
        public const int RemoveFromFill = 3;

        public virtual void AddItemToFill(IList fillParameters, int position, Hashtable identity)
        {
        }

        public virtual bool AutoRefreshFill(IList fillParameters)
        {
            return true;
        }

        public virtual int Count(IList countParameters)
        {
            throw new NotSupportedException("Assembler does not implement Count method");
        }

        public virtual void CreateItem(object item)
        {
            throw new NotSupportedException("Assembler does not implement CreateItem method");
        }

        public virtual void DeleteItem(object previousVersion)
        {
            throw new NotSupportedException("Assembler does not implement DeleteItem method");
        }

        public virtual IList Fill(IList list)
        {
            throw new NotSupportedException("Assembler does not implement Fill method");
        }

        public virtual object GetItem(IDictionary identity)
        {
            throw new NotSupportedException("Assembler does not implement GetItem method");
        }

        public virtual IList GetItems(IList identityList)
        {
            ArrayList list = new ArrayList(identityList.Count);
            for (int i = 0; i < identityList.Count; i++)
            {
                list.Add(this.GetItem(identityList[i] as IDictionary));
            }
            return list;
        }

        public virtual int RefreshFill(IList fillParameters, object item, bool isCreate)
        {
            return 1;
        }

        public virtual void RemoveItemFromFill(IList fillParameters, int position, Hashtable identity)
        {
        }

        public virtual void UpdateItem(object newVersion, object previousVersion, IList changes)
        {
            throw new NotSupportedException("Assembler does not implement UpdateItem method");
        }
    }
}

