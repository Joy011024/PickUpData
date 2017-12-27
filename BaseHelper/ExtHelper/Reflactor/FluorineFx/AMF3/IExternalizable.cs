namespace FluorineFx.AMF3
{
    using System;

    [CLSCompliant(false)]
    public interface IExternalizable
    {
        void ReadExternal(IDataInput input);
        void WriteExternal(IDataOutput output);
    }
}

