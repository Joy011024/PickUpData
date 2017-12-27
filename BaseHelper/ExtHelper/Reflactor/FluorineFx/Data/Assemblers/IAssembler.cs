namespace FluorineFx.Data.Assemblers
{
    using System;
    using System.Collections;

    public interface IAssembler
    {
        void AddItemToFill(IList fillParameters, int position, Hashtable identity);
        bool AutoRefreshFill(IList fillParameters);
        int Count(IList countParameters);
        void CreateItem(object item);
        void DeleteItem(object previousVersion);
        IList Fill(IList list);
        object GetItem(IDictionary identity);
        IList GetItems(IList identityList);
        int RefreshFill(IList fillParameters, object item, bool isCreate);
        void RemoveItemFromFill(IList fillParameters, int position, Hashtable identity);
        void UpdateItem(object newVersion, object previousVersion, IList changes);
    }
}

