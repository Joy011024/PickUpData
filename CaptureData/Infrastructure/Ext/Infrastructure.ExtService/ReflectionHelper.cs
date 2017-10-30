using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
namespace Infrastructure.ExtService
{
    public static class EntityReflection
    {
        public static PropertyInfo[] GetEntityProperty<T>(this T help) where T : class
        {
            return help.GetEntityProperty();
        }
        public static PropertyInfo[] GetEntityProperty<T>() where T : class
        {
            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();
            return pis;
        }
        /// <summary>
        /// 根据属性名获取属性信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="help"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<T>(this T help, string property) where T : class
        {
            Type t = typeof(T);
            PropertyInfo pi = t.GetProperty(property);
            return pi;
        }
        /// <summary>
        /// 提供属性获取实体类中属性的值【没有检测属性是否存在】
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="help"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static object GetPropertyValue<T>(this T help, PropertyInfo p) where T : class
        {
            if (p == null) { return null; }
            return p.GetValue(help,null);
        }
        public static string[] GetEntityProperties<T>(this T help) where T:class 
        {
            return typeof(T).GetProperties().Select(p => p.Name).ToArray();
        }
        public static string GetPropertyValue<T>(this T helper,string property) where T:class 
        {
            Type type = typeof(T);
            PropertyInfo pi= type.GetProperty(property);
            object obj= pi.GetValue(helper, null);
            if (obj == null)
            {
                return string.Empty;
            }
            return obj.ToString();
        }
    }
    public static class DataConvert
    {
        public static R MapObject<T, R>(this T source)
            where T : class
            where R : class
        {
            R result = System.Activator.CreateInstance<R>();
            List<string> commonProperty = new List<string>();
            Type st = source.GetType();
            PropertyInfo[] sp = st.GetProperties();
            Type rt = result.GetType();
            PropertyInfo[] rp = rt.GetProperties();
            foreach (PropertyInfo item in sp)
            {
                PropertyInfo pi = rp.Where(p => p.Name == item.Name).FirstOrDefault();
                if (pi == null)
                {
                    continue;
                }
                object obj = item.GetValue(source, null);
                //如果是字符串，需要对空串进行过滤
                if (item.PropertyType.Name == typeof(string).Name && (string.IsNullOrEmpty(obj as string)))
                {
                    continue;
                }
                if (obj == null) { continue; }
                //如果类型不一致需要强制类型转换【如果类型不一致且可空】
                if (pi.PropertyType.Name == typeof(Nullable<>).Name)
                {
                    NullableConverter nullableConverter = new NullableConverter(pi.PropertyType);//如何获取可空类型属性非空时的数据类型
                    Type nt = nullableConverter.UnderlyingType;
                    pi.SetValue(result, Convert.ChangeType(obj, nt), null);
                    continue;

                }
                pi.SetValue(result, Convert.ChangeType(obj, pi.PropertyType), null);
            }
            return result;
        }
    }
}
