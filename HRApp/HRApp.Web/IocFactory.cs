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
        public T IocConvert<T>(string assemblyDir, string assemblyName, string namespaceName, string className) where T : class
        {
            Assembly target = Assembly.LoadFrom(assemblyDir + assemblyName);//加载dll
            // object create = target.CreateInstance(namespaceName + "." + className, true);
            Type t = target.GetType(namespaceName + "." + className, false, true);//name=命名空间.类名称
            if (t == null)
            {//找不到对应的实体类
                return System.Activator.CreateInstance<T>();
            }
            //ConstructorInfo[] cis = t.GetConstructors();//BindingFlags.Public使用这个限定查找不到构造列表
            object obj = null;
            //如果 没有为该对象定义无参数的构造函数。
            if (!ContainDefaultConstryctor<T>(t))
            { //不含有无参构造函数

                obj = GuidConstryctorFill<T>(t);
            }
            else
            {
                obj = System.Activator.CreateInstance(t,true);//此处需要依赖默认含有无参的构造函数
            }
            if (obj == null)
            {//实体类不能转换为目标实体
                return System.Activator.CreateInstance<T>();
            }
            return (obj as T);
        }
        [Description("填充构造函数")]
        public void ConstructorFill<T>(T targetClass) 
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
        T GuidConstryctorFill<T>(Type target) where T : class //次构造函数只为实体类没有定义无参
        {
            ConstructorInfo cons = target.GetConstructors()[0];//构造函数列表
            ParameterInfo[] param = cons.GetParameters();
            for (int i = 0; i < param.Length; i++)
            {
                //对于构造函数进行默认值设定
                ParameterInfo pi = param[i];
                object obj = null;
                if (pi.HasDefaultValue)
                {
                    obj = pi.DefaultValue;
                }
                else
                {
                    Type paramType = pi.ParameterType;//参数的数据类型
                    if (!paramType.IsInterface)
                    {//参数为接口类
                        Type pt = param[i].GetType();//公共语言运行时导致使用的对象不一致
                        //如果存在系统默认的构造函数，使用默认
                        obj = System.Activator.CreateInstance(pt);

                    }
                  
                }
               
                param.SetValue(obj, i);
            }
            return ( System.Activator.CreateInstance(target, param) as T);
        }
        bool ContainDefaultConstryctor<T>( Type target) where T:class 
        {
            Type t = target;
            ConstructorInfo[] cons = t.GetConstructors();//构造函数列表【出现获取不到构造函数列表的情形】
            if (cons.Length == 0)
            {//这是默认的构造函数
                return true;
            }
            for (int i = 0; i < cons.Length; i++)
            {
                ConstructorInfo con = cons[i];
                ParameterInfo[] param = con.GetParameters();//参数列表
                if (param.Length == 0)
                {
                    return true;
                } 
            }
            return false;
        }
        [Description("属性注入")]
        public void IocFillProperty<T,Children>(T targetClass,Dictionary<string,object> propertyList) where T : class where Children:class
        {
            PropertyInfo[] pns = targetClass.GetEntityProperty(BindingFlags.Public | BindingFlags.Instance);//属性名列表
            //由于接口中没法定义 字段,此处只能使用属性
           string classProperty = typeof(T).Name;
            //是否需要实例化子类属性
           if (classProperty != typeof(Children).Name)
           {
               Children child = targetClass as Children;
               IocFillProperty(child, propertyList);
           }
           IocFillProperty(targetClass, propertyList);
        }
        public void IocFillProperty<T>(T targetClass, Dictionary<string, object> propertyList)
            where T : class
        {
            PropertyInfo[] pns = targetClass.GetEntityProperty(BindingFlags.Public | BindingFlags.Instance);//属性名列表
            //由于接口中没法定义 字段,此处只能使用属性
            string classProperty = typeof(T).Name;
            foreach (var property in pns)
            {
                string pn = string.Empty;
                string item = property.Name;
                string ptName = property.PropertyType.Name;
                if (propertyList.ContainsKey(classProperty + "." + ptName))
                {//1  实体类名称.属性数据类型名 作为字典中存储的key
                    pn = classProperty + "." + ptName;
                }
                else if (propertyList.ContainsKey(ptName))
                {//2  属性数据类型名 作为字典中存储的key
                    pn = ptName;
                }
                else if (propertyList.ContainsKey(classProperty + "." + item))
                {//3 实体类名称.属性名 作为字典中存储的key
                    pn = classProperty + "." + item;
                }
                else if (propertyList.ContainsKey(item))
                {//4 实体类名称.属性名 作为字典中存储的key
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