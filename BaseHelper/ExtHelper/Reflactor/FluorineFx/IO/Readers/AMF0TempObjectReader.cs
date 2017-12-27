namespace FluorineFx.IO.Readers
{
    using FluorineFx.AMF3;
    using FluorineFx.IO;
    using FluorineFx.IO.Bytecode;
    using System;

    internal class AMF0TempObjectReader : IReflectionOptimizer
    {
        public object CreateInstance()
        {
            throw new NotImplementedException();
        }

        public object ReadData(AMFReader reader, ClassDefinition classDefinition)
        {
            return reader.ReadObject();
        }
    }
}

