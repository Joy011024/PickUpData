namespace FluorineFx.IO.Bytecode.Lightweight
{
    using FluorineFx.AMF3;
    using FluorineFx.IO;
    using FluorineFx.IO.Bytecode;
    using System;

    internal class BytecodeProvider : IBytecodeProvider
    {
        public IReflectionOptimizer GetReflectionOptimizer(Type type, ClassDefinition classDefinition, AMFReader reader, object instance)
        {
            if (classDefinition == null)
            {
                return new AMF0ReflectionOptimizer(type, reader, instance);
            }
            return new AMF3ReflectionOptimizer(type, classDefinition, reader, instance);
        }
    }
}

