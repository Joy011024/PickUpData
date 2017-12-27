namespace FluorineFx.IO.Readers
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.IO;
    using FluorineFx.IO.Bytecode;
    using FluorineFx.IO.Bytecode.CodeDom;
    using FluorineFx.IO.Bytecode.Lightweight;
    using log4net;
    using System;
    using System.Collections;

    internal class AMF0OptimizedObjectReader : IAMFReader
    {
        private Hashtable _optimizedReaders = new Hashtable();
        private static readonly ILog log = LogManager.GetLogger(typeof(AMF0OptimizedObjectReader));

        public object ReadData(AMFReader reader)
        {
            object instance = null;
            string str2;
            string str = reader.ReadString();
            if (log.get_IsDebugEnabled())
            {
                str2 = string.Format("Attempt to read custom object {0}", str);
                log.Debug(str2);
            }
            IReflectionOptimizer optimizer = this._optimizedReaders[str] as IReflectionOptimizer;
            if (optimizer == null)
            {
                lock (this._optimizedReaders)
                {
                    if (!this._optimizedReaders.Contains(str))
                    {
                        if (log.get_IsDebugEnabled())
                        {
                            str2 = string.Format("Generating optimizer for type {0}", str);
                            log.Debug(str2);
                        }
                        this._optimizedReaders[str] = new AMF0TempObjectReader();
                        Type type = ObjectFactory.Locate(str);
                        if (type != null)
                        {
                            instance = ObjectFactory.CreateInstance(type);
                            reader.AddReference(instance);
                            if (type != null)
                            {
                                IBytecodeProvider provider = null;
                                if (FluorineConfiguration.Instance.OptimizerSettings.Provider == "codedom")
                                {
                                    provider = new BytecodeProvider();
                                }
                                if (FluorineConfiguration.Instance.OptimizerSettings.Provider == "il")
                                {
                                    provider = new BytecodeProvider();
                                }
                                optimizer = provider.GetReflectionOptimizer(type, null, reader, instance);
                                if (optimizer != null)
                                {
                                    this._optimizedReaders[str] = optimizer;
                                    return instance;
                                }
                                this._optimizedReaders[str] = new AMF0TempObjectReader();
                            }
                            return instance;
                        }
                        if (log.get_IsWarnEnabled())
                        {
                            log.Warn("Custom object " + str + " could not be loaded. An ActionScript typed object (ASObject) will be created");
                        }
                        optimizer = new AMF0TypedASObjectReader(str);
                        this._optimizedReaders[str] = optimizer;
                        return optimizer.ReadData(reader, null);
                    }
                    optimizer = this._optimizedReaders[str] as IReflectionOptimizer;
                    return optimizer.ReadData(reader, null);
                }
                return instance;
            }
            return optimizer.ReadData(reader, null);
        }
    }
}

