using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace CommonHelperEntity
{
    public static class ReflectionHelp
    {
        public static string GetObjectName<T>(this T entity) where T:class
        {
            Type type = typeof(T);
            return type.Name;
        }
        public static string[] GetAllProperties<T>(this T entity) where T : class
        {
            List<string> names = new List<string>();
            Type type = typeof(T);
            PropertyInfo[] pis = type.GetProperties();
            foreach (var item in pis)
            {
                names.Add(item.Name);
            }
            return names.ToArray();
        }
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="property">属性名</param>
        /// <param name="hasProperty">【输出】是否存在该属性</param>
        /// <returns>值</returns>
        public static object GetPropertyValue<T>(this T entity, string property,out bool hasProperty) where T : class
        {
            Type type =typeof(T);
            PropertyInfo pi= type.GetProperty(property);
            if (pi == null) 
            {
                hasProperty = false;
                return null;
            }
            hasProperty = true;
            return pi.GetValue(entity, null);
        }
        public static Dictionary<string, object> GetAllPorpertiesNameAndValues<T>(this T entity) where T : class
        {
            Dictionary<string, object> kv = new Dictionary<string, object>();
            Type type = typeof(T);
            PropertyInfo[] fis= type.GetProperties();
            foreach (PropertyInfo item in fis)
            {
                kv.Add(item.Name, item.GetValue(entity,null));
            }
            return kv;

        }
    }
}
