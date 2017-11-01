using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Infrastructure.ExtService;
namespace FactoryService
{
    public class IocHelper
    {
        public static T CreateObject<T>(string className, string nameSpace, string assembly, string assemblyDir) where T : class //此处提供的T为接口
        {
            GeneratorClass gc = new GeneratorClass();
            return gc.BaseAutoCreateType<T>(className, nameSpace, assembly, assemblyDir, ".dll");
        }
    }
}
