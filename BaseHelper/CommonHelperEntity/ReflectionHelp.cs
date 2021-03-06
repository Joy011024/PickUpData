﻿using System;
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
            Type type = entity.GetType();
            PropertyInfo[] pis = type.GetProperties();
            foreach (var item in pis)
            {
                names.Add(item.Name);
            }
            return names.ToArray();
        }
        /// <summary>
        /// 获取指定属性的属性类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Type GetPropertyType<T>(this T entity, string propertyName) where T : class
        {
           Type t= entity.GetType();
           PropertyInfo pi= t.GetProperty(propertyName);
           return pi.PropertyType;
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
            Type type = entity.GetType();// typeof(T);//当entity传递的是一个object时找不到属性集合
            PropertyInfo[] fis= type.GetProperties();
            foreach (PropertyInfo item in fis)
            {
                kv.Add(item.Name, item.GetValue(entity,null));
            }
            return kv;

        }
    }
}
