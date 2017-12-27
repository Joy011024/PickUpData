namespace FluorineFx.IO.Readers
{
    using FluorineFx;
    using FluorineFx.AMF3;
    using FluorineFx.IO;
    using FluorineFx.IO.Bytecode;
    using System;

    internal class AMF3TypedASObjectReader : IReflectionOptimizer
    {
        private string _typeIdentifier;

        public AMF3TypedASObjectReader(string typeIdentifier)
        {
            this._typeIdentifier = typeIdentifier;
        }

        public object CreateInstance()
        {
            throw new NotImplementedException();
        }

        public object ReadData(AMFReader reader, ClassDefinition classDefinition)
        {
            ASObject instance = new ASObject(this._typeIdentifier);
            reader.AddAMF3ObjectReference(instance);
            string key = reader.ReadAMF3String();
            instance.TypeName = this._typeIdentifier;
            while (key != string.Empty)
            {
                object obj3 = reader.ReadAMF3Data();
                instance.Add(key, obj3);
                key = reader.ReadAMF3String();
            }
            return instance;
        }
    }
}

