namespace FluorineFx.IO.Bytecode
{
    using FluorineFx.AMF3;
    using FluorineFx.IO;
    using System;

    internal interface IBytecodeProvider
    {
        IReflectionOptimizer GetReflectionOptimizer(Type type, ClassDefinition classDefinition, AMFReader reader, object instance);
    }
}

