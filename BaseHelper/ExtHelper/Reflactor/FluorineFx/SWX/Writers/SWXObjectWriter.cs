namespace FluorineFx.SWX.Writers
{
    using FluorineFx.SWX;
    using System;
    using System.Collections;

    internal class SWXObjectWriter : ISWXWriter
    {
        public void WriteData(SwxAssembler assembler, object data)
        {
            if (data is IList)
            {
                IList list = data as IList;
                object[] array = new object[list.Count];
                list.CopyTo(array, 0);
                assembler.PushArray(array);
            }
            else if (data is IDictionary)
            {
                assembler.PushAssociativeArray(data as IDictionary);
            }
            else
            {
                assembler.PushObject(data);
            }
        }

        public bool IsPrimitive
        {
            get
            {
                return false;
            }
        }
    }
}

