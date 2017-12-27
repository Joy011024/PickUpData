namespace FluorineFx.IO.Readers
{
    using FluorineFx;
    using FluorineFx.AMF3;
    using FluorineFx.Exceptions;
    using FluorineFx.IO;
    using FluorineFx.IO.Bytecode;
    using System;

    internal class AMF3ExternalizableReader : IReflectionOptimizer
    {
        public object CreateInstance()
        {
            throw new NotImplementedException();
        }

        public object ReadData(AMFReader reader, ClassDefinition classDefinition)
        {
            object instance = ObjectFactory.CreateInstance(classDefinition.ClassName);
            if (instance == null)
            {
                throw new FluorineException(__Res.GetString("Type_InitError", new object[] { classDefinition.ClassName }));
            }
            reader.AddAMF3ObjectReference(instance);
            if (!(instance is IExternalizable))
            {
                throw new FluorineException(__Res.GetString("Externalizable_CastFail", new object[] { instance.GetType().FullName }));
            }
            IExternalizable externalizable = instance as IExternalizable;
            DataInput input = new DataInput(reader);
            externalizable.ReadExternal(input);
            return instance;
        }
    }
}

