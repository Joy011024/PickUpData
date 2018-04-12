using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace Infrastructure.ExtService
{
    public class GeneratorClass
    {
        /// <summary>
        /// [命名空间和程序集名称一致]
        /// </summary>
        /// <param name="assemblyDir"></param>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="assemblyExtType"></param>
        /// <returns></returns>
        public Type AutoCreateType(string assemblyDir, string assemblyName, string className, string assemblyExtType = ".dll")
        {
            Assembly ass = Assembly.LoadFrom(assemblyDir + "/" + assemblyName + assemblyExtType);//程序集路径+程序集名称+程序集坐在的文件类型（可执行文件还是dll）
            // {Name = "ISyncTicketDataManage" FullName = "TicketData.IManage.ISyncTicketDataManage"}
            Type t = ass.GetType(assemblyName + "." + className, false);//参数说明 ：命名空间+类名
            return t;
        }
        /// <summary>
        /// [命名空间和程序集名称一致]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblyDir"></param>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="assemblyExtType"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 创建构造函数不带参数的实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="className"></param>
        /// <param name="nameSpanceName"></param>
        /// <param name="assemblyName"></param>
        /// <param name="assemblyDir"></param>
        /// <param name="assemblyBelongFileExt"></param>
        /// <returns></returns>
        public T BaseAutoCreateType<T>(string className,string  nameSpanceName,string assemblyName,string assemblyDir,string assemblyBelongFileExt=".dll") where T:class
        {
            Assembly ass = Assembly.LoadFrom(assemblyDir + "/" + assemblyName + assemblyBelongFileExt);//找到程序集
            Type t = ass.GetType(nameSpanceName + "." + className, false);//找到该命名空间下的类
            if (t == null)
            {//找不到对应的实体类
                return System.Activator.CreateInstance<T>();
            }
            object obj = System.Activator.CreateInstance(t);
            if (obj == null)
            {//实体类不能转换为目标实体

            }
            return (obj as T);
        }
        /// <summary>
        /// 创建待参数的实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramList"></param>
        /// <param name="className"></param>
        /// <param name="nameSpanceName"></param>
        /// <param name="assemblyName"></param>
        /// <param name="assemblyDir"></param>
        /// <param name="assemblyBelongFileExt"></param>
        /// <returns></returns>
        public T BaseAutoCreateTypeWithCtorParam<T>(object[] paramList, string className, string nameSpanceName,
            string assemblyName, string assemblyDir, string assemblyBelongFileExt = ".dll") where T : class
        {
            Assembly ass = Assembly.LoadFrom(assemblyDir + "/" + assemblyName + assemblyBelongFileExt);//找到程序集
            Type t = ass.GetType(nameSpanceName + "." + className, false);//找到该命名空间下的类
            object obj = System.Activator.CreateInstance(t, paramList);
            return (obj as T);
        }
    }
    public class AssemblyHelp
    {
        /// <summary>
        /// 提取程序集下全部的类对象
        /// </summary>
        /// <param name="dllPath"></param>
        public static Type[] GetAllClass(string dllPath, string baseTypeName = null)
        {
            Type[] ts = GetAllType(dllPath);
            if (string.IsNullOrEmpty(baseTypeName))
            {
                return ts;
            }
            List<Type> target = new List<Type>();
            foreach (Type item in ts)
            {
                if (item.BaseType!=null&&item.BaseType.Name.ToLower() == baseTypeName.ToLower())
                {
                    target.Add(item);
                }
            }
            return target.ToArray();
        }
        static Type[] GetAllType(string dllPath) 
        {
            Assembly ass = Assembly.LoadFrom(dllPath);
            Type[] ts = ass.GetTypes();
            return ts;
        }
        public static T[] GetAllType<T>(string dllType) where T:class
        {
            Type[] ts = GetAllType(dllType);
            List<T> es=new List<T>();
            Type et = typeof(T);
            foreach (Type item in ts)
            {
                if (item.BaseType != null && item.BaseType == et)
                {
                    object obj = System.Activator.CreateInstance(item);
                    T t = (obj as T);
                    if (t != null)
                    {
                        es.Add(t);
                    }
                }
            }
            return es.ToArray();
        }
    }
}
