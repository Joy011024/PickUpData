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
}
