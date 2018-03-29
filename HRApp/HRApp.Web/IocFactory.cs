using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.EnterpriseServices;
using Infrastructure.ExtService;
namespace HRApp.Web
{
    public class IocFactory
    {
    }
    public class InterfaceIocHelper 
    {
        public T IocConvert<T>(string assemblyDir,string assemblyName,string namespaceName, string className) where T:class
        {
            Assembly target = Assembly.LoadFrom(assemblyDir + assemblyName);//加载dll
            Type t = target.GetType(namespaceName+"."+className, false, true);//name=命名空间.类名称
            if (t == null)
            {//找不到对应的实体类
                return System.Activator.CreateInstance<T>();
            }
            object obj = System.Activator.CreateInstance(t);
            if (obj == null)
            {//实体类不能转换为目标实体
                return System.Activator.CreateInstance<T>();
            }
            return (obj as T);
        }
        [Description("填充构造函数")]
        public void CunstructorFill<T>(T targetClass) 
        {
            Type t = typeof(T).GetType();
            ConstructorInfo[] cons= t.GetConstructors();//构造函数列表
            for (int i = 0; i < cons.Length; i++)
            {
                ConstructorInfo con = cons[i];
                ParameterInfo[] param = con.GetParameters();//参数列表
                if (param.Length == 0)
                {
                    continue;
                }
                //进行参数注入

            }
        }
        [Description("属性注入")]
        public void IocFillProperty<T>(T targetClass,Dictionary<string,object> propertyList) where T : class
        {
           string[] pns= targetClass.GetEntityProperties();//属性名列表
           string classProperty = typeof(T).Name;
           foreach (var item in pns)
           {
               string pn = string.Empty;
               if (propertyList.ContainsKey(classProperty + "." + item))
               {//首先 实体类名称.属性名 作为字典中存储的key
                   pn = classProperty + "." + item;
               }
               else if (!propertyList.ContainsKey(item))
               {
                   pn = item;
               }
               else
               {
                   continue;
               }
               object pv = propertyList[pn];
               targetClass.SetPropertyValue(item, pv);
           }
        }
    }
}