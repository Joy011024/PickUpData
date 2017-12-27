namespace FluorineFx.IO.Readers
{
    using FluorineFx;
    using FluorineFx.AMF3;
    using FluorineFx.Configuration;
    using FluorineFx.IO;
    using FluorineFx.IO.Bytecode;
    using FluorineFx.IO.Bytecode.CodeDom;
    using FluorineFx.IO.Bytecode.Lightweight;
    using log4net;
    using System;
    using System.Collections;

    internal class AMF3OptimizedObjectReader : IAMFReader
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(AMF3OptimizedObjectReader));
        private Hashtable _optimizedReaders = new Hashtable();

        public object ReadData(AMFReader reader)
        {
            int index = reader.ReadAMF3IntegerData();
            bool flag = (index & 1) != 0;
            index = index >> 1;
            if (!flag)
            {
                return reader.ReadAMF3ObjectReference(index);
            }
            ClassDefinition classDefinition = reader.ReadClassDefinition(index);
            object instance = null;
            IReflectionOptimizer optimizer = this._optimizedReaders[classDefinition.ClassName] as IReflectionOptimizer;
            if (optimizer == null)
            {
                lock (this._optimizedReaders)
                {
                    if (classDefinition.IsTypedObject)
                    {
                        if (!this._optimizedReaders.Contains(classDefinition.ClassName))
                        {
                            this._optimizedReaders[classDefinition.ClassName] = new AMF3TempObjectReader();
                            Type type = ObjectFactory.Locate(classDefinition.ClassName);
                            if (type != null)
                            {
                                instance = ObjectFactory.CreateInstance(type);
                                if (classDefinition.IsExternalizable)
                                {
                                    optimizer = new AMF3ExternalizableReader();
                                    this._optimizedReaders[classDefinition.ClassName] = optimizer;
                                    instance = optimizer.ReadData(reader, classDefinition);
                                }
                                else
                                {
                                    reader.AddAMF3ObjectReference(instance);
                                    IBytecodeProvider provider = null;
                                    if (FluorineConfiguration.Instance.OptimizerSettings.Provider == "codedom")
                                    {
                                        provider = new BytecodeProvider();
                                    }
                                    if (FluorineConfiguration.Instance.OptimizerSettings.Provider == "il")
                                    {
                                        provider = new BytecodeProvider();
                                    }
                                    optimizer = provider.GetReflectionOptimizer(type, classDefinition, reader, instance);
                                    if (optimizer != null)
                                    {
                                        this._optimizedReaders[classDefinition.ClassName] = optimizer;
                                    }
                                    else
                                    {
                                        this._optimizedReaders[classDefinition.ClassName] = new AMF3TempObjectReader();
                                    }
                                }
                                return instance;
                            }
                            optimizer = new AMF3TypedASObjectReader(classDefinition.ClassName);
                            this._optimizedReaders[classDefinition.ClassName] = optimizer;
                            return optimizer.ReadData(reader, classDefinition);
                        }
                        optimizer = this._optimizedReaders[classDefinition.ClassName] as IReflectionOptimizer;
                        return optimizer.ReadData(reader, classDefinition);
                    }
                    optimizer = new AMF3TypedASObjectReader(classDefinition.ClassName);
                    this._optimizedReaders[classDefinition.ClassName] = optimizer;
                    return optimizer.ReadData(reader, classDefinition);
                }
            }
            return optimizer.ReadData(reader, classDefinition);
        }
    }
}

