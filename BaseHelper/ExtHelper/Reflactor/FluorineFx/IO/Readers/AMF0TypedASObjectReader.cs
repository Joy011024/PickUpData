namespace FluorineFx.IO.Readers
{
    using FluorineFx;
    using FluorineFx.AMF3;
    using FluorineFx.IO;
    using FluorineFx.IO.Bytecode;
    using System;

    internal class AMF0TypedASObjectReader : IReflectionOptimizer
    {
        private string _typeIdentifier;

        public AMF0TypedASObjectReader(string typeIdentifier)
        {
            this._typeIdentifier = typeIdentifier;
        }

        public object CreateInstance()
        {
            throw new NotImplementedException();
        }

        public object ReadData(AMFReader reader, ClassDefinition classDefinition)
        {
            ASObject obj2 = reader.ReadASObject();
            obj2.TypeName = this._typeIdentifier;
            return obj2;
        }
    }
}

