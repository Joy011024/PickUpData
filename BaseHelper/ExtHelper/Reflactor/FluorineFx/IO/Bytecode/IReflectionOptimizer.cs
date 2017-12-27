namespace FluorineFx.IO.Bytecode
{
    using FluorineFx.AMF3;
    using FluorineFx.IO;
    using System;

    public interface IReflectionOptimizer
    {
        object CreateInstance();
        object ReadData(AMFReader reader, ClassDefinition classDefinition);
    }
}

