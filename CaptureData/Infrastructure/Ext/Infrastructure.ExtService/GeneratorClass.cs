using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace Infrastructure.ExtService
{
    public class GeneratorClass
    {
        public Type AutoCreateType(string assemblyDir, string assemblyName, string className, string assemblyExtType = ".dll")
        {
            Assembly ass = Assembly.LoadFrom(assemblyDir + "/" + assemblyName + assemblyExtType);
            Type t = ass.GetType(assemblyName + "." + className, false);
            return t;
        }
        public T AutoCreateType<T>(string assemblyDir, string assemblyName, string className,string assemblyExtType=".dll") where T:class
        {
            Type t = AutoCreateType(assemblyDir, assemblyName, className, assemblyExtType);
            if (t == null)
            {//找不到对应的实体类
              return  System.Activator.CreateInstance<T>();
            }
            object obj = System.Activator.CreateInstance(t);
            if (obj == null)
            {//实体类不能转换为目标实体
            
            }
            return (obj as T);
        }
    }
}
