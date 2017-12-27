namespace FluorineFx.IO.Bytecode.CodeDom
{
    using FluorineFx.AMF3;
    using FluorineFx.IO;
    using FluorineFx.IO.Bytecode;
    using log4net;
    using System;

    internal class BytecodeProvider : IBytecodeProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BytecodeProvider));

        public IReflectionOptimizer GetReflectionOptimizer(Type type, ClassDefinition classDefinition, AMFReader reader, object instance)
        {
            if (classDefinition == null)
            {
                return new AMF0ReflectionOptimizer(type, reader).Generate(instance);
            }
            return new AMF3ReflectionOptimizer(type, classDefinition, reader).Generate(instance);
        }
    }
}

